using System;
namespace SharedActivities.Core {
    public class CrossViewModelExtra : CrossLibrary.CrossViewModel {
        public override void ViewAppearing() {
            base.ViewAppearing();
            GlobalValues.Settings.LangaugeChanged += this.Settings_LangaugeChanged;
        }

        public override void ViewDisappearing() {
            base.ViewDisappearing();
            GlobalValues.Settings.LangaugeChanged -= this.Settings_LangaugeChanged;
        }

        private void Settings_LangaugeChanged(object sender, EventArgs e) {
            RefreshUILocale();
        }
    }
}
