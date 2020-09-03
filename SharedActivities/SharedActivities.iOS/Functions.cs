using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Airbnb.Lottie;
using CoreAnimation;
using CoreGraphics;
using CrossLibrary;
using CrossLibrary.iOS;
using Foundation;
using SharedActivities.Core;
using UIKit;
using Xamarin.Essentials;

namespace SharedActivities.iOS {
    public static class Functions {
        public static void AddShadow(this UIView view, float offsetX = 2, float offsetY = 2, float radius = 2, UIColor color = null, float opacity = 0.5f) {
            view.Layer.AddShadow(offsetX, offsetY, radius, color, opacity);
        }

        public static void AddShadow(this UIView view, float distance) {
            view.Layer.AddShadow(distance, distance, distance, UIColor.Black, 0.5f);
        }
        public static void AddShadowAsPath(this UIView view, float distance) {
            view.Layer.AddShadow(distance, distance, distance, UIColor.Black, 0.5f);
        }



        public static void AddShadow(this CALayer layer, float offsetX = 2, float offsetY = 2, float radius = 2, UIColor color = null, float opacity = 0.5f) {
            if (color == null) {
                color = UIColor.Black;
            }

            layer.MasksToBounds = false;
            layer.ShadowOffset = new CoreGraphics.CGSize(offsetX, offsetY);
            layer.ShadowRadius = radius;
            layer.ShadowColor = color.CGColor;
            layer.ShadowOpacity = opacity;
            layer.ShouldRasterize = true;
            layer.RasterizationScale = UIScreen.MainScreen.Scale;

        }

        public static void RemoveShadow(this CALayer layer) {
            layer.ShadowOffset = new CoreGraphics.CGSize(0, 0);
            layer.ShadowRadius = 0;
            layer.ShadowColor = UIColor.Clear.CGColor;
            layer.ShadowOpacity = 0;

        }

        public static void RemoveShadow(this UIView view) {
            view.Layer.RemoveShadow();
        }

        public static LOTAnimationView AddLottieToView(this UIView view, string lottieJson, params (Color color, string pathName)[] colorPaths) {
            var lottieView = LottieFromString(lottieJson, colorPaths);
            view.AddSubviewAtBottomAndFit(lottieView);
            return lottieView;
        }

        public static LOTAnimationView AddLottieToView(this UIView view, string lottieJson, Color color) {
            var lottieView = LottieFromString(lottieJson, color);
            view.AddSubviewAtBottomAndFit(lottieView);
            return lottieView;
        }

        public static LOTAnimationView LottieFromString(string lottieJson, params (Color color, string pathName)[] colorPaths) {
            var lottieView = LottieFromString(lottieJson);
            lottieView.ColorAll(colorPaths);
            return lottieView;
        }
        public static LOTAnimationView LottieFromString(string lottieJson, Color color) {
            var lottieView = LottieFromString(lottieJson);
            lottieView.ColorAll(color);
            return lottieView;
        }


        public static LOTAnimationView LottieFromString(string lottieJson) {
            var path = Path.GetTempPath() + Guid.NewGuid().ToString();
            File.WriteAllText(path, lottieJson);
            var animation = LOTAnimationView.AnimationWithFilePath(path);
            animation.ContentMode = UIViewContentMode.ScaleAspectFit;
            File.Delete(path);
            return animation;
        }

        public static void ColorAll(this LOTAnimationView lottieView, Color color) {
            var keypath = LOTKeypath.KeypathWithString("**.Color");
            var colorValue = LOTColorValueCallback.WithCGColor(color.ToPlatformColor().CGColor);
            lottieView.SetValueDelegate(colorValue, keypath);
        }

        public static void ColorAll(this LOTAnimationView lottieView, params (Color color, string pathName)[] colorPaths) {
            foreach (var colorPath in colorPaths) {
                var keypath = LOTKeypath.KeypathWithString($"**.{colorPath.pathName}.Color");
                var colorValue = LOTColorValueCallback.WithCGColor(colorPath.color.ToPlatformColor().CGColor);
                lottieView.SetValueDelegate(colorValue, keypath);
            }

        }

        public static void AddSubviewAtBottomAndFit(this UIView view, UIView childView) {
            if (view.Subviews.Length > 0) {
                view.InsertSubviewBelow(childView, view.Subviews[0]);
            } else {
                view.AddSubview(childView);
            }
            PlatformFunctions.FillParentContraints(childView);
            childView.ContentMode = UIViewContentMode.ScaleAspectFit;
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
        public static async Task AnimateTextNumberAsync(this UILabel label, int durationMilis, int from, int to, string formattedString = "{0}") {
            await CommonFunctions.AnimateTextNumberAsync((value) => label.Text = value, durationMilis, from, to, formattedString);
        }

        public static NSAttributedString GetAttributedStringFromBracketedPlainText(string text, nfloat size) {
            return AttributedStringFromMatch(text, new Regex(@"\{(.*?)\}"), new UIStringAttributes() { Font = UIFont.BoldSystemFontOfSize(size) });
        }

        public static NSAttributedString GetAttributedStringFromBracketedPlainText(string text, UIColor matchColor) {
            return AttributedStringFromMatch(text, new Regex(@"\{(.*?)\}"), new UIStringAttributes() { ForegroundColor = matchColor });
        }

        public static void SetBracketedText(this UITextView textView, string text, UIColor matchColor) {
            textView.AttributedText = Functions.GetAttributedStringFromBracketedPlainText(text, matchColor);

        }

        public static NSAttributedString AttributedStringFromMatch(string text, Regex match, UIStringAttributes matchAttributes) {
            var attributes = new UIStringAttributes();
            var tagFinder = new TagFinder(text, match); //make text locations from regex for everything in curly brackets
            var attributedString = new NSMutableAttributedString(tagFinder.Text, attributes);
            attributedString.BeginEditing();
            foreach (var location in tagFinder.TextLocations) {
                var range = new NSRange(location.Start, location.Length);
                if (location.IsAMatch) {
                    attributedString.AddAttributes(matchAttributes, range);
                } else {

                }
            }
            attributedString.EndEditing();
            return attributedString;
        }


        public static void ReleaseChildren(this IEnumerable<UIView> views) {
            foreach (var view in views) {
                view.Subviews.ReleaseChildren();
                view.RemoveFromSuperview();
                view.Dispose();
            }
        }


        public static CAShapeLayer AddDashedBorder(this UIView view, UIColor color, float dashWidth = 4) {
            const string layerName = "Dash Border Layer";

            //
            if (view.Layer != null & view.Layer.Sublayers != null) {
                var layersToremove = view.Layer.Sublayers.Where(layer => layer.Name == layerName);
                foreach (var layer in layersToremove) {
                    layer.RemoveFromSuperLayer();
                    layer.Dispose();
                }
            }

            var border = new CAShapeLayer();
            border.Name = layerName;
            border.StrokeColor = color.CGColor;
            border.LineDashPattern = new NSNumber[] { dashWidth, dashWidth };
            border.Frame = view.Bounds;
            border.FillColor = null;
            border.Path = UIBezierPath.FromRect(view.Bounds).CGPath;
            view.Layer.AddSublayer(border);
            return border;

        }

        public static nfloat GetHeigthWithText(string text, UIFont font, nfloat width) {
            CGSize size = ((NSString)text).StringSize(
                font: font,
                constrainedToSize: new CGSize(width, float.MaxValue),
                lineBreakMode: UILineBreakMode.WordWrap);

            if (size.Height <= 0) {
                return 0;
            } else {
                return size.Height;
            }
        }

        /// <summary>
        /// Loads xib with same name as the class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T LoadViewFromXib<T>() where T : UIView {
            var xibName = typeof(T).Name;
            var arr = NSBundle.MainBundle.LoadNib(xibName, null, null);
            var v = arr.GetItem<T>(0);
            return v;
        }


        /// <summary>
        /// Get's a font of correct size to fit size, with fixed or non fixed number of lines.
        /// Currently just brute forces from max size, reducing the size by one per interation.
        /// May not be performant in a large loop
        /// </summary>
        /// <param name="text"></param>
        /// <param name="size"></param>
        /// <param name="font"></param>
        /// <param name="minFontSize"></param>
        /// <param name="maxFontSize"></param>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static UIFont ResizeFontToFitRect(string text, CGSize size, UIFont font, float minFontSize, float maxFontSize, int lines = 0) {
            float fontSize = maxFontSize; //<= 0 ? (float)font.PointSize : maxFontSize;
            //int fontSize = 50; // (float)font.PointSize;
            var descriptor = font.FontDescriptor;
            UIFont resizedFont = UIFont.FromDescriptor(descriptor, fontSize);

            //CGSize size = text.StringSize(resizedFont, rect.Size, UILineBreakMode.WordWrap);
            var actualHeight = text.StringSize(resizedFont, new CGSize(size.Width, double.MaxValue), UILineBreakMode.WordWrap).Height;
            var longestWord = text.GetLongestWord();
            var longestWordWidth = longestWord.StringSize(resizedFont).Width;

            bool useLongestWord = !longestWord.ContainsJapaneseChars();
            while ((actualHeight > size.Height
                    || (useLongestWord && longestWordWidth > size.Width)
                    || (lines > 0 && actualHeight > lines * resizedFont.LineHeight))
                    && !(fontSize < minFontSize)) {
                fontSize -= 1;
                resizedFont = UIFont.FromDescriptor(descriptor, fontSize);
                //size = text.StringSize(resizedFont, rect.Size, UILineBreakMode.WordWrap);
                actualHeight = text.StringSize(resizedFont, new CGSize(size.Width, double.MaxValue), UILineBreakMode.WordWrap).Height;
                longestWordWidth = longestWord.StringSize(resizedFont).Width;
            }

            return resizedFont;
        }


        public static void ResizeFont(this UILabel label, float minFontSize, float maxFontSize) {
            label.Font = ResizeFontToFitRect(label.Text, label.Frame.Size, label.Font, minFontSize, maxFontSize);
        }


    }
}
