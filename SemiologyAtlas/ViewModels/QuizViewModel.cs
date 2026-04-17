using System.Collections.ObjectModel;
using Microsoft.Maui.Graphics;
using SemiologyAtlas.Models;
using SemiologyAtlas.Services;

namespace SemiologyAtlas.ViewModels;

public sealed class QuizViewModel : LocalizedViewModel
{
    private static readonly Color SuccessFill = Color.FromArgb("#DDF4EA");
    private static readonly Color SuccessBorder = Color.FromArgb("#1F7A5A");
    private static readonly Color DangerFill = Color.FromArgb("#FCE4DE");
    private static readonly Color DangerBorder = Color.FromArgb("#A33A2B");
    private static readonly Color NeutralFill = Color.FromArgb("#FFF9F1");
    private static readonly Color NeutralBorder = Color.FromArgb("#D7E0E7");
    private static readonly Color Ink = Color.FromArgb("#0E1B2A");

    private readonly IReadOnlyList<QuizQuestion> questions = CurriculumRepository.GetQuizQuestions();
    private QuizQuestion? currentQuestion;
    private string feedbackTitle = string.Empty;
    private string feedbackText = string.Empty;
    private bool hasFeedback;
    private bool showNextButton;
    private bool isQuizComplete;
    private int currentIndex;
    private int score;

    public QuizViewModel()
    {
        Options = new ObservableCollection<QuizOptionItem>();
        ReviewCards = CurriculumRepository.GetStudyGuideCards();
        LoadQuestion(0);
    }

    public ObservableCollection<QuizOptionItem> Options { get; }

    public IReadOnlyList<StudyGuideCard> ReviewCards { get; }

    public QuizQuestion? CurrentQuestion
    {
        get => currentQuestion;
        private set
        {
            if (SetProperty(ref currentQuestion, value))
            {
                OnPropertyChanged(nameof(ProgressLabel));
            }
        }
    }

    public string FeedbackTitle
    {
        get => feedbackTitle;
        private set => SetProperty(ref feedbackTitle, value);
    }

    public string FeedbackText
    {
        get => feedbackText;
        private set => SetProperty(ref feedbackText, value);
    }

    public bool HasFeedback
    {
        get => hasFeedback;
        private set => SetProperty(ref hasFeedback, value);
    }

    public bool ShowNextButton
    {
        get => showNextButton;
        private set => SetProperty(ref showNextButton, value);
    }

    public bool IsQuizComplete
    {
        get => isQuizComplete;
        private set
        {
            if (SetProperty(ref isQuizComplete, value))
            {
                OnPropertyChanged(nameof(ScoreSummary));
            }
        }
    }

    public string KnowledgeCheckLabel => T("Chequeo de conocimiento", "Knowledge check");

    public string PageTitle => T("Quiz rapido de semiologia", "Semiology rapid quiz");

    public string HeroDescription => T(
        "Usa el quiz despues del estudio para confirmar reconocimiento de patrones y diferenciales semiologicos.",
        "Use the quiz after study to confirm pattern recognition and semiologic differentials.");

    public string ReviewDeckTitle => T("Repaso de alto rendimiento", "High-yield review");

    public string ReviewCardButtonText => T("Abrir tema", "Open topic");

    public string NextQuestionText => T("Siguiente pregunta", "Next question");

    public string RestartQuizText => T("Reiniciar quiz", "Restart quiz");

    public double Progress => questions.Count == 0 ? 0 : (double)(Math.Min(currentIndex + 1, questions.Count)) / questions.Count;

    public string ProgressLabel => CurrentQuestion is null
        ? T("No hay preguntas cargadas", "No questions loaded")
        : T(
            $"Pregunta {Math.Min(currentIndex + 1, questions.Count)} de {questions.Count}",
            $"Question {Math.Min(currentIndex + 1, questions.Count)} of {questions.Count}");

    public string ScoreSummary => IsQuizComplete
        ? T($"Puntaje final: {score} / {questions.Count}", $"Final score: {score} / {questions.Count}")
        : T($"Puntaje actual: {score} / {questions.Count}", $"Current score: {score} / {questions.Count}");

    public string CurrentAnimationKey => CurrentQuestion?.AnimationKey ?? "general_survey";

    public void SelectOption(QuizOptionItem option)
    {
        if (IsQuizComplete || ShowNextButton)
        {
            return;
        }

        foreach (var item in Options)
        {
            item.ResetAppearance();
            item.BorderColor = item.IsCorrect ? SuccessBorder : NeutralBorder;
        }

        if (option.IsCorrect)
        {
            option.BackgroundColor = SuccessFill;
            option.TextColor = Ink;
            option.BorderColor = SuccessBorder;
            score++;
            FeedbackTitle = T("Correcto", "Correct");
        }
        else
        {
            option.BackgroundColor = DangerFill;
            option.TextColor = Ink;
            option.BorderColor = DangerBorder;

            var correct = Options.First(item => item.IsCorrect);
            correct.BackgroundColor = SuccessFill;
            correct.TextColor = Ink;
            correct.BorderColor = SuccessBorder;
            FeedbackTitle = T("Revisa la distincion", "Review the distinction");
        }

        FeedbackText = option.Explanation;
        HasFeedback = true;
        ShowNextButton = true;
        OnPropertyChanged(nameof(ScoreSummary));
    }

    public void NextQuestion()
    {
        if (currentIndex >= questions.Count - 1)
        {
            IsQuizComplete = true;
            ShowNextButton = false;
            HasFeedback = true;
            FeedbackTitle = T("Quiz completado", "Quiz complete");
            FeedbackText = T(
                "Vuelve a correr el quiz despues de repasar las tarjetas para fijar las diferencias clave.",
                "Run the quiz again after reviewing the cards to reinforce the key distinctions.");
            return;
        }

        LoadQuestion(currentIndex + 1);
    }

    public void Restart()
    {
        score = 0;
        LoadQuestion(0);
    }

    private void LoadQuestion(int index)
    {
        currentIndex = index;
        CurrentQuestion = questions[index];
        ReplaceOptions(CurrentQuestion.Choices.Select(choice => new QuizOptionItem(choice)));
        FeedbackTitle = T("Elige una respuesta", "Pick an answer");
        FeedbackText = T(
            "Cada pregunta resalta una sola distincion semiologica de alto rendimiento.",
            "Each question highlights a single high-yield semiologic distinction.");
        HasFeedback = false;
        ShowNextButton = false;
        IsQuizComplete = false;
        OnPropertyChanged(nameof(Progress));
        OnPropertyChanged(nameof(ProgressLabel));
        OnPropertyChanged(nameof(ScoreSummary));
        OnPropertyChanged(nameof(CurrentAnimationKey));
    }

    private void ReplaceOptions(IEnumerable<QuizOptionItem> items)
    {
        Options.Clear();

        foreach (var item in items)
        {
            item.BackgroundColor = NeutralFill;
            item.BorderColor = NeutralBorder;
            item.TextColor = Ink;
            Options.Add(item);
        }
    }
}
