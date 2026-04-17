namespace SemiologyAtlas;

public partial class App : Application
{
	private Window? window;

	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		try
		{
			window = new Window(new AppShell());
		}
		catch (Exception exception)
		{
			window = new Window(CreateStartupErrorPage(exception));
		}

		return window;
	}

	public void ReloadShell()
	{
		if (window is not null)
		{
			window.Page = new AppShell();
		}
	}

	private static Page CreateStartupErrorPage(Exception exception)
	{
		var message = "The app failed to start.";

#if DEBUG
		message = exception.ToString();
#endif

		return new ContentPage
		{
			Title = "Startup error",
			Content = new ScrollView
			{
				Content = new VerticalStackLayout
				{
					Padding = new Thickness(24),
					Spacing = 12,
					Children =
					{
						new Label
						{
							Text = "Startup error",
							FontSize = 24,
							FontAttributes = FontAttributes.Bold
						},
						new Label
						{
							Text = message
						}
					}
				}
			}
		};
	}
}
