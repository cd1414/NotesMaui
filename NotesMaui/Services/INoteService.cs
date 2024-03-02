using NotesMaui.Models;

namespace NotesMaui.Services
{
    public interface INoteService
    {
        List<Note> GetAll();
        Note GetById(int id);
        void Update(int id, Note entityUpdated);
        void Add(Note newEntity);
        void Delete(int id);
    }
}