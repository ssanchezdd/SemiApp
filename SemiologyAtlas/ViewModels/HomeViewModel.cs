using SemiologyAtlas.Models;
using SemiologyAtlas.Services;

namespace SemiologyAtlas.ViewModels;

public sealed class HomeViewModel : LocalizedViewModel
{
    public IReadOnlyList<LearningPath> LearningPaths { get; } = CurriculumRepository.GetLearningPaths();

    public IReadOnlyList<string> TeachingRules { get; } = CurriculumRepository.GetTeachingRules();

    public IReadOnlyList<SystemModule> SpotlightModules { get; } =
        CurriculumRepository.GetSystemModules()
            .Take(3)
            .ToArray();

    public int ModuleCount => CurriculumRepository.GetSystemModules().Count;

    public int MovementCount => CurriculumRepository.GetMovementProfiles().Count;

    public int QuizCount => CurriculumRepository.GetQuizQuestions().Count;

    public string DeckLabel => T("Guia docente offline", "Offline teaching deck");

    public string AppTitle => T("Atlas de semiologia", "Semiology Atlas");

    public string HeroDescription => T(
        "Ensena semiologia general, trastornos del movimiento y sindromes por sistemas con lenguaje clinico rapido y util.",
        "Teach general semiology, movement disorders, and system syndromes with fast and useful clinical language.");

    public string ModulesStatLabel => T("Modulos de sistemas", "System modules");

    public string MovementStatLabel => T("Patrones de movimiento", "Movement cards");

    public string QuizStatLabel => T("Items de quiz", "Quiz items");

    public string StartWithTitle => T("Empieza con", "Start with");

    public string OpenModuleText => T("Abrir modulo", "Open module");

    public string TeachingRulesTitle => T("Reglas de ensenanza", "Teaching rules");

    public string SpotlightTitle => T("Modulos destacados", "Spotlight modules");

    public string HowToTeachTitle => T("Como ensenarlo", "How to teach it");
}
