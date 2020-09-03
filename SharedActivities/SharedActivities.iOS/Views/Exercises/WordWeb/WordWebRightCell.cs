using System;
using System.Drawing;
using Airbnb.Lottie;
using Foundation;
using SharedActivities.Core.ViewModels.Exercises;
using SharedActivities.iOS.CustomViews;
using UIKit;
using static CrossLibrary.MathAndGeometry;

namespace SharedActivities.iOS.Views.Exercises.WordWeb {
    public partial class WordWebRightCell : UITableViewCell {

        public static readonly NSString Key = new NSString(nameof(WordWebRightCell));
        public static readonly UINib Nib = UINib.FromName(Key, NSBundle.MainBundle);
        private LOTAnimationView dotImage;
        private Color color;

        public UIView TouchView => DotView;



        public int Position { get; private set; } = -1;
        public Color Color {
            get => color;
            set {
                color = value;
                dotImage.ColorAll(value);
            }
        }



        public WordWebRightCell(IntPtr handle) : base(handle) {
        }

        public void Setup(WordWebViewModel viewModel, int row, LineDrawingView lineDrawingView) {
            this.Position = row;
            if (DotView.Subviews.Length == 0) {
                dotImage = DotView.AddLottieToView(viewModel.RippleImageJson);
            }

            var cellCenter = DotView.Frame.ToFRect().Center;
            FPoint cellPoint = this.ConvertPointToView(cellCenter.ToCGPoint(), lineDrawingView).ToFPoint();
            viewModel.SetLinePositionForMatch(Position, cellPoint);
            Color = viewModel.GetMatchColor(row);
            PhraseLabel.Text = viewModel.GetMatchPhrase(row);
        }
    }
}
