using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace DrawCurve.Application.Utils
{
    public class FFMpegUtils
    {
        public static readonly string PathToFFMpeg = Path.Combine(Directory.GetParent(Environment.ProcessPath).FullName, "Utils", "ffmpeg.exe");
        public static void ConcatFrames(uint FPS, string paternFrames, string pathToFrames, string pathOutVideo, string outNameFile)
        {
            if(!Directory.Exists(pathOutVideo)) 
                Directory.CreateDirectory(pathOutVideo);
            
            pathOutVideo = Path.Combine(pathOutVideo, outNameFile + ".mp4");

            string arguments = $"-framerate {FPS} -i \"{pathToFrames}\\{paternFrames}\" -c:v libx264 -crf 20 -pix_fmt yuv420p \"{pathOutVideo}\"";

            Process.Start(new ProcessStartInfo(PathToFFMpeg, arguments)).WaitForExit();

            Thread.Sleep(1000);
        }
        public static void VideoConcatAudio(string pathToVideo, string pathToAudio, string pathOutVideo, string outNameFile)
        {
            if (!Directory.Exists(pathOutVideo))
                Directory.CreateDirectory(pathOutVideo);

            pathOutVideo = Path.Combine(pathOutVideo, outNameFile + ".mp4");

            string arguments = $"-i \"{pathToVideo}\" -i \"{pathToAudio}\" -shortest -c:v libx264 -crf 20 -pix_fmt yuv420p -b:a 192k \"{pathOutVideo}\"";

            Process.Start(new ProcessStartInfo(PathToFFMpeg, arguments)).WaitForExit();

            Thread.Sleep(1000);
        }
    }
}
