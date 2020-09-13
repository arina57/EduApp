using System;
using Android.Graphics;
using Android.OS;

using static AndroidX.ConstraintLayout.Widget.ConstraintLayout;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using CrossLibrary.Droid.Views;
using SharedActivities.Core.ViewModels.Exercises;
using SharedActivities.Droid.Helpers;
using Xamarin.Essentials;

namespace SharedActivities.Droid.Views.Exercises {
    public class PhraseMatch : CrossFragment<PhraseMatchViewModel> {
        private RecyclerView matchPhraseOptions;
        private RecyclerView mainPhraseOptions;
        private MatchPhraseOptionsAdapter matchPhraseOptionsAdapter;
        private MainPhraseOptionsAdapter mainPhraseOptionsAdapter;
        private int draggedItemId;
        private RecyclerViewDragScrollHelper recyclerDragScroller;
        public IExerciseLogic ExerciseLogic => ViewModel;


        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

            var view = inflater.Inflate(Resource.Layout.phrase_match, container, false);
            matchPhraseOptions = view.FindViewById<RecyclerView>(Resource.Id.matchPhraseOptions);
            mainPhraseOptions = view.FindViewById<RecyclerView>(Resource.Id.mainPhraseOptions);


            matchPhraseOptions.SetLayoutManager(new GridLayoutManager(this.Activity, 2));
            matchPhraseOptionsAdapter = new MatchPhraseOptionsAdapter(this);
            matchPhraseOptions.SetAdapter(matchPhraseOptionsAdapter);
            matchPhraseOptions.OverScrollMode = OverScrollMode.Never;
            matchPhraseOptions.Drag += View_DragAsync;
            //matchPhraseOptions.SetOnDragListener(this);
            matchPhraseOptions.Tag = -1;

            mainPhraseOptions.SetLayoutManager(new LinearLayoutManager(this.Activity));
            mainPhraseOptionsAdapter = new MainPhraseOptionsAdapter(this);
            mainPhraseOptions.SetAdapter(mainPhraseOptionsAdapter);
            mainPhraseOptions.OverScrollMode = OverScrollMode.Never;
            mainPhraseOptions.Drag += View_DragAsync;
            recyclerDragScroller = new RecyclerViewDragScrollHelper(mainPhraseOptions);
            return view;
        }





        public async void View_DragAsync(object sender, View.DragEventArgs e) {
            var view = sender as View;
            var action = e.Event.Action;
            await recyclerDragScroller.HandleDragAsync(view, e.Event);
            switch (action) {
                case DragAction.Started:
                    break;
                case DragAction.Entered:
                    view?.Background?.SetColorFilter(Core.GlobalColorPalette.Light.ToPlatformColor(), PorterDuff.Mode.Darken);
                    view?.Invalidate();

                    break;
                case DragAction.Drop:
                    if (view != mainPhraseOptions) {
                        var viewTag = (int)view.Tag; //This should be the id of the mainphrase or -1 for the match recycler view

                        ViewModel.SetAnswer(viewTag, draggedItemId);
                    }
                    break;
                case DragAction.Ended:
                    view?.Background?.ClearColorFilter();
                    view?.Invalidate();
                    matchPhraseOptionsAdapter.NotifyDataSetChanged();
                    mainPhraseOptionsAdapter.NotifyDataSetChanged();
                    break;
                case DragAction.Exited:
                    view?.Background?.ClearColorFilter();
                    view?.Invalidate();
                    break;
                case DragAction.Location:
                    break;
                default:
                    break;
            }
        }






        private void View_Touch(object sender, View.TouchEventArgs e) {
            var view = (View)sender;
            view.Visibility = ViewStates.Invisible;
            draggedItemId = (int)view.Tag;
            View.DragShadowBuilder shadowBuilder = new View.DragShadowBuilder(view);
            view.StartDragAndDropCompat(null, shadowBuilder, view, 0);
            e.Handled = true;

        }

        private void CheckAnswersButton_Click(object sender, EventArgs e) {
            ViewModel.Finish();

        }

        public override void OnResume() {
            base.OnResume();
        }

        public override void RefreshUILocale() {

        }
        public override void OnPause() {
            base.OnPause();
        }

        private class MatchPhraseOptionsAdapter : RecyclerView.Adapter {
            private PhraseMatch fragment;

            public MatchPhraseOptionsAdapter(PhraseMatch fragment) {
                this.fragment = fragment;
            }

            public override int ItemCount => fragment.ViewModel.AvailableMatchCount;

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
                var viewHolder = (MatchPhraseVocabOptionsViewHolder)holder;
                viewHolder.TextView.Tag = fragment.ViewModel.GetUnusedMatchId(position);
                viewHolder.TextView.Text = fragment.ViewModel.GetUnusedMatchPhrase(position);
                viewHolder.TextView.Visibility = ViewStates.Visible;
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
                var view = new TextView(fragment.Activity);
                LayoutParams param = new LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
                param.SetMargins(50, 10, 50, 10);

                view.LayoutParameters = param;
                view.SetPadding(20, 10, 20, 10);
                view.Touch += fragment.View_Touch;
                //view.SetBackgroundColor(Color.LightSeaGreen);
                view.SetBackgroundResource(Resource.Drawable.dashedboxsolid);
                //view.Drag += vocabDefinitionMatch.View_Drag;
                //view.LongClick += this.View_LongClick;
                var viewHolder = new MatchPhraseVocabOptionsViewHolder(view);
                return viewHolder;
            }



        }
        private class MatchPhraseVocabOptionsViewHolder : RecyclerView.ViewHolder {
            public TextView TextView { get; }

            public MatchPhraseVocabOptionsViewHolder(TextView view) : base(view) {
                this.TextView = view;
            }
        }

        private class MainPhraseOptionsAdapter : RecyclerView.Adapter {
            private PhraseMatch fragment;
            private RecyclerView.RecycledViewPool subRecyclerViewViewPool;

            public MainPhraseOptionsAdapter(PhraseMatch fragment) {
                this.fragment = fragment;
                subRecyclerViewViewPool = new RecyclerView.RecycledViewPool();
            }

            public override int ItemCount => fragment.ViewModel.MainPhraseCount;

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
                var viewHolder = (MainPhraseOptionsViewHolder)holder;
                viewHolder.TextView.Text = fragment.ViewModel.GetMainPhrase(position);
                viewHolder.View.Tag = fragment.ViewModel.GetMainPhraseId(position);
                //viewHolder.View.SetBackgroundColor(position % 2 == 0 ? Color.White : Color.LightBlue);
                viewHolder.AnswerAdapter.NotifyDataSetChanged();
                viewHolder.View.Background?.ClearColorFilter();
                fragment.recyclerDragScroller.PrepareCellView(viewHolder);



            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
                var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.phrase_match_cell, parent, false);
                var viewHolder = new MainPhraseOptionsViewHolder(view, fragment);
                viewHolder.MatchingMatchesRecyclerView.SetRecycledViewPool(subRecyclerViewViewPool);
                view.Drag += fragment.View_DragAsync;
                //view.SetOnDragListener(phraseMatchingPoolFragment);
                return viewHolder;
            }
        }

        private class MainPhraseOptionsViewHolder : RecyclerView.ViewHolder {
            public TextView TextView;
            public View View;
            public RecyclerView MatchingMatchesRecyclerView;
            public AnswersMatchesAdapter AnswerAdapter;
            public MainPhraseOptionsViewHolder(View view, PhraseMatch fragment) : base(view) {
                View = view;
                this.TextView = view.FindViewById<TextView>(Resource.Id.textView);
                MatchingMatchesRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.matchingMatchesRecyclerView);
                MatchingMatchesRecyclerView.SetLayoutManager(new LinearLayoutManager(view.Context));
                this.AnswerAdapter = new AnswersMatchesAdapter(this, fragment);
                MatchingMatchesRecyclerView.SetAdapter(AnswerAdapter);
            }

            public class AnswersMatchesAdapter : RecyclerView.Adapter {
                private PhraseMatch fragment;
                MainPhraseOptionsViewHolder mainPhraseOptionsViewHolder;

                public AnswersMatchesAdapter(MainPhraseOptionsViewHolder mainPhraseOptionsViewHolder, PhraseMatch fragment) {
                    this.fragment = fragment;
                    this.mainPhraseOptionsViewHolder = mainPhraseOptionsViewHolder;
                }

                public override int ItemCount => fragment.ViewModel.GetAnswerCount(mainPhraseOptionsViewHolder.LayoutPosition);

                public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
                    var viewHolder = (AnswerMatchesViewHolder)holder;
                    viewHolder.TextView.Tag = fragment.ViewModel.GetAnswerId(mainPhraseOptionsViewHolder.LayoutPosition, position);
                    viewHolder.TextView.Text = fragment.ViewModel.GetAnswerPhrase(mainPhraseOptionsViewHolder.LayoutPosition, position);
                    viewHolder.TextView.Visibility = ViewStates.Visible;
                }

                public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
                    var view = new TextView(fragment.Activity);
                    LayoutParams param = new LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
                    param.SetMargins(20, 10, 20, 10);
                    view.LayoutParameters = param;
                    view.SetPadding(20, 10, 20, 10);
                    //view.SetBackgroundColor(Color.LightSeaGreen);
                    view.SetBackgroundResource(Resource.Drawable.dashedboxsolid);
                    view.Touch += fragment.View_Touch;
                    //view.Drag += vocabDefinitionMatch.View_Drag;
                    //view.LongClick += this.View_LongClick;
                    var viewHolder = new AnswerMatchesViewHolder(view);
                    return viewHolder;
                }

                private class AnswerMatchesViewHolder : RecyclerView.ViewHolder {
                    public TextView TextView { get; }

                    public AnswerMatchesViewHolder(TextView view) : base(view) {
                        this.TextView = view;
                    }
                }
            }
        }

    }


}