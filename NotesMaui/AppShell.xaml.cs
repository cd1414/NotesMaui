using NotesMaui.Views;

namespace NotesMaui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(EditNotePage), typeof(EditNotePage));
    }
}

