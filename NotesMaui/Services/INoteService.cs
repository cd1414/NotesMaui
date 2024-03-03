using System.Collections.ObjectModel;
using NotesMaui.Models;

namespace NotesMaui.Services
{
    public interface INoteService
    {
        ObservableCollection<Note> GetAll();
        Note GetById(int id);
        void Update(int id, Note entityUpdated);
        void Add(Note newEntity);
        void Delete(int id);
        void Save(Note entity);
    }
}