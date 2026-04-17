using System.Collections.ObjectModel;
using SemiologyAtlas.Models;
using SemiologyAtlas.Services;

namespace SemiologyAtlas.ViewModels;

public sealed class SystemsViewModel : LocalizedViewModel
{
    private readonly IReadOnlyList<SystemModule> allModules = CurriculumRepository.GetSystemModules();
    private string searchText = string.Empty;
    private FilterChip? selectedFilter;
    private SystemModule? selectedModule;
    private bool noResultsVisible;

    public SystemsViewModel()
    {
        Filters = new ObservableCollection<FilterChip>(
        [
            new FilterChip(T("Todos", "All"), T("Todos", "All"), true),
            new FilterChip(T("General", "General"), T("General", "General")),
            new FilterChip(T("Neurologico", "Neurological"), T("Neurologico", "Neurological")),
            new FilterChip(T("Cardiovascular", "Cardiovascular"), T("Cardiovascular", "Cardiovascular")),
            new FilterChip(T("Respiratorio", "Respiratory"), T("Respiratorio", "Respiratory")),
            new FilterChip(T("GI/Higado", "GI/Hepatic"), T("Gastrointestinal", "Gastrointestinal"))
        ]);

        selectedFilter = Filters[0];
        VisibleModules = new ObservableCollection<SystemModule>();
        ApplyFilters();
    }

    public ObservableCollection<FilterChip> Filters { get; }

    public ObservableCollection<SystemModule> VisibleModules { get; }

    public string SearchText
    {
        get => searchText;
        set
        {
            if (SetProperty(ref searchText, value))
            {
                ApplyFilters();
            }
        }
    }

    public SystemModule? SelectedModule
    {
        get => selectedModule;
        private set
        {
            if (SetProperty(ref selectedModule, value))
            {
                OnPropertyChanged(nameof(HasSelectedModule));
            }
        }
    }

    public bool HasSelectedModule => SelectedModule is not null;

    public bool NoResultsVisible
    {
        get => noResultsVisible;
        private set => SetProperty(ref noResultsVisible, value);
    }

    public string LibraryLabel => T("Biblioteca por sistemas", "Systems library");

    public string PageTitle => T("Semiologia general por sistemas", "General semiology by system");

    public string HeroDescription => T(
        "Busca los modulos y abre la ficha de detalle para revisar guion de exploracion, hallazgos clave, perlas de estudio y banderas rojas.",
        "Search the modules and open the detail card to review the exam script, key findings, study pearls, and red flags.");

    public string SearchPlaceholder => T("Buscar signos, sistemas o sindromes", "Search signs, systems, or syndromes");

    public string NoResultsText => T("No hay modulos que coincidan con esa busqueda.", "No modules match that search.");

    public string ReviewButtonText => T("Revisar claves de cabecera", "Review bedside cues");

    public string ClinicalQuestionTitle => T("Pregunta clinica", "Clinical question");

    public string ChecklistTitle => T("Checklist de exploracion", "Exam checklist");

    public string KeyFindingsTitle => T("Hallazgos clave", "Key findings");

    public string StudyPearlsTitle => T("Perlas de estudio", "Study pearls");

    public string RedFlagsTitle => T("Banderas rojas", "Red flags");

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

    public void SelectModule(SystemModule module)
    {
        SelectedModule = module;
    }

    private void ApplyFilters()
    {
        var normalizedSearch = searchText.Trim();

        var filtered = allModules
            .Where(module =>
                (selectedFilter?.Value == T("Todos", "All") || module.System == selectedFilter?.Value) &&
                (string.IsNullOrWhiteSpace(normalizedSearch) ||
                 module.Title.Contains(normalizedSearch, StringComparison.OrdinalIgnoreCase) ||
                 module.Summary.Contains(normalizedSearch, StringComparison.OrdinalIgnoreCase) ||
                 module.ExamChecklist.Any(item => item.Contains(normalizedSearch, StringComparison.OrdinalIgnoreCase)) ||
                 module.Findings.Any(point => point.Title.Contains(normalizedSearch, StringComparison.OrdinalIgnoreCase) ||
                                              point.Detail.Contains(normalizedSearch, StringComparison.OrdinalIgnoreCase)) ||
                 module.StudyPearls.Any(point => point.Title.Contains(normalizedSearch, StringComparison.OrdinalIgnoreCase) ||
                                                 point.Detail.Contains(normalizedSearch, StringComparison.OrdinalIgnoreCase))))
            .ToList();

        ReplaceItems(VisibleModules, filtered);

        NoResultsVisible = filtered.Count == 0;
        SelectedModule = filtered.FirstOrDefault();
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
