using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawCurve.Application.Config
{
    public class RenderApplicationConfig
    {
        public FFMpegConfig FFMpegConf { get; set; } = new();
        public RenderSVGConfig RenderSVG { get; set; } = new();
        public QueueOptionsConfig QueueOptions { get; set; } = new();
        public LimiterConfig Limiter { get; set; } = new();
    }

    public class FFMpegConfig
    {
        public string Path { get; set; } = "default";
        public string VideoCodec { get; set; } = "libx264";
        public string PixelFormat { get; set; } = "yuv420p";
        public int CRF { get; set; } = 23;
        public string AudioBitrate { get; set; } = "192k";
    }

    public class RenderSVGConfig
    {
        public int CountSegments { get; set; } = 1000;
        public double IndexError { get; set; } = 0.001;
    }

    public class QueueOptionsConfig
    {
        public int MaxProccessRenderFrames { get; set; } = 10;
        public int MaxProccessConcatFrames { get; set; } = 10;
        public int MaxProccessConcatAudio { get; set; } = 10;
    }

    public class LimiterConfig
    {
        public class VideoLimiter
        {
            public int MaxWidth { get; set; } = 2560;
            public int MinWidth { get; set; } = 100;
            public int MaxHeight { get; set; } = 1440;
            public int MinHeight { get; set; } = 100;
            public int MaxFPS { get; set; } = 144;
            public int MinFPS { get; set; } = 10;
            public int MinTime { get; set; } = 10;
            public int MaxTime { get; set; } = 600;
        }

        public VideoLimiter Video = new();

    }

}
