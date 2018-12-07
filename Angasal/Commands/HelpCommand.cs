using AngasalPI;
using System;

/// <summary>
/// Angasal commands namespace
/// </summary>
namespace Angasal.Commands
{
    /// <summary>
    /// Help command class
    /// </summary>
    internal class HelpCommand : ICommand
    {
        /// <summary>
        /// Keys
        /// </summary>
        public string[] Keys => new string[] { "help", "commands", "command", "cmds", "cmd", "?" };

        /// <summary>
        /// Description
        /// </summary>
        public string Description => "Shows help topics";

        /// <summary>
        /// Full description
        /// </summary>
        public string FullDescription => "This command shows the available commands and help topics." + Environment.NewLine + "\t\tUsage: help <command (optional)>";

        /// <summary>
        /// Execute command
        /// </summary>
        /// <param name="context">Command context</param>
        public void Execute(CommandContext context)
        {
            Webserver webserver = context.Webserver as Webserver;
            if (webserver != null)
            {
                ICommand command = null;
                if (context.Arguments.Length > 0)
                {
                    command = webserver.GetCommandByKey(context.Arguments[0]);
                }
                context.StandardOutput.WriteLine("");
                if (command != null)
                {
                    context.StandardOutput.WriteLine("== Help topic ==");
                    context.StandardOutput.WriteLine("");
                    bool first = true;
                    foreach (string key in command.Keys)
                    {
                        if (first)
                        {
                            first = false;
                            context.StandardOutput.Write("\t");
                        }
                        else
                        {
                            context.StandardOutput.Write(", ");
                        }
                        context.StandardOutput.Write(key);
                    }
                    context.StandardOutput.Write(" : ");
                    context.StandardOutput.WriteLine(command.Description);
                    context.StandardOutput.Write("\t\t");
                    context.StandardOutput.WriteLine(command.FullDescription);
                    context.StandardOutput.WriteLine("");
                    context.StandardOutput.WriteLine("== End of help topic ==");
                }
                else
                {
                    context.StandardOutput.WriteLine("== Help topics ==");
                    context.StandardOutput.WriteLine("");
                    foreach (ICommand cmd in webserver.Commands)
                    {
                        if (cmd != null)
                        {
                            bool first = true;
                            foreach (string key in cmd.Keys)
                            {
                                if (first)
                                {
                                    first = false;
                                    context.StandardOutput.Write("\t");
                                }
                                else
                                {
                                    context.StandardOutput.Write(", ");
                                }
                                context.StandardOutput.Write(key);
                            }
                            context.StandardOutput.Write(" : ");
                            context.StandardOutput.WriteLine(cmd.Description);
                        }
                    }
                    context.StandardOutput.WriteLine("");
                    context.StandardOutput.WriteLine("== End of help topics ==");
                }
            }
        }
    }
}
