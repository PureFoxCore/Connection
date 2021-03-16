using System;

namespace CSLogger
{
    public static class Logger
    {
        public static void Trace(object message) =>
            InternalLog(message, LogLevel.Trace);

        public static void Info(object message) =>
            InternalLog(message, LogLevel.Info);

        public static void Warn(object message) =>
            InternalLog(message, LogLevel.Warn);

        public static void Error(object message) =>
            InternalLog(message, LogLevel.Error);

        public static void Critical(object message) =>
            InternalLog(message, LogLevel.Critical);

        public static void Log(object message, LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Trace:
                    Trace(message);
                    break;
                case LogLevel.Info:
                    Info(message);
                    break;
                case LogLevel.Warn:
                    Warn(message);
                    break;
                case LogLevel.Error:
                    Error(message);
                    break;
                case LogLevel.Critical:
                    Critical(message);
                    break;
            }
        }

        private static void InternalLog(object message, LogLevel level)
        {
            var fColor = Console.ForegroundColor;
            var bColor = Console.BackgroundColor;

            switch (level)
            {
                case LogLevel.Trace:
                    SetForeground(ConsoleColor.White);
                    break;
                case LogLevel.Info:
                    SetForeground(ConsoleColor.Green);
                    break;
                case LogLevel.Warn:
                    SetForeground(ConsoleColor.Yellow);
                    break;
                case LogLevel.Error:
                    SetForeground(ConsoleColor.Red);
                    break;
                case LogLevel.Critical:
                    SetForeground(ConsoleColor.Black);
                    SetBackground(ConsoleColor.Red);
                    break;
            }
            
            Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss.fff")}] [{level.ToString()}] {message}");
            
            SetForeground(fColor);
            SetBackground(bColor);
        }

        private static void SetForeground(ConsoleColor color = ConsoleColor.White) =>
            Console.ForegroundColor = color;

        private static void SetBackground(ConsoleColor color = ConsoleColor.Black) =>
            Console.BackgroundColor = color;
    }
}