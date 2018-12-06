using System;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// Angasal deamon namespace
/// </summary>
namespace Angasald
{
    /// <summary>
    /// Command parser class
    /// </summary>
    internal static class CommandParser
    {
        /// <summary>
        /// Commands
        /// </summary>
        private static Dictionary<string, ICommand> commandsDictionary;

        /// <summary>
        /// Commands
        /// </summary>
        private static ICommand[] commands;

        /// <summary>
        /// Commands
        /// </summary>
        public static ICommand[] Commands
        {
            get
            {
                InitCommands();
                if (commands == null)
                {
                    commands = new ICommand[0];
                }
                return commands.Clone() as ICommand[];
            }
        }

        /// <summary>
        /// Initialize commands
        /// </summary>
        public static void InitCommands()
        {
            try
            {
                if (commandsDictionary == null)
                {
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    if (assembly != null)
                    {
                        Type[] types = assembly.GetTypes();
                        if (types != null)
                        {
                            List<ICommand> commands = new List<ICommand>();
                            commandsDictionary = new Dictionary<string, ICommand>();
                            foreach (Type type in types)
                            {
                                if (type != null)
                                {
                                    if (type.IsClass && typeof(ICommand).IsAssignableFrom(type))
                                    {
                                        ICommand command = Activator.CreateInstance(type) as ICommand;
                                        if (command != null)
                                        {
                                            foreach (string key in command.Keys)
                                            {
                                                string k = key.Trim().ToLower();
                                                if (commandsDictionary.ContainsKey(k))
                                                {
                                                    Console.Error.WriteLine("Duplicate key \"" + k + "\" in \"" + type.FullName + "\". Registered command class: \"" + commandsDictionary[k] + "\"");
                                                }
                                                else
                                                {
                                                    commandsDictionary.Add(k, command);
                                                }
                                            }
                                            commands.Add(command);
                                        }
                                    }
                                }
                            }
                            CommandParser.commands = commands.ToArray();
                            commands.Clear();
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
        /// Parse command
        /// </summary>
        /// <param name="command">Command</param>
        public static void Parse(string command)
        {
            string[] args = command.Split(' ');
            if (args != null)
            {
                if (args.Length > 0)
                {
                    string arg = args[0];
                    ICommand cmd = GetCommandByKey(arg);
                    if (cmd != null)
                    {
                        cmd.Execute(new CommandArguments(command.Substring((command.Length > arg.Length) ? arg.Length + 1 : arg.Length)));
                    }
                    else
                    {
                        Console.Error.WriteLine("Unknown command \"" + arg.Trim().ToLower() + "\"");
                    }
                }
            }
        }

        /// <summary>
        /// Get command by key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Command if successful, otherwise "null"</returns>
        public static ICommand GetCommandByKey(string key)
        {
            ICommand ret = null;
            InitCommands();
            if ((key != null) && (commandsDictionary != null))
            {
                string k = key.Trim().ToLower();
                if (commandsDictionary.ContainsKey(k))
                {
                    ret = commandsDictionary[k];
                }
            }
            return ret;
        }
    }
}
