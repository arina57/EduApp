using System;
using System.Drawing;
using SharedActivities.Core;
using SharedActivities.Core.Models;
using System.Collections.Generic;
using System.Linq;
using SharedActivities.Core.ViewModels.Exercises.Results;
using CrossLibrary;
using SharedActivities.Core.Models.DialogueGapFillExercises;

namespace SharedActivities.Core.ViewModels.Exercises {
    public class GapFillViewModel : ExerciseViewModel {

        DialogueGapFillExercise dialogueGapFillExercise;

        public Color HoveredLinkColor { get; } = Color.FromArgb(127, GlobalColorPalette.VeryDark);
        public bool PhrasesShuffled => dialogueGapFillExercise == null;

        public override string TitleText => string.Empty;
        public override string SubtitleText => ActivityData.ActivityDescription;

        public override string SituationText => dialogueGapFillExercise == null ?
            string.Empty : dialogueGapFillExercise.Situation.GetString();

        public int RoleCount => dialogueGapFillExercise == null ? 0 : usedRoles.Count;

        public string GetRoleName(int position) => dialogueGapFillExercise == null ?
            string.Empty :
            usedRoles.First(role => role.Id == dialogueGapFillExercise.Lines[position].RoleId).Name;

        public int GetRoleNumber(int position) => dialogueGapFillExercise == null ?
            -1 :
            usedRoles.IndexOf(usedRoles.Where(role => role.Id == dialogueGapFillExercise.Lines[position].RoleId).First());

        public string GetRoleImageJson(int rolePosition) => dialogueGapFillExercise == null ?
            string.Empty :
            roleLottieJson[Role(rolePosition)];



        private void SetupGapFill(IGapFillModel gapFillModel, ModuleFunctions moduleFunctions) {
            if (gapFillModel is DialogueGapFillExercise dialogueGapFill) {
                dialogueGapFillExercise = dialogueGapFill;
                var usedRoleIds = dialogueGapFillExercise.Lines.Select(line => line.RoleId);
                usedRoles = moduleFunctions.Roles.Where(role => usedRoleIds.Contains(role.Id)).ToList();
                roleLottieJson = moduleFunctions.GetRoleLottieJson(usedRoles);
                ScoringViewModel = new ScoringViewModel(ActivityData, this, true);
                ResultsViewModel = new GapFillResultsViewModel(this);
            }
            ScoringViewModel = new ScoringViewModel(ActivityData, this, true);
            ResultsViewModel = new GapFillResultsViewModel(this);

        }

        private Role Role(int position) => dialogueGapFillExercise == null ? null : usedRoles.Where(role => role.Id == dialogueGapFillExercise.Lines[position].RoleId).FirstOrDefault();
        private List<Role> usedRoles;
        private Dictionary<Role, string> roleLottieJson;



        public GapFillViewModel(IGapFillModel gapFillModel, ModuleFunctions moduleFunctions) : base(gapFillModel) {
            Phrases = gapFillModel.Phrases;
            SetupGapFill(gapFillModel, moduleFunctions);
            Reset();
        }
        //List of shuffled indexs
        private List<int> shuffledPhraseIndexes;
        private List<int> remainingUnusedTagIndexes;
        private int[][] answers; //[Tag finder][tag]
        private List<TagFinder> tagFinders;
        private List<string> allTagStrings;
        public Color DefaultTagColor => GlobalColorPalette.Light;
        public string DoneText => Resx.String.CheckAnswersButton;
        public string BlankTextString { get; } = "" + '\u00A0' + '\u00A0' + '\u00A0' + '\u00A0' + '\u00A0' + '\u00A0' + '\u00A0' + '\u00A0';

        public int UnusedTagCount => remainingUnusedTagIndexes.Count;
        public int PhraseCount => tagFinders.Count;
        public int GetTagCount(int phrasePosition) => tagFinders[PhraseIndex(phrasePosition)].MatchCount;

        public string GetPhrase(int phrasePosition) => tagFinders[PhraseIndex(phrasePosition)].SourceText;
        public override bool UseFinishButton => true;
        public int GetUnusedTagId(int index) => remainingUnusedTagIndexes[index];
        public override int NumberOfCorrectAnswers => CheckCorrectAnswerCount();
        public override int TotalNumberOfQuestions => allTagStrings.Count;


        public IEnumerable<string> Phrases { get; }

        public string GetUnusedTagString(int index) => allTagStrings[GetUnusedTagId(index)];
        public bool GetCorrect(int phrasePosition, int match) {
            var phraseIndex = PhraseIndex(phrasePosition);
            return answers[phraseIndex][match] > -1 && allTagStrings[answers[phraseIndex][match]] == tagFinders[phraseIndex].OriginalTaggedTextValues[match];
        }

        public void RemoveAnswerAtPosition(int postion, int gapIndex) {
            var tagIndex = GetAnswerTagIndex(postion, gapIndex);
            RemoveAnswer(tagIndex);
        }


        public string GetAnswerTagString(int phrasePosition, int gapIndex) {
            var answerIndex = answers[PhraseIndex(phrasePosition)][gapIndex];
            var answerString = BlankTextString;

            if (answerIndex > -1) {
                answerString = allTagStrings[answerIndex];
            }
            if (Finished) {
                answerString += GetCorrect(phrasePosition, gapIndex) ? "○" : "✖︎";
            }
            return answerString;
        }



        public override void Reset() {
            base.Reset();
            tagFinders = new List<TagFinder>();
            foreach (var phrase in Phrases) {
                var tagFinder = new TagFinder(phrase);
                for (int i = 0; i < tagFinder.MatchCount; i++) {
                    tagFinder.ReplaceTextAtLocation(i, BlankTextString);
                }
                tagFinders.Add(tagFinder);
            }
            allTagStrings = tagFinders.SelectMany(tagFinder => tagFinder.OriginalTaggedTextValues).ToList();
            answers = new int[tagFinders.Count][];
            for (int i = 0; i < answers.Length; i++) {
                answers[i] = new int[tagFinders[i].MatchCount];
                answers[i].Populate(-1);
            }
            remainingUnusedTagIndexes = Enumerable.Range(0, allTagStrings.Count).ToList();
            remainingUnusedTagIndexes.Shuffle();

            shuffledPhraseIndexes = Enumerable.Range(0, tagFinders.Count).ToList();
            if (PhrasesShuffled) {
                shuffledPhraseIndexes.Shuffle();
            }

        }



        public void SetAnswer(int phrasePosition, int gapIndex, int tagIndex) {
            RemoveAnswer(tagIndex);
            RemoveAnswer(answers[PhraseIndex(phrasePosition)][gapIndex]);
            answers[PhraseIndex(phrasePosition)][gapIndex] = tagIndex;
            remainingUnusedTagIndexes.Remove(tagIndex);
        }
        public void RemoveAnswer(int tagIndex) {
            if (tagIndex > -1) {
                for (int i = 0; i < answers.GetLength(0); i++) {
                    answers[i].Replace(tagIndex, -1);
                }
                if (!remainingUnusedTagIndexes.Contains(tagIndex)) {
                    remainingUnusedTagIndexes.Add(tagIndex);
                }
            }
        }




        public override bool QuestionAnsweredCorrectly(int questionNumber) {
            var indexes = answers.GetIndexOfPosition(questionNumber);
            var answer = answers[indexes.outerIndex][indexes.innerIndex];
            for (int phrase = 0; phrase < answers.GetLength(0); phrase++) {
                if (answer > -1 && allTagStrings[answer] == tagFinders[indexes.outerIndex].OriginalTaggedTextValues[indexes.innerIndex]) {
                    return true;
                }
            }
            return false;
        }




        public int GetAnswerTagIndex(int phrasePosition, int gapIndex) => answers[PhraseIndex(phrasePosition)][gapIndex];



        private int CheckCorrectAnswerCount() {
            var score = 0;
            for (int phrase = 0; phrase < answers.GetLength(0); phrase++) {
                for (int match = 0; match < answers[phrase].Length; match++) {
                    if (answers[phrase][match] > -1 && allTagStrings[answers[phrase][match]] == tagFinders[phrase].OriginalTaggedTextValues[match]) {
                        score++;
                    }
                }
            }
            return score;
        }




        public Color GetTagColor(int phrasePosition, int gapIndex) =>
            !Finished ? DefaultTagColor :
            GetCorrect(phrasePosition, gapIndex) ? Color.Green : Color.Red;

        /// <summary>
        /// Get the original index of the phrase from the shuffled ones.
        /// If it's not shuffled then it's just the index
        /// </summary>
        /// <param name="phrasePosition"></param>
        /// <returns></returns>
        public int PhraseIndex(int phrasePosition) => PhrasesShuffled ? shuffledPhraseIndexes[phrasePosition] : phrasePosition;



        public override void ViewCreated() {
            base.ViewCreated();
        }

    }
}
