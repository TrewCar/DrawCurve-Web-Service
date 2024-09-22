using DrawCurve.Core.Actions;
using DrawCurve.Core.Config;
using DrawCurve.Core.Objects;
using DrawCurve.Core.Tags;
using NAudio.Wave;
using SFML.Graphics;
using SFML.System;

namespace DrawCurve.Core.Window
{
    public class LisajuFormsRender : Render
    {
        private float[] samplesLeft = new float[0];
        private float[] samplesRight = new float[0];
        private float maxAmplitude = 0;
        private int sampleRate = 0;
        private float scale = 0;
        public override List<ActionBase> active { get; set; } = new();
        public LisajuFormsRender() : base() { }

        public LisajuFormsRender(RenderConfig config, List<ObjectRender> Objects) : base(config, Objects)
        {
        }
        public override void Init()
        { 
            base.Init();
            using (var reader = new AudioFileReader(RenderConfig.PathMusic))
            {
                int totalSamples = (int)reader.Length / sizeof(float) / reader.WaveFormat.Channels;
                samplesLeft = new float[totalSamples];
                samplesRight = new float[totalSamples];

                float[] buffer = new float[reader.WaveFormat.SampleRate * reader.WaveFormat.Channels];
                int samplesRead, bufferIndex = 0;

                while ((samplesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    for (int i = 0; i < samplesRead; i += 2) // Чтение по 2 канала
                    {
                        samplesLeft[bufferIndex] = buffer[i]; // Левый канал
                        samplesRight[bufferIndex] = buffer[i + 1]; // Правый канал
                        bufferIndex++;
                    }
                }
                sampleRate = reader.WaveFormat.SampleRate;
                RenderConfig.Time = (int)reader.TotalTime.TotalSeconds;


            }

            for (int i = 0; i < samplesLeft.Length; i++)
            {
                maxAmplitude = Math.Max(maxAmplitude, Math.Max(samplesLeft[i], samplesRight[i]));
            }

            scale = Math.Min(window.Size.X, window.Size.Y) / 4.0f / maxAmplitude;
        }
        public override bool TickRender(float deltaTime)
        {
            var background = new RectangleShape(new Vector2f(window.Size.X, window.Size.Y));
            background.FillColor = RenderConfig.Colors["background"];
            window.Draw(background);

            int step = (int)(sampleRate / RenderConfig.FPS);
            int sampleOffset = this.CountFrame * step;
            List<Vertex> line = new List<Vertex>();

            for (int t = 0; t < step; t++)
            {
                int currentSampleIndex = sampleOffset + t;
                if (currentSampleIndex >= samplesLeft.Length || currentSampleIndex >= samplesRight.Length)
                    break;

                float x = scale * samplesLeft[currentSampleIndex];
                float y = scale * samplesRight[currentSampleIndex];

                line.Add(new Vertex(new Vector2f(window.Size.X / 2 + x, window.Size.Y / 2 - y), Color.Green));
            }

            window.Draw(line.ToArray(), PrimitiveType.LineStrip);

            return true;
        }

        public override RenderConfig GetDefaultRenderConfig()
        {
            return new RenderConfig()
            {
                Title = "Title name",
                Tags = new List<TagRender>(),
                FPS = 60,
                Time = 20,

                IndexSmooth = 1,

                SpeedRender = 1,

                Width = 1000,
                Height = 1000,

                ActionsConfig = new(),

                PathMusic = @"https://zaycev.storm.zerocdn.com/a196fcb864f7ecc7d0e59057d14eadb8:2024092213/track/24511654.mp3",

                Colors = new()
                {
                    { "background", Color.Black },
                    { "line", Color.Green },
                },
            };
        }
    }
}
