using System;
using System.Collections.Generic;
using CoreAnimation;
using Foundation;
using SharedActivities.Core.Models;
using UIKit;
using Xamarin.Essentials;

namespace SharedActivities.iOS.CustomViews {
    [Register("LineDrawingView")]
    public class LineDrawingView : UIView {
        private List<CAShapeLayer> shapeLayers = new List<CAShapeLayer>();

        public List<ColoredLine> Lines { get; set; } = new List<ColoredLine>();

        public ColoredLine Line { get; set; }

        public LineDrawingView() : base() {
        }

        public LineDrawingView(IntPtr handle) : base(handle) {

        }

        private void DrawLines() {

            //remove the old lines
            foreach (var shapeLayer in shapeLayers) {
                shapeLayer.RemoveFromSuperLayer();
                shapeLayer.Dispose();
            }
            shapeLayers.Clear();


            foreach (var line in Lines) {
                MakeShapeLayer(line);
            }
            if (Line != null) {
                MakeShapeLayer(Line);
            }


            foreach (var shapeLayer in shapeLayers) {
                this.Layer.AddSublayer(shapeLayer);
            }

        }


        private void MakeShapeLayer(ColoredLine line) {
            UIBezierPath path = new UIBezierPath();
            path.MoveTo(line.Start.ToCGPoint());
            path.AddLineTo(line.Stop.ToCGPoint());
            var shapeLayer = new CAShapeLayer();
            shapeLayer.Path = path.CGPath;
            shapeLayer.FillColor = UIColor.Clear.CGColor;
            shapeLayer.StrokeColor = line.Color.ToPlatformColor().CGColor;
            shapeLayer.LineWidth = line.Width;
            shapeLayers.Add(shapeLayer);
        }


        public void Refresh() {
            DrawLines();
        }

        public override void LayoutSubviews() {
            base.LayoutSubviews();
            DrawLines();
        }

    }
}