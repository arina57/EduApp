using System;
using SharedActivities.Core.Models;
using SharedActivities.Core.ViewModels.Exercises.Results;

namespace SharedActivities.Core.ViewModels.Exercises {
    public interface IExerciseLogic {
        event EventHandler ExerciseFinished;
        event EventHandler TextChanged;
        string TitleText { get; }
        string SubtitleText { get; }
        string SituationText { get; }
        int NumberOfCorrectAnswers { get; }
        int TotalNumberOfQuestions { get; }
        ScoringViewModel ScoringViewModel { get; }
        bool Finished { get; }
        IdentityModel ActivityData { get; }

        bool QuestionAnsweredCorrectly(int questionNumber);
    }
}
