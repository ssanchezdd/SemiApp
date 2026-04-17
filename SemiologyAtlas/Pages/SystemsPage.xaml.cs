using SemiologyAtlas.Models;
using SemiologyAtlas.Services;
using SemiologyAtlas.ViewModels;

namespace SemiologyAtlas.Pages;

public partial class SystemsPage : ContentPage
{
    private readonly SystemsViewModel viewModel = new();

    public SystemsPage()
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

    private void OnReviewModuleClicked(object? sender, EventArgs e)
    {
        if (sender is Button { CommandParameter: SystemModule module })
        {
            viewModel.SelectModule(module);
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
