using Angasal;
using System;

/// <summary>
/// Angasal daemon namespace
/// </summary>
namespace Angasald
{
    /// <summary>
    /// Program class
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Keep running
        /// </summary>
        private static bool keepRunning = true;

        /// <summary>
        /// Keep running
        /// </summary>
        public static bool KeepRunning => keepRunning;

        /// <summary>
        /// Main entry point
        /// </summary>
        /// <param name="args">Command line arguments</param>
        static void Main(string[] args)
        {
            try
            {
                using (Webserver webserver = Webserver.Start())
                {
                    if (webserver != null)
                    {
                        while (keepRunning)
                        {
                            Console.Write("$ ");
                            CommandParser.Parse(Console.ReadLine());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }

        /// <summary>
        /// Stop webserver
        /// </summary>
        public static void StopWebserver()
        {
            keepRunning = false;
        }
    }
}
