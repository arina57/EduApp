using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CrossLibrary;
using SharedActivities.Core.Models;
using SharedActivities.Core.Models.PhraseMatchingPoolModel;
using SharedActivities.Core.ViewModels.Exercises.Results;
using static CrossLibrary.MathAndGeometry;

namespace SharedActivities.Core.ViewModels.Exercises {
    public class WordWebViewModel : ExerciseViewModel {
        FPoint[][] mainPhraseLinePoint;
        FPoint[] matchPhraseLinePoint;
        int[][] positionedAnswer;
        private int draggedMatchItemId;
        private int draggedMainItemId;
        private int draggedMainItemPostion;
        private List<int>[] matchPhraseIndexes;
        protected List<string> matchPhraseStrings;
        public int LineWidth { get; set; } = 6;

        public override bool UseFinishButton => true;
        public bool DragStartedFromMainPhrase { get; private set; }
        public FPoint LineStart { get; private set; }
        public Color LineColor { get; private set; }
        public int MainPhraseCount => phraseMatchingPoolExercise.PhraseSets.Count();
        public Color MainPhraseColor(int position) => mainPhraseColors[position];
        public int PossibleMatches(int mainPhraseIndex) => matchPhraseIndexes[mainPhraseIndex].Count;
        private List<Color> mainPhraseColors;
        public int MatchCount => phraseMatchingPoolExercise.PhraseSets.Sum(phraseSet => phraseSet.MatchingPhrases.Count);

        public override string TitleText => string.Empty;

        public override string SubtitleText => ActivityData.ActivityDescription;
        public override string SituationText => string.Empty;

        public override int NumberOfCorrectAnswers => CheckCorrectAnswerCount();
        public string RippleImageJson { get; } = Resx.Lottie.circle_animation;

        public override int TotalNumberOfQuestions => MainPhraseCount;



        PhraseMatchExercise phraseMatchingPoolExercise;
        private int[] shuffledMainPhraseIndexes;
        private List<int> shuffledMatchIndexes;



        public WordWebViewModel(PhraseMatchExercise phraseMatchingPoolExercise) : base(phraseMatchingPoolExercise) {
            this.phraseMatchingPoolExercise = phraseMatchingPoolExercise;
            ScoringViewModel = new ScoringViewModel(phraseMatchingPoolExercise.ActivityData, this, true);
            ResultsViewModel = new WordWebResultsViewModel(this);
            mainPhraseLinePoint = new FPoint[MainPhraseCount][];
            positionedAnswer = new int[MainPhraseCount][];
            //mainPhraseColors = CommonFunctions.GetGradient(GlobalColorPalette.VeryDark, Color.Yellow, MainPhraseCount);
            mainPhraseColors = Enumerable.Range(0, MainPhraseCount).Select(number => ColorHelper.ColorFromHsla((number + 1) / (float)MainPhraseCount, 0.5f, 0.5f, 1)).ToList();


            matchPhraseStrings = phraseMatchingPoolExercise.AllMatchingPhrases.ToList();
            matchPhraseIndexes = GetMatchPhraseIndexes();
            for (int x = 0; x < mainPhraseLinePoint.Length; x++) {
                mainPhraseLinePoint[x] = new FPoint[PossibleMatches(x)];
                positionedAnswer[x] = new int[PossibleMatches(x)];
                for (int y = 0; y < mainPhraseLinePoint[x].Length; y++) {
                    mainPhraseLinePoint[x][y] = new FPoint(-1, -1);
                    positionedAnswer[x][y] = -1;
                }
            }
            matchPhraseLinePoint = new FPoint[MatchCount];
            Reset();
        }






        public void DragFromMainPhraseStarted(int mainPhrase, int position, FPoint mainPhrasePoint) {
            SetLinePositionForMainPhrase(mainPhrase, position, mainPhrasePoint);
            DragFromMainPhraseStarted(mainPhrase, position);
        }

        public void DragFromMatchPhraseStarted(int match, FPoint matchPoint) {
            SetLinePositionForMatch(match, matchPoint);
            DragFromMatchPhraseStarted(match);
        }

        public void DroppedInMatchPhrase(int match, FPoint matchPoint) {
            SetAnswer(draggedMainItemId, draggedMainItemPostion, match);
            SetLinePositionForMatch(match, matchPoint);
        }

        public void DropppedInMainPhrase(int mainPhrase, int position, FPoint mainPhrasePoint) {
            SetAnswer(mainPhrase, position, draggedMatchItemId);
            SetLinePositionForMainPhrase(mainPhrase, position, mainPhrasePoint);
        }


        public void DragFromMainPhraseStarted(int mainPhrase, int position) {
            var currentAnswer = positionedAnswer[mainPhrase][position];
            RemoveMainPhrase(mainPhrase, position);
            //if (currentAnswer == -1) {
            draggedMainItemId = mainPhrase;
            draggedMainItemPostion = position;
            DragStartedFromMainPhrase = true;
            LineStart = mainPhraseLinePoint[mainPhrase][position];
            LineColor = MainPhraseColor(mainPhrase);
            
        }




        public void DragFromMatchPhraseStarted(int match) {
            var currectAnswerPos = FindAnswer(match);
            RemoveAnswer(match);
            draggedMatchItemId = match;
            DragStartedFromMainPhrase = false;
            LineStart = matchPhraseLinePoint[match];
            LineColor = Color.Black;
  
        }




        public void DroppedInMatchPhrase(int match) {
            SetAnswer(draggedMainItemId, draggedMainItemPostion, match);
        }

        public void DropppedInMainPhrase(int mainPhrase, int position) {
            SetAnswer(mainPhrase, position, draggedMatchItemId);
        }



        public void SetLinePositionForMatch(int position, FPoint point) {
            matchPhraseLinePoint[position] = point;
        }

        public void SetLinePositionForMainPhrase(int mainPhrase, int position, FPoint point) {
            mainPhraseLinePoint[mainPhrase][position] = point;
        }

        public int GetAnswerCount(int mainIndex) => positionedAnswer[mainIndex].Count(answer => answer != -1);

        public string GetMatchPhrase(int match) {
            return matchPhraseStrings[shuffledMatchIndexes[match]];
        }



        public string GetMainPhrase(int position) {
            return phraseMatchingPoolExercise.PhraseSets[shuffledMainPhraseIndexes[position]].MainPhrase;
        }

        public void SetAnswer(int mainPhraseId, int position, int matchId) {
            RemoveAnswer(matchId);
            if (PossibleMatches(mainPhraseId) > GetAnswerCount(mainPhraseId)) {
                positionedAnswer[mainPhraseId][position] = matchId;
            }
        }

        private int CheckCorrectAnswerCount() {
            var score = 0;
            for (int mainphraseIndex = 0; mainphraseIndex < positionedAnswer.Length; mainphraseIndex++) {
                var correct = QuestionAnsweredCorrectly(mainphraseIndex);
                score += correct ? 1 : 0;
            }
            return score;
        }

        public void RemoveAnswer(int matchIndex) {
            for (int x = 0; x < positionedAnswer.Length; x++) {
                for (int y = 0; y < positionedAnswer[x].Length; y++) {
                    if (positionedAnswer[x][y] == matchIndex) {
                        positionedAnswer[x][y] = -1;
                        return;
                    }
                }
            }
        }

        public void ClearAnswers() {
            for (int x = 0; x < positionedAnswer.Length; x++) {
                for (int y = 0; y < positionedAnswer[x].Length; y++) {
                    positionedAnswer[x][y] = -1;
                }
            }
        }

        protected int[] FindAnswer(int matchIndex) {
            for (int x = 0; x < positionedAnswer.Length; x++) {
                for (int y = 0; y < positionedAnswer[x].Length; y++) {
                    if (positionedAnswer[x][y] == matchIndex) {
                        positionedAnswer[x][y] = -1;
                        return new int[] { x, y };
                    }
                }
            }
            return null;
        }

        public void RemoveMainPhrase(int mainPhraseId, int position) {
            positionedAnswer[mainPhraseId][position] = -1;
        }

        public Color GetMatchColor(int match) {
            for (int x = 0; x < positionedAnswer.Length; x++) {
                for (int y = 0; y < positionedAnswer[x].Length; y++) {
                    if (positionedAnswer[x][y] == match) {
                        return MainPhraseColor(x);
                    }
                }
            }
            return Color.Gray;
        }

        public List<ColoredLine> GetLinesForAnswers() {
            var lines = new List<ColoredLine>();
            for (int x = 0; x < positionedAnswer.Length; x++) {
                for (int y = 0; y < positionedAnswer[x].Length; y++) {
                    if (positionedAnswer[x][y] > -1) {
                        var lineStart = mainPhraseLinePoint[x][y];
                        var lineEnd = matchPhraseLinePoint[positionedAnswer[x][y]];
                        lines.Add(new ColoredLine(lineStart, lineEnd, MainPhraseColor(x), LineWidth));
                    }
                }
            }
            return lines;
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


        public override void Reset() {
            base.Reset();
            shuffledMainPhraseIndexes = Enumerable.Range(0, MainPhraseCount).ToArray();
            if (!phraseMatchingPoolExercise.IsOrdered) {
                shuffledMainPhraseIndexes.Shuffle();
            }
            shuffledMatchIndexes = Enumerable.Range(0, MatchCount).ToList();
            shuffledMatchIndexes.Shuffle();
            ClearAnswers();
        }

        public override bool QuestionAnsweredCorrectly(int questionNumber) {
            bool containsAllMatches = true;
            for (int matchIndex = 0; matchIndex < positionedAnswer[questionNumber].Length && containsAllMatches; matchIndex++) {
                containsAllMatches &= positionedAnswer[questionNumber][matchIndex] != -1 && phraseMatchingPoolExercise.PhraseSets[shuffledMainPhraseIndexes[questionNumber]].MatchingPhrases.Contains(GetMatchPhrase(positionedAnswer[questionNumber][matchIndex]));
            }
            return containsAllMatches;
        }
    }
}
