
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using CrossLibrary.Droid.Views;
using SharedActivities.Core.ViewModels.Exercises;
using SharedActivities.Droid.CustomViews;

namespace SharedActivities.Droid.Views.Exercises {
    public class ReadingQuiz : CrossFragment<ReadingOptionQuizViewModel> {
        private RecyclerView readingRecyclerView;
        private DiscreteProgressView progressView;
        private ReadingRecyclerAdapter adapter;
        public IExerciseLogic ExerciseLogic => ViewModel;





        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = inflater.Inflate(Resource.Layout.reading_quiz, container, false);
            readingRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.readingRecyclerView);
            progressView = view.FindViewById<DiscreteProgressView>(Resource.Id.progressView);


            //var optionQuiz = new OptionQuiz(ViewModel);
            //optionQuiz.ShowIn(Resource.Id.questionsView);

            progressView.ProgressTracker = ViewModel;
            readingRecyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
            adapter = new ReadingRecyclerAdapter(this);
            this.readingRecyclerView.SetAdapter(adapter);

            return view;
        }

        public override void RefreshUILocale() {

        }

        public override void OnPause() {
            base.OnPause();
        }

        private class ReadingRecyclerAdapter : RecyclerView.Adapter {
            ReadingOptionQuizViewModel readingQuizLogic;
            ReadingQuiz readingQuiz;
            string cache = string.Empty;
            public ReadingRecyclerAdapter(ReadingQuiz readingQuiz) {
                this.readingQuiz = readingQuiz;
                this.readingQuizLogic = readingQuiz.ViewModel;

            }

            public override int ItemCount => readingQuizLogic.LineCount;

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
                var viewHolder = ((GapFillView.GapfillViewHolder)holder);

                viewHolder.LineTextView.Text = readingQuizLogic.LineText(position);
                viewHolder.RoleNameText.Text = readingQuizLogic.RoleName(position);
                if(!string.IsNullOrWhiteSpace(readingQuizLogic.GetRoleImageJson(position))) {
                    viewHolder.CharacterImageView.SetAnimationFromJson(readingQuizLogic.GetRoleImageJson(position), readingQuizLogic.GetRoleImageJson(position));
                }
                
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
                var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.gap_fill_cell, parent, false);
                var viewHolder = new GapFillView.GapfillViewHolder(view);
                return viewHolder;
            }
        }
    }
}