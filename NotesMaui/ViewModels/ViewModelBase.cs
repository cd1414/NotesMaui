using CommunityToolkit.Mvvm.ComponentModel;

namespace NotesMaui.ViewModels
{
    public class ViewModelBase : ObservableObject
    {
        public virtual Task OnNavigatingTo(Dictionary<string, object> parameter)
            => Task.CompletedTask;
        public virtual Task OnNavigatedFrom(bool isForwardNavigation)
            => Task.CompletedTask;
        public virtual Task OnNavigatedTo()
            => Task.CompletedTask;
    }
}