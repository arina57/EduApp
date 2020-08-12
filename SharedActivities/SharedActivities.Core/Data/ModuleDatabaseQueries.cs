using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrossLibrary;
using SharedActivities.Core.Models;
using SharedActivities.Core.Models.Database;
using SQLite;

namespace SharedActivities.Core.Data {
	public class ModuleDatabaseQueries {
		public const string DatabaseName = "SharedActivitiesDb.db3";
		static ModuleDatabaseQueries localDatabase;
		public static ModuleDatabaseQueries LocalDatabase {
			get {
				if (localDatabase == null) {
					var databasepath = CommonFunctions.GetLocalDatabaseFilePath(DatabaseName);
					localDatabase = new ModuleDatabaseQueries(databasepath);
				}
				return localDatabase;
			}
		}



		private string databasePath;
		public ModuleDatabaseQueries(string databasePath) {
			this.databasePath = databasePath;
			SetupDatabase();
		}

		private void SetupDatabase() {

			using (var connection = new SQLiteConnection(databasePath)) {
				connection.CreateTable<ExerciseAttemptStats>();
				connection.CreateTable<AttemptRecord>();
				connection.CreateTable<Settings>();
			}
		}

		public Settings GetSettings() {
			using (var connection = new SQLiteConnection(databasePath)) {
				try {
					var table = connection.Table<Settings>();
					return table.FirstOrDefault();
				} catch {
					connection.DropTable<Settings>();
					connection.CreateTable<Settings>();
					return null;
				}
			}
		}

		public int SaveSettings(Settings item) {
			using (var connection = new SQLiteConnection(databasePath)) {
				return connection.InsertOrReplace(item);
			}
		}


		public int GetTotalPoints() {
			var score = 0;
			using (var connection = new SQLiteConnection(databasePath)) {
				if (SharedFunctions.TableExists<ExerciseAttemptStats>(connection)) {
					var table = connection.Table<ExerciseAttemptStats>();
					score = table.Sum(item => item.Points);
				}
				if (SharedFunctions.TableExists<AttemptRecord>(connection)) {
					var table = connection.Table<AttemptRecord>();
					score += table.Sum(item => item.Points);
				}
				return score;
			}
		}

		public void EndAllCurrentAttempts() {
			using (var connection = new SQLiteConnection(databasePath)) {
				var attempts = CurrentActiveAttemptRecords().ToList();
				attempts.ForEach(attempt => {
					attempt.EndedIncomplete = true;
				});

				connection.UpdateAll(attempts);
			}
		}

		private List<AttemptRecord> CurrentActiveAttemptRecords() {
			using (var connection = new SQLiteConnection(databasePath)) {
				if (connection.TableExists<AttemptRecord>()) {
					var table = connection.Table<AttemptRecord>();
					var query = table.AsEnumerable().Where(i =>
							i.AppSessionId == GlobalValues.Settings.AppSessionCount
							&& !i.NoLongerActive);
					return query.ToList();
				}
				return new List<AttemptRecord>();
			}
		}


		public ExerciseAttemptStats GetExistOrNewExerciseAttemptStats(IdentityModel activityDataModel) {
			using (var connection = new SQLiteConnection(databasePath)) {
				if (SharedFunctions.TableExists<ExerciseAttemptStats>(connection)) {
					var table = connection.Table<ExerciseAttemptStats>();
					var query = table.Where(i =>
						i.CourseId == activityDataModel.CourseId &&
						i.UnitId == activityDataModel.UnitId &&
						i.ActivityId == activityDataModel.ActivityId);
					if (query.Count() > 0) {
						return query.First();
					}
				}
			}
			return new ExerciseAttemptStats(activityDataModel);
		}


		public void UpdateAttempt(IdentityModel activityDataModel, bool completed, int points, int correctAnswers = 0, int totalAnswers = 0) {
			UpdateIndvidualAttempt(activityDataModel, completed, points, correctAnswers, totalAnswers);
			UpdateAcculativeExerciseAttemptStats(activityDataModel, completed, points, correctAnswers, totalAnswers);
		}

		public void UpdateIndvidualAttempt(IdentityModel activityDataModel, bool completed, int points, int correctAnswers = 0, int totalAnswers = 0) {
			var currentAttemptRecord = GetExistingOrNewActiveAttemptRecord(activityDataModel);
			currentAttemptRecord.ActivityCompleted = completed;
			currentAttemptRecord.Points += points;
			currentAttemptRecord.ItemsCorrect += correctAnswers;
			currentAttemptRecord.ItemsAnswered += totalAnswers;

			using (var connection = new SQLiteConnection(databasePath)) {
				if (currentAttemptRecord.InstallAttemptId > 0) {
					connection.Update(currentAttemptRecord);
				} else {
					connection.Insert(currentAttemptRecord);
				}
			}
		}

		/// <summary>
		/// These are acculative stats, not individual
		/// </summary>
		/// <param name="activityDataModel"></param>
		/// <param name="completed"></param>
		/// <param name="points"></param>
		/// <param name="correctAnswers"></param>
		/// <param name="totalAnswers"></param>
		/// <returns></returns>
		private void UpdateAcculativeExerciseAttemptStats(IdentityModel activityDataModel, bool completed, int points, int correctAnswers = 0, int totalAnswers = 0) {
			var exerciseAttemptStats = GetExistOrNewExerciseAttemptStats(activityDataModel);
			exerciseAttemptStats.Points += points;
			exerciseAttemptStats.ItemsCorrect += correctAnswers;
			exerciseAttemptStats.ItemsAnswered += totalAnswers;
			if (completed) {
				exerciseAttemptStats.TimesCompleted++;
				if (totalAnswers > 0 && correctAnswers == totalAnswers) {
					exerciseAttemptStats.TimesCompletedWithFullPoints++;
				}
			}
			using (var connection = new SQLiteConnection(databasePath)) {
				if (exerciseAttemptStats.Id > 0) {
					connection.Update(exerciseAttemptStats);
				} else {
					connection.Insert(exerciseAttemptStats);
				}
			}
		}




		private AttemptRecord GetExistingOrNewActiveAttemptRecord(IdentityModel activityDataModel) {
			using (var connection = new SQLiteConnection(databasePath)) {
				return SessionAttemptRecords(connection, activityDataModel)
				.Where(attempt => !attempt.NoLongerActive).FirstOrDefault() ?? new AttemptRecord(activityDataModel, SessionAttempts(activityDataModel) + 1);
			}
		}

		private IEnumerable<AttemptRecord> SessionAttemptRecords(SQLiteConnection connection, IdentityModel activityDataModel) {
			if (connection.TableExists<AttemptRecord>()) {
				var table = connection.Table<AttemptRecord>();
				var query = ExercisesAttemptRecords(connection, activityDataModel)
					.Where(i => i.AppSessionId == GlobalValues.Settings.AppSessionCount);
				return query;
			}
			return Enumerable.Empty<AttemptRecord>();

		}

		private IEnumerable<AttemptRecord> ExercisesAttemptRecords(SQLiteConnection connection, IdentityModel activityDataModel) {
			if (connection.TableExists<AttemptRecord>()) {
				var table = connection.Table<AttemptRecord>();
				var query = table.Where(i =>
					i.ProductId == activityDataModel.CourseId &&
					i.UnitId == activityDataModel.UnitId &&
					i.ActivityId == activityDataModel.ActivityId);
				return query;
			}
			return Enumerable.Empty<AttemptRecord>();
		}

		public int SessionAttempts(IdentityModel activityDataModel) {
			using (var connection = new SQLiteConnection(databasePath)) {
				var attempts = SessionAttemptRecords(connection, activityDataModel).Select(i => (int?)i.SessionAttempt).Max();
				return attempts ?? 0;
			}
		}


		/// <summary>
		/// Increments the amount of time in the activity if it is over 1 second
		/// </summary>
		/// <param name="activityDataModel"></param>
		/// <param name="activtiyTime"></param>
		public void IncrementTimeInActivity(IdentityModel activityDataModel, TimeSpan activtiyTime) {
			if (activtiyTime > TimeSpan.FromSeconds(1)) {
				IncrementIndividualAttemptTime(activityDataModel, activtiyTime);
				IncrementAcculativeAttemptTime(activityDataModel, activtiyTime);
			}
		}

		private void IncrementAcculativeAttemptTime(IdentityModel activityDataModel, TimeSpan activtyTime) {
			var exerciseAttemptStats = GetExistOrNewExerciseAttemptStats(activityDataModel);
			exerciseAttemptStats.TimeInActivity += activtyTime;
			using (var connection = new SQLiteConnection(databasePath)) {
				if (exerciseAttemptStats.Id > 0) {
					connection.Update(exerciseAttemptStats);
				} else {
					connection.Insert(exerciseAttemptStats);
				}
			}
		}

		private void IncrementIndividualAttemptTime(IdentityModel activityDataModel, TimeSpan activtyTime) {
			var currentAttemptRecord = GetExistingOrNewActiveAttemptRecord(activityDataModel);
			currentAttemptRecord.TimeInActivity += activtyTime;
			using (var connection = new SQLiteConnection(databasePath)) {
				if (currentAttemptRecord.InstallAttemptId > 0) {
					connection.Update(currentAttemptRecord);
				} else {
					connection.Insert(currentAttemptRecord);
				}
			}
		}

		public void SkipActivity(IdentityModel activityDataModel) {
			var currentAttemptRecord = GetExistingOrNewActiveAttemptRecord(activityDataModel);
			currentAttemptRecord.ActivitySkipped = true;
			using (var connection = new SQLiteConnection(databasePath)) {
				if (currentAttemptRecord.InstallAttemptId > 0) {
					connection.Update(currentAttemptRecord);
				} else {
					connection.Insert(currentAttemptRecord);
				}
			}
		}


		public void IncrementAudioRecording(IdentityModel activityDataModel, TimeSpan recordingTime) {
			using (var connection = new SQLiteConnection(databasePath)) {
				var currentAttemptRecord = GetExistingOrNewActiveAttemptRecord(activityDataModel);
				currentAttemptRecord.AudioRecorded += recordingTime;
				currentAttemptRecord.RecordedTimes++;
				if (currentAttemptRecord.InstallAttemptId > 0) {
					connection.Update(currentAttemptRecord);
				} else {
					connection.Insert(currentAttemptRecord);
				}
			}
		}

		public void IncrementAudioPlayBackOnLastAttempt(IdentityModel activityDataModel, TimeSpan playbackTime) {
			IncrementIndividualPlayBackOnLastAttempt(activityDataModel, playbackTime);
			IncrementAcculativePlayBack(activityDataModel, playbackTime);
		}

		private void IncrementAcculativePlayBack(IdentityModel activityDataModel, TimeSpan playbackTime) {

			var exerciseAttemptStats = GetExistOrNewExerciseAttemptStats(activityDataModel);
			exerciseAttemptStats.AudioPlayedBack += playbackTime;
			using (var connection = new SQLiteConnection(databasePath)) {
				if (exerciseAttemptStats.Id > 0) {
					connection.Update(exerciseAttemptStats);
				} else {
					connection.Insert(exerciseAttemptStats);
				}
			}
		}

		private void IncrementIndividualPlayBackOnLastAttempt(IdentityModel activityDataModel, TimeSpan playbackTime) {

			var currentAttemptRecord = GetLastOrNewActiveAttemptRecord(activityDataModel);
			currentAttemptRecord.AudioPlayedBack += playbackTime;
			currentAttemptRecord.PlayBackTimes++;
			using (var connection = new SQLiteConnection(databasePath)) {
				if (currentAttemptRecord.InstallAttemptId > 0) {
					connection.Update(currentAttemptRecord);
				} else {
					connection.Insert(currentAttemptRecord);
				}
			}
		}

		private AttemptRecord GetLastOrNewActiveAttemptRecord(IdentityModel activityDataModel) {
			return SessionAttemptRecords(activityDataModel).OrderBy(item => item.InstallAttemptId).LastOrDefault()
				?? new AttemptRecord(activityDataModel, SessionAttempts(activityDataModel) + 1);
		}


		public List<AttemptRecord> SessionAttemptRecords(IdentityModel activityDataModel) {
			using (var connection = new SQLiteConnection(databasePath)) {
				if (connection.TableExists<AttemptRecord>()) {
					var table = connection.Table<AttemptRecord>();
					var query = ExercisesAttemptRecords(connection, activityDataModel)
						.Where(i => i.AppSessionId == GlobalValues.Settings.AppSessionCount);
					return query.ToList();
				}
			}
			return new List<AttemptRecord>();
		}


		public List<ExerciseAttemptStats> GetExerciseAttemptStats(int courseId) {
			using (var connection = new SQLiteConnection(databasePath)) {
				if (SharedFunctions.TableExists<ExerciseAttemptStats>(connection)) {
					var table = connection.Table<ExerciseAttemptStats>();
					var query = table.Where(i =>
						i.CourseId == courseId);
					return query.ToList();
				}
			}
			return null;
		}

	}
}
