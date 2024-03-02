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

        private INoteService notesService;

        public NoteViewModel(INoteService _noteService)
        {
            notesService = _noteService;
            LoadNotes();
        }

        [RelayCommand]
        void LoadNotes()
        {
            Notes = new ObservableCollection<Note>(notesService.GetAll());
        }

        [RelayCommand]
        void SelectionChanged(object selectedItem)
        {
            Shell.Current.GoToAsync($"{nameof(EditNotePage)}?Id={((Note)selectedItem).Id}");
        }
    }
}

