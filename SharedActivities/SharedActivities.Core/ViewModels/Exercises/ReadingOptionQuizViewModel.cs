using System;
using System.Collections.Generic;
using System.Linq;
using SharedActivities.Core.Models;
using SharedActivities.Core.Models.DialogueGapFillExercises;
using SharedActivities.Core.Models.OptionQuizModel;

namespace SharedActivities.Core.ViewModels.Exercises {
    public class ReadingOptionQuizViewModel : OptionQuizContainerViewModel {
        private DialogueGapFillExercise dialogueGapFillExercise;


        public override bool QuestionAnsweredCorrectly(int questionNumber) => OptionQuizViewModel.QuestionAnsweredCorrectly(questionNumber);
        private List<Role> usedRoles;
        private Dictionary<Role, string> roleLottieJson;


        public int LineCount => dialogueGapFillExercise.Lines.Count;
        public override string SubtitleText => ActivityData.ActivityDescription;
        public override string SituationText => dialogueGapFillExercise.Situation.GetString();

        public int RoleCount => usedRoles.Count;
        public IReadOnlyList<Role> Roles => usedRoles.AsReadOnly();

        protected override bool AnswerGrid => false;

        public ReadingOptionQuizViewModel(OptionQuizData optionQuiz, DialogueGapFillExercise dialogueGapFillExercise, ModuleFunctions moduleFunctions) : base(optionQuiz) {
            this.dialogueGapFillExercise = dialogueGapFillExercise;

            usedRoles = dialogueGapFillExercise.Lines
                .GroupBy(line => line.RoleId)
                .Select(group => group.First().RoleId)
                .Join(moduleFunctions.Roles, roleId => roleId,
                    role => role.Id,
                    (roleId, role) => role).ToList();
            roleLottieJson = moduleFunctions.GetRoleLottieJson(usedRoles);
        }

        public string LineText(int position) => dialogueGapFillExercise.Lines[position].GetTextWithoutTagsOrBrackets();
        public string GetRoleImageJson(int rolePosition) => roleLottieJson[Role(rolePosition)];
        public string RoleName(int position) => Role(position).Name;
        private Role Role(int position) => usedRoles.Where(role => role.Id == dialogueGapFillExercise.Lines[position].RoleId).FirstOrDefault();

    }
}
 