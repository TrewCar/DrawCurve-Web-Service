using System.Diagnostics;

namespace DrawCurve.Core.Helpers
{
    public class FPSCounter
    {
        private Stopwatch stopwatch = new Stopwatch();
        private int frameCount = 0;
        private double elapsedTime = 0;

        public float DeltaTime { get; private set; } = 0;

        public bool PrintFPS;

        public double FPS { get; private set; }
        public FPSCounter()
        {
            stopwatch.Start();
        }

        public double Update()
        {
            frameCount++;
            elapsedTime += stopwatch.ElapsedMilliseconds;
            DeltaTime = stopwatch.ElapsedMilliseconds / 1000f;
            FPS = frameCount / (elapsedTime / 1000);

            frameCount = 0;
            elapsedTime = 0;

            stopwatch.Restart(); // Сбросить таймер для следующего кадра
            return FPS;
        }
    }
}
