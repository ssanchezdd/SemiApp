namespace SemiologyAtlas.Models;

public sealed class QuizQuestion
{
    public QuizQuestion(
        string key,
        string category,
        string prompt,
        string bedsidePearl,
        string animationKey,
        string relatedRoute,
        IReadOnlyList<QuizChoice> choices)
    {
        Key = key;
        Category = category;
        Prompt = prompt;
        BedsidePearl = bedsidePearl;
        AnimationKey = animationKey;
        RelatedRoute = relatedRoute;
        Choices = choices;
    }

    public string Key { get; }

    public string Category { get; }

    public string Prompt { get; }

    public string BedsidePearl { get; }

    public string AnimationKey { get; }

    public string RelatedRoute { get; }

    public IReadOnlyList<QuizChoice> Choices { get; }
}
