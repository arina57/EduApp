using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using CrossLibrary;
using SharedActivities.Core.Models;
using SharedActivities.Core.ViewModels.Exercises;
using SharedActivities.Core.Models.OptionQuizModel;
using SharedActivities.Core.Models.DialogueGapFillExercises;
using SharedActivities.Core.Models.PhraseMatchingPoolModel;

namespace SharedActivities.Core {
    public class ModuleFunctions {
        private List<IActivityDataModel> allActivityDataModel;
        public IReadOnlyList<IActivityDataModel> AllActivityDataModel => allActivityDataModel.AsReadOnly();
        private List<Role> allRoles;
        public IReadOnlyList<Role> Roles => allRoles.AsReadOnly();



        public string GetFilename(Role role) => (role.Gender == Gender.Male ? "male" : role.Gender == Gender.Female ? "female" : "other") + role.Appearance.ToString("D3");
        public string GetImageJson(Role role) => Resx.Lottie.ResourceManager.GetString(GetFilename(role));

        public Dictionary<Role, string> GetRoleLottieJson(IEnumerable<Role> roles) {
            var roleImages = new Dictionary<Role, string>();
            foreach (var role in roles) {
                roleImages[role] = GetImageJson(role);
            }
            return roleImages;
        }

        public ModuleFunctions(List<IActivityDataModel> allActivityDataModel, List<Role> allRoles) {
            this.allActivityDataModel = allActivityDataModel;
            this.allRoles = allRoles;

        }


        public ExerciseViewModel GetViewModelFor(IActivityDataModel activityDataModel) {

            switch (activityDataModel) {
                case IGapFillModel gapFillExercise:
                    return new GapFillViewModel(gapFillExercise, this);
                case Models.DialogueOptionQuizModel.DialogueOptionQuiz dialogueOptionQuiz:
                    return new DialogueOptionQuizViewModel(dialogueOptionQuiz, this);
                case OptionQuizData optionQuizData:
                    if (optionQuizData.ActivityData.ActivityId == 8) {
                        var gapFill = allActivityDataModel.OfType<DialogueGapFillExercise>()
                            .Where(exercise => exercise.ActivityData.Matches(optionQuizData.ActivityData.CourseId, optionQuizData.ActivityData.UnitId))
                            .First();
                        return new ReadingOptionQuizViewModel(optionQuizData, gapFill, this);
                    } else {
                        return new BasicOptionQuizViewModel(optionQuizData);
                    }
                case PhraseMatchingPoolExercise phraseMatchingPoolExercise:
                    return phraseMatchingPoolExercise.ActivityData.ActivityId switch
                    {
                        6 => new BasicOptionQuizViewModel(new MatchingPoolOptionQuizData(phraseMatchingPoolExercise, 4)),
                        5 => new PhraseMatchViewModel(phraseMatchingPoolExercise),
                        _ => GetRandomPhraseMatchingPoolExercise(phraseMatchingPoolExercise)
                    };
                default:
                    throw new Exception("Could not match type");
            }
        }


        private ExerciseViewModel GetRandomPhraseMatchingPoolExercise(PhraseMatchingPoolExercise phraseMatchingPoolExercise) {

            var number = CommonFunctions.StaticRandom.Next(3);
            return number switch
            {
                0 => new PhraseMatchViewModel(phraseMatchingPoolExercise),
                1 => new BasicOptionQuizViewModel(new MatchingPoolOptionQuizData(phraseMatchingPoolExercise, 4)),
                _ => new PhraseMatchViewModel(phraseMatchingPoolExercise),
            };
        }

    }
}
