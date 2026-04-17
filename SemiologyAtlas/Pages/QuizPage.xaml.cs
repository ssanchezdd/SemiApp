using SemiologyAtlas.Services;
using SemiologyAtlas.ViewModels;

namespace SemiologyAtlas.Pages;

public partial class QuizPage : ContentPage
{
    private readonly QuizViewModel viewModel = new();

    public QuizPage()
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void OnOptionClicked(object? sender, EventArgs e)
    {
        if (sender is Button { CommandParameter: QuizOptionItem option })
        {
            viewModel.SelectOption(option);
        }
    }

    private void OnNextQuestionClicked(object? sender, EventArgs e)
    {
        viewModel.NextQuestion();
    }

    private void OnRestartClicked(object? sender, EventArgs e)
    {
        viewModel.Restart();
    }

    private async void OnOpenReviewClicked(object? sender, EventArgs e)
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
