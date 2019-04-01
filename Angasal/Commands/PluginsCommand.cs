using AngasalPI;
using System;

/// <summary>
/// Angasal commands namespace
/// </summary>
namespace Angasal.Commands
{
    /// <summary>
    /// Plugins command class
    /// </summary>
    internal class PluginsCommand : ICommand
    {
        /// <summary>
        /// Keys
        /// </summary>
        public string[] Keys => new string[] { "plugins", "plugin", "modules", "module" };

        /// <summary>
        /// Description
        /// </summary>
        public string Description => "Lists all loaded plugins";

        /// <summary>
        /// Full description
        /// </summary>
        public string FullDescription => "This command lists all loaded plugins." + Environment.NewLine + "\t\tReload all plugins and commands with \"reload\"" + Environment.NewLine + "\t\tUsage: plugins <plugin (optional)>";

        /// <summary>
        /// Execute commmand
        /// </summary>
        /// <param name="context">Command context</param>
        public void Execute(CommandContext context)
        {
            bool list_all = true;
            context.StandardOutput.WriteLine("");
            if (context.Arguments.Length > 0)
            {
                foreach (IPlugin plugin in context.WebServer.Plugins)
                {
                    if (plugin != null)
                    {
                        if (plugin.GetType().FullName.Trim().ToLower() == context.Arguments[0].Trim().ToLower())
                        {
                            context.StandardOutput.WriteLine("== Plugin information ==");
                            context.StandardOutput.WriteLine("");
                            context.StandardOutput.Write("\tType: ");
                            context.StandardOutput.WriteLine(plugin.GetType().FullName);
                            context.StandardOutput.Write("\tName: ");
                            context.StandardOutput.WriteLine(plugin.Name);
                            context.StandardOutput.Write("\tAuthor: ");
                            context.StandardOutput.WriteLine(plugin.Author);
                            context.StandardOutput.Write("\tVersion: ");
                            context.StandardOutput.WriteLine(plugin.Version);
                            context.StandardOutput.WriteLine("");
                            context.StandardOutput.WriteLine("== End of plugin information ==");
                            list_all = false;
                            break;
                        }
                    }
                }
            }
            context.StandardOutput.WriteLine("");
            if (list_all)
            {
                context.StandardOutput.WriteLine("== Plugins ==");
                context.StandardOutput.WriteLine("");
                foreach (IPlugin plugin in context.WebServer.Plugins)
                {
                    if (plugin != null)
                    {
                        context.StandardOutput.Write("\t");
                        context.StandardOutput.Write(plugin.GetType().FullName);
                        context.StandardOutput.Write(" => ");
                        context.StandardOutput.Write(plugin.Name);
                        context.StandardOutput.Write(" ( Version: ");
                        context.StandardOutput.Write(plugin.Version);
                        context.StandardOutput.WriteLine(" )");
                    }
                }
                context.StandardOutput.WriteLine("");
                context.StandardOutput.WriteLine("== End of plugins ==");
            }
        }
    }
}
