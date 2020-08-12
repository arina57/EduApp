
using CrossLibrary;
using SharedActivities.Core.Data;
using SharedActivities.Core.Models;
using SharedActivities.Core.Models.Database;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
namespace SharedActivities.Core.ViewModels.Exercises.Results {
    public class ScoringViewModel : CrossViewModelExtra {




        private string multiplierText = string.Empty;
        public string MultiplierText {
            get => multiplierText;
            set {
                multiplierText = value;
                ProperyChanged(() => MultiplierText);
            }
        }

        private bool multiplierTextVisible = false;
        public bool MultiplierTextVisible {
            get => multiplierTextVisible;
            set {
                multiplierTextVisible = value;
                ProperyChanged(() => MultiplierTextVisible);
            }
        }


        private bool perfectBonusTextVisible = false;
        public bool PerfectBonusTextVisible {
            get => perfectBonusTextVisible;
            set {
                perfectBonusTextVisible = value;
                ProperyChanged(() => PerfectBonusTextVisible);
            }
        }
        private string perfectBonusText = string.Empty;
        public string PerfectBonusText {
            get => perfectBonusText;
            private set {
                perfectBonusText = value;
                ProperyChanged(() => PerfectBonusText);
            }
        }

        private string scoreText = string.Empty;
        public string ScoreText {
            get => scoreText;
            set {
                scoreText = value;
                ProperyChanged(() => ScoreText);
            }
        }

        private string pointsText = string.Empty;
        public string PointsText {
            get => pointsText;
            set {
                pointsText = value;
                ProperyChanged(() => PointsText);
            }
        }

        private float resultAlpha = 0;
        public float ResultAlpha {
            get => resultAlpha;
            set {
                resultAlpha = value;
                ProperyChanged(() => ResultAlpha);
            }
        }

        private float waitingAlpha = 1;
        public float WaitingAlpha {
            get => waitingAlpha;
            set {
                waitingAlpha = value;
                ProperyChanged(() => WaitingAlpha);
            }
        }

        private float progress = 0;
        public float Progress {
            get => progress;
            private set {
                progress = value;
                ProperyChanged(() => Progress);
            }
        }





        private async Task AnimateScore(CancellationToken cancellationToken = default) {


            MultiplierTextVisible = false;
            PerfectBonusTextVisible = false;
            ResultAlpha = 0;
            WaitingAlpha = 1;
            Progress = 0;

            var scoreAnimationTask = CommonFunctions.AnimateTextNumberAsync(value => ScoreText = value,
                                                         0,
                                                         NumberOfCorrectAnswers,
                                                         "{0}/" + TotalNumberOfQuestions, cancellationToken);

            var pointsAnimationTask = CommonFunctions.AnimateTextNumberAsync(value => PointsText = value,
                                                                            0,
                                                                            NumberOfCorrectAnswers,
                                                                            Resx.String.PlusPoints,
                                                                            cancellationToken);
            //Progress = CorrectPercent;
            var progressTask = CommonFunctions.AnimateFloatValueAsync(value => Progress = value, 200 + NumberOfCorrectAnswers * 200, 0,
                                                                      CorrectPercent,
                                                                      cancellationToken: cancellationToken);
            await Task.WhenAll(scoreAnimationTask, pointsAnimationTask, progressTask);

            if (PointMultiplier > 1 && CurrentSessionBaseScore > 0) {
                MultiplierTextVisible = true;
                var ordinal = SharedFunctions.ToOrdinalString(TotalCompletedCount,
                    CultureInfo.DefaultThreadCurrentUICulture);
                var nthAttemptBonus = string.Format(Resx.String.NthAttemptBonus, ordinal, "{0}");
                await CommonFunctions.AnimateTextNumberAsync(value => MultiplierText = value,
                                                             1,
                                                             PointMultiplier,
                                                             nthAttemptBonus,
                                                             cancellationToken);
                await CommonFunctions.AnimateTextNumberAsync(
                    value => PointsText = value,
                    NumberOfCorrectAnswers,
                    CurrentSessionBaseScore,
                    Resx.String.PlusPoints,
                    cancellationToken);
            }

            if (PerfectScoreBonus > 0) {
                PerfectBonusTextVisible = true;
                await CommonFunctions.AnimateTextNumberAsync(value => PerfectBonusText = value, 0, BonusPointsFromPerfectScore, Resx.String.PerfectScoreBonus, cancellationToken, 1000);
                await CommonFunctions.AnimateTextNumberAsync(value => PointsText = value, CurrentSessionBaseScore, CurrentSessionScore, Resx.String.PlusPoints, cancellationToken, 1000);
            }
            await Task.WhenAll(
            CommonFunctions.AnimateFloatValueAsync(value => ResultAlpha = value, 500, 0, 1, cancellationToken: cancellationToken),
            CommonFunctions.AnimateFloatValueAsync(value => WaitingAlpha = value, 500, 1, 0, cancellationToken: cancellationToken));


        }





        Task animateScoreTask;
        CancellationTokenSource source;
        public async override void RefreshUILocale() {
            base.RefreshUILocale();
            ProperyChanged(() => FeedbackString);
            if (source != null) {
                source.Cancel();
            }
            if (animateScoreTask != null) {
                await animateScoreTask;
            }
            source = new CancellationTokenSource();
            animateScoreTask = AnimateScore(source.Token);
        }



        IdentityModel activityDataModel;
        ExerciseViewModel scoredExerciseLogic;
        public event EventHandler ScoreChanged;
        private readonly int maxPointMultiplier = 5;
        private readonly int maxPerfectScoreBonus = 10;
        /// <summary>
        /// dimishes to 1 from the max depending on the times completed
        /// </summary>
        public int PointMultiplier => TotalCompletedCount < maxPointMultiplier ? maxPointMultiplier - TotalCompletedCount + 1 : 1;





        /// <summary>
        /// dimishes to 1 from the max depending on the times completed
        /// </summary>
        public int PerfectScoreBonus => CompletedPerfect ? PerfectScoreCount < maxPerfectScoreBonus ? maxPerfectScoreBonus - PerfectScoreCount + 1 : 1 : 0;
        public int TotalNumberOfQuestions => scoredExerciseLogic.TotalNumberOfQuestions;
        public int NumberOfCorrectAnswers => scoredExerciseLogic.NumberOfCorrectAnswers;
        public bool CompletedPerfect => scoredExerciseLogic.TotalNumberOfQuestions == scoredExerciseLogic.NumberOfCorrectAnswers;
        public bool CompletedGreat => CorrectPercent >= 0.9;
        public bool CompletedGood => CorrectPercent >= 0.5;
        public int BonusPointsFromPerfectScore => CompletedPerfect ? PerfectScoreBonus * PointMultiplier : 0;
        public int CurrentSessionBaseScore => NumberOfCorrectAnswers * PointMultiplier;
        public int CurrentSessionScore => CurrentSessionBaseScore + BonusPointsFromPerfectScore;

        public bool QuestionAnsweredCorrectly(int questionNumber) => scoredExerciseLogic.QuestionAnsweredCorrectly(questionNumber);

        public int TotalCompletedCount => currectStats == null ? 0 : currectStats.TimesCompleted;
        public int CompletedCountWithoutPerfects => currectStats == null ? 0 : currectStats.TimesCompleted - PerfectScoreCount;
        public int PerfectScoreCount => currectStats == null ? 0 : currectStats.TimesCompletedWithFullPoints;
        public float CorrectPercent => (NumberOfCorrectAnswers / (float)TotalNumberOfQuestions);

        //private int currentSessionAttemptNumber;
        //private int perfectScoreCountUntilNow;


        public string FeedbackString => CompletedGood ? CompletedPerfect ?
            Resx.String.FeedbackPerfect :
            Resx.String.FeedbackGood :
            Resx.String.FeedbackNotGood;

        public string ThinkingImageJson => Resx.Lottie.Thinking;

        public string ResultImageJson => CompletedGood ? CompletedGreat ?
            Resx.Lottie.ScoringPerfect :
            Resx.Lottie.ScoringGood :
            Resx.Lottie.ScoringBad;

        bool ScoreAtEndOnly { get; }
        private ExerciseAttemptStats currectStats => ModuleDatabaseQueries.LocalDatabase.GetExistOrNewExerciseAttemptStats(activityDataModel);


        public bool Perfect => NumberOfCorrectAnswers == TotalNumberOfQuestions;
        public ScoringViewModel(IdentityModel activityDataModel, ExerciseViewModel scoredExerciseLogic, bool scoreAtEndOnly) {
            this.activityDataModel = activityDataModel;
            this.scoredExerciseLogic = scoredExerciseLogic;
            this.ScoreAtEndOnly = scoreAtEndOnly;
            //currectStats = AttemptDatabaseQueries.LocalDatabase.GetExistOrNewExerciseAttemptStats(activityDataModel);
            scoredExerciseLogic.ExerciseFinished += this.ScoredExerciseLogic_ExerciseFinished;
            //currentSessionAttemptNumber = TotalCompletedCount;
            //perfectScoreCountUntilNow = PerfectScoreCount;
            //scoredExerciseLogic.ScoreChanged += this.ScoredExerciseLogic_ScoreChanged;
        }

        //private void ScoredExerciseLogic_ScoreChanged(object sender, ScoreChangedEventArgs e) {
        //    if (!ScoreAtEndOnly) {
        //        if (e.Correct) {
        //            ModulesDatabase.LocalDatabase.IncrementPoints(activityDataModel, PointMultiplier, 1, 1);
        //            ScoreChanged?.Invoke(this, e);
        //        }
        //    }
        //}

        private void ScoredExerciseLogic_ExerciseFinished(object sender, EventArgs e) {
            if (ScoreAtEndOnly) {
                ModuleDatabaseQueries.LocalDatabase.UpdateAttempt(activityDataModel, true, CurrentSessionScore, NumberOfCorrectAnswers, TotalNumberOfQuestions);
                if (NumberOfCorrectAnswers > 0) {
                    ScoreChanged?.Invoke(this, EventArgs.Empty);
                }
            } else {
                //Points were added during the activity so only add the perfect bonus;
                ModuleDatabaseQueries.LocalDatabase.UpdateAttempt(activityDataModel, true, BonusPointsFromPerfectScore);
                if (BonusPointsFromPerfectScore > 0) {
                    ScoreChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            //currectStats = AttemptDatabaseQueries.LocalDatabase.GetExistOrNewExerciseAttemptStats(activityDataModel);
        }

        internal void SetAnswered(bool correct) {
            if (!ScoreAtEndOnly) {
                if (correct) {
                    ModuleDatabaseQueries.LocalDatabase.UpdateAttempt(activityDataModel, false, PointMultiplier, 1, 1);
                    ScoreChanged?.Invoke(this, EventArgs.Empty);
                } else {
                    ModuleDatabaseQueries.LocalDatabase.UpdateAttempt(activityDataModel, false, 0, 0, 1);
                }
            }
        }
    }
}
