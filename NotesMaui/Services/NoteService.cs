using NotesMaui.Models;

namespace NotesMaui.Services
{
    public class NoteService : INoteService
    {
        readonly List<Note> Notes = new List<Note>()
        {
            new Note { Id = 1, Title = "MVVM", Content = "Model View ViewModel pattern", CreationDate = DateTime.Now.AddDays(-2), LastUpdateDate = DateTime.Now.AddDays(-2) },
            new Note { Id = 2, Title = "MVC", Content = "Model View Controller pattern", CreationDate = DateTime.Now.AddDays(1), LastUpdateDate = DateTime.Now.AddDays(1) }
        };

        public void Add(Note newEntity)
        {
            Notes.Add(newEntity);
        }

        public void Delete(int id)
        {
            var noteTarget = Notes.FirstOrDefault(note => note.Id == id);

            if (noteTarget == null)
                throw new Exception("Note not found.");

            Notes.Remove(noteTarget);
        }

        public List<Note> GetAll()
        {
            return Notes;
        }

        public Note GetById(int id)
        {
            return Notes.FirstOrDefault(note => note.Id == id);
        }

        public void Update(int id, Note entityUpdated)
        {
            if (id != entityUpdated.Id) return;

            var noteTarget = Notes.FirstOrDefault(note => note.Id == id);
            noteTarget.Title = entityUpdated.Title;
            noteTarget.Content = entityUpdated.Content;
            noteTarget.LastUpdateDate = DateTime.Now;
        }
    }
}