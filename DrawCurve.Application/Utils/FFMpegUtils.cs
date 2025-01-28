using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DrawCurve.Application.Utils
{
    public class FFMpegUtils
    {
        public static string PathToFFMpeg = Path.Combine(Directory.GetParent(Environment.ProcessPath).FullName, "Utils", "ffmpeg.exe");

        public static void ConcatFrames(uint FPS, string paternFrames, string pathToFrames, string pathOutVideo, string outNameFile)
        {
            if (!Directory.Exists(pathOutVideo))
                Directory.CreateDirectory(pathOutVideo);

            pathOutVideo = Path.Combine(pathOutVideo, outNameFile + ".mp4");

            string arguments = $"-framerate {FPS} -i \"{Path.Combine(pathToFrames, paternFrames)}\" -c:v libx264 -crf 20 -pix_fmt yuv420p \"{pathOutVideo}\"";

            RunFFMpeg(arguments);
        }

        public static void VideoConcatAudio(string pathToVideo, string pathToAudio, string pathOutVideo, string outNameFile)
        {
            if (!Directory.Exists(pathOutVideo))
                Directory.CreateDirectory(pathOutVideo);

            pathOutVideo = Path.Combine(pathOutVideo, outNameFile + ".mp4");

            string arguments = $"-i \"{pathToVideo}\" -i \"{pathToAudio}\" -shortest -c:v libx264 -crf 20 -pix_fmt yuv420p -b:a 192k \"{pathOutVideo}\"";

            RunFFMpeg(arguments);
        }

        private static void RunFFMpeg(string arguments)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(PathToFFMpeg, arguments)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                };

                using (Process process = Process.Start(startInfo))
                {
                    if (process != null)
                    {
                        process.WaitForExit();

                        // Optionally, you can handle the output and errors here
                        string output = process.StandardOutput.ReadToEnd();
                        string error = process.StandardError.ReadToEnd();

                        if (!string.IsNullOrEmpty(error))
                        {
                            Console.WriteLine($"FFMpeg Error: {error}");
                        }

                        Console.WriteLine($"FFMpeg Output: {output}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing FFMpeg: {ex.Message}");
            }
        }
    }
}
