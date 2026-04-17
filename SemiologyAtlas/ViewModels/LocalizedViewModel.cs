using SemiologyAtlas.Services;

namespace SemiologyAtlas.ViewModels;

public abstract class LocalizedViewModel : BaseViewModel
{
    protected static string T(string spanish, string english)
    {
        return LocalizationService.Translate(spanish, english);
    }

    public string LanguageStatusText => T("Idioma: Espanol", "Language: English");

    public string LanguageButtonText => T("Cambiar a ingles", "Switch to Spanish");
}
