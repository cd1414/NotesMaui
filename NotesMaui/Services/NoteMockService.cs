using System.Collections.ObjectModel;
using NotesMaui.Models;

namespace NotesMaui.Services
{
    public class NoteMockService : INoteService
    {
        private readonly ObservableCollection<Note> Notes = new ObservableCollection<Note>()
        {
            new Note { Id = 1, Title = "MVVM", Content = "Model View ViewModel pattern", CreationDate = DateTime.Now.AddDays(-2), LastUpdateDate = DateTime.Now.AddDays(-2) },
            new Note { Id = 2, Title = "MVC", Content = "The MVC pattern separates the concerns of an application into three distinct components, each responsible for a specific aspect of the application's functionality", CreationDate = DateTime.Now.AddDays(1), LastUpdateDate = DateTime.Now.AddDays(1) },
            new Note { Id = 3, Title = "MVC", Content = "Model View Controller pattern", CreationDate = DateTime.Now.AddDays(1), LastUpdateDate = DateTime.Now.AddDays(1) }
        };


        public void Add(Note newEntity)
        {
            newEntity.Id = Notes.LastOrDefault(new Note { Id = 0 }).Id + 1;
            newEntity.LastUpdateDate = DateTime.Now;
            Notes.Add(newEntity);
        }

        public void Delete(int id)
        {
            var noteTarget = Notes.FirstOrDefault(note => note.Id == id);

            if (noteTarget == null)
                throw new Exception("Note not found.");

            Notes.Remove(noteTarget);
        }

        public ObservableCollection<Note> GetAll(bool isPreview = false)
        {
            // Mon. March 4th exists and issue with max length on Entry and Editor Controls

            int titleMaxLength = 25;
            int contentMaxLength = 80;

            return new ObservableCollection<Note>(
                    Notes.Select(note => new Note
                    {
                        Id = note.Id,
                        Title = isPreview & note.Title?.Length > titleMaxLength ? $"{note.Title?[..titleMaxLength]}.." : note.Title,
                        Content = isPreview & note.Content?.Length > contentMaxLength ? $"{note.Content?[..contentMaxLength]}.." : note.Content,
                        CreationDate = note.CreationDate,
                        LastUpdateDate = note.LastUpdateDate
                    })
                );
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

        public void Update(Note entity)
        {
            var noteTarget = Notes.FirstOrDefault(note => note.Id == entity.Id);
            noteTarget.Title = entity.Title;
            noteTarget.Content = entity.Content;
            noteTarget.LastUpdateDate = DateTime.Now;
        }

        public void Save(Note entity)
        {
            if (entity.Id == 0)
                Add(entity);

            Update(entity);
        }
    }
}