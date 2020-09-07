using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Content;
using Android.Runtime;

using Android.Util;
using Android.Views;
using Android.Views.Animations;
using AndroidX.RecyclerView.Widget;
using Com.Airbnb.Lottie;
using SharedActivities.Core.CrossPlatformInterfaces;
using static SharedActivities.Core.GlobalEnums;

namespace SharedActivities.Droid.CustomViews {

    [Register("sharedactivities.droid.customcontrols.DiscreteProgressView")]
    public class DiscreteProgressView : RecyclerView {
        public event EventHandler LastAnimationFinished;
        public bool Finished { get; private set; } = false;
        private IDiscreteProgressTracker discreteProgressTracker;
        private ScaleAnimation select;
        private Animation deselect;
        private ScaleAnimation deselected;
        private DiscreteProgressViewAdapter discreteProgressViewAdapter;
        private bool first = true;

        public IDiscreteProgressTracker ProgressTracker {
            get => discreteProgressTracker;
            set {
                if (discreteProgressTracker != null) {
                    discreteProgressTracker.ProgressChanged -= StateChanged;
                    discreteProgressTracker.QuizReset -= StateChanged;
                }
                discreteProgressTracker = value;
                discreteProgressTracker.ProgressChanged += StateChanged;
                discreteProgressTracker.QuizReset += StateChanged;
            }
        }

        private void StateChanged(object sender, EventArgs e) {

            discreteProgressViewAdapter.NotifyItemChanged(discreteProgressTracker.CurrentQuestionNumber);
            discreteProgressViewAdapter.NotifyItemChanged(discreteProgressTracker.CurrentQuestionNumber - 1);
        }

        public DiscreteProgressView(Context context) : base(context) {
        }

        public DiscreteProgressView(Context context, IAttributeSet attrs) : base(context, attrs) {
            Init();
        }


        public DiscreteProgressView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) {
            Init();
        }


        protected DiscreteProgressView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) {
            Init();
        }

        private void Init() {
            //NotDoneImage = Context.GetDrawable(Resource.Drawable.notdone);
            //Correctmage = Context.GetDrawable(Resource.Drawable.correct);
            //IncorrectImage = Context.GetDrawable(Resource.Drawable.incorrect);
            InitView();
        }

        private void InitView() {
            Finished = false;
            select = new ScaleAnimation(0.8f, 1f, 0.8f, 1f, Dimension.RelativeToSelf, 0.5f, Dimension.RelativeToSelf, 0.5f) { FillEnabled = true, FillAfter = true, Duration = 500 };
            deselect = new ScaleAnimation(1f, 0.8f, 1f, 0.8f, Dimension.RelativeToSelf, 0.5f, Dimension.RelativeToSelf, 0.5f) { FillEnabled = true, FillAfter = true, Duration = 500 };
            deselected = new ScaleAnimation(0.8f, 0.8f, 0.8f, 0.8f, Dimension.RelativeToSelf, 0.5f, Dimension.RelativeToSelf, 0.5f) { FillEnabled = true, FillAfter = true, Duration = 0 };
            deselected.AnimationEnd += this.Deselected_AnimationEnd;
            this.SetLayoutManager(new LinearLayoutManager(this.Context, LinearLayoutManager.Horizontal, false));
            discreteProgressViewAdapter = new DiscreteProgressViewAdapter(this);
            this.SetAdapter(discreteProgressViewAdapter);
        }

        protected override void OnAttachedToWindow() {
            base.OnAttachedToWindow();
            if (first) {
                first = false;
                //InitView();
            }
            discreteProgressViewAdapter.NotifyDataSetChanged();
        }

        private void Deselected_AnimationEnd(object sender, Animation.AnimationEndEventArgs e) {
            if (ProgressTracker.Finished) {
                Finished = true;
                LastAnimationFinished?.Invoke(this, new EventArgs());
            }
        }




        private class DiscreteProgressViewAdapter : Adapter {
            private DiscreteProgressView discreteProgressView;
            private string json;
            private int correctFrame;
            private int incorrectFrame;
            private int notdoneframe;

            public DiscreteProgressViewAdapter(DiscreteProgressView discreteProgressView) {
                this.discreteProgressView = discreteProgressView;
                json = Core.Resx.Lottie.marubatsu;
            }

            public override int ItemCount => discreteProgressView.ProgressTracker == null ? 0 : discreteProgressView.ProgressTracker.TotalNumberOfQuestions;

            public async override void OnBindViewHolder(ViewHolder holder, int position) {
                var viewHolder = (ImageViewHolder)holder;

                if (viewHolder.ImageView.Composition == null) {
                    await viewHolder.ImageView.SetAnimationFromJsonAsync(json, json);
                    correctFrame = Convert.ToInt32(viewHolder.ImageView.Composition.StartFrame);
                    incorrectFrame = Convert.ToInt32(viewHolder.ImageView.Composition.EndFrame);
                    notdoneframe = Convert.ToInt32(viewHolder.ImageView.Composition.EndFrame / 2);
                }




                if (discreteProgressView.ProgressTracker.AnswerProgress(position) == ProgressState.Correct) {
                    viewHolder.ImageView.Frame = correctFrame;
                } else if (discreteProgressView.ProgressTracker.AnswerProgress(position) == ProgressState.Incorrect) {
                    viewHolder.ImageView.Frame = incorrectFrame;
                } else {
                    viewHolder.ImageView.Frame = notdoneframe;
                }
                var animationTasks = new List<Task>();
                if (discreteProgressView.ProgressTracker.CurrentQuestionNumber == position) {
                    viewHolder.ImageView.Animation = discreteProgressView.select;
                } else if (discreteProgressView.ProgressTracker.CurrentQuestionNumber - 1 == position) {
                    viewHolder.ImageView.Animation = discreteProgressView.deselect;
                    viewHolder.ImageView.Frame = notdoneframe;
                    if (discreteProgressView.ProgressTracker.AnswerProgress(position) == ProgressState.Correct) {
                        viewHolder.ImageView.SetMinFrame(correctFrame);
                        viewHolder.ImageView.SetMaxFrame(notdoneframe);
                        viewHolder.ImageView.Speed = -1;

                    } else {
                        viewHolder.ImageView.SetMinFrame(notdoneframe);
                        viewHolder.ImageView.SetMaxFrame(incorrectFrame);
                        viewHolder.ImageView.Speed = 1;
                    }
                    animationTasks.Add(viewHolder.ImageView.PlayAnimationAsync());
                } else {
                    viewHolder.ImageView.Animation = discreteProgressView.deselected;
                }
                animationTasks.Add(viewHolder.ImageView.Animation?.StartAsync());
                await Task.WhenAll(animationTasks);
                if (discreteProgressView.ProgressTracker.CurrentQuestionNumber != position) {
                    viewHolder.ImageView.Animation = discreteProgressView.deselected;
                }

            }

            public override ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
                var view = new LottieAnimationView(parent.Context);
                if (discreteProgressView.LayoutParameters.Width == ViewGroup.LayoutParams.WrapContent) {
                    view.LayoutParameters.Width = ViewGroup.LayoutParams.WrapContent;
                } else {
                    view.LayoutParameters.Width = (int)(parent.Width / (float)ItemCount);
                }
                var viewHolder = new ImageViewHolder(view);
                return viewHolder;
            }

            private class ImageViewHolder : ViewHolder {
                public View View { get; private set; }
                public LottieAnimationView ImageView { get; private set; }
                public ImageViewHolder(LottieAnimationView view) : base(view) {
                    ImageView = view;
                }
            }
        }
    }
}