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

        [ObservableProperty]
        string searchCriteria;
        IDispatcherTimer autoSearchTimer;

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
            Notes = new ObservableCollection<Note>(notesService.GetAll());
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

        [RelayCommand]
        void SearchNotes()
        {
            if (autoSearchTimer == null)
                autoSearchTimer = CreateTimer();
            autoSearchTimer.Start();
        }

        void AutoSearch_Tick(object sender, EventArgs e)
        {
            Notes = new ObservableCollection<Note>(notesService.Search(SearchCriteria));
        }

        IDispatcherTimer CreateTimer()
        {
            var timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(0.5);
            timer.Tick += AutoSearch_Tick;
            return timer;
        }
    }
}

