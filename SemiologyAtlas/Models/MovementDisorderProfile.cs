using SemiologyAtlas.Services;

namespace SemiologyAtlas.Models;

public sealed class MovementDisorderProfile
{
    public MovementDisorderProfile(
        string key,
        string family,
        string name,
        string hallmark,
        string whatYouSee,
        string bedsideTask,
        string distinguishingPearl,
        string teachingPrompt,
        string animationKey,
        IReadOnlyList<ClinicalPoint> comparisonPoints,
        IReadOnlyList<string> examChecklist,
        IReadOnlyList<string> pitfalls)
    {
        Key = key;
        Family = family;
        Name = name;
        Hallmark = hallmark;
        WhatYouSee = whatYouSee;
        BedsideTask = bedsideTask;
        DistinguishingPearl = distinguishingPearl;
        TeachingPrompt = teachingPrompt;
        AnimationKey = animationKey;
        ComparisonPoints = comparisonPoints;
        ExamChecklist = examChecklist;
        Pitfalls = pitfalls;
    }

    public string Key { get; }

    public string Family { get; }

    public string Name { get; }

    public string Hallmark { get; }

    public string WhatYouSee { get; }

    public string BedsideTask { get; }

    public string DistinguishingPearl { get; }

    public string TeachingPrompt { get; }

    public string AnimationKey { get; }

    public IReadOnlyList<ClinicalPoint> ComparisonPoints { get; }

    public IReadOnlyList<string> ExamChecklist { get; }

    public IReadOnlyList<string> Pitfalls { get; }

    public string FocusActionText => LocalizationService.Translate("Enfocar este signo", "Focus on this sign");
}
