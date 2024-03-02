using NotesMaui.ViewModels;

namespace NotesMaui.Views;

[QueryProperty(nameof(noteId), "Id")]
public partial class EditNotePage : ContentPage
{
    public EditNotePage(EditNoteViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    public int noteId
    {
        set => ((EditNoteViewModel)BindingContext).noteId = value;
    }
}
