namespace MyAPI.Logging
{
    public class Loggingv2 : ILogging
    {
        public void Log(string message, string type)
        {
            if (type == "error")
            {
                //Console.Error.WriteLine(message);
                Console.BackgroundColor= ConsoleColor.Red;
                Console.WriteLine("Error - "+ message);
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
            {

                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine(message);
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }
    }
}
