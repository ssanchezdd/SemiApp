using Microsoft.Maui.Graphics;
using SemiologyAtlas.Models;

namespace SemiologyAtlas.ViewModels;

public sealed class QuizOptionItem : BaseViewModel
{
    private Color backgroundColor = Color.FromArgb("#FFF9F1");
    private Color textColor = Color.FromArgb("#0E1B2A");
    private Color borderColor = Color.FromArgb("#D7E0E7");

    public QuizOptionItem(QuizChoice choice)
    {
        Choice = choice;
    }

    public QuizChoice Choice { get; }

    public string Text => Choice.Text;

    public bool IsCorrect => Choice.IsCorrect;

    public string Explanation => Choice.Explanation;

    public Color BackgroundColor
    {
        get => backgroundColor;
        set => SetProperty(ref backgroundColor, value);
    }

    public Color TextColor
    {
        get => textColor;
        set => SetProperty(ref textColor, value);
    }

    public Color BorderColor
    {
        get => borderColor;
        set => SetProperty(ref borderColor, value);
    }

    public void ResetAppearance()
    {
        BackgroundColor = Color.FromArgb("#FFF9F1");
        TextColor = Color.FromArgb("#0E1B2A");
        BorderColor = Color.FromArgb("#D7E0E7");
    }
}
