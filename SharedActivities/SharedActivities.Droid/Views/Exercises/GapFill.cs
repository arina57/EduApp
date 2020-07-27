using System;
using Android.Widget;
using CrossLibrary.Droid.Views;
using SharedActivities.Core.ViewModels.Exercises;
using AndroidX.RecyclerView.Widget;
using Android.OS;
using Android.Views;
using System.Drawing;
using Xamarin.Essentials;
using SharedActivities.Core;
using SharedActivities.Droid.CustomViews;
using Com.Airbnb.Lottie;

namespace SharedActivities.Droid.Views.Exercises {
    public class GapFill : CrossFragment<GapFillViewModel> {
        private TextView invisibleTextView;
        //private LottieAnimationView doneIcon;
        private RecyclerView gapfillRecyclerView;
        private RecyclerView vocabRecyclerView;
        private VocabRecyclerAdapter vocabRecyclerAdapter;
        private GapfillRecyclerAdapter gapfillRecyclerAdapter;
        private int draggedItemId;
        //private Task<LottieDrawable> doneIconImage = Functions.LottieDrawableFromJsonStringAsync(Resx.Lottie.round_check_box_solidcheck, "Resx.Lottie.round_check_box_solidcheck", (GlobalColorPalette.Light, "Background"));
        public IExerciseLogic ExerciseLogic => ViewModel;
        public GapFill() {
        }

        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            var view = inflater.Inflate(Resource.Layout.gap_fill, container, false);
            invisibleTextView = view.FindViewById<TextView>(Resource.Id.invisibleTextView);
            //doneIcon = view.FindViewById<LottieAnimationView>(Resource.Id.doneLottieView);

            gapfillRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.gapfillRecyclerView);
            vocabRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.vocabRecyclerView);


            invisibleTextView.SetTextColor(Color.White.ToPlatformColor());
            invisibleTextView.SetBackgroundColor(GlobalColorPalette.Light.ToPlatformColor());

            vocabRecyclerView.SetLayoutManager(new GridLayoutManager(this.Activity, 3));
            vocabRecyclerAdapter = new VocabRecyclerAdapter(this);
            this.vocabRecyclerView.SetAdapter(vocabRecyclerAdapter);
            vocabRecyclerView.Tag = -1;
            vocabRecyclerView.Drag += View_Drag;
            view.Drag += View_Drag;
            gapfillRecyclerView.Drag += View_Drag;
            gapfillRecyclerView.SetLayoutManager(new LinearLayoutManager(this.Activity));
            gapfillRecyclerAdapter = new GapfillRecyclerAdapter(this);
            this.gapfillRecyclerView.SetAdapter(gapfillRecyclerAdapter);

            return view;
        }


        public override void OnPause() {
            base.OnPause();
        }

        public override void OnResume() {
            base.OnResume();
            //doneIcon.SetImageDrawable(await doneIconImage);
        }
        public override void RefreshUILocale() {
        }

        private void View_Touch(object sender, View.TouchEventArgs e) {
            var view = (TextView)sender;
            view.Visibility = ViewStates.Invisible;
            draggedItemId = (int)view.Tag;
            View.DragShadowBuilder shadowBuilder = new View.DragShadowBuilder(view);
            view.StartDragAndDropCompat(null, shadowBuilder, view, 0);
            e.Handled = true;

        }

        private void ReplaceableSpanTextView_LinkTouched(object sender, ReplaceTextClickedEventArgs e) {
            var view = (ReplaceableSpanTextView)sender;
            var viewPosition = (int)view.Tag;
            var gapPosition = e.Index;

            draggedItemId = ViewModel.GetAnswerTagIndex(viewPosition, gapPosition);
            if (draggedItemId > -1) {
                //invisibleTextView.SetTextColor(Color.Blue);
                invisibleTextView.Text = e.TagFinder.MatchTextLocations[gapPosition].Value;
                View.DragShadowBuilder shadowBuilder = new View.DragShadowBuilder(invisibleTextView);
                invisibleTextView.StartDragAndDropCompat(null, shadowBuilder, invisibleTextView, 0);
            }
            view.ReplaceText(gapPosition, ViewModel.BlankTextString);
        }


        public void View_Drag(object sender, View.DragEventArgs e) {
            var view = sender as View;

            var action = e.Event.Action;
            var replaceableSpanTextView = view as ReplaceableSpanTextView;
            switch (action) {
                case DragAction.Started:

                    break;
                case DragAction.Entered:
                    view?.Background?.SetColorFilter(ViewModel.HoveredLinkColor.ToPlatformColor(), Android.Graphics.PorterDuff.Mode.SrcIn);

                    view?.Invalidate();
                    break;
                case DragAction.Drop:
                    var phraseIndex = (int)view.Tag; //This should be the id of the mainphrase or -1 for the match recycler view
                    if (replaceableSpanTextView != null) {
                        var gapIndex = replaceableSpanTextView.GetSpanIndexFromLocation((int)e.Event.GetX(), (int)e.Event.GetY());
                        if (gapIndex > -1) {
                            ViewModel.SetAnswer(phraseIndex, gapIndex, draggedItemId);
                        }
                    } else {
                        ViewModel.RemoveAnswer(draggedItemId);
                    }
                    view?.Background?.ClearColorFilter();
                    view?.Invalidate();
                    break;
                case DragAction.Ended:
                    if (replaceableSpanTextView != null) {
                        replaceableSpanTextView.RemoveLinkBackgroundColors();
                    }
                    vocabRecyclerAdapter.NotifyDataSetChanged();
                    gapfillRecyclerAdapter.NotifyDataSetChanged();
                    break;
                case DragAction.Exited:
                    if (replaceableSpanTextView != null) {
                        replaceableSpanTextView.RemoveLinkBackgroundColors();
                    }
                    view?.Background?.ClearColorFilter();
                    view?.Invalidate();
                    break;
                case DragAction.Location:
                    if (replaceableSpanTextView != null) {
                        var gapIndex = replaceableSpanTextView.GetSpanIndexFromLocation((int)e.Event.GetX(), (int)e.Event.GetY());
                        if (gapIndex > -1) {

                            replaceableSpanTextView.SetLinkBackgroundColor(gapIndex, ViewModel.HoveredLinkColor);
                        } else {
                            replaceableSpanTextView.RemoveLinkBackgroundColors();
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private class VocabRecyclerAdapter : RecyclerView.Adapter {
            private GapFill dialogueGapFill;

            public VocabRecyclerAdapter(GapFill unorderedGapFill) {
                this.dialogueGapFill = unorderedGapFill;
            }

            public override int ItemCount => dialogueGapFill.ViewModel.UnusedTagCount;

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
                var viewHolder = (VocabViewHold)holder;
                viewHolder.TextView.Text = dialogueGapFill.ViewModel.GetUnusedTagString(position);
                viewHolder.TextView.Tag = dialogueGapFill.ViewModel.GetUnusedTagId(position);
                viewHolder.TextView.Visibility = ViewStates.Visible;
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
                var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.gap_fill_vocab_cell, parent, false);
                var viewHolder = new VocabViewHold(view);
                viewHolder.TextView.SetTextColor(Color.White.ToPlatformColor());
                viewHolder.TextView.SetBackgroundColor(GlobalColorPalette.Light.ToPlatformColor());
                viewHolder.TextView.Touch += dialogueGapFill.View_Touch;
                return viewHolder;
            }

            private class VocabViewHold : RecyclerView.ViewHolder {
                public TextView TextView { get; }
                public VocabViewHold(View itemView) : base(itemView) {
                    TextView = itemView.FindViewById<TextView>(Resource.Id.vocabTextView);
                }
            }
        }

        private class GapfillRecyclerAdapter : RecyclerView.Adapter {
            private GapFill dialogueGapFill;

            public GapfillRecyclerAdapter(GapFill dialogueGapFill) {
                this.dialogueGapFill = dialogueGapFill;
            }

            public override int ItemCount => dialogueGapFill.ViewModel.PhraseCount;

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
                var viewHolder = (GapfillViewHolder)holder;
                viewHolder.LineTextView.Tag = position;
                viewHolder.LineTextView.Text = dialogueGapFill.ViewModel.GetPhrase(position);
                for (int i = 0; i < viewHolder.LineTextView.ReplaceableTextCount; i++) {
                    viewHolder.LineTextView.ReplaceText(i, dialogueGapFill.ViewModel.GetAnswerTagString(position, i));
                }
                var json = dialogueGapFill.ViewModel.GetRoleImageJson(position);
                if (string.IsNullOrWhiteSpace(json)) {
                    viewHolder.CharacterImageView.Visibility = ViewStates.Gone;
                } else {
                    viewHolder.CharacterImageView.Visibility = ViewStates.Visible;
                    viewHolder.CharacterImageView.SetAnimationFromJson(json, json);
                }


                viewHolder.RoleNameText.Text = dialogueGapFill.ViewModel.GetRoleName(position);
                viewHolder.RoleNameText.Visibility = string.IsNullOrWhiteSpace(viewHolder.RoleNameText.Text) ? ViewStates.Gone : ViewStates.Visible;
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
                var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.gap_fill_cell, parent, false);
                var viewHolder = new GapfillViewHolder(view);
                view.Drag += dialogueGapFill.View_Drag;
                viewHolder.LineTextView.Drag += dialogueGapFill.View_Drag;
                viewHolder.LineTextView.LinkTouched += dialogueGapFill.ReplaceableSpanTextView_LinkTouched;
                viewHolder.LineTextView.LinkColor = Color.White.ToPlatformColor();
                viewHolder.LineTextView.LinkBackgroundColor = GlobalColorPalette.Light.ToPlatformColor();
                viewHolder.LineTextView.LinkUnderLine = false;
                return viewHolder;
            }
        }
        public class GapfillViewHolder : RecyclerView.ViewHolder {
            public ReplaceableSpanTextView LineTextView { get; }
            public TextView RoleNameText { get; }
            public LottieAnimationView CharacterImageView { get; }

            public GapfillViewHolder(View itemView) : base(itemView) {
                LineTextView = ItemView.FindViewById<ReplaceableSpanTextView>(Resource.Id.lineTextView);
                RoleNameText = ItemView.FindViewById<TextView>(Resource.Id.roleTextView);
                CharacterImageView = itemView.FindViewById<LottieAnimationView>(Resource.Id.characterImageView);
            }
        }

    }
}
