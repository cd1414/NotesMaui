using NotesMaui.ViewModels;

namespace NotesMaui.Views;

public partial class MainPage : ContentPage
{
    public MainPage(NoteViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}