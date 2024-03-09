using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using NotesMaui.Services;
using NotesMaui.ViewModels;
using NotesMaui.Views;

namespace NotesMaui;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            fonts.AddFont("fontello.ttf", "Icons");
        }).UseMauiCommunityToolkit();
#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<EditNotePage>();

        builder.Services.AddSingleton<INoteService, NoteMockService>();
        builder.Services.AddTransient<EditNoteViewModel>();
        builder.Services.AddTransient<NoteViewModel>();

        builder.Services.AddSingleton<NavigationService>();

        return builder.Build();
    }
}