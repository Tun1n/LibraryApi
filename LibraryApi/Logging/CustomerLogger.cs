namespace LibraryApi.Logging
{
    public class CustomerLogger : ILogger
    {
        readonly string loggerName;
        readonly CustomLoggerProviderConfiguration loggerConfig;

        public CustomerLogger(string name, CustomLoggerProviderConfiguration config)
        {
            loggerName = name;
            loggerConfig = config;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == loggerConfig.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
                Exception exception, Func<TState, Exception, string> formatter)
        {
            string message = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";

            WriteTextInFile(message);
        }

        private void WriteTextInFile(string message)
        {
            string filePath = string.IsNullOrWhiteSpace(loggerConfig.FilePath)
                ? Path.Combine(Directory.GetCurrentDirectory(), "Log.txt")
                : Path.IsPathRooted(loggerConfig.FilePath)
                    ? loggerConfig.FilePath
                    : Path.Combine(Directory.GetCurrentDirectory(), loggerConfig.FilePath);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            using (StreamWriter streamWriter = new StreamWriter(filePath, true))
            {
                try
                {
                    streamWriter.WriteLine(message);
                    streamWriter.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
