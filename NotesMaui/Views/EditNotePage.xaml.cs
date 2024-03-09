using NotesMaui.ViewModels;

namespace NotesMaui.Views;

public partial class EditNotePage : ContentPage
{
    public EditNotePage(EditNoteViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
