using Microsoft.Extensions.Logging;

namespace DrawCurve.Application.Logger
{
    public class CustomLogger : ILogger
    {
        private readonly string _name;
        private readonly Func<CustomLoggerConfiguration> _getCurrentConfig;
        private static readonly object _lock = new object();

        public CustomLogger(string name, Func<CustomLoggerConfiguration> getCurrentConfig)
        {
            _name = name;
            _getCurrentConfig = getCurrentConfig;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _getCurrentConfig().LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var config = _getCurrentConfig();
            string message = formatter(state, exception);

            // Форматирование логов
            string formattedMessage = $"[{DateTime.Now:dd-MM-yyyy HH:mm:ss.fff}] [{config.GetShortName(logLevel)}] [{_name}]";
            if (exception != null)
            {
                message += $"\nException: {exception}";
            }

            // Вывод логов в консоль с цветами
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = config.GetLogLevelColor(logLevel);
            Console.WriteLine(formattedMessage);
            Console.ForegroundColor = originalColor;
            Console.WriteLine("\t" + message);

            // Сохранение логов в файл
            lock (_lock)
            {
                File.AppendAllText(config.LogFile, formattedMessage + Environment.NewLine + "\t" + message + Environment.NewLine);
            }
        }
    }
}
