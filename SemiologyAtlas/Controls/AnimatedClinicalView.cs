using SemiologyAtlas.Services;

namespace SemiologyAtlas.Controls;

public sealed class AnimatedClinicalView : ContentView
{
    public static readonly BindableProperty AnimationKeyProperty =
        BindableProperty.Create(
            nameof(AnimationKey),
            typeof(string),
            typeof(AnimatedClinicalView),
            string.Empty,
            propertyChanged: OnAnimationKeyChanged);

    private readonly GraphicsView graphicsView;
    private readonly ClinicalAnimationDrawable drawable;
    private bool isLoopRunning;
    private long animationLoopToken;

    public AnimatedClinicalView()
    {
        drawable = new ClinicalAnimationDrawable();
        graphicsView = new GraphicsView
        {
            Drawable = drawable,
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill
        };

        Content = graphicsView;
    }

    public string AnimationKey
    {
        get => (string)GetValue(AnimationKeyProperty);
        set => SetValue(AnimationKeyProperty, value);
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (Handler is null)
        {
            StopAnimationLoop();
            return;
        }

        _ = RefreshAnimationAsync();
    }

    protected override void OnParentSet()
    {
        base.OnParentSet();

        if (Parent is null)
        {
            StopAnimationLoop();
            return;
        }

        _ = RefreshAnimationAsync();
    }

    private static void OnAnimationKeyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AnimatedClinicalView view)
        {
            _ = view.RefreshAnimationAsync();
        }
    }

    private async Task RefreshAnimationAsync()
    {
        if (Handler is null || Parent is null)
        {
            return;
        }

        var profile = await ClinicalAnimationLibrary.GetProfileAsync(AnimationCatalog.NormalizeKey(AnimationKey));
        drawable.SetProfile(profile);
        graphicsView.Invalidate();
        StartAnimationLoop();
    }

    private void StartAnimationLoop()
    {
        if (isLoopRunning || Dispatcher is null)
        {
            return;
        }

        isLoopRunning = true;
        var loopToken = ++animationLoopToken;

        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(33), () =>
        {
            if (!isLoopRunning || loopToken != animationLoopToken || Parent is null || Handler is null)
            {
                isLoopRunning = false;
                return false;
            }

            graphicsView.Invalidate();
            return true;
        });
    }

    private void StopAnimationLoop()
    {
        isLoopRunning = false;
        animationLoopToken++;
    }
}
