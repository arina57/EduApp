using System;
using System.Collections.Generic;
using System.Drawing;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.RecyclerView.Widget;
using Com.Airbnb.Lottie;
using CrossLibrary.Droid.Views;
using SharedActivities.Core.Models;
using SharedActivities.Core.ViewModels.Exercises;
using SharedActivities.Droid.CustomViews;
using Xamarin.Essentials;
using static CrossLibrary.MathAndGeometry;

namespace SharedActivities.Droid.Views.Exercises {
    public class WordWeb : CrossFragment<WordWebViewModel>, ViewTreeObserver.IOnGlobalLayoutListener {
        private View dragHackView;
        private RecyclerView matchPhraseOptions;
        private RecyclerView mainPhraseOptions;
        //private LottieAnimationView doneIcon;
        private ConstraintLayout view;
        private MatchPhraseOptionsAdapter matchPhraseOptionsAdapter;
        private MainPhraseOptionsAdapter mainPhraseOptionsAdapter;
        private LineDrawingView lineDrawingView;
        private LinearLayoutManager matchPhraseOptionsLayoutManager;



        private LinearLayoutManager mainPhraseOptionsLayoutManager;
        //private Task<LottieDrawable> doneIconImage = Functions.LottieDrawableFromJsonStringAsync(Resx.Lottie.round_check_box_solidcheck, "Resx.Lottie.round_check_box_solidcheck", (GlobalColorPalette.Light, "Background"));

        PoolLineMatchCellViewHolder touchedPoolLineMatchCellViewHolder;
        public IExerciseLogic ExerciseLogic => ViewModel;
        Dictionary<int, MainPhraseCellViewHolder> mainPhraseCellViewHolder = new Dictionary<int, MainPhraseCellViewHolder>();
        public WordWeb() {

        }

        public override void Prepare(WordWebViewModel model) {
            base.Prepare(model);
            this.ViewModel.LineWidth = 6;

        }

        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

            view = inflater.Inflate(Resource.Layout.wordweb, container, false) as ConstraintLayout;
            dragHackView = new View(this.Activity);
            view.AddView(dragHackView);
            matchPhraseOptions = view.FindViewById<RecyclerView>(Resource.Id.matchPhraseOptions);
            mainPhraseOptions = view.FindViewById<RecyclerView>(Resource.Id.mainPhraseOptions);

            lineDrawingView = view.FindViewById<LineDrawingView>(Resource.Id.lineDrawingView);
            matchPhraseOptionsLayoutManager = new LinearLayoutManager(this.Activity);
            matchPhraseOptions.SetLayoutManager(matchPhraseOptionsLayoutManager);
            matchPhraseOptionsAdapter = new MatchPhraseOptionsAdapter(this);
            matchPhraseOptions.SetAdapter(matchPhraseOptionsAdapter);
            matchPhraseOptions.OverScrollMode = OverScrollMode.Never;
            //matchPhraseOptions.Drag += View_Drag;
            matchPhraseOptions.Tag = -1;
            matchPhraseOptions.LayoutChange += this.MatchPhraseOptions_LayoutChange;

            mainPhraseOptionsLayoutManager = new LinearLayoutManager(this.Activity);
            mainPhraseOptions.SetLayoutManager(mainPhraseOptionsLayoutManager);
            mainPhraseOptionsAdapter = new MainPhraseOptionsAdapter(this);
            mainPhraseOptions.SetAdapter(mainPhraseOptionsAdapter);
            mainPhraseOptions.OverScrollMode = OverScrollMode.Never;
            mainPhraseOptions.LayoutChange += this.MainPhraseOptions_LayoutChange;

            view.Drag += View_DragInView;
            view.ViewTreeObserver.AddOnGlobalLayoutListener(this);
            return view;
        }

        private void MatchPhraseOptions_LayoutChange(object sender, View.LayoutChangeEventArgs e) {
            //FindMatchPhraseDots();
        }

        private void MainPhraseOptions_LayoutChange(object sender, View.LayoutChangeEventArgs e) {
            //FindMainPhraseDots();
        }

        public void OnGlobalLayout() {
            view.ViewTreeObserver.RemoveOnGlobalLayoutListener(this);
            RefreshUILocale();
        }

        private void RefreshLines() {
            FindConnectorDotsPositions();
            lineDrawingView.Lines = ViewModel.GetLinesForAnswers();
            lineDrawingView.Invalidate();
        }



        public override void RefreshUILocale() {
            matchPhraseOptionsAdapter.NotifyDataSetChanged();
            mainPhraseOptionsAdapter.NotifyDataSetChanged();
            RefreshLines();
        }

        private void FindConnectorDotsPositions() {
            FindMainPhraseDots();
            FindMatchPhraseDots();
        }

        private void FindMatchPhraseDots() {
            for (int matchPhrase = 0; matchPhrase < matchPhraseOptions.ChildCount; matchPhrase++) {
                var matchCellView = matchPhraseOptions.GetChildAt(matchPhrase);
                var matchPhraseDotView = matchCellView.FindViewById<LottieAnimationView>(Resource.Id.lineConnectorImageView);
                var pos = matchPhraseDotView.GetRectRelativeTo(lineDrawingView).Center;
                ViewModel.SetLinePositionForMatch(matchPhrase, pos);
            }
        }

        private void FindMainPhraseDots() {
            for (int mainPhrase = 0; mainPhrase < mainPhraseOptions.ChildCount; mainPhrase++) {
                var cellView = mainPhraseOptions.GetChildAt(mainPhrase);
                var recycler = cellView.FindViewById<RecyclerView>(Resource.Id.lineConnectorRecyclerView);
                for (int mainPhraseSubPoint = 0; mainPhraseSubPoint < recycler.ChildCount; mainPhraseSubPoint++) {
                    var innerCellView = recycler.GetChildAt(mainPhraseSubPoint);
                    var mainPhraseDotView = innerCellView.FindViewById<LottieAnimationView>(Resource.Id.lineConnectorImageView);
                    var pos = mainPhraseDotView.GetRectRelativeTo(lineDrawingView).Center;
                    ViewModel.SetLinePositionForMainPhrase(mainPhrase, mainPhraseSubPoint, pos);
                }
            }
        }

        public async override void OnResume() {
            base.OnResume();
            lineDrawingView.Lines = ViewModel.GetLinesForAnswers();
            lineDrawingView.Invalidate();
            //doneIcon.SetImageDrawable(await doneIconImage);
        }

        public override void OnPause() {
            base.OnPause();
        }


        private void View_DragInView(object sender, View.DragEventArgs e) {
            if (e.Event.Action != DragAction.Drop && e.Event.Action != DragAction.Ended) {
                if (e.Event.GetX() > 0 && e.Event.GetY() > 0) {
                    lineDrawingView.Line.Stop = new FPoint(e.Event.GetX(), e.Event.GetY());
                }
            } else {
                lineDrawingView.Line = null;
            }
            lineDrawingView.Invalidate();
        }



        private class MainPhraseOptionsAdapter : RecyclerView.Adapter {
            private WordWebViewModel logic;
            WordWeb fragment;
            private RecyclerView.RecycledViewPool viewPool = new RecyclerView.RecycledViewPool();
            public MainPhraseOptionsAdapter(WordWeb fragment) {
                this.logic = fragment.ViewModel;
                this.fragment = fragment;
            }


            public override int ItemCount => logic.MainPhraseCount;

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
                var viewHolder = (MainPhraseCellViewHolder)holder;
                fragment.mainPhraseCellViewHolder[position] = viewHolder;
                viewHolder.ItemView.LayoutParameters.Height = fragment.mainPhraseOptions.MeasuredHeight / ItemCount;
                viewHolder.LineConnectorRecyclerView.GetAdapter().NotifyDataSetChanged();
                viewHolder.TextView.Text = logic.GetMainPhrase(position);

                if (logic.Finished) {
                    viewHolder.CorrectView.Visibility = ViewStates.Visible;
                    viewHolder.CorrectView.SetAnimationFromJson(Core.Resx.Lottie.marubatsu, "marubatsu");
                    viewHolder.CorrectView.Progress = logic.QuestionAnsweredCorrectly(position) ? 0 : 1;
                } else {
                    viewHolder.CorrectView.Visibility = ViewStates.Invisible;
                }


            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
                var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.wordweb_left_cell, parent, false);
                var viewHolder = new MainPhraseCellViewHolder(view, fragment);
                view.LayoutParameters.Height = parent.MeasuredHeight / ItemCount;
                viewHolder.LineConnectorRecyclerView.SetRecycledViewPool(viewPool);

                return viewHolder;
            }
        }

        private class MainPhraseCellViewHolder : RecyclerView.ViewHolder {
            public TextView TextView { get; private set; }
            public LottieAnimationView CorrectView { get; private set; }
            public RecyclerView LineConnectorRecyclerView { get; private set; }

            public MainPhraseCellViewHolder(View view, WordWeb fragment) : base(view) {
                this.TextView = view.FindViewById<TextView>(Resource.Id.textView);
                this.LineConnectorRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.lineConnectorRecyclerView);
                CorrectView = view.FindViewById<LottieAnimationView>(Resource.Id.correctView);
                LineConnectorRecyclerView.SetLayoutManager(new LinearLayoutManager(view.Context, LinearLayoutManager.Vertical, false));
                LineConnectorRecyclerView.SetAdapter(new LineConnectorAdapter(this, fragment));
            }


        }
        private class LineConnectorAdapter : RecyclerView.Adapter {
            private MainPhraseCellViewHolder mainPhraseCellViewHolder;
            private int MainPhrase => mainPhraseCellViewHolder.AdapterPosition;
            private WordWebViewModel ViewModel => fragment.ViewModel;
            LineDrawingView LineDrawingView => fragment.lineDrawingView;
            WordWeb fragment;
            Color Color => ViewModel.MainPhraseColor(MainPhrase);
            public LineConnectorAdapter(MainPhraseCellViewHolder mainPhraseCellViewHolder, WordWeb fragment) {
                this.mainPhraseCellViewHolder = mainPhraseCellViewHolder;
                this.fragment = fragment;
            }

            public override int ItemCount => ViewModel.PossibleMatches(MainPhrase);

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
                var viewHolder = (LineConnectorViewHolder)holder;
                viewHolder.ItemView.LayoutParameters.Height = mainPhraseCellViewHolder.LineConnectorRecyclerView.MeasuredHeight / ItemCount;
                viewHolder.Color = Color;
                var pos = viewHolder.LineConnectorImageView.GetRectRelativeTo(LineDrawingView).Center;
                //ViewModel.SetLinePositionForMainPhrase(MainPhrase, position, pos);
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
                var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.wordweb_left_cell_connector, parent, false);
                var viewHolder = new LineConnectorViewHolder(view);
                view.LayoutParameters.Height = parent.MeasuredHeight / ItemCount;
                viewHolder.LineConnectorImageView.SetAnimationFromJson(ViewModel.RippleImageJson, "RippleImageJson");
                view.Touch += (s, e) => MainPhraseLineConnectorImageView_Touch(MainPhrase, viewHolder);
                view.Drag += (s, e) => MainPhraseLineConnectorImageView_DragInView(MainPhrase, viewHolder, e);

                return viewHolder;
            }


            private void MainPhraseLineConnectorImageView_Touch(int mainPhrase, LineConnectorViewHolder viewHolder) {
                var startPoint = viewHolder.LineConnectorImageView.GetRectRelativeTo(LineDrawingView).Center;
                //logic.DragFromMainPhraseStarted(mainPhrase, viewHolder.AdapterPosition, startPoint);
                ViewModel.DragFromMainPhraseStarted(mainPhrase, viewHolder.AdapterPosition);
                LineDrawingView.Line = new ColoredLine(ViewModel.LineStart, startPoint, ViewModel.LineColor, ViewModel.LineWidth);
                ViewModel.SetLinePositionForMainPhrase(mainPhrase, viewHolder.AdapterPosition, startPoint);
                View.DragShadowBuilder shadowBuilder = new View.DragShadowBuilder(fragment.dragHackView);
                LineDrawingView.StartDragAndDropCompat(null, shadowBuilder, null, 0);
                viewHolder.LineConnectorImageView.PlayAnimation();
            }

            private void MainPhraseLineConnectorImageView_DragInView(int mainPhrase, LineConnectorViewHolder viewHolder, View.DragEventArgs e) {
                LineDrawingView.Lines = ViewModel.GetLinesForAnswers();
                if (!ViewModel.DragStartedFromMainPhrase) {
                    switch (e.Event.Action) {
                        case DragAction.Entered:
                            var pos = viewHolder.LineConnectorImageView.GetRectRelativeTo(LineDrawingView).Center;
                            LineDrawingView.Line = new ColoredLine(ViewModel.LineStart, pos, ViewModel.MainPhraseColor(mainPhrase), ViewModel.LineWidth);
                            fragment.touchedPoolLineMatchCellViewHolder.Color = LineDrawingView.Line.Color;
                            viewHolder.LineConnectorImageView.PlayAnimation();
                            ViewModel.SetLinePositionForMainPhrase(mainPhrase, viewHolder.AdapterPosition, pos);
                            break;
                        case DragAction.Drop:
                            //logic.DropppedInMainPhrase(mainPhrase, viewHolder.AdapterPosition, viewHolder.LineConnectorImageView.GetRectRelativeTo(lineDrawingView).Center);
                            ViewModel.DropppedInMainPhrase(mainPhrase, viewHolder.AdapterPosition);
                            //fragment.mainPhraseOptionsAdapter.NotifyItemChanged(mainPhrase);
                            break;
                        case DragAction.Exited:

                            LineDrawingView.Line = new ColoredLine(ViewModel.LineStart, new FPoint(e.Event.GetX(), e.Event.GetY()), ViewModel.LineColor, ViewModel.LineWidth);
                            //fragment.mainPhraseOptionsAdapter.NotifyItemChanged(mainPhrase);
                            fragment.touchedPoolLineMatchCellViewHolder.Color = ViewModel.GetMatchColor(fragment.touchedPoolLineMatchCellViewHolder.AdapterPosition);
                            viewHolder.LineConnectorImageView.CancelAnimation();
                            viewHolder.LineConnectorImageView.Frame = 0;
                            break;
                        case DragAction.Started:
                            fragment.RefreshLines();
                            break;
                        case DragAction.Ended:
                            //fragment.RefreshUILocale();
                            fragment.RefreshLines();
                            break;
                        case DragAction.Location:
                            break;
                    }
                } else {
                    e.Handled = false;
                }

                LineDrawingView.Invalidate();
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
            private WordWeb fragment;
            private WordWebViewModel ViewModel => fragment.ViewModel;
            private LineDrawingView LineDrawingView => fragment.lineDrawingView;
            public MatchPhraseOptionsAdapter(WordWeb fragment) {
                this.fragment = fragment;
            }

            public override int ItemCount => fragment.ViewModel.MatchCount;

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
                var viewHolder = (PoolLineMatchCellViewHolder)holder;
                viewHolder.ItemView.LayoutParameters.Height = fragment.matchPhraseOptions.MeasuredHeight / ItemCount;
                viewHolder.TextView.Text = fragment.ViewModel.GetMatchPhrase(position);
                viewHolder.Color = ViewModel.GetMatchColor(position);
                var pos = viewHolder.LineConnectorImageView.GetRectRelativeTo(LineDrawingView).Center;
                //fragment.ViewModel.SetLinePositionForMatch(position, pos);
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
                var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.wordweb_right_cell, parent, false);
                var viewHolder = new PoolLineMatchCellViewHolder(view);
                view.LayoutParameters.Height = parent.MeasuredHeight / ItemCount;
                viewHolder.LineConnectorImageView.SetAnimationFromJson(ViewModel.RippleImageJson, "RippleImageJson");
                viewHolder.ItemView.Touch += (s, e) => MatchPhraseLineConnectorImageView_Touch(viewHolder);
                viewHolder.ItemView.Drag += (s, e) => MatchPhraseLineConnectorImageView_DragInView(viewHolder, e);
                return viewHolder;
            }


            private void MatchPhraseLineConnectorImageView_Touch(PoolLineMatchCellViewHolder viewHolder) {
                fragment.touchedPoolLineMatchCellViewHolder = viewHolder;
                var matchPosition = viewHolder.AdapterPosition;
                if (matchPosition > -1) {

                    //logic.DragFromMatchPhraseStarted(matchPosition, viewHolder.LineConnectorImageView.GetRectRelativeTo(LineDrawingView).Center);
                    var rect = viewHolder.LineConnectorImageView.GetRectRelativeTo(LineDrawingView);
                    var startPoint = new FPoint(rect.MidX, rect.MidY);
                    ViewModel.DragFromMatchPhraseStarted(matchPosition, startPoint);
                    LineDrawingView.Line = new ColoredLine(ViewModel.LineStart, startPoint, ViewModel.LineColor, ViewModel.LineWidth);
                    //probably not the best way to do this. Probably doesn't StartDragAndDrop - touch handler would probably work
                    View.DragShadowBuilder shadowBuilder = new View.DragShadowBuilder(fragment.dragHackView);
                    LineDrawingView.StartDragAndDropCompat(null, shadowBuilder, null, 0);
                    viewHolder.LineConnectorImageView.PlayAnimation();
                }
            }
            private void MatchPhraseLineConnectorImageView_DragInView(PoolLineMatchCellViewHolder viewHolder, View.DragEventArgs e) {
                LineDrawingView.Lines = ViewModel.GetLinesForAnswers();
                if (viewHolder.AdapterPosition > -1 && ViewModel.DragStartedFromMainPhrase) {
                    switch (e.Event.Action) {
                        case DragAction.Entered:
                            var pos = viewHolder.LineConnectorImageView.GetRectRelativeTo(LineDrawingView).Center;
                            LineDrawingView.Line = new ColoredLine(ViewModel.LineStart, pos, ViewModel.LineColor, ViewModel.LineWidth);
                            viewHolder.Color = ViewModel.LineColor;
                            viewHolder.LineConnectorImageView.PlayAnimation();
                            fragment.ViewModel.SetLinePositionForMatch(viewHolder.AdapterPosition, pos);
                            break;
                        case DragAction.Drop:
                            //logic.DroppedInMatchPhrase(viewHolder.AdapterPosition, viewHolder.LineConnectorImageView.GetRectRelativeTo(LineDrawingView).Center);
                            ViewModel.DroppedInMatchPhrase(viewHolder.AdapterPosition);
                            break;
                        case DragAction.Exited:
                            //fragment.matchPhraseOptionsAdapter.NotifyItemChanged(viewHolder.AdapterPosition);
                            viewHolder.Color = ViewModel.GetMatchColor(viewHolder.AdapterPosition);
                            viewHolder.LineConnectorImageView.CancelAnimation();
                            viewHolder.LineConnectorImageView.Frame = 0;
                            break;
                        case DragAction.Started:
                            fragment.RefreshLines();
                            break;
                        case DragAction.Ended:
                            //fragment.RefreshUILocale();
                            fragment.RefreshLines();
                            break;
                        case DragAction.Location:
                            break;
                    }
                } else {
                    e.Handled = false;
                }

                LineDrawingView.Invalidate();
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
