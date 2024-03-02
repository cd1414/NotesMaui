using CommunityToolkit.Mvvm.ComponentModel;
using NotesMaui.Models;
using NotesMaui.Services;

namespace NotesMaui.ViewModels
{
    public partial class EditNoteViewModel : ObservableObject
    {
        [ObservableProperty]
        private Note currentNote;

        private INoteService notesService;

        public EditNoteViewModel(INoteService _noteService)
        {
            notesService = _noteService;
        }

        public int noteId
        {
            set
            {
                LoodNote(value);
            }
        }

        private void LoodNote(int noteId)
        {
            CurrentNote = notesService.GetById(noteId);
        }


    }
}

