using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotesMaui.Models;
using NotesMaui.Services;
using Note = NotesMaui.Models.Note;

namespace NotesMaui.ViewModels
{
    public partial class EditNoteViewModel : ObservableObject
    {
        [ObservableProperty]
        string title;

        [ObservableProperty]
        string content;

        [ObservableProperty]
        DateTime lastUpdateDate;

        bool isJustLoaded;
        bool isLoading;
        bool isEditing;
        IDispatcherTimer lastEditTimer;

        private INoteService notesService;

        private List<Memento> undoList = new();
        private List<Memento> redoList = new();

        public EditNoteViewModel(INoteService _noteService)
        {
            notesService = _noteService;
        }

        public int noteId
        {
            set => LoadNoteById(value);
        }

        private void LoadNoteById(int noteId)
        {
            isLoading = true;
            isEditing = false;
            isJustLoaded = false;

            var note = notesService.GetById(noteId);
            Title = note.Title;
            Content = note.Content;
            LastUpdateDate = note.LastUpdateDate;

            isJustLoaded = true;
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

        void EnableUndoRedo()
        {
            if (isLoading) return;

            if (redoList?.Count > 0) redoList.Clear();

            if (isJustLoaded)
            {
                undoList.Add(CreateMemento());
                isJustLoaded = false;
            }
            else if (lastEditTimer == null || !lastEditTimer.IsRunning)
            {
                lastEditTimer = Application.Current.Dispatcher.CreateTimer();
                lastEditTimer.Interval = TimeSpan.FromSeconds(5);
                lastEditTimer.Tick += (s, e) =>
                {
                    undoList.Add(CreateMemento());
                    lastEditTimer.Stop();
                };
                lastEditTimer.Start();
            }

            isEditing = true;
        }

        [RelayCommand]
        private void Undo()
        {
            if (!CanUndo) return;

            isLoading = true;
            // get state to restore
            var stateToRestore = undoList.Last();

            // save current state in the stack
            redoList.Add(CreateMemento());

            // restore prev. state
            SetMemento(stateToRestore);

            // remove from undo list
            if (undoList.Count > 0)
                undoList.RemoveAt(undoList.Count - 1);


            isLoading = false;
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
            if (stateToRestore != null)
                SetMemento(stateToRestore);

            // remvoe from redo list
            if (redoList.Count > 0)
                redoList.RemoveAt(redoList.Count - 1);

            isLoading = false;

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

        bool CanUndo => !isEditing | isLoading | undoList?.Count == 0;
        bool CanRedo => !isEditing | isLoading | redoList?.Count == 0;

    }
}

