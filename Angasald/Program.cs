﻿using Angasal;
using System;
using System.IO;

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
        /// Main entry point
        /// </summary>
        /// <param name="args">Command line arguments</param>
        static void Main(string[] args)
        {
            try
            {
                using (MemoryStream output_memory_stream = new MemoryStream())
                {
                    using (StreamWriter output_writer = new StreamWriter(output_memory_stream))
                    {
                        StreamReader output_reader = new StreamReader(output_memory_stream);
                        using (WebServer web_server = WebServer.Start(80, false, output_writer, output_writer))
                        {
                            if (web_server != null)
                            {
                                while (web_server.IsRunning)
                                {
                                    Console.Clear();
                                    output_writer.Flush();
                                    output_memory_stream.Seek(0L, SeekOrigin.Begin);
                                    Console.WriteLine(output_reader.ReadToEnd());
                                    Console.Write("$ ");
                                    web_server.ParseCommand(Console.ReadLine());
                                }
                            }
                        }
                        Console.Clear();
                        output_writer.Flush();
                        output_memory_stream.Seek(0L, SeekOrigin.Begin);
                        Console.WriteLine(output_reader.ReadToEnd());
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }
    }
}
