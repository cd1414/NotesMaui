using System.Collections.ObjectModel;
using NotesMaui.Models;

namespace NotesMaui.Services
{
    public interface INoteService
    {
        ObservableCollection<Note> GetAll();
        Note GetById(int id);
        void Delete(int id);
        void Save(Note entity);
        ObservableCollection<Note> Search(string filter);
    }
}