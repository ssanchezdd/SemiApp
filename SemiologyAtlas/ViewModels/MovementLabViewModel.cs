using System.Collections.ObjectModel;
using SemiologyAtlas.Models;
using SemiologyAtlas.Services;

namespace SemiologyAtlas.ViewModels;

public sealed class MovementLabViewModel : LocalizedViewModel
{
    private readonly IReadOnlyList<MovementDisorderProfile> allProfiles = CurriculumRepository.GetMovementProfiles();
    private FilterChip? selectedFilter;
    private MovementDisorderProfile? selectedProfile;

    public MovementLabViewModel()
    {
        Filters = new ObservableCollection<FilterChip>(
        [
            new FilterChip(T("Todos", "All"), T("Todos", "All"), true),
            new FilterChip(T("Hipocinetico", "Hypokinetic"), T("Hipocinetico", "Hypokinetic")),
            new FilterChip(T("Hipercinetico", "Hyperkinetic"), T("Hipercinetico", "Hyperkinetic")),
            new FilterChip(T("Marcha", "Gait"), T("Marcha", "Gait"))
        ]);

        selectedFilter = Filters[0];
        VisibleProfiles = new ObservableCollection<MovementDisorderProfile>();
        ApplyFilters();
    }

    public ObservableCollection<FilterChip> Filters { get; }

    public ObservableCollection<MovementDisorderProfile> VisibleProfiles { get; }

    public MovementDisorderProfile? SelectedProfile
    {
        get => selectedProfile;
        private set
        {
            if (SetProperty(ref selectedProfile, value))
            {
                OnPropertyChanged(nameof(HasSelectedProfile));
            }
        }
    }

    public bool HasSelectedProfile => SelectedProfile is not null;

    public string NeurologicLabel => T("Sistema neurologico", "Neurological system");

    public string PageTitle => T("Laboratorio de trastornos del movimiento", "Movement disorders lab");

    public string HeroDescription => T(
        "Empieza decidiendo si el paciente se mueve poco, se mueve de mas o camina con un patron alterado. Luego compara el signo cardinal.",
        "Start by deciding whether the patient moves too little, too much, or walks with an altered pattern. Then compare the cardinal sign.");

    public string HypokineticTitle => T("Hipocinetico", "Hypokinetic");

    public string HypokineticBody => T(
        "Pobreza o lentitud de movimiento: bradicinesia, rigidez y temblor parkinsoniano.",
        "Paucity or slowness of movement: bradykinesia, rigidity, and parkinsonian tremor.");

    public string HyperkineticTitle => T("Hipercinetico", "Hyperkinetic");

    public string HyperkineticBody => T(
        "Exceso de movimiento involuntario: temblor esencial, corea, distonia y mioclonus.",
        "Excess involuntary movement: essential tremor, chorea, dystonia, and myoclonus.");

    public string FocusButtonText => T("Enfocar este signo", "Focus on this sign");

    public string BedsideTaskTitle => T("Maniobra de cabecera", "Bedside task");

    public string TeachingPearlTitle => T("Perla docente", "Teaching pearl");

    public string DistinguishTitle => T("Como distinguirlo de sus parecidos", "Distinguish it from look-alikes");

    public string ComparisonPointsTitle => T("Puntos de comparacion", "Comparison points");

    public string ChecklistTitle => T("Checklist de exploracion", "Exam checklist");

    public string PitfallsTitle => T("Errores frecuentes", "Common pitfalls");

    public void SelectFilter(FilterChip chip)
    {
        if (selectedFilter == chip)
        {
            return;
        }

        foreach (var filter in Filters)
        {
            filter.IsSelected = false;
        }

        chip.IsSelected = true;
        selectedFilter = chip;
        ApplyFilters();
    }

    public void SelectProfile(MovementDisorderProfile profile)
    {
        SelectedProfile = profile;
    }

    private void ApplyFilters()
    {
        var filtered = allProfiles
            .Where(profile => selectedFilter?.Value == T("Todos", "All") || profile.Family == selectedFilter?.Value)
            .ToList();

        ReplaceItems(VisibleProfiles, filtered);
        SelectedProfile = filtered.FirstOrDefault();
    }

    private static void ReplaceItems<T>(ObservableCollection<T> collection, IEnumerable<T> items)
    {
        collection.Clear();

        foreach (var item in items)
        {
            collection.Add(item);
        }
    }
}
