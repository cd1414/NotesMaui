using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotesMaui.Models;
using NotesMaui.Services;
using NotesMaui.Views;
using Note = NotesMaui.Models.Note;

namespace NotesMaui.ViewModels
{
    public partial class EditNoteViewModel : ViewModelBase
    {
        [ObservableProperty]
        int id;

        [ObservableProperty]
        string title;

        [ObservableProperty]
        string content;

        [ObservableProperty]
        DateTime lastUpdateDate;

        bool saveMemento;
        bool isLoading;
        bool isEditing;
        IDispatcherTimer autoSaveMemento;

        private INoteService notesService;
        private NavigationService navigationService;

        private List<Memento> undoList = new();
        private List<Memento> redoList = new();

        [ObservableProperty]
        bool undoEnabled;

        public EditNoteViewModel(INoteService _noteService, NavigationService _navigationService)
        {
            notesService = _noteService;
            navigationService = _navigationService;
            UndoEnabled = false;
        }

        public override Task OnNavigatingTo(Dictionary<string, object> parameter)
        {
            var id = 0;

            if (parameter is not null && parameter.TryGetValue("Id", out _))
                id = (int)parameter["Id"];

            LoadNoteById(id);

            return base.OnNavigatingTo(parameter);
        }

        private void LoadNoteById(int noteId)
        {
            isLoading = true;
            isEditing = false;
            saveMemento = false;

            var note = noteId > 0
                ? notesService.GetById(noteId)
                : CreateNote();

            Id = note.Id;
            Title = note.Title;
            Content = note.Content;
            LastUpdateDate = note.LastUpdateDate;

            saveMemento = true;
            isEditing = false;
            isLoading = false;
        }

        partial void OnContentChanging(string oldValue, string newValue)
        {
            EnableUndoRedo();
        }

        partial void OnTitleChanging(string oldValue, string newValue)
        {
            EnableUndoRedo();
        }

        Note CreateNote()
        {
            var date = DateTime.Now;
            return new()
            {
                CreationDate = date,
                LastUpdateDate = date
            };
        }

        void AutoSaveMemento_Tick(object sender, EventArgs e)
        {
            undoList.Add(CreateMemento());
            autoSaveMemento.Stop();
        }

        void StartAutoSaveMemento()
        {
            autoSaveMemento = Application.Current.Dispatcher.CreateTimer();
            autoSaveMemento.Interval = TimeSpan.FromSeconds(5);
            autoSaveMemento.Tick += AutoSaveMemento_Tick;
            autoSaveMemento.Start();
        }

        void EnableUndoRedo()
        {
            if (isLoading) return;

            if (redoList?.Count > 0) redoList.Clear();

            if (autoSaveMemento == null || !autoSaveMemento.IsRunning)
                StartAutoSaveMemento();

            if (saveMemento)
            {
                undoList.Add(CreateMemento());
                saveMemento = false;
            }

            UndoEnabled = true;
            isEditing = true;
        }

        [RelayCommand]
        private void Undo()
        {
            if (!CanUndo) return;

            // save current state to the redo list
            redoList.Add(CreateMemento());

            // get state to restore
            var stateToRestore = undoList.Last();

            // restore prev. state
            isLoading = true;
            SetMemento(stateToRestore);
            isLoading = false;

            // remove from undo list
            undoList.RemoveAt(undoList.Count - 1);
            saveMemento = true;
        }

        [RelayCommand]
        private void Redo()
        {
            if (!CanRedo) return;

            isLoading = true;
            // get state to restore
            var stateToRestore = redoList.Count > 0 ? redoList.Last() : null;

            // save current state in the stack
            undoList.Add(CreateMemento());

            // restore state
            isLoading = true;
            if (stateToRestore != null)
                SetMemento(stateToRestore);
            isLoading = false;

            // remvoe from redo list
            if (redoList.Count > 0)
                redoList.RemoveAt(redoList.Count - 1);

            isLoading = false;

        }

        [RelayCommand]
        async Task Save()
        {
            var note = new Note
            {
                Id = Id,
                Title = Title,
                Content = Content
            };

            notesService.Save(note);
            await navigationService.NavigateToPage<MainPage>(null);
        }

        [RelayCommand]
        async Task Cancel()
        {
            await navigationService.NavigateBack();
        }

        [RelayCommand]
        async Task Delete(int id)
        {
            if (id == 0) return;

            var action = await App.Current.MainPage.DisplayActionSheet("Do you want to delete this note?", "Cancel", "Delete Note");

            if (action.Equals("Delete Note"))
            {
                notesService.Delete(id);
                //await navigationService.NavigateToPage<MainPage>(null);

                await navigationService.NavigateBack();
            }
        }

        public Memento CreateMemento()
        {
            var note = new Note
            {
                Title = Title,
                Content = Content
            };

            return new Memento(JsonSerializer.Serialize(note));
        }

        public void SetMemento(Memento memento)
        {
            var newState = JsonSerializer.Deserialize<Note>(memento.State);

            Title = newState.Title;
            Content = newState.Content;

        }

        bool CanUndo => (isEditing | !isLoading) && undoList?.Count > 0;
        bool CanRedo => (isEditing | !isLoading) && redoList?.Count > 0;

    }
}

