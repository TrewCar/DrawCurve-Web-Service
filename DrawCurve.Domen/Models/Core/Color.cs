namespace DrawCurve.Domen.Models.Core
{
    public class Color
    {
        public Color() { }
        public Color(byte R, byte G, byte B, byte A)
        {
            this.R = R;
            this.G = G;
            this.B = B;
            this.A = A;
        }
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public int A { get; set; }
    }
}
