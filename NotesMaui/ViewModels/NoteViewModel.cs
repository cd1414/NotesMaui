using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotesMaui.Models;
using NotesMaui.Services;
using NotesMaui.Views;

namespace NotesMaui.ViewModels
{
    public partial class NoteViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<Note> notes;

        private INoteService notesService;
        private NavigationService navigationService;

        public NoteViewModel(
            INoteService _noteService,
            NavigationService _navigationService)
        {
            notesService = _noteService;
            navigationService = _navigationService;
            LoadNotes();
        }


        [RelayCommand]
        void LoadNotes()
        {
            Notes = new ObservableCollection<Note>(notesService.GetAll(true));
        }

        [RelayCommand]
        async Task SelectionChanged(Note selectedItem)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "Id", selectedItem.Id }
            };
            await navigationService.NavigateToPage<EditNotePage>(parameters);
        }

        public override Task OnNavigatedTo()
        {
            LoadNotes();
            return base.OnNavigatedTo();
        }

        [RelayCommand]
        async Task CreateNote()
        {
            await navigationService.NavigateToPage<EditNotePage>(null);
        }
    }
}

