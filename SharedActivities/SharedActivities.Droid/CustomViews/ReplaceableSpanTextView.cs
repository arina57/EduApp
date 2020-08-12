using System;
using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using Android.Text;
using Android.Text.Style;
using Android.Util;
using System.Text.RegularExpressions;
using Android.Text.Method;
using Android.Graphics;
using Java.Lang;
using static Android.Graphics.Paint;
using Xamarin.Essentials;
using SharedActivities.Core;

namespace SharedActivities.Droid.CustomViews {
    [Register("sharedlibrary.droid.customcontrols.ReplaceableSpanTextView")]
    public class ReplaceableSpanTextView : TextView  {

        //public Func<string, string> LinkClickedAssign;
        public event EventHandler<ReplaceTextClickedEventArgs> LinkTouched;
        //public event EventHandler NonLinkClicked;
        public event EventHandler ProcessedTextReady;
        public bool LinkUnderLine { get; set; } = true;
        public Color LinkColor { get; set; } = Color.Blue;
        public Color LinkBackgroundColor { get; set; } = Color.Transparent;

        private List<TagSpan> replacementSpans = new List<TagSpan>();
        private SpannableString spannableString;
        readonly static Regex replacableTextPattern = new Regex(@"\{(.*?)\}");
        //private bool linkClicked;
        private TagFinder tagfinder;
        public int ReplaceableTextCount => tagfinder.MatchCount;

        private Dictionary<int, Color> linkBackgroundColor = new Dictionary<int, Color>();


        /// <summary>
        /// Set an individual link's background color
        /// </summary>
        public void SetLinkBackgroundColor(int index, System.Drawing.Color color) {
            SetLinkBackgroundColor(index, color.ToPlatformColor());
        }

        /// <summary>
        /// Set an individual link's background color
        /// </summary>
        public void SetLinkBackgroundColor(int index, Color color) {
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
            get { return tagfinder.Text; }
            set {
                tagfinder = new TagFinder(value, replacableTextPattern);
                GatherLinkForText();
            }
        }

        public void ReplaceText(int indexToReplace, string textToReplace) {
            tagfinder.ReplaceTextAtLocation(indexToReplace, textToReplace);
            GatherLinkForText();
        }

        public ReplaceableSpanTextView(Context context) : base(context) {
            this.Touch += this.ReplaceableTextTextView_Touch;

        }

        public ReplaceableSpanTextView(Context context, IAttributeSet attrs) : base(context, attrs) {
            this.Touch += this.ReplaceableTextTextView_Touch;
        }

        private void ReplaceableTextTextView_Touch(object sender, TouchEventArgs e) {
            var spanIndex = GetSpanIndexFromLocation((int)e.Event.GetX(), (int)e.Event.GetY());

            if (spanIndex > -1) {
                LinkTouched?.Invoke(this, new ReplaceTextClickedEventArgs(spanIndex, tagfinder));
            }

        }



        public int GetSpanIndexFromLocation(int x, int y) {
            for (int i = 0; i < ReplaceableTextCount; i++) {
                if (GetSpanLocation(i).Contains(x, y)) {
                    return i;
                }
            }
            return -1;
        }

        private Rect GetSpanLocation(int spanIndex) {
            var rect = new Rect();
            var span = replacementSpans[spanIndex];


            return span.Rect.ToFloatRect().ToRect();
        }


        private void GatherLinkForText() {
            spannableString = new SpannableString(tagfinder.Text);
            replacementSpans.Clear();

            foreach (var location in tagfinder.TextLocations) {
                if (location.IsAMatch) {
                    var background = linkBackgroundColor.ContainsKey(location.MatchNumber) ? linkBackgroundColor[location.MatchNumber] : LinkBackgroundColor;

                    var clickableSpan = new TagSpan(this) { TextColor = LinkColor, BackgroundColor = background, Underline = LinkUnderLine, HorizontalPadding = 50 };
                    //clickableSpan.Click += (w) => {
                    //    LinkClicked?.Invoke(this, new ReplaceTextClickedEventArgs() { Index = location.MatchNumber, Text = location.Value });
                    //};


                    replacementSpans.Add(clickableSpan);
                    spannableString.SetSpan(clickableSpan, location.Start, location.End + 1, SpanTypes.ExclusiveExclusive);
                    TextFormatted = spannableString;
                } else {
                    //var clickableSpan = new ReplacementSpan();
                    //clickableSpan.Click += (w) =>
                    //{
                    //	NonLinkClicked?.Invoke(this, new EventArgs());
                    //};
                    //spannableString.SetSpan(clickableSpan, location.Start, location.End + 1, SpanTypes.Composing);
                }
            }
            SetText(spannableString, TextView.BufferType.Spannable);
            MovementMethod = new LinkMovementMethod();
            LinksClickable = true;
            ProcessedTextReady?.Invoke(this, new EventArgs());
        }




        /*
	 * This is class which gives us the clicks on the links which we then can use.
	 */
        public class TagSpan : ReplacementSpan {
            private ReplaceableSpanTextView replaceableSpanTextView;

            public bool Underline { get; set; } = true;
            public Color TextColor { get; set; } = Color.Blue;
            public Color BackgroundColor { get; set; } = Color.Transparent;
            public RectF Rect { get; private set; }
            public int HorizontalPadding { get; set; } = 20;
            public int HorizontalMargin { get; set; } = 0;


            public TagSpan(ReplaceableSpanTextView replaceableSpanTextView, Color color, bool underline) : base() {
                this.Rect = new RectF();
                this.Underline = underline;
                this.TextColor = color;
            }

            public TagSpan(ReplaceableSpanTextView replaceableSpanTextView) : base() {
                this.replaceableSpanTextView = replaceableSpanTextView;
                this.Rect = new RectF();
            }

            //public override void UpdateDrawState(TextPaint ds) {
            //    ds.UnderlineText = Underline;
            //    ds.Color = TextColor;
            //    ds.BgColor = BackgroundColor;

            //    //base.UpdateDrawState(ds);
            //}

            public override void Draw(Canvas canvas, ICharSequence text, int start, int end, float x, int top, int y, int bottom, Paint paint) {
                float width = paint.MeasureText(text.SubSequence(start, end).ToString());
                Rect = new RectF(x + HorizontalMargin, top + 2, x + width + HorizontalMargin + HorizontalPadding * 2, bottom);
                paint.Color = BackgroundColor;
                canvas.DrawRect(Rect, paint);
                var textPaint = new TextPaint(paint) { Color = TextColor, UnderlineText = Underline };
                textPaint.TextAlign = Align.Center;
                float textHeight = textPaint.Descent() - textPaint.Ascent();
                float textOffset = (textHeight / 2) - textPaint.Descent();
                canvas.DrawText(text, start, end, Rect.CenterX(), Rect.CenterY() + textOffset, textPaint);
            }

            public override int GetSize(Paint paint, ICharSequence text, int start, int end, Paint.FontMetricsInt fm) {
                return Convert.ToInt32(HorizontalMargin * 2 + HorizontalPadding * 2 + paint.MeasureText(text.SubSequence(start, end).ToString()));
            }
        }

 


    }
}
