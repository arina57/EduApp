using System;
using System.Collections.Generic;
using System.Linq;
using SharedActivities.Core.Models.PhraseMatchingPoolModel;
using SharedActivities.Core.ViewModels.Exercises.Interfaces;

namespace SharedActivities.Core.Models {
    public class MatchQuizData : IOptionQuizExercise {
        public TranslatedText Situation { get; } = new TranslatedText() { English = string.Empty };

        public int QuestionAnswerSetCount => matchingPoolQuestionAnswerSets.Count;

        public bool IsOrdered => false;

        public IdentityModel ActivityData { get; }

        public IQuestionAnswerSet GetQuestionAnswerSet(int index) {
            return matchingPoolQuestionAnswerSets[index];
        }

        List<MatchingPoolQuestionAnswerSet> matchingPoolQuestionAnswerSets = new List<MatchingPoolQuestionAnswerSet>();

        public MatchQuizData(PhraseMatchExercise phraseMatchingPoolExercise, int answersPerQuestion) {
            ActivityData = phraseMatchingPoolExercise.ActivityData;
            var allAnswers = phraseMatchingPoolExercise.PhraseSets.SelectMany(phraseSet => phraseSet.MatchingPhrases).ToList();
            Random rando = new Random();
            foreach (PhraseSet phraseSet in phraseMatchingPoolExercise.PhraseSets) {
                foreach (string answer in phraseSet.MatchingPhrases) {
                    var matchingPoolQuestionAnswerSet = new MatchingPoolQuestionAnswerSet();
                    matchingPoolQuestionAnswerSet.Question = phraseSet.MainPhrase;
                    matchingPoolQuestionAnswerSet.AnswerOptions = new List<AnswerOption>();
                    matchingPoolQuestionAnswerSet.AnswerOptions.Add(new AnswerOption(answer, true));
                    while (matchingPoolQuestionAnswerSet.AnswerOptions.Count < answersPerQuestion) {
                        var newWrongAnswer = allAnswers[rando.Next(0, allAnswers.Count)];
                        if (!phraseSet.MatchingPhrases.Contains(newWrongAnswer) && !matchingPoolQuestionAnswerSet.AnswerOptions.Any(answerOption => answerOption.Text == newWrongAnswer)) {
                            matchingPoolQuestionAnswerSet.AnswerOptions.Add(new AnswerOption(newWrongAnswer, false));
                        }
                    }
                    matchingPoolQuestionAnswerSets.Add(matchingPoolQuestionAnswerSet);
                }
            }
        }

        public class MatchingPoolQuestionAnswerSet : IQuestionAnswerSet {

            public string Question { get; set; }

            public List<AnswerOption> AnswerOptions { get; set; }

            public TranslatedText Explaination { get; } = new TranslatedText() { English = string.Empty };

        }
    }
}
