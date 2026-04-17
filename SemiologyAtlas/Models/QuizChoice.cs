namespace SemiologyAtlas.Models;

public sealed class QuizChoice
{
    public QuizChoice(string text, bool isCorrect, string explanation)
    {
        Text = text;
        IsCorrect = isCorrect;
        Explanation = explanation;
    }

    public string Text { get; }

    public bool IsCorrect { get; }

    public string Explanation { get; }
}
