namespace DrawCurve.Domen.Models
{
    public class ActionConfig
    {
        public string Key { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public float Step { get; set; }

        public float Start { get; set; }
        public float End { get; set; }

        public float MaxValue { get; set; }
        public float MinValue { get; set; }
    }
}
