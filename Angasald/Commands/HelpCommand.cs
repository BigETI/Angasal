using System;

/// <summary>
/// Angasal deamon commands namespace
/// </summary>
namespace Angasald.Commands
{
    /// <summary>
    /// Help command class
    /// </summary>
    internal class HelpCommand : ICommand
    {
        /// <summary>
        /// Keys
        /// </summary>
        public string[] Keys => new string[] { "help", "commands", "cmds", "?" };

        /// <summary>
        /// Description
        /// </summary>
        public string Description => "Show help topic";

        /// <summary>
        /// Full description
        /// </summary>
        public string FullDescription => "This command shows the available commands and help topics";

        /// <summary>
        /// Execute command
        /// </summary>
        /// <param name="args"></param>
        public void Execute(CommandArguments args)
        {
            ICommand command = null;
            if (args.Arguments.Length > 0)
            {
                command = CommandParser.GetCommandByKey(args.Arguments[0]);
            }
            if (command != null)
            {
                Console.WriteLine("== Help topic ==");
                bool first = true;
                foreach (string key in command.Keys)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        Console.Write(", ");
                    }
                    Console.Write(key);
                }
                Console.WriteLine(" : " + command.Description);
                Console.WriteLine("\t" + command.FullDescription);
                Console.WriteLine("== End of help topic ==");
            }
            else
            {
                Console.WriteLine("== Help topics ==");
                foreach (ICommand cmd in CommandParser.Commands)
                {
                    if (cmd != null)
                    {
                        bool first = true;
                        foreach (string key in cmd.Keys)
                        {
                            if (first)
                            {
                                first = false;
                            }
                            else
                            {
                                Console.Write(", ");
                            }
                            Console.Write(key);
                        }
                        Console.WriteLine(" : " + cmd.Description);
                    }
                }
                Console.WriteLine("== End of help topics ==");
            }
        }
    }
}
