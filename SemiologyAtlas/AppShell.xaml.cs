using SemiologyAtlas.Services;

namespace SemiologyAtlas;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		ApplyLanguage();
	}

	private void ApplyLanguage()
	{
		Title = LocalizationService.Translate("Atlas de semiologia", "Semiology Atlas");
		HomeTab.Title = LocalizationService.Translate("Inicio", "Atlas");
		SystemsTab.Title = LocalizationService.Translate("Sistemas", "Systems");
		MovementTab.Title = LocalizationService.Translate("Movimiento", "Movement");
		QuizTab.Title = LocalizationService.Translate("Quiz", "Quiz");
	}
}
