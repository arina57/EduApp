using System;
using Android.Content;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using Android.Content.Res;
using Android.Graphics;
using Android.Util;
using static CrossLibrary.MathAndGeometry;
using Android.Text;
using Android.Widget;


namespace SharedActivities.Droid.CustomViews {



    [Register("sharedactivities.droid.customcontrols.FontTextView")]
    public class FontTextView : AppCompatTextView {
        public event EventHandler TextSizeChanged;
        private static readonly int defaultOutlineSize = 0;
        private static readonly Color defaultOutlineColor = Color.Transparent;
        // data
        private float mOutlineSize;
        private Color mOutlineColor;
        private Color mTextColor;
        private float mShadowRadius;
        private float mShadowDx;
        private float mShadowDy;
        private Color mShadowColor;
        float _spacingMult = 1;
        float _spacingAdd = 0;
        private float _minTextSize = 1;
        private float _maxTextSize = 300;
        private bool _needsResize;
        private float _textSize;
        private float _lastTextSize;

        public override float TextSize {
            get => base.TextSize;
            set {
                base.TextSize = value;
                _textSize = value;
            }
        }

        public override void SetTextSize([GeneratedEnum] ComplexUnitType unit, float size) {
            base.SetTextSize(unit, size);
            _minTextSize = size;
        }


        public float MaxTextSize {
            get => _maxTextSize;
            set {
                _maxTextSize = value;
                RequestLayout();
                Invalidate();
            }
        }

        public float MinTextSize {
            get => _minTextSize;
            set {
                _minTextSize = value;
                RequestLayout();
                Invalidate();
            }
        }


        public event EventHandler<OnTextResizeEventArgs> OnTextResize;

        public FontTextView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle) {
            Init(attrs);
            _textSize = TextSize;
            _lastTextSize = TextSize;
        }

        public FontTextView(Context context, IAttributeSet attrs) : base(context, attrs) {
            Init(attrs);
            _textSize = TextSize;
            _lastTextSize = TextSize;
        }

        public FontTextView(Context context) : base(context) {
            Init(null);
            _textSize = TextSize;
            _lastTextSize = TextSize;
        }

        protected FontTextView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) {
            _textSize = TextSize;
            _lastTextSize = TextSize;
        }

        //public virtual FloatRect SafeAreaRect { get; protected set; } = new FloatRect();


        private void Init(IAttributeSet attrs) {
            mOutlineSize = defaultOutlineSize;
            mOutlineColor = defaultOutlineColor;
            mTextColor = new Color(CurrentTextColor);

            if (attrs != null) {
                TypedArray a = Context.ObtainStyledAttributes(attrs, Resource.Styleable.FontTextView);
                string fontName = a.GetString(Resource.Styleable.FontTextView_fontName);
                if (fontName != null) {
                    Typeface myTypeface = Typeface.CreateFromAsset(Context.Assets, fontName);
                    this.Typeface = myTypeface;
                }
                if (a.HasValue(Resource.Styleable.FontTextView_outlineSize)) {
                    mOutlineSize = (float)a.GetFloat(Resource.Styleable.FontTextView_outlineSize, defaultOutlineSize);
                }
                // outline color
                if (a.HasValue(Resource.Styleable.FontTextView_outlineColor)) {
                    mOutlineColor = a.GetColor(Resource.Styleable.FontTextView_outlineColor, defaultOutlineColor);
                }
                // shadow (the reason we take shadow from attributes is because we use API level 15 and only from 16 we have the get methods for the shadow attributes)
                if (a.HasValue(Resource.Styleable.FontTextView_android_shadowRadius)
                        || a.HasValue(Resource.Styleable.FontTextView_android_shadowDx)
                        || a.HasValue(Resource.Styleable.FontTextView_android_shadowDy)
                        || a.HasValue(Resource.Styleable.FontTextView_android_shadowColor)) {
                    mShadowRadius = a.GetFloat(Resource.Styleable.FontTextView_android_shadowRadius, 0);
                    mShadowDx = a.GetFloat(Resource.Styleable.FontTextView_android_shadowDx, 0);
                    mShadowDy = a.GetFloat(Resource.Styleable.FontTextView_android_shadowDy, 0);
                    mShadowColor = a.GetColor(Resource.Styleable.FontTextView_android_shadowColor, Color.Transparent);
                }

                a.Recycle();
            }
        }

        float _textElevation;
        public float TextElevation {
            get => _textElevation;
            set {
                _textElevation = value;
                SetShadowLayer(GetBlurRadius(), GetBlurRadius(), GetBlurRadius(), Color.Black);
            }
        }

        private float GetBlurRadius() {
            var maxElevation = Functions.DpToPx(24f);
            return Math.Min(25f * (_textElevation / maxElevation), 25f);
        }


        private void SetFont(string fontName) {
            if (fontName != null) {
                Typeface myTypeface = Typeface.CreateFromAsset(Context.Assets, "fonts/" + fontName);
                this.Typeface = myTypeface;
            }
        }

        private void SetPaintToOutline() {
            Paint paint = Paint;
            paint.SetStyle(Android.Graphics.Paint.Style.Stroke);
            paint.StrokeWidth = mOutlineSize;
            base.SetTextColor(mOutlineColor);
            base.SetShadowLayer(mShadowRadius, mShadowDx, mShadowDy, mShadowColor);
        }

        private void SetPaintToRegular() {
            Paint paint = Paint;
            paint.SetStyle(Android.Graphics.Paint.Style.Fill);
            paint.StrokeWidth = 0;
            base.SetTextColor(mTextColor);
            base.SetShadowLayer(0, 0, 0, Color.Transparent);
        }

        //protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec) {
        //    SetPaintToOutline();
        //    //ResizeTextToFit();
        //    base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
        //}




        public override void SetTextColor(Color color) {
            base.SetTextColor(color);
            mTextColor = color;
        }

        public override void SetShadowLayer(float radius, float dx, float dy, Color color) {
            base.SetShadowLayer(radius, dx, dy, color);
            mShadowRadius = radius;
            mShadowDx = dx;
            mShadowDy = dy;
            mShadowColor = color;
        }
        public void SetOutlineSize(float size) {
            mOutlineSize = size;
        }

        public void SetOutlineColor(Color color) {
            mOutlineColor = color;
        }

        //protected virtual void UpdateSafeAreaRect() {
        //    SafeAreaRect = new FloatRect(PaddingLeft, PaddingTop, Width - PaddingLeft - PaddingRight, Height - PaddingTop - PaddingBottom);
        //}

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom) {
            if (changed || _needsResize) {
                //UpdateSafeAreaRect();
                //var widthLimit = (right - left) - CompoundPaddingLeft - CompoundPaddingRight;
                //var heightLimit = (bottom - top) - CompoundPaddingBottom - CompoundPaddingTop;
                //ResizeText(widthLimit, heightLimit);
                //ResizeText();
                //var widthLimit = (right - left) - CompoundPaddingLeft - CompoundPaddingRight;
                //var heightLimit = (bottom - top) - CompoundPaddingBottom - CompoundPaddingTop;
                //ResizeTextToFit(widthLimit, heightLimit);
            }
            base.OnLayout(changed, left, top, right, bottom);
        }


        public override void Draw(Canvas canvas) {
            base.Draw(canvas);
        }

        protected override void OnDraw(Canvas canvas) {
            SetPaintToOutline();
            base.OnDraw(canvas);
            SetPaintToRegular();
            //var safeAreaPath = new Path();
            //safeAreaPath.AddRect(SafeAreaRect.ToRectF(), Path.Direction.Cw);
            //var safeAreaPaint = new Paint() { Color = Color.Blue };
            //canvas.DrawPath(safeAreaPath, safeAreaPaint);
            base.OnDraw(canvas);
            if (_lastTextSize != TextSize) {
                _lastTextSize = TextSize;
                TextSizeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh) {
            if (w != oldw || h != oldh) {
                _needsResize = true;
            }
            //base.OnSizeChanged(w, h, oldw, oldh);
        }


        protected override void OnTextChanged(Java.Lang.ICharSequence text, int start, int lengthBefore, int lengthAfter) {
            _needsResize = true;
            ResetTextSize();
            //base.OnTextChanged(text, start, lengthBefore, lengthAfter);
        }

        public override void SetLineSpacing(float add, float mult) {
            base.SetLineSpacing(add, mult);
            _spacingAdd = add;
            _spacingMult = mult;
        }



        /// <summary>
        /// Get's the height of the text when restricted within the safe area of the view
        /// </summary>
        /// <returns></returns>
        public int GetMeasuredTextHeight() {
            var freeSpec = MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
            var widthSpec = MeasureSpec.MakeMeasureSpec(Convert.ToInt32(this.Width - PaddingLeft - PaddingRight), MeasureSpecMode.Exactly);
            Measure(widthSpec, freeSpec);
            return MeasuredHeight;
        }

        /// <summary>
        /// Gets the widths of all the text on single line.
        /// </summary>
        /// <returns></returns>
        public int GetMeasuredTextWidth() {
            var freeSpec = MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
            var heightSpec = MeasureSpec.MakeMeasureSpec(Convert.ToInt32(this.Height - PaddingBottom - PaddingTop), MeasureSpecMode.Exactly);
            Measure(freeSpec, heightSpec);
            return MeasuredWidth;
        }


        public float GetTextLenght(string text) {

            Rect textBounds = new Rect();
            Paint.GetTextBounds(text, 0, text.Length, textBounds);
            var textLength = Paint.MeasureText(text);
            //var floatBounds = new FloatRect(0, 0, textLength, textBounds.Height());
            return textLength;
        }

        public FloatRect GetTextBounds(string text) {
            Rect textBounds = new Rect();
            Paint.GetTextBounds(text, 0, text.Length, textBounds);
            var floatRect = textBounds.ToFloatRect();
            floatRect.MoveTo(0, 0);
            return floatRect;
        }

        public override AutoSizeTextType AutoSizeTextType => base.AutoSizeTextType;
        public void ResizeTextToFit(int width, int height) {

            if (String.IsNullOrEmpty(this.Text)) {
                return;
            }
            var granularity = AutoSizeStepGranularity > 0 ? AutoSizeStepGranularity : 1;
            var targetTextSize = _maxTextSize;
            var words = this.Text.Split(' ');

            //Make sure each individual word fits the width
            foreach (var word in words) {
                var wordLenght = GetTextLenght(word);
                while (wordLenght > width && Paint.TextSize > _minTextSize) {

                    Paint.TextSize -= granularity;
                    wordLenght = GetTextLenght(word);
                }
            }

            //make sure text fits the height
            //var textHeight = GetMeasuredTextHeight();
            var textHeight = GetTextHeight(new Java.Lang.String(this.Text), Paint, width, targetTextSize);
            while (textHeight > height && targetTextSize > _minTextSize) {
                targetTextSize -= granularity;
                textHeight = GetTextHeight(new Java.Lang.String(this.Text), Paint, width, targetTextSize);
            }
            //TextSize = Paint.TextSize;
            Paint.TextSize = targetTextSize;
            _needsResize = false;
        }



        /// <summary>
        /// Set the text size of the text paint object and use a static layout to render text off screen before measuring
        /// </summary>
        /// <param name="source"></param>
        /// <param name="paint"></param>
        /// <param name="width"></param>
        /// <param name="textSize"></param>
        /// <returns></returns>
        private int GetTextHeight(Java.Lang.ICharSequence source, TextPaint paint, int width, float textSize) {
            // modified: make a copy of the original TextPaint object for measuring
            // (apparently the object gets modified while measuring, see also the
            // docs for TextView.getPaint() (which states to access it read-only)
            // Update the text paint object
            var paintCopy = new TextPaint(paint) {
                TextSize = textSize
            };

            // Measure using a static layout
            var layout = new StaticLayout(source, paintCopy, width, Layout.Alignment.AlignNormal, _spacingMult, _spacingAdd, true);

            return layout.Height;
        }

        public float GetTextLenght(Java.Lang.ICharSequence text, TextPaint paint, float textSize) {
            var paintCopy = new TextPaint(paint) {
                TextSize = textSize
            };
            Rect textBounds = new Rect();
            Paint.GetTextBounds(text.ToString(), 0, text.Length(), textBounds);
            var textLength = paintCopy.MeasureText(text, 0, text.Length());
            //var floatBounds = new FloatRect(0, 0, textLength, textBounds.Height());
            return textLength;
        }

        //public void ResizeText() {
        //    ResizeText(Convert.ToInt32(SafeAreaRect.Width), Convert.ToInt32(SafeAreaRect.Height));
        //}

        public void ResizeText(int width, int height) {
            if (string.IsNullOrEmpty(Text) || width <= 0 || height <= 0) {
                return;
            }

            var oldTextSize = Paint.TextSize;
            float targetTextSize = _maxTextSize;

            // Get the required text height
            int textHeight = GetTextHeight(new Java.Lang.String(this.Text), Paint, width, targetTextSize);
            var granularity = AutoSizeStepGranularity > 0 ? AutoSizeStepGranularity : 1;
            while (textHeight > height && targetTextSize > _minTextSize) {
                targetTextSize = System.Math.Max(targetTextSize - granularity, _minTextSize);
                textHeight = GetTextHeight(new Java.Lang.String(this.Text), Paint, width, targetTextSize);
            }

            // Some devices try to auto adjust line spacing, so force default line spacing
            // and invalidate the layout as a side effect
            Paint.TextSize = targetTextSize;
            SetLineSpacing(_spacingAdd, _spacingMult);
            _needsResize = false;
        }

        public void ResetTextSize() {
            if (_textSize > 0) {
                base.SetTextSize(ComplexUnitType.Px, _textSize);
                _maxTextSize = _textSize;
            }
        }
        public class OnTextResizeEventArgs : EventArgs {
            public TextView TextView { get; set; }
            public float OldSize { get; set; }
            public float NewSize { get; set; }
        }
    }
}
