using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using CrossLibrary;
using SharedActivities.Core.Models;
using SharedActivities.Core.ViewModels.Exercises;

namespace SharedActivities.Core {
    public class ModuleFunctions {
        public Assembly ImageResourceAssembly { get; }
        private List<IActivityDataModel> allActivityDataModel;
        public IReadOnlyList<IActivityDataModel> AllActivityDataModel => allActivityDataModel.AsReadOnly();
        private List<Role> allRoles;
        public IReadOnlyList<Role> Roles => allRoles.AsReadOnly();



        public string GetFilename(Role role) => (role.Gender == Gender.Male ? "male" : role.Gender == Gender.Female ? "female" : "other") + role.Appearance.ToString("D3");

        public string GetImageJson(Role role) => ResourceLoader.GetEmbeddedResourceString(ImageResourceAssembly, @"Resources.Lottie.Faces." + GetFilename(role) + ".json");

        public Dictionary<Role, string> GetRoleLottieJson(IEnumerable<Role> roles) {
            var roleImages = new Dictionary<Role, string>();
            foreach (var role in roles) {
                roleImages[role] = GetImageJson(role);
            }
            return roleImages;
        }

        public ModuleFunctions(List<IActivityDataModel> allActivityDataModel, List<Role> allRoles, Assembly imageResourceAssembly) {
            ImageResourceAssembly = imageResourceAssembly;
            this.allActivityDataModel = allActivityDataModel;
            this.allRoles = allRoles;

        }


        public ExerciseViewModel GetViewModelFor(IActivityDataModel activityDataModel) {

            switch (activityDataModel) {
                case IGapFillModel gapFillExercise:
                    return new GapFillViewModel(gapFillExercise, this);
             
                default:
                    throw new Exception("Could not match type");
            }
        }


        
    }
}
