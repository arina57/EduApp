using System;
using Android.Animation;
using Android.OS;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Com.Airbnb.Lottie;
using CrossLibrary.Droid.Views;
using SharedActivities.Core.ViewModels.Exercises;
using SharedActivities.Droid.CustomViews;

namespace SharedActivities.Droid.Views.Exercises {
    public class DialogueOptionQuizView : CrossFragment<DialogueOptionQuizViewModel> {
        RecyclerView roleRecyclerView;
        private DiscreteProgressView progressView;
        private RoleListAdapter roleListAdapter;
        public DialogueOptionQuizView() {

        }

        public override void Prepare(DialogueOptionQuizViewModel model) {
            base.Prepare(model);
            ViewModel.ScoringViewModel.ScoreChanged += this.Logic_ScoreChanged;
            //ViewModel.ExerciseFinished += this.Logic_ExerciseFinished;
        }
        //bool AutoPopupScoreWhenDone { get; set; } = false;



        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            // Use this to return your custom view for this Fragment
            var view = inflater.Inflate(Resource.Layout.dialogue_option_quiz, container, false);
            roleRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.roleRecyclerView);
            progressView = view.FindViewById<DiscreteProgressView>(Resource.Id.progressView);

            progressView.ProgressTracker = ViewModel;
            roleRecyclerView.SetLayoutManager(new LinearLayoutManager(this.Activity, LinearLayoutManager.Horizontal, false));
            roleListAdapter = new RoleListAdapter(this);
            roleRecyclerView.SetAdapter(roleListAdapter);
            roleRecyclerView.OverScrollMode = OverScrollMode.Never;
            return view;
        }

        //private void Logic_ExerciseFinished(object sender, EventArgs e) {
        //    if(AutoPopupScoreWhenDone) {
        //        var chooseBestSentenceResults = new ResultExplanation(ViewModel.OptionQuizViewModel);
        //        chooseBestSentenceResults.ShowOver();
        //    }
        //}

        private void Logic_ScoreChanged(object sender, EventArgs e) {
            RefreshUILocale();
            roleListAdapter.NotifyDataSetChanged();
            if (roleListAdapter.ItemCount > 0) {
                roleRecyclerView.SmoothScrollToPosition(ViewModel.CurrentRoleId - 1);
            }

        }

        public override void OnPause() {
            base.OnPause();
        }

        public override void RefreshUILocale() {
            roleRecyclerView.Visibility = ViewModel.Finished ? ViewStates.Invisible : ViewStates.Visible;
        }



        private class RoleListAdapter : RecyclerView.Adapter {
            private DialogueOptionQuizView chooseBestSentence;
            DialogueOptionQuizViewModel chooseBestSentenceLogic;
            public RoleListAdapter(DialogueOptionQuizView chooseBestSentence) {
                this.chooseBestSentence = chooseBestSentence;
                chooseBestSentenceLogic = chooseBestSentence.ViewModel;
            }

            public override int ItemCount => chooseBestSentenceLogic.RoleCount;

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
                var viewHolder = (ViewHolder)holder;
                viewHolder.View.LayoutParameters.Width = (int)(chooseBestSentence.roleRecyclerView.Width / (float)ItemCount);


                if (!string.IsNullOrWhiteSpace(chooseBestSentenceLogic.GetRoleImageJson(position))) {
                    viewHolder.RoleImageView.SetAnimationFromJson(chooseBestSentenceLogic.GetRoleImageJson(position), "roleImage" + position);
                    viewHolder.RoleImageView.Progress = 0;
                    viewHolder.RoleImageView.RepeatCount = ValueAnimator.Infinite;
                    viewHolder.RoleImageView.RepeatMode = 1;
                    viewHolder.RoleImageView.PlayAnimation();
                }
                
                viewHolder.RoleTextView.Text = chooseBestSentenceLogic.GetRoleName(position);
                if (chooseBestSentenceLogic.IsCurrentRole(position)) {
                    viewHolder.Select();
                } else {
                    viewHolder.Deselect();
                }
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
                var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.dialogue_option_quiz_role_cell, parent, false);
                view.LayoutParameters.Width = (int)(parent.Width / (float)ItemCount);
                var viewHolder = new ViewHolder(view);
                return viewHolder;
            }

            private class ViewHolder : RecyclerView.ViewHolder {
                public View View { get; private set; }
                public LottieAnimationView RoleImageView { get; private set; }
                public TextView RoleTextView { get; private set; }


                private Animation outSet;
                private Animation inSet;

                public ViewHolder(View view) : base(view) {
                    this.View = view;
                    RoleImageView = view.FindViewById<LottieAnimationView>(Resource.Id.roleImageView);
                    RoleTextView = view.FindViewById<TextView>(Resource.Id.roleTextView);
                    inSet = AnimationUtils.LoadAnimation(View.Context, Resource.Animation.ScaleFadeInAndPulse); // new AnimationSet(true) { FillEnabled = true, FillAfter = true };
                    outSet = AnimationUtils.LoadAnimation(View.Context, Resource.Animation.ScaleFadeOut);
                }

                public void Select() {
                    RoleImageView.StartAnimation(inSet);
                }

                public void Deselect() {
                    RoleImageView.StartAnimation(outSet);
                }

            }
        }
    }
}