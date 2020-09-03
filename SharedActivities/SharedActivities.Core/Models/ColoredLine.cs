using System;
using System.Drawing;
using static CrossLibrary.MathAndGeometry;

namespace SharedActivities.Core.Models {
    public class ColoredLine {
        private LineF line;

        //public ColoredLine(FPoint start, FPoint stop) {
        //    line.Start = start;
        //    line.Stop = stop;
        //}
        public ColoredLine() {

        }
        public ColoredLine(FPoint start, FPoint stop) {
            this.Start = start;
            this.Stop = stop;
        }

        public ColoredLine(FPoint start, FPoint stop, Color color, int width) {
            this.Color = color;
            this.Width = width;
            this.Start = start;
            this.Stop = stop;
        }

        public Color Color { get; set; } = Color.Black;
        public LineF Line { get => line; set => line = value; }
        public int Width { get; set; } = 1;
        public FPoint Start { get => line.Start; set => line.Start = value; }
        public FPoint Stop { get => line.Stop; set => line.Stop = value; }
    }
}
