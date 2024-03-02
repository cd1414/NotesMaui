using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotesMaui.Models;
using NotesMaui.Services;
using NotesMaui.Views;

namespace NotesMaui.ViewModels
{
    public partial class NoteViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Note> notes;

        private NotesService notesService;

        public NoteViewModel()
        {
            notesService = new NotesService();
            LoadNotes();
        }

        [RelayCommand]
        void LoadNotes()
        {
            Notes = new ObservableCollection<Note>(notesService.GetAllNotes());
        }

        [RelayCommand]
        void SelectionChanged(object selectedItem)
        {
            Shell.Current.GoToAsync($"{nameof(EditNotePage)}?Id={((Note)selectedItem).Id}");
        }
    }
}

