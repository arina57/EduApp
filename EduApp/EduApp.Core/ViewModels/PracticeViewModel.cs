using System;
using CrossLibrary.Interfaces;
using SharedActivities.Core;

namespace EduApp.Core.ViewModels {
    public class PracticeViewModel : CrossViewModelExtra {
        private ModuleFunctions moduleFunctions;
        private int unitId;

        ICrossContainerView ExerciseContainer => FindCrossContainerView("PracticeContainer");
        public PracticeViewModel(ModuleFunctions moduleFunctions, int unitId) {
            this.moduleFunctions = moduleFunctions;
            this.unitId = unitId;
        }

        public override void ViewCreated() {
            base.ViewCreated();
            ExerciseContainer.ShowView(moduleFunctions.GetUnitPracticeViewModel(ConstValues.ProductId, unitId));
        }
    }
}
