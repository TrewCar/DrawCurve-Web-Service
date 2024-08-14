namespace DrawCurve.Domen.Models.Core.Objects
{
    public class LineCurve : ObjectRender
    {
        public LineCurve() { }
        public LineCurve(float Lenght, float Angle, float RPS) 
        {
            this.Length = Lenght;
            this.Angle = Angle;
            this.RPS = RPS;
        }
        public float Angle {  get; set; }

        public float Length {  get; set; }

        public float RPS {  get; set; }
    }
}
