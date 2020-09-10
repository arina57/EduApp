using System;
using System.Drawing;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Com.Airbnb.Lottie;
using CrossLibrary.Droid.Views;
using SharedActivities.Core.ViewModels.Exercises;
using SharedActivities.Core.ViewModels.Exercises.Results;
using SharedActivities.Droid.CustomViews;
using Xamarin.Essentials;


namespace SharedActivities.Droid.Views.Exercises.Results {
    public class WordWebResults : CrossFragment<WordWebResultsViewModel> {
        private View view;
        private FrameLayout topFrame;
        private TextView resultsTitleTextView;
        private RecyclerView matchPhraseOptions;
        private RecyclerView mainPhraseOptions;
        private MatchPhraseOptionsAdapter matchPhraseOptionsAdapter;
        private MainPhraseOptionsAdapter mainPhraseOptionsAdapter;
        private LineDrawingView lineDrawingView;
        public WordWebResults() {
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            view = inflater.Inflate(Resource.Layout.wordweb_results, container, false);
            topFrame = view.FindViewById<FrameLayout>(Resource.Id.topFrame);
            resultsTitleTextView = view.FindViewById<TextView>(Resource.Id.resultsTitleTextView);
            matchPhraseOptions = view.FindViewById<RecyclerView>(Resource.Id.matchPhraseOptions);
            mainPhraseOptions = view.FindViewById<RecyclerView>(Resource.Id.mainPhraseOptions);
            matchPhraseOptionsAdapter = new MatchPhraseOptionsAdapter(ViewModel.WordWebViewModel);
            mainPhraseOptionsAdapter = new MainPhraseOptionsAdapter(ViewModel.WordWebViewModel);
            lineDrawingView = view.FindViewById<LineDrawingView>(Resource.Id.lineDrawingView);
            matchPhraseOptions.SetLayoutManager(new LinearLayoutManager(this.Activity));
            matchPhraseOptions.SetAdapter(matchPhraseOptionsAdapter);
            matchPhraseOptions.OverScrollMode = OverScrollMode.Never;
            //matchPhraseOptions.Drag += View_Drag;
            matchPhraseOptions.Tag = -1;
            mainPhraseOptions.SetLayoutManager(new LinearLayoutManager(this.Activity));
            mainPhraseOptions.SetAdapter(mainPhraseOptionsAdapter);
            mainPhraseOptions.OverScrollMode = OverScrollMode.Never;


            view.ViewTreeObserver.GlobalLayout += ViewTreeObserver_GlobalLayout;
            return view;
        }

        bool first = true;
        private void ViewTreeObserver_GlobalLayout(object sender, EventArgs e) {
            if (first) {
                //On the first Global layout change the heights of the views.
                first = false;
                topFrame.LayoutParameters.Height = Convert.ToInt32(view.Height * 0.75);
                mainPhraseOptions.LayoutParameters.Height = Convert.ToInt32(view.Height * 0.9);
                matchPhraseOptions.LayoutParameters.Height = Convert.ToInt32(view.Height * 0.9);
                matchPhraseOptionsAdapter.NotifyDataSetChanged();
                mainPhraseOptionsAdapter.NotifyDataSetChanged();
            } else {
                //On the second layout remove the listener and refresh the lines.
                view.ViewTreeObserver.GlobalLayout -= ViewTreeObserver_GlobalLayout;
                RefreshLines();
            }
        }


        private void RefreshLines() {
            FindConnectorDotsPositions();
            lineDrawingView.Lines = ViewModel.WordWebViewModel.GetLinesForAnswers();
            lineDrawingView.Invalidate();
        }



        public override void RefreshUILocale() {
            resultsTitleTextView.Text = ViewModel.WordWebViewModel.TitleText;
            matchPhraseOptionsAdapter.NotifyDataSetChanged();
            mainPhraseOptionsAdapter.NotifyDataSetChanged();
            RefreshLines();
        }

        private void FindConnectorDotsPositions() {
            for (int mainPhrase = 0; mainPhrase < mainPhraseOptions.ChildCount; mainPhrase++) {
                var cellView = mainPhraseOptions.GetChildAt(mainPhrase);
                var recycler = cellView.FindViewById<RecyclerView>(Resource.Id.lineConnectorRecyclerView);
                for (int mainPhraseSubPoint = 0; mainPhraseSubPoint < recycler.ChildCount; mainPhraseSubPoint++) {
                    var innerCellView = recycler.GetChildAt(mainPhraseSubPoint);
                    var mainPhraseDotView = innerCellView.FindViewById<LottieAnimationView>(Resource.Id.lineConnectorImageView);
                    var pos = mainPhraseDotView.GetRectRelativeTo(lineDrawingView).Center;
                    ViewModel.WordWebViewModel.SetLinePositionForMainPhrase(mainPhrase, mainPhraseSubPoint, pos);
                }
            }
            for (int matchPhrase = 0; matchPhrase < matchPhraseOptions.ChildCount; matchPhrase++) {
                var matchCellView = matchPhraseOptions.GetChildAt(matchPhrase);
                var matchPhraseDotView = matchCellView.FindViewById<LottieAnimationView>(Resource.Id.lineConnectorImageView);
                var pos = matchPhraseDotView.GetRectRelativeTo(lineDrawingView).Center;
                ViewModel.WordWebViewModel.SetLinePositionForMatch(matchPhrase, pos);
            }
        }
        private class MainPhraseOptionsAdapter : RecyclerView.Adapter {
            private WordWebViewModel viewModel;
            RecyclerView recyclerView;
            private RecyclerView.RecycledViewPool viewPool = new RecyclerView.RecycledViewPool();
            public MainPhraseOptionsAdapter(WordWebViewModel viewModel) {
                this.viewModel = viewModel;
            }


            public override int ItemCount => viewModel.MainPhraseCount;

            public override void OnAttachedToRecyclerView(RecyclerView recyclerView) {
                base.OnAttachedToRecyclerView(recyclerView);
                this.recyclerView = recyclerView;
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
                var viewHolder = (MainPhraseCellViewHolder)holder;
                viewHolder.ItemView.LayoutParameters.Height = recyclerView.MeasuredHeight / ItemCount;
                viewHolder.LineConnectorRecyclerView.GetAdapter().NotifyDataSetChanged();
                viewHolder.TextView.Text = viewModel.GetMainPhrase(position);

                if (viewModel.Finished) {
                    viewHolder.CorrectView.Visibility = ViewStates.Visible;
                    viewHolder.CorrectView.SetAnimationFromJson(Core.Resx.Lottie.marubatsu, "marubatsu");
                    viewHolder.CorrectView.Progress = viewModel.QuestionAnsweredCorrectly(position) ? 0 : 1;
                    viewHolder.CorrectView.Alpha = 0.5f;
                } else {
                    viewHolder.CorrectView.Visibility = ViewStates.Invisible;
                }


            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
                var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.wordweb_left_cell, parent, false);
                var viewHolder = new MainPhraseCellViewHolder(view, viewModel);
                view.LayoutParameters.Height = parent.MeasuredHeight / ItemCount;
                viewHolder.LineConnectorRecyclerView.SetRecycledViewPool(viewPool);

                return viewHolder;
            }
        }

        private class MainPhraseCellViewHolder : RecyclerView.ViewHolder {
            public TextView TextView { get; private set; }
            public LottieAnimationView CorrectView { get; private set; }
            public RecyclerView LineConnectorRecyclerView { get; private set; }

            public MainPhraseCellViewHolder(View view, WordWebViewModel viewModel) : base(view) {
                this.TextView = view.FindViewById<TextView>(Resource.Id.textView);
                this.LineConnectorRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.lineConnectorRecyclerView);
                CorrectView = view.FindViewById<LottieAnimationView>(Resource.Id.correctView);
                LineConnectorRecyclerView.SetLayoutManager(new LinearLayoutManager(view.Context, LinearLayoutManager.Vertical, false));
                LineConnectorRecyclerView.SetAdapter(new LineConnectorAdapter(this, viewModel));
            }


        }
        private class LineConnectorAdapter : RecyclerView.Adapter {
            private MainPhraseCellViewHolder mainPhraseCellViewHolder;
            private WordWebViewModel viewModel;
            Color Color => viewModel.MainPhraseColor(mainPhraseCellViewHolder.AdapterPosition);
            public LineConnectorAdapter(MainPhraseCellViewHolder mainPhraseCellViewHolder, WordWebViewModel viewModel) {
                this.mainPhraseCellViewHolder = mainPhraseCellViewHolder;
                this.viewModel = viewModel;
            }

            public override int ItemCount => viewModel.PossibleMatches(mainPhraseCellViewHolder.AdapterPosition);

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
                var viewHolder = (LineConnectorViewHolder)holder;
                viewHolder.ItemView.LayoutParameters.Height = mainPhraseCellViewHolder.LineConnectorRecyclerView.MeasuredHeight / ItemCount;
                viewHolder.Color = Color;
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
                var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.wordweb_left_cell_connector, parent, false);
                var viewHolder = new LineConnectorViewHolder(view);
                view.LayoutParameters.Height = parent.MeasuredHeight / ItemCount;
                viewHolder.LineConnectorImageView.SetAnimationFromJson(viewModel.RippleImageJson, "RippleImageJson");
                return viewHolder;
            }

        }
        private class LineConnectorViewHolder : RecyclerView.ViewHolder {


            public LottieAnimationView LineConnectorImageView { get; private set; }
            private Color color;
            public Color Color {
                get => color;
                set {
                    color = value;
                    LineConnectorImageView.SetColor(color.ToPlatformColor());
                }
            }


            public LineConnectorViewHolder(View view) : base(view) {
                this.LineConnectorImageView = view.FindViewById<LottieAnimationView>(Resource.Id.lineConnectorImageView);
                LineConnectorImageView.RepeatMode = LottieDrawable.Restart;
                LineConnectorImageView.RepeatCount = 0;
                LineConnectorImageView.Speed = 1f;
            }


        }


        private class MatchPhraseOptionsAdapter : RecyclerView.Adapter {
            RecyclerView recyclerView;
            private WordWebViewModel viewModel;

            public override void OnAttachedToRecyclerView(RecyclerView recyclerView) {
                base.OnAttachedToRecyclerView(recyclerView);
                this.recyclerView = recyclerView;
            }

            public MatchPhraseOptionsAdapter(WordWebViewModel viewModel) {
                this.viewModel = viewModel;
            }

            public override int ItemCount => viewModel.MatchCount;

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
                var viewHolder = (PoolLineMatchCellViewHolder)holder;
                viewHolder.ItemView.LayoutParameters.Height = recyclerView.MeasuredHeight / ItemCount;
                viewHolder.TextView.Text = viewModel.GetMatchPhrase(position);
                viewHolder.Color = viewModel.GetMatchColor(position);
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
                var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.wordweb_right_cell, parent, false);
                var viewHolder = new PoolLineMatchCellViewHolder(view);
                view.LayoutParameters.Height = parent.MeasuredHeight / ItemCount;
                viewHolder.LineConnectorImageView.SetAnimationFromJson(viewModel.RippleImageJson, "RippleImageJson");
                return viewHolder;
            }






        }


        private class PoolLineMatchCellViewHolder : RecyclerView.ViewHolder {
            public TextView TextView { get; private set; }
            public LottieAnimationView LineConnectorImageView { get; private set; }
            private Color color;
            public Color Color {
                get => color;
                set {
                    color = value;
                    LineConnectorImageView.SetColor(color.ToPlatformColor());
                }
            }

            public void ClearColor() {
                LineConnectorImageView.ClearColorFilter();
                color = Color.Transparent;
            }

            public PoolLineMatchCellViewHolder(View view) : base(view) {
                this.TextView = view.FindViewById<TextView>(Resource.Id.textView);
                this.LineConnectorImageView = view.FindViewById<LottieAnimationView>(Resource.Id.lineConnectorImageView);
                LineConnectorImageView.RepeatMode = LottieDrawable.Restart;
                LineConnectorImageView.RepeatCount = 0;
                LineConnectorImageView.Speed = 1f;


            }
        }
    }
}
