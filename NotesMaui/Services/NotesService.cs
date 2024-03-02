using NotesMaui.Models;

namespace NotesMaui.Services
{
    public class NotesService
    {
        private List<Note> notes = new List<Note>();
        public NotesService()
        {
            notes = new List<Note>
            {
                new Note {
                    Id = 1,
                    Title = "MVVM",
                    Content = "Model View ViewModel (MVVM) is a pattern that pattern helps cleanly separate an application's business and presentation logic from its user interface (UI).",
                    CreationDate = new DateTime(2024, 01, 28, 10, 00, 00),
                    LastUpdateDate = new DateTime(2024, 01, 28, 10, 00, 00)
                },
                new Note {
                    Id = 2,
                    Title = "Design Changes",
                    Content = "How design thinking transforms organization and inspires innovation.",
                    CreationDate = new DateTime(2024, 01, 29, 10, 00, 00),
                    LastUpdateDate = new DateTime(2024, 01, 29, 10, 00, 00)
                }
            };
        }

        public List<Note> GetAllNotes() => notes;

        public Note GetNoteById(int id) => notes.FirstOrDefault(note => note.Id == id);
    }
}

