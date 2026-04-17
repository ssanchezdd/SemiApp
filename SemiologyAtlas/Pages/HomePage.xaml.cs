using SemiologyAtlas.ViewModels;
using SemiologyAtlas.Services;

namespace SemiologyAtlas.Pages;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
        BindingContext = new HomeViewModel();
    }

    private async void OnOpenRouteClicked(object? sender, EventArgs e)
    {
        if (sender is Button { CommandParameter: string route })
        {
            await Shell.Current.GoToAsync(route);
        }
    }

    private void OnToggleLanguageClicked(object? sender, EventArgs e)
    {
        LocalizationService.SetLanguage(
            LocalizationService.IsSpanish
                ? AppLanguage.English
                : AppLanguage.Spanish);

        ((App)Application.Current!).ReloadShell();
    }
}
