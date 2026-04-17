using System.Text.Json;
using SemiologyAtlas.Models;

namespace SemiologyAtlas.Services;

public static class ClinicalAnimationLibrary
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static readonly SemaphoreSlim Gate = new(1, 1);
    private static IReadOnlyDictionary<string, ClinicalAnimationProfile>? profiles;

    public static async Task<ClinicalAnimationProfile> GetProfileAsync(string animationKey)
    {
        var allProfiles = await GetProfilesAsync();
        var normalizedKey = AnimationCatalog.NormalizeKey(animationKey);

        if (allProfiles.TryGetValue(normalizedKey, out var profile))
        {
            return profile;
        }

        return allProfiles["general_survey"];
    }

    private static async Task<IReadOnlyDictionary<string, ClinicalAnimationProfile>> GetProfilesAsync()
    {
        if (profiles is not null)
        {
            return profiles;
        }

        await Gate.WaitAsync();

        try
        {
            if (profiles is not null)
            {
                return profiles;
            }

            using var stream = await FileSystem.OpenAppPackageFileAsync(AnimationCatalog.LibraryAssetName);
            var library = await JsonSerializer.DeserializeAsync<ClinicalAnimationLibraryData>(stream, SerializerOptions)
                ?? new ClinicalAnimationLibraryData();

            profiles = library.Profiles
                .Where(profile => !string.IsNullOrWhiteSpace(profile.Key))
                .ToDictionary(
                    profile => profile.Key,
                    StringComparer.OrdinalIgnoreCase);

            if (!profiles.ContainsKey("general_survey"))
            {
                profiles = new Dictionary<string, ClinicalAnimationProfile>(profiles, StringComparer.OrdinalIgnoreCase)
                {
                    ["general_survey"] = CreateFallbackProfile()
                };
            }

            return profiles;
        }
        finally
        {
            Gate.Release();
        }
    }

    private static ClinicalAnimationProfile CreateFallbackProfile()
    {
        return new ClinicalAnimationProfile
        {
            Key = "general_survey",
            Skeletons =
            [
                new ClinicalAnimationSkeleton
                {
                    Id = "patient",
                    Stroke = "#173F5F",
                    Fill = "#173F5F",
                    StrokeWidth = 6,
                    JointRadius = 4,
                    Joints = new Dictionary<string, float[]>
                    {
                        ["neck"] = [160, 58],
                        ["shoulder_left"] = [130, 78],
                        ["elbow_left"] = [118, 114],
                        ["wrist_left"] = [110, 150],
                        ["sternum"] = [160, 90],
                        ["abdomen"] = [160, 124],
                        ["shoulder_right"] = [190, 78],
                        ["elbow_right"] = [202, 114],
                        ["wrist_right"] = [210, 150]
                    },
                    Bones =
                    [
                        ["neck", "shoulder_left"],
                        ["shoulder_left", "elbow_left"],
                        ["elbow_left", "wrist_left"],
                        ["neck", "sternum"],
                        ["sternum", "abdomen"],
                        ["neck", "shoulder_right"],
                        ["shoulder_right", "elbow_right"],
                        ["elbow_right", "wrist_right"]
                    ]
                }
            ],
            Figures =
            [
                new ClinicalAnimationFigure
                {
                    Id = "head",
                    Kind = "circle",
                    Center = [160, 34],
                    Radius = 17,
                    Stroke = "#173F5F",
                    Fill = "#F8E3C6",
                    StrokeWidth = 4
                }
            ],
            Motions =
            [
                new ClinicalAnimationMotion
                {
                    Type = "oscillation",
                    Targets = ["patient:sternum", "patient:abdomen"],
                    Axis = "y",
                    Amplitude = 2.5f,
                    Frequency = 0.24f
                },
                new ClinicalAnimationMotion
                {
                    Type = "oscillation",
                    Targets =
                    [
                        "patient:shoulder_left",
                        "patient:elbow_left",
                        "patient:wrist_left",
                        "patient:shoulder_right",
                        "patient:elbow_right",
                        "patient:wrist_right"
                    ],
                    Axis = "x",
                    Amplitude = 2.5f,
                    Frequency = 0.18f,
                    Phase = 90
                }
            ]
        };
    }
}
