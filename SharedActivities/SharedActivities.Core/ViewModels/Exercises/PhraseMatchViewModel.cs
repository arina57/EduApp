using System;
using System.Collections.Generic;
using System.Linq;
using CrossLibrary;
using SharedActivities.Core.Models.PhraseMatchingPoolModel;
using SharedActivities.Core.ViewModels.Exercises.Results;

namespace SharedActivities.Core.ViewModels.Exercises {
    public class PhraseMatchViewModel : ExerciseViewModel {

        PhraseMatchExercise phraseMatchingPoolExercise;
        public PhraseMatchViewModel(PhraseMatchExercise phraseMatchingPoolExercise) : base(phraseMatchingPoolExercise) {
            this.phraseMatchingPoolExercise = phraseMatchingPoolExercise;
            matchPhraseIndexes = GetMatchPhraseIndexes();
            matchPhraseStrings = phraseMatchingPoolExercise.AllMatchingPhrases.ToList();
            MaxMatchesPerMainPhrase = phraseMatchingPoolExercise.MaxMatchesPerMainPhrase;
            ScoringViewModel = new ScoringViewModel(phraseMatchingPoolExercise.ActivityData, this, true);
            ResultsViewModel = new PhraseMatchResultsViewModel(this);
            Reset();
        }
        public override string TitleText => string.Empty;
        public override string SubtitleText => ActivityData.ActivityDescription;
        public override string SituationText => string.Empty;
        public int MaxMatchesPerMainPhrase { get; private set; }

        public int AvailableMatchCount => shuffledUnusedMatchIndexes.Count;
        protected int MatchCount => phraseMatchingPoolExercise.PhraseSets.Sum(phraseSet => phraseSet.MatchingPhrases.Count);
        public int MainPhraseCount => phraseMatchingPoolExercise.PhraseSets.Count();

        public int PossibleMatches(int mainPhraseIndex) => matchPhraseIndexes[mainPhraseIndex].Count;

        public override int NumberOfCorrectAnswers => CheckCorrectAnswerCount();

        public override int TotalNumberOfQuestions => MainPhraseCount;

        public string DoneText => Resx.String.CheckAnswersButton;

        public override bool UseFinishButton => true;

        protected List<string> matchPhraseStrings;
        private List<int> shuffledUnusedMatchIndexes;
        private int[] shuffledMainPhraseIndexes;
        private List<int>[] matchPhraseIndexes;
        protected List<int>[] answerList;




        public override void ViewAppearing() {
            base.ViewAppearing();

        }

        public override void ViewAppeared() {
            base.ViewAppeared();
            //RefreshUILocale();
        }

        private int GetMatchPhraseMainPhraseIndex(int matchIndex) {
            if (matchIndex < 0) {
                throw new IndexOutOfRangeException();
            }
            for (int i = 0; i < matchPhraseIndexes.Length; i++) {
                if (matchPhraseIndexes[i].Contains(matchIndex)) {
                    return i;
                }
            }
            throw new KeyNotFoundException();
        }




        /// <summary>
        /// Translates from flat list of matching phrases' indexes to array of mainphrase indexes with their matching phrase indexes
        /// </summary>
        /// <returns></returns>
        private List<int>[] GetMatchPhraseIndexes() {
            var matchIndex = 0;
            var indexes = new List<int>[MainPhraseCount];
            for (int i = 0; i < MainPhraseCount; i++) {
                if (phraseMatchingPoolExercise.PhraseSets[i].MatchingPhrases.Count > 0) {
                    indexes[i] = Enumerable.Range(matchIndex, phraseMatchingPoolExercise.PhraseSets[i].MatchingPhrases.Count).ToList();
                } else {
                    indexes[i] = new List<int>();
                }
                matchIndex += phraseMatchingPoolExercise.PhraseSets[i].MatchingPhrases.Count;
            }
            return indexes;
        }




        public bool[] CheckAnswerCorrect() {
            var correct = new bool[MainPhraseCount];
            for (int i = 0; i < MainPhraseCount; i++) {
                correct[i] = QuestionAnsweredCorrectly(i);
            }
            return correct;
        }

        public override bool QuestionAnsweredCorrectly(int questionNumber) =>
            matchPhraseIndexes[questionNumber].Count == answerList[questionNumber].Count
            && !matchPhraseIndexes[questionNumber].Except(answerList[questionNumber]).Any();

        private int CheckCorrectAnswerCount() {
            var correct = 0;
            for (int i = 0; i < MainPhraseCount; i++) {
                if (QuestionAnsweredCorrectly(i)) {
                    correct++;
                }
            }
            return correct;
        }


        public string GetUnusedMatchPhrase(int index) => matchPhraseStrings[shuffledUnusedMatchIndexes[index]];

        public int GetUnusedMatchId(int index) => shuffledUnusedMatchIndexes[index];


        public string GetMainPhrase(int mainIndex) {
            return phraseMatchingPoolExercise.PhraseSets[shuffledMainPhraseIndexes[mainIndex]].MainPhrase;
        }
        public int GetMainPhraseId(int mainIndex) => shuffledMainPhraseIndexes[mainIndex];

        public List<int> GetAnswer(int mainIndex) => GetAnswerMatchIndexes(shuffledMainPhraseIndexes[mainIndex]);

        public int GetAnswerId(int mainIndex, int matchIndex) => GetAnswerMatchIndexes(shuffledMainPhraseIndexes[mainIndex])[matchIndex];

        public int GetAnswerCount(int mainIndex) => GetAnswerMatchIndexes(shuffledMainPhraseIndexes[mainIndex]).Count;

        public string GetAnswerPhrase(int mainIndex, int matchIndex) => matchPhraseStrings[GetAnswerMatchIndexes(shuffledMainPhraseIndexes[mainIndex])[matchIndex]];

        private List<int> GetAnswerMatchIndexes(int mainIndex) => answerList[mainIndex];

        public override void Reset() {
            base.Reset();
            shuffledMainPhraseIndexes = Enumerable.Range(0, MainPhraseCount).ToArray();
            if (!phraseMatchingPoolExercise.IsOrdered) {
                shuffledMainPhraseIndexes.Shuffle();
            }
            shuffledUnusedMatchIndexes = Enumerable.Range(0, MatchCount).ToList();
            shuffledUnusedMatchIndexes.Shuffle();
            answerList = new List<int>[MainPhraseCount];
            for (int i = 0; i < answerList.Length; i++) {
                answerList[i] = new List<int>();
            }
        }


        public virtual void SetAnswer(int mainPhraseId, int matchId) {
            //if the answer is out of bounds, then remove it.
            if (mainPhraseId < 0 || mainPhraseId >= answerList.Length) {
                RemoveAnswer(matchId);
            } else {
                //Check if the number of answers is less than number of answer possible for that question
                if (answerList[mainPhraseId].Count < matchPhraseIndexes[mainPhraseId].Count) {
                    //Remove the match from it's previous position, if it has one
                    RemoveAnswer(matchId);
                    //Add it to it's new postion
                    answerList[mainPhraseId].Add(matchId);
                    //Remove it from the unsed matches, if it was one.
                    shuffledUnusedMatchIndexes.Remove(matchId);
                }
            }
        }



        public virtual void RemoveAnswer(int matchId) {
            foreach (List<int> answers in answerList) {
                answers.Remove(matchId);
            }
            shuffledUnusedMatchIndexes.Remove(matchId); // remove it first to make sure there arent dupes
            shuffledUnusedMatchIndexes.Add(matchId);
        }


    }
}
