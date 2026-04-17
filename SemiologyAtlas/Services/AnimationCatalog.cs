namespace SemiologyAtlas.Services;

public static class AnimationCatalog
{
    public const string LibraryAssetName = "clinical_animation_library.json";

    public static string NormalizeKey(string animationKey)
    {
        if (string.IsNullOrWhiteSpace(animationKey))
        {
            return "general_survey";
        }

        return animationKey.Trim();
    }
}
