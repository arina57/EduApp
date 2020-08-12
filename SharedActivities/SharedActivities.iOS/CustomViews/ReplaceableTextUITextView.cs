using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using CoreGraphics;
using Foundation;
using SharedActivities.Core;
using UIKit;
using Xamarin.Essentials;

namespace SharedActivities.iOS.CustomViews {
    [Register("ReplaceableTextUITextView"), DesignTimeVisible(true)]
    public class ReplaceableTextUITextView : UITextView {
        public event EventHandler<ReplaceTextClickedEventArgs> LinkClicked;
        public event EventHandler<ReplaceTextDraggedEventArgs> LinkDragged;
        public event EventHandler NonLinkClicked;
        public event EventHandler Clicked;
        //public bool AutoHeight { get; set; } = true;

        private TagFinder tagfinder;
        public bool IsLabel { get; set; }

        public UIFont LinkFont { get; set; } = UIFont.SystemFontOfSize(17);
        public UIColor LinkColor { get; set; } = UIColor.Blue;
        public UIColor LinkBackgroundColor { get; set; } = UIColor.Clear;
        public bool LinkUnderlined { get; set; } = true;

        public UIFont NonLinkFont { get; set; } = UIFont.SystemFontOfSize(17);
        public UIColor NonLinkColor { get; set; } = UIColor.Black;
        private UITapGestureRecognizer tapGesture = new UITapGestureRecognizer();
        private UIPanGestureRecognizer dragGesture = new UIPanGestureRecognizer();

        readonly static Regex replacableTextPattern = new Regex(@"\{(.*?)\}");
        //private string _text;
        public bool AllowTouchThroughNonLink { get; set; } = true;

        private Dictionary<int, UIColor> linkBackgroundColor = new Dictionary<int, UIColor>();


        public ReplaceableTextUITextView(IntPtr handle) : base(handle) {
            Initialise();
        }

        public ReplaceableTextUITextView() {
            Initialise();

        }


        /// <summary>
        /// Set an individual link's background color
        /// </summary>
        public void SetLinkBackgroundColor(int index, Color color) {
            SetLinkBackgroundColor(index, color.ToPlatformColor());
        }

        /// <summary>
        /// Set an individual link's background color
        /// </summary>
        public void SetLinkBackgroundColor(int index, UIColor color) {
            if (!linkBackgroundColor.ContainsKey(index) ||
                linkBackgroundColor.ContainsKey(index) && linkBackgroundColor[index] != color) {
                linkBackgroundColor[index] = color;
                GatherLinkForText();
            }
        }

        /// <summary>
        /// Removes a link's individual background colours and sets it back to LinkBackgroundColor
        /// </summary>
        public void RemoveLinkBackgroundColor(int index) {
            if (linkBackgroundColor.ContainsKey(index)) {
                linkBackgroundColor.Remove(index);
                GatherLinkForText();
            }
        }

        /// <summary>
        /// Removes the individual background colours and sets it back to LinkBackgroundColor
        /// </summary>
        public void RemoveLinkBackgroundColors() {
            if (linkBackgroundColor.Count > 0) {
                linkBackgroundColor.Clear();
                GatherLinkForText();
            }
        }


        public new string Text {
            get { return base.Text; }
            set {
                tagfinder = new TagFinder(value, replacableTextPattern);
                GatherLinkForText();
                LayoutSubviews();
            }
        }

        public int ReplaceableTextCount => tagfinder.MatchCount;

        public string TaggedText => tagfinder.TaggedText;

        //public override void AddGestureRecognizer(UIGestureRecognizer gestureRecognizer) {
        //	base.AddGestureRecognizer(gestureRecognizer);
        //}

        public override void LayoutSubviews() {
            base.LayoutSubviews();
            TextContainerInset = UIEdgeInsets.Zero;
            TextContainer.LineFragmentPadding = 0;
            //if (AutoHeight && (Constraints == null || Constraints.Length < 1)) {
            //    AutoAdjustHeight();
            //}

        }


    

        private void Initialise() {
            TextColor = NonLinkColor;
            Font = NonLinkFont;
            TextAlignment = UITextAlignment.Left;
            Editable = false;
            SelectedRange = new NSRange(0, 0);
            Selectable = false;
            ScrollEnabled = false;
            TextContainer.MaximumNumberOfLines = 1000;
            TextContainer.LineBreakMode = UILineBreakMode.TailTruncation;
            tapGesture.AddTarget(() => CheckTapAtPosition(tapGesture.LocationOfTouch(0, this)));
            this.AddGestureRecognizer(tapGesture);
        }

        public void ReplaceText(int indexToReplace, string textToReplace) {
            tagfinder.ReplaceTextAtLocation(indexToReplace, textToReplace);
            GatherLinkForText();
        }



        private void GatherLinkForText() {
            //base.Text = tagfinder.Text;
            var atts = new UIStringAttributes();
            //atts.BackgroundColor = UIColor.Black;
            var attributedString = new NSMutableAttributedString(tagfinder.Text, atts);
            attributedString.BeginEditing();
            foreach (var location in tagfinder.TextLocations) {
                var range = new NSRange(location.Start, location.Length);
                if (location.IsAMatch) {
                    var background = linkBackgroundColor.ContainsKey(location.MatchNumber) ? linkBackgroundColor[location.MatchNumber] : LinkBackgroundColor;

                    attributedString.AddAttribute(UIStringAttributeKey.ForegroundColor, LinkColor, range);
                    attributedString.AddAttribute(UIStringAttributeKey.BackgroundColor, background, range);

                    attributedString.AddAttribute(UIStringAttributeKey.Font, LinkFont, range);
                    if (LinkUnderlined) {
                        attributedString.AddAttribute(UIStringAttributeKey.UnderlineStyle, NSNumber.FromInt32((int)NSUnderlineStyle.Single), range);
                    }
                } else {
                    attributedString.AddAttribute(UIStringAttributeKey.ForegroundColor, NonLinkColor, range);
                    attributedString.AddAttribute(UIStringAttributeKey.Font, NonLinkFont, range);
                }
            }
            attributedString.EndEditing();
            this.AttributedText = attributedString;
        }



        public TagFinder.TextLocation GetTextLocation(CGPoint pos) {
            pos.Y += this.ContentOffset.Y;
            UITextPosition tapPos = this.GetClosestPositionToPoint(pos);
            var tokenizer = this.Tokenizer;
            if (tokenizer == null) {
                tokenizer = new MyUITextInputStringTokenizer(this.WeakTokenizer.Handle);
            }
            UITextRange textRange = tokenizer.GetRangeEnclosingPosition(tapPos, UITextGranularity.Word, UITextDirection.Forward);
            int position = (int)this.GetOffsetFromPosition(this.BeginningOfDocument, tapPos);
            var location = tagfinder.GetTextLocationAtPosition(position);
            return location;
        }

        public TagFinder.TextLocation CheckTapAtPosition(CGPoint pos) {
            var location = GetTextLocation(pos);
            if (location != null && location.IsAMatch) {
                LinkClicked?.Invoke(this, new ReplaceTextClickedEventArgs(location.MatchNumber, tagfinder));
            } else {
                NonLinkClicked?.Invoke(this, new EventArgs());

            }
            Clicked?.Invoke(this, new EventArgs());
            return location;
            //String word = string.Empty;
            //if (textRange != null) {
            //	word = this.TextInRange(textRange);
            //}
            //return word;
        }

        bool firstHit = true;
        public override UIView HitTest(CGPoint point, UIEvent uievent) {

            if (!AllowTouchThroughNonLink) {
                return base.HitTest(point, uievent);
            }

            TagFinder.TextLocation location = null;

            if (firstHit) {
                location = CheckTapAtPosition(point);
            }
            firstHit = !firstHit;
            if (location != null && location.IsAMatch) {
                return this;
            } else {
                var view = base.HitTest(point, uievent);
                return view == this ? null : view;
            }

        }

        public class ReplaceTextDraggedEventArgs {
            public CGPoint DragPoint;
            public int Index;
            public string Text;
            public UIPanGestureRecognizer DragGesture;
        }
    }


}
