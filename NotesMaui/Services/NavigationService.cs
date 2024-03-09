using System.Diagnostics;
using NotesMaui.ViewModels;
using NotesMaui.Views;

namespace NotesMaui.Services
{
    public class NavigationService
    {
        readonly IServiceProvider _services;

        protected INavigation Navigation
        {
            get
            {
                var navigation = Application.Current.MainPage.Navigation;

                if (navigation is null)
                {
                    if (Debugger.IsAttached)
                        Debugger.Break();
                    throw new Exception();
                }

                return navigation;
            }
        }

        public NavigationService(IServiceProvider services)
            => _services = services;

        public Task NavigateToEditPage()
            => NavigateToPage<EditNotePage>();


        private ViewModelBase GetPageViewModelBase(Page page)
            => page?.BindingContext as ViewModelBase;

        private Task NavigateToPage<T>() where T : Page
        {
            var page = ResolvePage<T>();
            if (page is not null)
                return Navigation.PushAsync(page, true);
            throw new InvalidOperationException($"Unable to resolve type {typeof(T).FullName}");
        }

        public async Task NavigateToPage<T>(Dictionary<string, object> parameter)
            where T : Page
        {
            var toPage = ResolvePage<T>();
            if (toPage is not null)
            {
                //Subscribe to the toPage's NavigatedTo event
                toPage.NavigatedTo += Page_NavigatedTo;
                //Get VM of the toPage
                var toViewModel = GetPageViewModelBase(toPage);
                //Call navigatingTo on VM, passing in the paramter
                if (toViewModel is not null)
                    await toViewModel.OnNavigatingTo(parameter);
                //Navigate to requested page
                await Navigation.PushAsync(toPage, true);
                //Subscribe to the toPage's NavigatedFrom event
                toPage.NavigatedFrom += Page_NavigatedFrom;
            }
            else
                throw new InvalidOperationException($"Unable to resolve type {typeof(T).FullName}");
        }

        private T? ResolvePage<T>() where T : Page
            => _services.GetService<T>();

        private async void Page_NavigatedTo(object sender, NavigatedToEventArgs e)
            => await CallNavigatedTo(sender as Page);

        private Task CallNavigatedTo(Page page)
        {
            var fromViewModel = GetPageViewModelBase(page);

            if (fromViewModel is null)
                return Task.CompletedTask;

            return fromViewModel.OnNavigatedTo();
        }

        private async void Page_NavigatedFrom(object sender, NavigatedFromEventArgs e)
        {
            bool isForwardNavigation =
                Navigation.NavigationStack.Count > 1
                && Navigation.NavigationStack[^2] == sender;

            if (sender is Page thisPage)
            {
                if (!isForwardNavigation)
                {
                    thisPage.NavigatedTo -= Page_NavigatedTo;
                    thisPage.NavigatedFrom -= Page_NavigatedFrom;
                }

                await CallNavigatedFrom(thisPage, isForwardNavigation);
            }
        }

        private Task CallNavigatedFrom(Page page, bool isForward)
        {
            var fromViewModel = GetPageViewModelBase(page);

            if (fromViewModel is null)
                return Task.CompletedTask;

            return fromViewModel.OnNavigatedFrom(isForward);
        }

        public Task NavigateBack()
        {
            if (Navigation.NavigationStack.Count > 1)
                return Navigation.PopAsync();
            throw new InvalidOperationException("No pages to navigate back to!");
        }
    }
}

