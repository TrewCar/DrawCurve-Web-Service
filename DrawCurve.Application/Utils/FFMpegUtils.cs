using DrawCurve.Application.Config;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Threading.Channels;

namespace DrawCurve.Application.Services
{
    public class FFMpegService : BackgroundService
    {
        private readonly Channel<string> _taskQueue;
        private readonly ILogger<FFMpegService> _logger;
        private readonly FFMpegConfig _config;
        private readonly string _ffmpegPath;

        public FFMpegService(ILogger<FFMpegService> logger, IOptions<RenderApplicationConfig> config)
        {
            _logger = logger;
            _taskQueue = Channel.CreateUnbounded<string>();
            _config = config.Value.FFMpegConf;

            _ffmpegPath = _config.Path.ToLower() != "default"
                ? _config.Path
                : Path.Combine(AppContext.BaseDirectory, "Utils", "ffmpeg.exe");

            _logger.LogInformation($"FFMpeg path set to: {_ffmpegPath}");
        }

        public Task QueueFFMpegTask(string arguments)
        {
            return _taskQueue.Writer.WriteAsync(arguments).AsTask();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var arguments in _taskQueue.Reader.ReadAllAsync(stoppingToken))
            {
                await RunFFMpegAsync(arguments, stoppingToken);
            }
        }

        public async Task ConcatFrames(uint FPS, string patternFrames, string pathToFrames, string pathOutVideo, string outNameFile)
        {
            Directory.CreateDirectory(pathOutVideo);
            string outputPath = Path.Combine(pathOutVideo, $"{outNameFile}.mp4");

            string arguments = $"-framerate {FPS} -i \"{Path.Combine(pathToFrames, patternFrames)}\" " +
                               $"-c:v {_config.VideoCodec} -crf {_config.CRF} -pix_fmt {_config.PixelFormat} \"{outputPath}\"";

            await QueueFFMpegTask(arguments);
        }

        public async Task VideoConcatAudio(string pathToVideo, string pathToAudio, string pathOutVideo, string outNameFile)
        {
            Directory.CreateDirectory(pathOutVideo);
            string outputPath = Path.Combine(pathOutVideo, $"{outNameFile}.mp4");

            string arguments = $"-i \"{pathToVideo}\" -i \"{pathToAudio}\" -shortest " +
                               $"-c:v {_config.VideoCodec} -crf {_config.CRF} -pix_fmt {_config.PixelFormat} " +
                               $"-b:a {_config.AudioBitrate} \"{outputPath}\"";

            await QueueFFMpegTask(arguments);
        }

        private async Task RunFFMpegAsync(string arguments, CancellationToken token)
        {
            try
            {
                var startInfo = new ProcessStartInfo(_ffmpegPath, arguments)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                };

                using var process = Process.Start(startInfo);
                if (process != null)
                {
                    string output = await process.StandardOutput.ReadToEndAsync();
                    string error = await process.StandardError.ReadToEndAsync();

                    await process.WaitForExitAsync(token);

                    if (!string.IsNullOrEmpty(error))
                    {
                        _logger.LogError($"FFMpeg Error: {error}");
                    }

                    _logger.LogInformation($"FFMpeg Output: {output}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing FFMpeg");
            }
        }
    }
}
