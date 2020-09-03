using System;
using System.Collections.Generic;
using SharedActivities.Core.Models;

namespace SharedActivities.Core.ViewModels.Exercises.Interfaces {
    public interface IOptionQuizExercise : IActivityDataModel {
        TranslatedText Situation { get; }
        int QuestionAnswerSetCount { get; }
        IQuestionAnswerSet GetQuestionAnswerSet(int index);
        bool IsOrdered { get; }
    }

    public interface IQuestionAnswerSet {
        string Question { get; }
        List<AnswerOption> AnswerOptions { get; }
        TranslatedText Explaination { get; }
    }
}
