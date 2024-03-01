namespace NotesMaui.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        lstNotes.ItemsSource = new List<string> { "1", "2", "1", "2", "1", "2", "1", "2", "1", "2", "1", "2" };
    }

    void lstNotes_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        Shell.Current.GoToAsync(nameof(EditNotePage));
    }
}


