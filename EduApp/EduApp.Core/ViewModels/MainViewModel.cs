using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using CrossLibrary;
using SharedActivities.Core;
using SharedActivities.Core.Models;
using SharedActivities.Core.ViewModels.Exercises;

namespace EduApp.Core.ViewModels {
    public class MainViewModel : CrossViewModel {
        public string ButtonText => Resources.String.GapFillButton;
        private ModuleFunctions moduleFunctions;
        public MainViewModel() {
           
        }

        public override void ViewCreated() {
            base.ViewCreated();
            var a = GetRoles();
            var b = GetActivities();
            var c = GetExerciseData();
            moduleFunctions = new ModuleFunctions(c, a);
        }


        private static List<Role> GetRoles() {
            return SharedFunctions.GetXmlRoot<Roles>(Resources.Resources.Roles).Items;
        }

        private static List<Activity> GetActivities() {
            return SharedFunctions.GetXmlRoot<Activities>(Resources.Resources.ActivityNames).Items;
        }


        private static List<IActivityDataModel> GetExerciseData() {
            List<IActivityDataModel> exerciseData = new List<IActivityDataModel>();
            exerciseData.AddRange(SharedFunctions.GetXmlRoot<DialogueGapFillExercises>(Resources.Resources.DialogueGapFill).Items);
            return exerciseData;
        }

        public void Button_Clicked(object sender, EventArgs e) {
            var gapfill = moduleFunctions.AllActivityDataModel.Where(activity => activity is IGapFillModel).First() as IGapFillModel;
            var gapFillViewModel = new GapFillViewModel(gapfill, moduleFunctions);
            gapFillViewModel.Show();
        }
    }
}
