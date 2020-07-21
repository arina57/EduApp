using System;
using System.Globalization;
using CrossLibrary;
using SharedActivities.Core.Data;
using SQLite;

namespace SharedActivities.Core.Models.Database {
    public class Settings {

        [PrimaryKey]
        public int Id { get; set; } = 1;

        public CultureInfo Language { get; set; }
        public Guid InstallId { get; set; }

        public DateTime FirstRunDate { get; set; } = DateTime.Now;
        public TimeSpan SavedTimeInApp { get; set; }
        public int AppSessionCount { get; set; }


        [Ignore]
        public static bool FirstSession { get; private set; }

        [Ignore]
        public TimeSpan TotalTimeInApp => SavedTimeInApp + TimeSinceAppear;

        private DateTime appearTime = DateTime.Now;
        private CultureInfo currentCulture;

        [Ignore]
        public TimeSpan TimeSinceAppear => appearTime > DateTime.MinValue ? DateTime.Now.Subtract(appearTime) : TimeSpan.Zero;


        public void OnAppOpened() {
            AppSessionCount++;
            Save();
        }

        public void OnAppFocused() {
            if (appearTime > DateTime.MinValue) {
                SavedTimeInApp = TotalTimeInApp;
                Save();
            }
            appearTime = DateTime.Now;
        }

        public void OnAppLostFocused() {
            if (appearTime > DateTime.MinValue) {
                SavedTimeInApp = TotalTimeInApp;
                Save();
            }
            appearTime = DateTime.MinValue;
        }

        public Settings() {
            //var settings = await GeneralDatabase.LocalDatabase.GetSettings();
            //Language = settings.Language;
        }

        public Settings(CultureInfo currentCulture) {
            this.Language = currentCulture;
        }

        /// <summary>
        /// Loads instance from database.
        /// </summary>
        /// <returns>The load.</returns>
        public static Settings Load() {
            return ModuleDatabaseQueries.LocalDatabase.GetSettings();
        }

        /// <summary>
        /// Saves this instance to database.
        /// </summary>
		public void Save() {
            ModuleDatabaseQueries.LocalDatabase.SaveSettings(this);
        }

        public event EventHandler LangaugeChanged;

        /// <summary>
        /// Creates a new instance.
        /// Loads settings from the SQLite if an entry exists
        /// If there isn't a entry it creates one.
        /// </summary>
        /// <returns>The instance.</returns>
        public static Settings NewInstance() {
            var settings = Settings.Load();
            if (settings == null) {
                FirstSession = true;

                var currentCulture = CommonFunctions.GetDefaultCulture();
                settings = new Settings(currentCulture) {
                    InstallId = Guid.NewGuid(),
                    FirstRunDate = DateTime.Now
                };
                settings.Save();
            } else {
                FirstSession = false;
            }
            SharedFunctions.SetLanguage(settings.Language);
            return settings;
        }


        [Ignore]
        public CultureInfo CurrentLanguage {
            get {
                return CultureInfo.DefaultThreadCurrentUICulture;

            }
            set {
                Language = value;
                Save();
                SharedFunctions.SetLanguage(this.Language);
                LangaugeChanged?.Invoke(this, new EventArgs());
            }
        }

        public void SwitchLanguage() {
            if (CurrentLanguage.SameLanguage(GlobalValues.English)) {
                CurrentLanguage = GlobalValues.Japanese;
            } else {
                CurrentLanguage = GlobalValues.English;
            }
        }

        public void RefreshLanguage() {
            LangaugeChanged?.Invoke(this, new EventArgs());
        }
    }
}
