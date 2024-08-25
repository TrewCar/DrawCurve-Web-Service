using DrawCurve.Application.Utils;
using Microsoft.Extensions.Logging;

namespace DrawCurve.Application.Logger
{
    public class CustomLoggerProvider : ILoggerProvider
    {
        private readonly Func<CustomLoggerConfiguration> _getCurrentConfig;
        private readonly CustomLoggerConfiguration _config;

        public CustomLoggerProvider(CustomLoggerConfiguration cnf)
        {
            _config = cnf;
            _getCurrentConfig = () =>
            {
                if (!Directory.Exists(DirectoryHelper.PathToLogs))
                    Directory.CreateDirectory(DirectoryHelper.PathToLogs);

                var config = new CustomLoggerConfiguration()
                {
                    LogLevel = _config.LogLevel,
                    LogFile = (string)_config.LogFile.Clone(),
                };

                string logFileName = $"{DateTime.Now:dd-MM-yyyy} - {_config.LogFile}";
                config.LogFile = Path.Combine(DirectoryHelper.PathToLogs, logFileName);

                // Создание файла, если его не существует
                if (!File.Exists(_config.LogFile))
                {
                    using (File.Create(_config.LogFile)) { }
                }

                return config;
            };

        }

        public ILogger CreateLogger(string categoryName)
        {
            return new CustomLogger(categoryName, _getCurrentConfig);
        }

        public void Dispose() { }
    }
}
