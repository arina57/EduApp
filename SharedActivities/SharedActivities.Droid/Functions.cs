using System;
using System.Collections.Generic;
using System.Linq;

using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Content.Res;
using Android.Views.InputMethods;
using Java.Util;
using Android.Util;
using static CrossLibrary.MathAndGeometry;
using Android.Views.Animations;
using System.Threading.Tasks;
using Android.Text;
using Android.Text.Style;
using System.Text.RegularExpressions;
using Android.Content.PM;
using Android.Net;
using Xamarin.Essentials;
using System.Reflection;
using Android.Animation;
using Com.Airbnb.Lottie;
using Com.Airbnb.Lottie.Model;
using static Android.Views.View;
using Com.Airbnb.Lottie.Value;
using Locale = Java.Util.Locale;
using Plugin.CurrentActivity;
using System.Globalization;
using AndroidX.AppCompat.App;
using SharedActivities.Core;

namespace SharedActivities.Droid {
    /// <summary>
    /// Static functions for Android
    /// </summary>
    public static class Functions {


        public static CultureInfo GetLanguage(Locale locale) => new CultureInfo(locale.Language);

		public static Locale GetLocale(CultureInfo language) => new Locale(language.Name);
		public static int GetToolbarHeight() {
			TypedArray styledAttributes = CrossCurrentActivity.Current.Activity.Theme.ObtainStyledAttributes(
				new int[] { Droid.Resource.Attribute.actionBarSize });
			int x = (int)styledAttributes.GetDimension(0, 0);
			styledAttributes.Recycle();
			return x;
		}
		public static Point GetScreenSize() {
			Point size = new Point();
			CrossCurrentActivity.Current.Activity.Window.WindowManager.DefaultDisplay.GetSize(size);
			return size;
		}


        [Obsolete]
        public static bool IsNetworkConnected() {
			ConnectivityManager cm = (ConnectivityManager)CrossCurrentActivity.Current.Activity.GetSystemService(Context.ConnectivityService);
			return cm.ActiveNetworkInfo != null;
		}


		public static AndroidX.AppCompat.Widget.Toolbar CreateToolBar() {
			AndroidX.AppCompat.Widget.Toolbar toolbar = new AndroidX.AppCompat.Widget.Toolbar(CrossCurrentActivity.Current.Activity);
			toolbar.Id = 1;
			toolbar.SetBackgroundColor(Color.White);
			return toolbar;
		}

		public static RelativeLayout AddToolbar(this View view) {
			return AddToolbar(view, CreateToolBar());
		}

		public static RelativeLayout AddToolbar(this View view, AndroidX.AppCompat.Widget.Toolbar toolbar) {
			Context context = view.Context;
			RelativeLayout relativeLayout = new RelativeLayout(context);
			RelativeLayout.LayoutParams lp = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);

			TypedValue tv = new TypedValue();
			int actionBarHeight = 0;
			if (context.Theme.ResolveAttribute(Resource.Attribute.actionBarSize, tv, true)) {
				actionBarHeight = TypedValue.ComplexToDimensionPixelSize(tv.Data, context.Resources.DisplayMetrics);
				//toolbar.LayoutParameters.Height = actionBarHeight;
			}
			RelativeLayout.LayoutParams toolbarParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, actionBarHeight);

			//activity.SetSupportActionBar(toolbar);
			//activity.SupportActionBar.SetWindowTitle("Test");
			//activity.SupportActionBar.Title = "test title";
			view.Id = 2;
			lp.AddRule(LayoutRules.Below, toolbar.Id);
			relativeLayout.AddView(toolbar, toolbarParams);
			relativeLayout.AddView(view, lp);

			if (context is AppCompatActivity compAppAct) {
				compAppAct.SetSupportActionBar(toolbar);
			}

			return relativeLayout;
		}










		//public static void ReplaceFragment(this Fragment parent, FragmentExtra fragment) {
		//	FragmentTransaction ft = parent.FragmentManager.BeginTransaction();

		//	ft.Replace(Android.Resource.Id.Content, fragment);
		//          ft.AddToBackStack(fragment.UnqueId);
		//          ft.Commit();
		//}

		//public static void AddFragment(this Fragment parent, FragmentExtra fragment) {
		//	FragmentTransaction ft = parent.FragmentManager.BeginTransaction();

		//	ft.Add(Android.Resource.Id.Content, fragment);
		//          ft.AddToBackStack(fragment.UnqueId);
		//          ft.Commit();
		//}

		public static void HideKeyboardFrom(View view) {
			InputMethodManager imm = (InputMethodManager)CrossCurrentActivity.Current.Activity.GetSystemService(Android.App.Activity.InputMethodService);
			imm.HideSoftInputFromWindow(view.WindowToken, 0);
		}
		public static void ShowKeyboardOn(View view) {
			InputMethodManager imm = (InputMethodManager)CrossCurrentActivity.Current.Activity.GetSystemService(Android.App.Activity.InputMethodService);
			imm.ShowSoftInput(view, 0);
			view.RequestFocus();
		}

		public static void HideKeyboard() {
			try {
				if (CrossCurrentActivity.Current.Activity == null || CrossCurrentActivity.Current.Activity.IsFinishing) {
					return;
				}

				Window window = CrossCurrentActivity.Current.Activity.Window;
				if (window == null) {
					return;
				}

				View view = window.CurrentFocus;
				//give decorView a chance
				if (view == null) {
					view = window.DecorView;
				}

				if (view == null) {
					return;
				}

				InputMethodManager imm = (InputMethodManager)CrossCurrentActivity.Current.Activity.ApplicationContext.GetSystemService(Context.InputMethodService);
				if (imm == null || !imm.IsActive) {
					return;
				}

				imm.HideSoftInputFromWindow(view.WindowToken, 0);
			} catch (System.Exception e) {
				//e.printStackTrace();
			}
		}


		public static bool IsPackageInstalledAndEnabled(string packagename) {
			try {
				PackageManager packageManager = CrossCurrentActivity.Current.Activity.PackageManager;
				//packageManager.GetPackageInfo(packagename, 0);
				var info = packageManager.GetApplicationInfo(packagename, 0);
				return info.Enabled;
			} catch {
				return false;
			}
		}

		//public static int MeasureTextWidth(TextView textView) {
		//    Rect bounds = new Rect();
		//    var text = textView.Text;

		//    textView.Paint.GetTextBounds(text, 0, text.Length, bounds);
		//    int width = bounds.Left + bounds.Width();
		//    return width;
		//}

		//public static int MeasureTextHeight(this Context context, string text, float width, float height, Typeface typeface, float textSize) {
		//    var textView = new TextView(context);
		//    textView.Typeface = typeface;
		//    textView.TextSize = textSize;

		//    Rect bounds = new Rect();
		//    var text = textView.Text;
		//    textView.Paint.GetTextBounds(text, 0, text.Length, bounds);
		//    int width = bounds.Bottom + bounds.Height();
		//    return width;
		//}

		//public static Rect GetTextBounds(this TextView textview) {

		//}



		public static SpannableString GetSpannableStringFromBracketedPlainText(string text, Color matchColor) {
			return SpannableStringFromMatch(text, new Regex(@"\{(.*?)\}"), new ForegroundColorSpan(matchColor), new StyleSpan(TypefaceStyle.Bold));
		}
		public static SpannableString GetSpannableStringFromBracketedPlainText(string text) {
			return SpannableStringFromMatch(text, new Regex(@"\{(.*?)\}"), new StyleSpan(TypefaceStyle.Bold));
		}

		public static void SetBracketedText(this TextView textView, string text, Color matchColor) {
			var spannedText = GetSpannableStringFromBracketedPlainText(text, matchColor);
			textView.SetText(spannedText, TextView.BufferType.Spannable);
		}


		public static void SetBracketedText(this TextView textView, string text) {
			var spannedText = GetSpannableStringFromBracketedPlainText(text);
			textView.SetText(spannedText, TextView.BufferType.Spannable);
		}

		public static SpannableString SpannableStringFromMatch(string text, Regex match, params CharacterStyle[] matchAttributes) {
			var tagFinder = new TagFinder(text, match); //make text locations from regex for everything in curly brackets
			var spannableString = new SpannableString(tagFinder.Text);
			foreach (var location in tagFinder.TextLocations) {
				if (location.IsAMatch) {
					foreach (var style in matchAttributes) {
						spannableString.SetSpan(style, location.Start, location.End + 1, SpanTypes.ExclusiveExclusive);
					}
				}
			}
			return spannableString;
		}




		public static DPoint GetRelativePosition(View fromView, View toView) {
			var fromScreenLocation = new int[2];
			fromView.GetLocationOnScreen(fromScreenLocation);
			var toScreenLocation = new int[2];
			toView.GetLocationOnScreen(toScreenLocation);
			return new DPoint(fromScreenLocation[0] + toScreenLocation[0], fromScreenLocation[1] + toScreenLocation[1]);
		}


		public static FloatRect GetRectOnScreen(this View view) {
			int[] l = new int[2];
			view.GetLocationOnScreen(l);
			FloatRect rect = new FloatRect(l[0], l[1], view.Width, view.Height);
			return rect;
		}

		public static FPoint GetPositionOnScreen(this View fromView) {
			int[] fromViewPosition = new int[2];
			fromView.GetLocationOnScreen(fromViewPosition);
			FPoint newPoint = new FPoint(fromViewPosition[0], fromViewPosition[1]);
			return newPoint;
		}

		public static FPoint TranslatePositionOnScreen(this View fromView, FPoint point) {
			var fromViewPosition = fromView.GetPositionOnScreen();
			FPoint newPoint = new FPoint(point.X - fromViewPosition.X, point.Y - fromViewPosition.Y);
			return newPoint;
		}

		public static FPoint GetPointRelativeTo(this View fromView, View toView, FPoint fromPoint) {
			var toViewRect = toView.GetRectOnScreen();
			var fromViewRect = fromView.GetRectOnScreen();
			var pointInToView = new FPoint(fromViewRect.X - toViewRect.X + fromPoint.X, fromViewRect.Y - toViewRect.Y + fromPoint.Y);
			return pointInToView;
		}

		public static FloatRect GetRectRelativeTo(this View view, View toView) {
			int[] l = new int[2];
			toView.GetLocationOnScreen(l);
			var rect = view.GetRectOnScreen();
			rect.Offset(-l[0], -l[1]);
			return rect;
		}

		public static FloatRect GetVisableRect(this View view) {
			Rect rect = new Rect();
			try {
				view.GetGlobalVisibleRect(rect);
				return new FloatRect(rect.Left, rect.Top, rect.Width(), rect.Height());
			} catch {
				return FloatRect.Empty;
			}
		}


		public static FloatRect GetRect(this View view) {
			Rect rect = new Rect();
			try {
				view.GetDrawingRect(rect);
				return new FloatRect(rect.Left, rect.Top, rect.Width(), rect.Height());
			} catch {
				return FloatRect.Empty;
			}
		}

		public static float PxToDp(float px) {
			var density = CrossCurrentActivity.Current.Activity.Resources.DisplayMetrics.Density;
			float dp = px / density;
			return dp;
		}

		public static float DpToPx(float dp) {
			var density = CrossCurrentActivity.Current.Activity.Resources.DisplayMetrics.Density;
			float px = dp * density;
			return px;
		}
		public static double PxToDp(double px) {
			var density = CrossCurrentActivity.Current.Activity.Resources.DisplayMetrics.Density;
			double dp = px / density;
			return dp;
		}

		public static double DpToPx(double dp) {
			var density = CrossCurrentActivity.Current.Activity.Resources.DisplayMetrics.Density;
			double px = dp * density;
			return px;
		}

		public static DPoint DpToPx(DPoint point) {
			var density = CrossCurrentActivity.Current.Activity.Resources.DisplayMetrics.Density;
			return new DPoint(point.X * density, point.Y * density);
		}

		public static FloatRect DpToPx(FloatRect rect) {
			var density = CrossCurrentActivity.Current.Activity.Resources.DisplayMetrics.Density;
			return new FloatRect(rect.X * density, rect.Y * density, rect.Width * density, rect.Height * density);
		}

		public static RectF ToRectF(this FloatRect rect) {
			return new RectF(rect.X, rect.Y, rect.MaxX, rect.MaxY);
		}

		public static Rect ToRect(this FloatRect rect) {
			return new Rect(Convert.ToInt32(rect.X), Convert.ToInt32(rect.Y), Convert.ToInt32(rect.MaxX), Convert.ToInt32(rect.MaxY));
		}

		public static FloatRect ToFloatRect(this RectF rect) {
			return new FloatRect(rect.Left, rect.Top, rect.Width(), rect.Height());
		}

		public static FloatRect ToFloatRect(this Rect rect) {
			return new FloatRect() { Left = rect.Left, Top = rect.Top, Right = rect.Right, Bottom = rect.Bottom };
		}

		public static void AlphaFade(EventHandler<Animation.AnimationEndEventArgs> animationEndHandler, params View[] views) {
			AlphaAnimation animation = new AlphaAnimation(0.2f, 1.0f) { Duration = 500 };
			animation.AnimationEnd += animationEndHandler;
			foreach (var view in views) {
				view.Alpha = 1f;
				view.StartAnimation(animation);
			}

		}

		/// <summary>
		/// Updates the text in intervals.
		/// If less than 20ms have passed since last update, then it won't update, to avoid excessive ui updates
		/// </summary>
		/// <param name="label"></param>
		/// <param name="durationMilis"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="formattedString"></param>
		/// <returns></returns>
		public static async Task AnimateTextNumberAsync(this TextView label, int durationMilis, int from, int to, string formattedString = "{0}") {
			if (from != to) {
				int lenght = System.Math.Abs(from - to);
				var delayPerLoop = durationMilis / lenght;
				var delaySinceLastDisplay = 0;
				const int displayMinDelay = 20; //The minimum interval between loops
				label.Text = string.Format(formattedString, from);
				for (int i = 0; i <= lenght; i++) {
					if (delaySinceLastDisplay >= displayMinDelay) {
						int step = from < to ? from + i : to + lenght - i;
						label.Text = string.Format(formattedString, step);
					}
					await Task.Delay(delayPerLoop);
					delaySinceLastDisplay += delayPerLoop;

				}
			}
			label.Text = string.Format(formattedString, to);
		}


		/// <summary>
		/// Updates the text in intervals.
		/// If less than 20ms have passed since last update, then it won't update, to avoid excessive ui updates
		/// </summary>
		/// <param name="label"></param>
		/// <param name="durationMilis"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		public static async Task AnimateTextValuesAsync(this TextView label, int durationMilis, params string[] values) {
			if (values.Length > 0) {
				var delayPerLoop = durationMilis / values.Length;
				var delaySinceLastDisplay = 0;
				const int displayMinDelay = 20;
				label.Text = values.First();
				foreach (var value in values) {
					if (delaySinceLastDisplay >= displayMinDelay) {
						label.Text = value;
					}
					await Task.Delay(delayPerLoop);
					delaySinceLastDisplay += delayPerLoop;
				}
				label.Text = values.Last();
			}
		}


		public static void SetColor(this LottieAnimationView lottieAnimationView, Color color) {
			lottieAnimationView.AddValueCallback(new KeyPath("**"), LottieProperty.ColorFilter, new Com.Airbnb.Lottie.Value.LottieValueCallback(new SimpleColorFilter(color)));
		}


	


		public static Bitmap ToBitmap(this byte[] imageBytes) {
			return BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
		}

		public static async Task<Bitmap> ToBitmapAsync(this byte[] imageBytes) {
			return await BitmapFactory.DecodeByteArrayAsync(imageBytes, 0, imageBytes.Length);
		}


		public static Bitmap TryToBitmap(this byte[] imageBytes) {
			try {
				return BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
			} catch {
				return null;
			}
		}

		public static async Task<Bitmap> TryToBitmapAsync(this byte[] imageBytes) {
			try {
				return await BitmapFactory.DecodeByteArrayAsync(imageBytes, 0, imageBytes.Length);
			} catch {
				return null;
			}
		}

		public static async Task StartAsync(this Animation animation) {
			var taskCompletionSource = new TaskCompletionSource<object>();
			EventHandler<Animation.AnimationEndEventArgs> completionEvent = (s, e) => taskCompletionSource.TrySetResult(null);

			try {
				animation.AnimationEnd += completionEvent;
				animation.Start();
				await taskCompletionSource.Task;
			} finally {
				animation.AnimationEnd -= completionEvent;
			}
		}

		public static async Task StartAsync(this Animator animator) {
			var taskCompletionSource = new TaskCompletionSource<object>();
			EventHandler completionEvent = (s, e) => taskCompletionSource.TrySetResult(null);

			try {
				animator.AnimationEnd += completionEvent;
				animator.AnimationCancel += completionEvent;
				animator.Start();
				await taskCompletionSource.Task;
			} finally {
				animator.AnimationEnd -= completionEvent;
			}
		}

		public static void ColorAll(this LottieAnimationView lottieAnimationView, Color color, params string[] pathName) {
			var filter = new SimpleColorFilter(color.ToArgb());
			List<string> names = new List<string> { "**" };
			names.AddRange(pathName);

			var keyPath = new KeyPath(names.ToArray());
			var callback = new LottieValueCallback(filter);
			lottieAnimationView.AddValueCallback(keyPath, LottieProperty.ColorFilter, callback);
		}

		public static void ColorAll(this LottieDrawable lottieDrawable, Color color, params string[] pathName) {
			var filter = new SimpleColorFilter(color.ToArgb());
			List<string> names = new List<string> { "**" };
			names.AddRange(pathName);

			var keyPath = new KeyPath(names.ToArray());
			var callback = new LottieValueCallback(filter);
			lottieDrawable.AddValueCallback(keyPath, LottieProperty.ColorFilter, callback);
		}

		public static void ColorAll(this LottieAnimationView lottieAnimationView, System.Drawing.Color color, params string[] pathName) {
			lottieAnimationView.ColorAll(color.ToPlatformColor(), pathName);
		}

		public static void ColorAll(this LottieDrawable lottieDrawable, System.Drawing.Color color, params string[] pathName) {
			lottieDrawable.ColorAll(color.ToPlatformColor(), pathName);
		}



		public static void ColorAll(this LottieDrawable lottieDrawable, params (System.Drawing.Color color, string pathName)[] colorPaths) {
			foreach (var colorPath in colorPaths) {
				lottieDrawable.ColorAll(colorPath.color, colorPath.pathName);
			}
		}

		public static async Task SetAnimationFromJsonAsync(this LottieAnimationView lottieAnimationView, string jsonString, string cacheKey) {
			var taskCompletionSource = new TaskCompletionSource<object>();
			EventHandler completionEvent = (s, e) => taskCompletionSource.TrySetResult(null);
			LottieCompositionListener lottieOnCompositionLoadedListener = new LottieCompositionListener();

			try {
				lottieAnimationView.AddLottieOnCompositionLoadedListener(lottieOnCompositionLoadedListener);
				lottieOnCompositionLoadedListener.CompositionLoaded += completionEvent;
				lottieAnimationView.SetAnimationFromJson(jsonString, cacheKey);
				await taskCompletionSource.Task;
			} finally {
				lottieOnCompositionLoadedListener.CompositionLoaded -= completionEvent;
			}
		}

		public static async Task PlayAnimationAsync(this LottieAnimationView lottieAnimationView) {
			var taskCompletionSource = new TaskCompletionSource<object>();
			EventHandler completionEvent = (s, e) => taskCompletionSource.TrySetResult(null);
			LottieCompositionListener lottieListener = new LottieCompositionListener();

			try {
				lottieListener.AnimationCanceled += completionEvent;
				lottieListener.AnimationEnded += completionEvent;
				lottieAnimationView.AddAnimatorListener(lottieListener);
				lottieAnimationView.PlayAnimation();
				await taskCompletionSource.Task;
			} finally {
				lottieListener.AnimationCanceled -= completionEvent;
				lottieListener.AnimationEnded -= completionEvent;
			}
		}

		public static async Task<LottieComposition> LottieCompositionFromJsonStringAsync(string json, string cacheKey) {
			var taskCompletionSource = new TaskCompletionSource<object>();
			EventHandler completionEvent = (s, e) => taskCompletionSource.TrySetResult(s);

			var lottieTask = LottieCompositionFactory.FromJsonString(json, cacheKey);
			LottieListener lottieListener = new LottieListener();
			lottieTask.AddListener(lottieListener);

			try {
				lottieListener.Result += completionEvent;
				return await taskCompletionSource.Task as LottieComposition;
			} finally {
				lottieListener.Result -= completionEvent;
				lottieListener.Dispose();
				lottieTask.Dispose();
			}
		}




		public static async Task<LottieDrawable> LottieDrawableFromJsonStringAsync(string json, string cacheKey) {
			var composition = await LottieCompositionFromJsonStringAsync(json, cacheKey);
			var lottieDrawable = new LottieDrawable();
			lottieDrawable.SetComposition(composition);
			//composition.Dispose();
			return lottieDrawable;
		}

		public static async Task<LottieDrawable> LottieDrawableFromJsonStringAsync(string json, string cacheKey, params (System.Drawing.Color color, string pathName)[] colorPaths) {
			var lottieDrawable = await LottieDrawableFromJsonStringAsync(json, cacheKey);
			foreach (var colorPath in colorPaths) {
				lottieDrawable.ColorAll(colorPath.color, colorPath.pathName);
			}
			return lottieDrawable;
		}


		public static async Task<LottieDrawable> LottieDrawableFromJsonStringAsync(string json, string cacheKey, params (System.Drawing.Color color, string[] pathName)[] colorPaths) {
			var lottieDrawable = await LottieDrawableFromJsonStringAsync(json, cacheKey);
			foreach (var colorPath in colorPaths) {
				lottieDrawable.ColorAll(colorPath.color, colorPath.pathName);
			}
			return lottieDrawable;
		}

		public static async Task<LottieDrawable> LottieDrawableFromJsonStringAsync(string json, string cacheKey, System.Drawing.Color color, params string[] pathName) {
			var lottieDrawable = await LottieDrawableFromJsonStringAsync(json, cacheKey);
			lottieDrawable.ColorAll(color, pathName);
			return lottieDrawable;
		}


		private class LottieListener : Java.Lang.Object, ILottieListener {
			public event EventHandler Result;
			public void OnResult(Java.Lang.Object p0) {
				Result?.Invoke(p0, EventArgs.Empty);
			}
		}
		private class LottieCompositionListener : Java.Lang.Object, ILottieOnCompositionLoadedListener, Animator.IAnimatorListener, ValueAnimator.IAnimatorUpdateListener {
			public event EventHandler CompositionLoaded;
			public event EventHandler AnimationCanceled;
			public event EventHandler AnimationEnded;
			public event EventHandler AnimationRepeated;
			public event EventHandler AnimationStarted;
			public event EventHandler AnimationUpdated;

			public void OnCompositionLoaded(LottieComposition composition) {
				CompositionLoaded?.Invoke(composition, new EventArgs());
			}

			public void OnAnimationCancel(Animator animation) {
				AnimationCanceled?.Invoke(animation, new EventArgs());
			}

			public void OnAnimationEnd(Animator animation) {
				AnimationEnded?.Invoke(animation, new EventArgs());
			}

			public void OnAnimationRepeat(Animator animation) {
				AnimationRepeated?.Invoke(animation, new EventArgs());
			}

			public void OnAnimationStart(Animator animation) {
				AnimationStarted?.Invoke(animation, new EventArgs());
			}

			public void OnAnimationUpdate(ValueAnimator animation) {
				AnimationUpdated?.Invoke(animation, new EventArgs());
			}
		}

		public static void SetMargins(this View view, int left, int top, int right, int bottom) {
			if (view.LayoutParameters == null) {
				var layoutMarginParams = new ViewGroup.MarginLayoutParams(view.Context, null);
			} else if (view.LayoutParameters is ViewGroup.MarginLayoutParams layoutMarginParams) {
				layoutMarginParams.SetMargins(left, top, right, bottom);
				view.RequestLayout();
			}
		}

		public static void StartDragAndDropCompat(this View view, ClipData data, DragShadowBuilder shadowBuilder, Java.Lang.Object myLocalState, int flags) {
			if (Build.VERSION.SdkInt > BuildVersionCodes.N) {
				view.StartDragAndDrop(data, shadowBuilder, myLocalState, flags);
			} else {
#pragma warning disable CS0618 // Type or member is obsolete
				view.StartDrag(data, shadowBuilder, myLocalState, flags);
#pragma warning restore CS0618 // Type or member is obsolete
			}
		}
	}
}
