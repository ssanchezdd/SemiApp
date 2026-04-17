using System.Diagnostics;
using Microsoft.Maui.Graphics;
using SemiologyAtlas.Models;

namespace SemiologyAtlas.Controls;

public sealed class ClinicalAnimationDrawable : IDrawable
{
    private const float TwoPi = MathF.PI * 2f;
    private static readonly Color DefaultStroke = Color.FromArgb("#173F5F");
    private static readonly Color DefaultFill = Color.FromArgb("#F8E3C6");

    private readonly Stopwatch stopwatch = Stopwatch.StartNew();
    private ClinicalAnimationProfile? profile;

    public void SetProfile(ClinicalAnimationProfile? nextProfile)
    {
        profile = nextProfile;
        stopwatch.Restart();
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if (profile is null || dirtyRect.Width <= 0 || dirtyRect.Height <= 0)
        {
            return;
        }

        var scale = MathF.Min(dirtyRect.Width / profile.Width, dirtyRect.Height / profile.Height);

        if (scale <= 0)
        {
            return;
        }

        var offsetX = dirtyRect.X + ((dirtyRect.Width - (profile.Width * scale)) / 2f);
        var offsetY = dirtyRect.Y + ((dirtyRect.Height - (profile.Height * scale)) / 2f);

        canvas.SaveState();
        canvas.Antialias = true;
        canvas.Translate(offsetX, offsetY);
        canvas.Scale(scale, scale);

        DrawProfile(canvas, profile, (float)stopwatch.Elapsed.TotalSeconds);

        canvas.RestoreState();
    }

    private static void DrawProfile(ICanvas canvas, ClinicalAnimationProfile animationProfile, float time)
    {
        var skeletonStates = BuildSkeletonStates(animationProfile);

        foreach (var motion in animationProfile.Motions)
        {
            ApplyMotion(skeletonStates, motion, time);
        }

        foreach (var figure in animationProfile.Figures)
        {
            DrawFigure(canvas, figure);
        }

        foreach (var skeleton in animationProfile.Skeletons)
        {
            if (!skeletonStates.TryGetValue(skeleton.Id, out var joints))
            {
                continue;
            }

            DrawSkeleton(canvas, skeleton, joints);
        }
    }

    private static Dictionary<string, Dictionary<string, PointF>> BuildSkeletonStates(ClinicalAnimationProfile animationProfile)
    {
        var state = new Dictionary<string, Dictionary<string, PointF>>(StringComparer.OrdinalIgnoreCase);

        foreach (var skeleton in animationProfile.Skeletons)
        {
            var joints = new Dictionary<string, PointF>(StringComparer.OrdinalIgnoreCase);

            foreach (var pair in skeleton.Joints)
            {
                if (pair.Value.Length >= 2)
                {
                    joints[pair.Key] = new PointF(pair.Value[0], pair.Value[1]);
                }
            }

            state[skeleton.Id] = joints;
        }

        return state;
    }

    private static void DrawSkeleton(ICanvas canvas, ClinicalAnimationSkeleton skeleton, IReadOnlyDictionary<string, PointF> joints)
    {
        canvas.StrokeColor = ParseColor(skeleton.Stroke, DefaultStroke);
        canvas.FillColor = ParseColor(skeleton.Fill, DefaultFill);
        canvas.StrokeSize = skeleton.StrokeWidth;
        canvas.StrokeLineCap = LineCap.Round;
        canvas.StrokeLineJoin = LineJoin.Round;

        foreach (var bone in skeleton.Bones)
        {
            if (bone.Length < 2 ||
                !joints.TryGetValue(bone[0], out var start) ||
                !joints.TryGetValue(bone[1], out var end))
            {
                continue;
            }

            canvas.DrawLine(start.X, start.Y, end.X, end.Y);
        }

        if (skeleton.JointRadius <= 0)
        {
            return;
        }

        foreach (var joint in joints.Values)
        {
            canvas.FillCircle(joint.X, joint.Y, skeleton.JointRadius);
        }
    }

    private static void DrawFigure(ICanvas canvas, ClinicalAnimationFigure figure)
    {
        var stroke = ParseColor(figure.Stroke, DefaultStroke);
        var fill = ParseColor(figure.Fill, DefaultFill);
        var opacity = figure.Opacity <= 0 ? 1f : figure.Opacity;

        canvas.StrokeColor = ApplyOpacity(stroke, opacity);
        canvas.FillColor = ApplyOpacity(fill, opacity);
        canvas.StrokeSize = figure.StrokeWidth;
        canvas.StrokeLineCap = LineCap.Round;
        canvas.StrokeLineJoin = LineJoin.Round;

        switch (figure.Kind.ToLowerInvariant())
        {
            case "circle":
                if (figure.Center?.Length >= 2 && figure.Radius > 0)
                {
                    if (!string.IsNullOrWhiteSpace(figure.Fill))
                    {
                        canvas.FillCircle(figure.Center[0], figure.Center[1], figure.Radius);
                    }

                    canvas.DrawCircle(figure.Center[0], figure.Center[1], figure.Radius);
                }

                break;

            case "ellipse":
                if (figure.Center?.Length >= 2 && figure.Width > 0 && figure.Height > 0)
                {
                    var rect = new RectF(
                        figure.Center[0] - (figure.Width / 2f),
                        figure.Center[1] - (figure.Height / 2f),
                        figure.Width,
                        figure.Height);

                    if (!string.IsNullOrWhiteSpace(figure.Fill))
                    {
                        canvas.FillEllipse(rect);
                    }

                    canvas.DrawEllipse(rect);
                }

                break;

            case "line":
                if (figure.Start?.Length >= 2 && figure.End?.Length >= 2)
                {
                    canvas.DrawLine(figure.Start[0], figure.Start[1], figure.End[0], figure.End[1]);
                }

                break;

            case "polyline":
                if (figure.Points.Count >= 2)
                {
                    var path = new PathF();
                    path.MoveTo(figure.Points[0][0], figure.Points[0][1]);

                    for (var index = 1; index < figure.Points.Count; index++)
                    {
                        var point = figure.Points[index];

                        if (point.Length >= 2)
                        {
                            path.LineTo(point[0], point[1]);
                        }
                    }

                    canvas.DrawPath(path);
                }

                break;
        }
    }

    private static void ApplyMotion(
        IReadOnlyDictionary<string, Dictionary<string, PointF>> skeletonStates,
        ClinicalAnimationMotion motion,
        float time)
    {
        if (skeletonStates.Count == 0)
        {
            return;
        }

        switch (motion.Type.ToLowerInvariant())
        {
            case "oscillation":
                ApplyOffsetMotion(skeletonStates, motion, ResolveOscillation(motion, time));
                break;

            case "tap_decay":
                ApplyOffsetMotion(skeletonStates, motion, ResolveTapDecay(motion, time));
                break;

            case "impulse":
                ApplyOffsetMotion(skeletonStates, motion, ResolveImpulse(motion, time, randomize: false));
                break;

            case "random_impulse":
                ApplyOffsetMotion(skeletonStates, motion, ResolveImpulse(motion, time, randomize: true));
                break;

            case "rotation":
                ApplyRotationMotion(skeletonStates, motion, time);
                break;
        }
    }

    private static PointF ResolveOscillation(ClinicalAnimationMotion motion, float time)
    {
        var theta = (TwoPi * motion.Frequency * time) + DegreesToRadians(motion.Phase);
        var sin = MathF.Sin(theta);
        var cos = MathF.Cos(theta);

        if (motion.Axis.Equals("circle", StringComparison.OrdinalIgnoreCase))
        {
            var radiusX = motion.AmplitudeX != 0 ? motion.AmplitudeX : motion.Amplitude;
            var radiusY = motion.AmplitudeY != 0 ? motion.AmplitudeY : motion.Amplitude;
            return new PointF(radiusX * cos, radiusY * sin);
        }

        var dx = 0f;
        var dy = 0f;

        if (motion.Axis.Equals("x", StringComparison.OrdinalIgnoreCase) || motion.AmplitudeX != 0)
        {
            dx = (motion.AmplitudeX != 0 ? motion.AmplitudeX : motion.Amplitude) * sin;
        }

        if (motion.Axis.Equals("y", StringComparison.OrdinalIgnoreCase) || motion.AmplitudeY != 0)
        {
            dy = (motion.AmplitudeY != 0 ? motion.AmplitudeY : motion.Amplitude) * sin;
        }

        return new PointF(dx, dy);
    }

    private static PointF ResolveTapDecay(ClinicalAnimationMotion motion, float time)
    {
        var theta = (TwoPi * motion.Frequency * time) + DegreesToRadians(motion.Phase);
        var tapWave = MathF.Abs(MathF.Sin(theta));
        var loopProgress = PositiveMod(time, 4f) / 4f;
        var amplitudeEnd = motion.AmplitudeEnd != 0 ? motion.AmplitudeEnd : motion.Amplitude * 0.35f;
        var currentAmplitude = Lerp(motion.Amplitude, amplitudeEnd, loopProgress);

        return motion.Axis.Equals("x", StringComparison.OrdinalIgnoreCase)
            ? new PointF(currentAmplitude * tapWave, 0)
            : new PointF(0, -currentAmplitude * tapWave);
    }

    private static PointF ResolveImpulse(ClinicalAnimationMotion motion, float time, bool randomize)
    {
        var interval = motion.Interval > 0 ? motion.Interval : 1f;
        var duration = motion.Duration > 0 ? motion.Duration : MathF.Min(0.16f, interval / 2f);
        var shifted = time + ((motion.Phase / 360f) * interval);
        var cycleTime = PositiveMod(shifted, interval);

        if (cycleTime > duration)
        {
            return new PointF(0, 0);
        }

        var envelope = MathF.Sin(MathF.PI * cycleTime / duration);
        var xAmplitude = motion.AmplitudeX != 0 ? motion.AmplitudeX : motion.Axis.Equals("x", StringComparison.OrdinalIgnoreCase) ? motion.Amplitude : motion.Amplitude * 0.75f;
        var yAmplitude = motion.AmplitudeY != 0 ? motion.AmplitudeY : motion.Axis.Equals("y", StringComparison.OrdinalIgnoreCase) ? motion.Amplitude : motion.Amplitude;

        if (!randomize)
        {
            return new PointF(xAmplitude * envelope, -yAmplitude * envelope);
        }

        var segment = (int)MathF.Floor(shifted / interval);
        var xDirection = HashSigned(segment, motion.Target, "x");
        var yDirection = HashSigned(segment, motion.Target, "y");

        return new PointF(xAmplitude * envelope * xDirection, yAmplitude * envelope * yDirection);
    }

    private static void ApplyOffsetMotion(
        IReadOnlyDictionary<string, Dictionary<string, PointF>> skeletonStates,
        ClinicalAnimationMotion motion,
        PointF offset)
    {
        foreach (var target in ResolveTargets(skeletonStates, motion))
        {
            var currentPoint = skeletonStates[target.SkeletonId][target.JointName];
            skeletonStates[target.SkeletonId][target.JointName] = new PointF(
                currentPoint.X + offset.X,
                currentPoint.Y + offset.Y);
        }
    }

    private static void ApplyRotationMotion(
        IReadOnlyDictionary<string, Dictionary<string, PointF>> skeletonStates,
        ClinicalAnimationMotion motion,
        float time)
    {
        if (!TryResolvePoint(skeletonStates, motion.Pivot, out var pivot))
        {
            return;
        }

        var angle = motion.Angle;

        if (motion.Frequency > 0)
        {
            angle += motion.Amplitude * MathF.Sin((TwoPi * motion.Frequency * time) + DegreesToRadians(motion.Phase));
        }

        var radians = DegreesToRadians(angle);

        foreach (var target in ResolveTargets(skeletonStates, motion))
        {
            var currentPoint = skeletonStates[target.SkeletonId][target.JointName];
            skeletonStates[target.SkeletonId][target.JointName] = Rotate(currentPoint, pivot, radians);
        }
    }

    private static IEnumerable<JointTarget> ResolveTargets(
        IReadOnlyDictionary<string, Dictionary<string, PointF>> skeletonStates,
        ClinicalAnimationMotion motion)
    {
        if (!string.IsNullOrWhiteSpace(motion.Target) &&
            TryResolveTarget(skeletonStates, motion.Target, out var singleTarget))
        {
            yield return singleTarget;
        }

        foreach (var target in motion.Targets)
        {
            if (TryResolveTarget(skeletonStates, target, out var resolvedTarget))
            {
                yield return resolvedTarget;
            }
        }
    }

    private static bool TryResolveTarget(
        IReadOnlyDictionary<string, Dictionary<string, PointF>> skeletonStates,
        string? descriptor,
        out JointTarget target)
    {
        target = default;

        if (string.IsNullOrWhiteSpace(descriptor))
        {
            return false;
        }

        var parts = descriptor.Split(':', 2, StringSplitOptions.TrimEntries);

        if (parts.Length == 2 &&
            skeletonStates.TryGetValue(parts[0], out var joints) &&
            joints.ContainsKey(parts[1]))
        {
            target = new JointTarget(parts[0], parts[1]);
            return true;
        }

        foreach (var skeleton in skeletonStates)
        {
            if (skeleton.Value.ContainsKey(descriptor))
            {
                target = new JointTarget(skeleton.Key, descriptor);
                return true;
            }
        }

        return false;
    }

    private static bool TryResolvePoint(
        IReadOnlyDictionary<string, Dictionary<string, PointF>> skeletonStates,
        string? descriptor,
        out PointF point)
    {
        point = default;

        if (!TryResolveTarget(skeletonStates, descriptor, out var target))
        {
            return false;
        }

        point = skeletonStates[target.SkeletonId][target.JointName];
        return true;
    }

    private static PointF Rotate(PointF point, PointF pivot, float radians)
    {
        var translatedX = point.X - pivot.X;
        var translatedY = point.Y - pivot.Y;
        var cos = MathF.Cos(radians);
        var sin = MathF.Sin(radians);

        return new PointF(
            pivot.X + (translatedX * cos) - (translatedY * sin),
            pivot.Y + (translatedX * sin) + (translatedY * cos));
    }

    private static float DegreesToRadians(float degrees) => degrees * (MathF.PI / 180f);

    private static float PositiveMod(float value, float divisor)
    {
        if (divisor == 0)
        {
            return 0;
        }

        var remainder = value % divisor;
        return remainder < 0 ? remainder + divisor : remainder;
    }

    private static float Lerp(float start, float end, float progress) => start + ((end - start) * progress);

    private static float HashSigned(int segment, string seed, string axis)
    {
        var hash = HashCode.Combine(segment, seed, axis);
        return (hash & 1) == 0 ? -1f : 1f;
    }

    private static Color ParseColor(string? hex, Color fallback)
    {
        if (string.IsNullOrWhiteSpace(hex))
        {
            return fallback;
        }

        try
        {
            return Color.FromArgb(hex);
        }
        catch
        {
            return fallback;
        }
    }

    private static Color ApplyOpacity(Color color, float opacity)
    {
        return new Color(
            (float)color.Red,
            (float)color.Green,
            (float)color.Blue,
            (float)(color.Alpha * opacity));
    }

    private readonly record struct JointTarget(string SkeletonId, string JointName);
}
