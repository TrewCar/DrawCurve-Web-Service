using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawCurve.Application.Utils
{
    public class DirectoryHelper
    {
        public static readonly string PathToProccessFolder = Directory.GetParent(Environment.ProcessPath).FullName;

        public static string GetPathToSaveFrame(string key) => Path.Combine(PathToProccessFolder, "DataVideo", "Frames", key);

        public static string GetPathToSaveVideo(string key) => Path.Combine(PathToProccessFolder, "DataVideo", "Video", key);

        public static string GetPathToSaveResult(string key) => Path.Combine(PathToProccessFolder, "DataVideo", "Result", key);
    }
}
