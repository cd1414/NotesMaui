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
            saveMemento = false;

            var note = notesService.GetById(noteId);

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

        void EnableUndoRedo()
        {
            if (isLoading) return;

            if (redoList?.Count > 0) redoList.Clear();

            if (saveMemento)
            {
                undoList.Add(CreateMemento());
                saveMemento = false;
            }

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
        void Save()
        {
            var note = new Note
            {
                Id = Id,
                Title = Title,
                Content = Content
            };

            notesService.Save(note);

            var currentNotes = notesService.GetAll();

            Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        void Cancel()
        {
            Shell.Current.GoToAsync("..");
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

