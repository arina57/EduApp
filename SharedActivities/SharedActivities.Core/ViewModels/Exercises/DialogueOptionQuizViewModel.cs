using System;
using System.Collections.Generic;
using System.Linq;
using SharedActivities.Core.Models;
using SharedActivities.Core.Models.DialogueOptionQuizModel;

namespace SharedActivities.Core.ViewModels.Exercises {
    /// <summary>
    /// An ordered conversation where you have to choose the correct options
    /// </summary>
    public class DialogueOptionQuizViewModel : OptionQuizContainerViewModel {
        private DialogueOptionQuiz dialogueOptionQuizData;
        private List<Role> usedRoles;
        private Dictionary<Role, string> roleLottieJson;

        public override string TitleText => string.Empty;
        public override string SubtitleText => ActivityData.ActivityDescription;
        public DialogueOptionQuizViewModel(DialogueOptionQuiz dialogueOptionQuizData, ModuleFunctions moduleFunctions) : base(dialogueOptionQuizData) {
            this.dialogueOptionQuizData = dialogueOptionQuizData;
            var usedRoleIds = this.dialogueOptionQuizData.GetRoleIds();
            usedRoles = moduleFunctions.Roles.Where(role => usedRoleIds.Contains(role.Id)).ToList();
            roleLottieJson = moduleFunctions.GetRoleLottieJson(usedRoles);
            Reset();
        }

        public override string SituationText => this.dialogueOptionQuizData.Situation.GetString();

        //public int LastSelectedOption { get; private set; }
        //public bool LastSelectionWasCorrect { get; private set; }
        private Line CurrentQuestion => dialogueOptionQuizData.Lines[CurrentQuestionIndex];
        public int CurrentRoleId => Finished ? -1 : CurrentQuestion.RoleId;
        public string GetRoleImageJson(int rolePosition) => roleLottieJson[usedRoles[rolePosition]];

        public int RoleCount => usedRoles.Count;

        protected override bool AnswerGrid => false;

        public string GetRoleName(int position) {
            return usedRoles[position].Name;
        }

        public bool IsCurrentRole(int position) {
            return CurrentRoleId == usedRoles[position].Id;
        }

        public override bool QuestionAnsweredCorrectly(int questionNumber) => OptionQuizViewModel.QuestionAnsweredCorrectly(questionNumber);
    }
}

