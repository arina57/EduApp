using System;
using static SharedActivities.Core.GlobalEnums;

namespace SharedActivities.Core.CrossPlatformInterfaces {
    public interface IDiscreteProgressTracker {
        event EventHandler QuizReset;
        event EventHandler ProgressChanged;
        int CurrentQuestionNumber { get; }
        int TotalNumberOfQuestions { get; }
        ProgressState AnswerProgress(int questionNumber);
        bool Finished { get; }
    }

    public class ScoreChangedEventArgs : EventArgs {
        public bool Correct { get; }

        public ScoreChangedEventArgs(bool correct) {
            Correct = correct;
        }
    }
}
