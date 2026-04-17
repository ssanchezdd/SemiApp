namespace SemiologyAtlas.Models;

public sealed class ClinicalAnimationLibraryData
{
    public List<ClinicalAnimationProfile> Profiles { get; set; } = [];
}

public sealed class ClinicalAnimationProfile
{
    public string Key { get; set; } = string.Empty;

    public float Width { get; set; } = 320f;

    public float Height { get; set; } = 210f;

    public List<ClinicalAnimationSkeleton> Skeletons { get; set; } = [];

    public List<ClinicalAnimationFigure> Figures { get; set; } = [];

    public List<ClinicalAnimationMotion> Motions { get; set; } = [];
}

public sealed class ClinicalAnimationSkeleton
{
    public string Id { get; set; } = "primary";

    public string Stroke { get; set; } = "#173F5F";

    public string Fill { get; set; } = "#173F5F";

    public float StrokeWidth { get; set; } = 5f;

    public float JointRadius { get; set; } = 4f;

    public Dictionary<string, float[]> Joints { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    public List<string[]> Bones { get; set; } = [];
}

public sealed class ClinicalAnimationFigure
{
    public string Id { get; set; } = string.Empty;

    public string Kind { get; set; } = "circle";

    public string Stroke { get; set; } = "#173F5F";

    public string Fill { get; set; } = string.Empty;

    public float StrokeWidth { get; set; } = 3f;

    public float[]? Center { get; set; }

    public float[]? Start { get; set; }

    public float[]? End { get; set; }

    public List<float[]> Points { get; set; } = [];

    public float Radius { get; set; }

    public float Width { get; set; }

    public float Height { get; set; }

    public float Opacity { get; set; } = 1f;
}

public sealed class ClinicalAnimationMotion
{
    public string Type { get; set; } = "oscillation";

    public string Target { get; set; } = string.Empty;

    public List<string> Targets { get; set; } = [];

    public string Pivot { get; set; } = string.Empty;

    public string Axis { get; set; } = "y";

    public float Amplitude { get; set; }

    public float AmplitudeEnd { get; set; }

    public float AmplitudeX { get; set; }

    public float AmplitudeY { get; set; }

    public float Frequency { get; set; } = 1f;

    public float Phase { get; set; }

    public float Interval { get; set; }

    public float Duration { get; set; }

    public float Angle { get; set; }
}
