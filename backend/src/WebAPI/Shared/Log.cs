using System;

namespace WebAPI.Shared
{
    public static class Log
    {
        public static void Debug(string message)
        {
            Console.WriteLine(message);
        }

        public static void Exception(string message, Exception exception)
        {
            Console.WriteLine("EXCEPTION: message");
            Console.WriteLine(exception);
        }
    }
}