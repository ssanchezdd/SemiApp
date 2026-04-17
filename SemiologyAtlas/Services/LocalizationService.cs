using Microsoft.Maui.Storage;

namespace SemiologyAtlas.Services;

public enum AppLanguage
{
    Spanish,
    English
}

public static class LocalizationService
{
    private const string LanguagePreferenceKey = "app_language";
    private static bool initialized;
    private static AppLanguage currentLanguage = AppLanguage.Spanish;

    public static AppLanguage CurrentLanguage
    {
        get
        {
            EnsureInitialized();
            return currentLanguage;
        }
        private set => currentLanguage = value;
    }

    public static bool IsSpanish => CurrentLanguage == AppLanguage.Spanish;

    public static string Translate(string spanish, string english)
    {
        return IsSpanish ? spanish : english;
    }

    public static void SetLanguage(AppLanguage language)
    {
        EnsureInitialized();

        if (CurrentLanguage == language)
        {
            return;
        }

        CurrentLanguage = language;

        try
        {
            Preferences.Default.Set(LanguagePreferenceKey, language.ToString());
        }
        catch
        {
            // Keep the in-memory language even if persistence fails on a device/emulator.
        }
    }

    private static void EnsureInitialized()
    {
        if (initialized)
        {
            return;
        }

        initialized = true;

        try
        {
            var stored = Preferences.Default.Get(LanguagePreferenceKey, nameof(AppLanguage.Spanish));
            CurrentLanguage = Enum.TryParse<AppLanguage>(stored, true, out var parsed)
                ? parsed
                : AppLanguage.Spanish;
        }
        catch
        {
            CurrentLanguage = AppLanguage.Spanish;
        }
    }
}
