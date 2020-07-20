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


        private static List<Role> GetRoles() {
            return ResourceLoader.GetXmlRoot<Roles>(typeof(MainViewModel).Assembly, "Resources.Roles.xml").Items;
        }

        private static List<Activity> GetActivities() {
            return ResourceLoader.GetXmlRoot<Activities>(typeof(MainViewModel).Assembly, "Resources.ActivityNames.xml").Items;
        }


        private static List<IActivityDataModel> GetExerciseData() {
            List<IActivityDataModel> exerciseData = new List<IActivityDataModel>();
            exerciseData.AddRange(ResourceLoader.GetXmlRoot<DialogueGapFillExercises>(typeof(MainViewModel).Assembly, "Resources.DialogueGapFill.xml").Items);
            return exerciseData;
        }

    }
}
