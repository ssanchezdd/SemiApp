using SemiologyAtlas.Services;

namespace SemiologyAtlas.Models;

public sealed class LearningPath
{
    public LearningPath(string title, string subtitle, string workload, string route)
    {
        Title = title;
        Subtitle = subtitle;
        Workload = workload;
        Route = route;
    }

    public string Title { get; }

    public string Subtitle { get; }

    public string Workload { get; }

    public string Route { get; }

    public string ActionText => LocalizationService.Translate("Abrir modulo", "Open module");
}
