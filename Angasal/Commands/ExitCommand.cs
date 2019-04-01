using AngasalPI;

/// <summary>
/// Angasal commands namespace
/// </summary>
namespace Angasal.Commands
{
    /// <summary>
    /// Exit command class
    /// </summary>
    internal class ExitCommand : ICommand
    {
        /// <summary>
        /// Keys
        /// </summary>
        public string[] Keys => new string[] { "exit", "close", "quit" };

        /// <summary>
        /// Description
        /// </summary>
        public string Description => "Closes the web server";

        /// <summary>
        /// Full description
        /// </summary>
        public string FullDescription => "This command shuts down the web server.";

        /// <summary>
        /// Execute command
        /// </summary>
        /// <param name="args">Command context</param>
        public void Execute(CommandContext context)
        {
            WebServer web_server = context.WebServer as WebServer;
            if (web_server != null)
            {
                web_server.Stop();
            }
        }
    }
}
