using System;

namespace Sockets.Chat.Model.Loggers
{
    public class ConsoleLogger : ILogger
    {
        #region Singleton

        private static volatile ConsoleLogger mInstance;
        private static object _lock = new Object();

        private ConsoleLogger() { }

        public static ConsoleLogger Instance
        {
            get
            {
                if (mInstance == null)
                {
                    lock (_lock)
                    {
                        if (mInstance == null)
                            mInstance = new ConsoleLogger();
                    }
                }

                return mInstance;
            }
        }

        #endregion

        public void Info(string mesage)
        {
            WriteColoredMessage(mesage, ConsoleColor.White);
        }

        public void Debug(string message)
        {
            WriteColoredMessage(message, ConsoleColor.DarkGray);
        }

        public void Error(string message)
        {
            WriteColoredMessage(message, ConsoleColor.Red);
        }

        private void WriteColoredMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"{DateTime.Now} | {message}");
            Console.ResetColor();
        }
    }
}
