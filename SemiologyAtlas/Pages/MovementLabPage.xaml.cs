using SemiologyAtlas.Models;
using SemiologyAtlas.Services;
using SemiologyAtlas.ViewModels;

namespace SemiologyAtlas.Pages;

public partial class MovementLabPage : ContentPage
{
    private readonly MovementLabViewModel viewModel = new();

    public MovementLabPage()
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void OnFilterClicked(object? sender, EventArgs e)
    {
        if (sender is Button { CommandParameter: FilterChip chip })
        {
            viewModel.SelectFilter(chip);
        }
    }

    private void OnFocusProfileClicked(object? sender, EventArgs e)
    {
        if (sender is Button { CommandParameter: MovementDisorderProfile profile })
        {
            viewModel.SelectProfile(profile);
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
