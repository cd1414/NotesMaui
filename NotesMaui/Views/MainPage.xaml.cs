using NotesMaui.ViewModels;

namespace NotesMaui.Views;

public partial class MainPage : ContentPage
{
    public MainPage(NoteViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    //protected override void OnAppearing()
    //{
    //    base.OnAppearing();
    //    //lstNotes.SelectedItem = null;
    //}

    //void lstNotes_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    //{
    //    //if (e.CurrentSelection == null || e.CurrentSelection.Count == 0) return;

    //    //Shell.Current.GoToAsync(nameof(EditNotePage));

    //}
}