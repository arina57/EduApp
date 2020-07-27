using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using CrossLibrary;
using SharedActivities.Core;
using SharedActivities.Core.Models;

namespace EduApp.Core.ViewModels {
    public class MainViewModel : CrossViewModel {
        public MainViewModel() {
           
        }

        public override void ViewCreated() {
            base.ViewCreated();
            var a = GetRoles();
            var b = GetActivities();
            var c = GetExerciseData();
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

    }
}
