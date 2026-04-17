using SemiologyAtlas.Services;

namespace SemiologyAtlas.Models;

public sealed class StudyGuideCard
{
    public StudyGuideCard(string title, string summary, string route, string animationKey)
    {
        Title = title;
        Summary = summary;
        Route = route;
        AnimationKey = animationKey;
    }

    public string Title { get; }

    public string Summary { get; }

    public string Route { get; }

    public string AnimationKey { get; }

    public string OpenActionText => LocalizationService.Translate("Abrir tema", "Open topic");
}
