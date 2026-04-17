using SemiologyAtlas.Services;

namespace SemiologyAtlas.Models;

public sealed class SystemModule
{
    public SystemModule(
        string key,
        string system,
        string title,
        string summary,
        string examinationScript,
        string coreQuestion,
        string animationKey,
        IReadOnlyList<string> examChecklist,
        IReadOnlyList<ClinicalPoint> findings,
        IReadOnlyList<ClinicalPoint> studyPearls,
        IReadOnlyList<string> redFlags,
        IReadOnlyList<string> pitfalls)
    {
        Key = key;
        System = system;
        Title = title;
        Summary = summary;
        ExaminationScript = examinationScript;
        CoreQuestion = coreQuestion;
        AnimationKey = animationKey;
        ExamChecklist = examChecklist;
        Findings = findings;
        StudyPearls = studyPearls;
        RedFlags = redFlags;
        Pitfalls = pitfalls;
    }

    public string Key { get; }

    public string System { get; }

    public string Title { get; }

    public string Summary { get; }

    public string ExaminationScript { get; }

    public string CoreQuestion { get; }

    public string AnimationKey { get; }

    public IReadOnlyList<string> ExamChecklist { get; }

    public IReadOnlyList<ClinicalPoint> Findings { get; }

    public IReadOnlyList<ClinicalPoint> StudyPearls { get; }

    public IReadOnlyList<string> RedFlags { get; }

    public IReadOnlyList<string> Pitfalls { get; }

    public string TeachingLabel => LocalizationService.Translate("Como ensenarlo", "How to teach it");

    public string ReviewActionText => LocalizationService.Translate("Revisar claves de cabecera", "Review bedside cues");
}
