namespace Luxora_api.Logging
{
    public enum LogType
    {
        Info,
        Warning,
        Error
    }
    public class Logging : ILogging
    {
 

        public void log(string message, LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR - {message}");
                    break;

                case LogType.Warning:
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"WARNING - {message}");
                    break;

                case LogType.Info:
                default:
                    Console.WriteLine($"INFO - {message}");
                    break;
            }

            Console.ResetColor(); // أفضل من تعيين Black يدويًا
        }
    }

   
}
