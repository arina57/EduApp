using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Airbnb.Lottie;
using CoreAnimation;
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
    }
}
