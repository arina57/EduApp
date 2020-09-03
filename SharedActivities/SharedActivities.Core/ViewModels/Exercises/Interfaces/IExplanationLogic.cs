using System;
using SharedActivities.Core.ViewModels.Exercises.Results;

namespace SharedActivities.Core.ViewModels.Exercises.Interfaces {
    public interface IExplanationLogic {
        string GetEnteredAnswerText(int questionNumber);
        bool QuestionAnsweredCorrectly(int questionNumber);
        string GetExplainationFor(int questionNumber);
        int TotalNumberOfQuestions { get; }
        int GetAnswerOptionCount(int questionNumber);
        bool GetAnswerOptionCorrect(int questionNumber, int answerNumber);
        string GetAnswerOptionText(int questionNumber, int answerNumber);
        string TitleText { get; }
        string SituationText { get; }
        string ScoreText { get; }
        ScoringViewModel ScoringViewModel { get; }
    }
}
