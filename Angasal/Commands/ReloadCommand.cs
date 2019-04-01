using AngasalPI;

/// <summary>
/// Angasal commands namespace
/// </summary>
namespace Angasal.Commands
{
    /// <summary>
    /// Reload command class
    /// </summary>
    internal class ReloadCommand : ICommand
    {
        /// <summary>
        /// Keys
        /// </summary>
        public string[] Keys => new string[] { "reload" };

        /// <summary>
        /// Description
        /// </summary>
        public string Description => "Reloads all plugins and commands";

        /// <summary>
        /// Full description
        /// </summary>
        public string FullDescription => "This command reloads all plugins and commands.";

        /// <summary>
        /// Execute command
        /// </summary>
        /// <param name="context">Context</param>
        public void Execute(CommandContext context)
        {
            WebServer web_server = context.WebServer as WebServer;
            if (web_server != null)
            {
                context.StandardOutput.WriteLine("Reloading plugins...");
                web_server.ReloadPlugins();
                context.StandardOutput.WriteLine("Finished reloading plugins!");
            }
        }
    }
}
