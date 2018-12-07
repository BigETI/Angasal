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
        public string Description => "Closes the webserver";

        /// <summary>
        /// Full description
        /// </summary>
        public string FullDescription => "This command shuts down the webserver.";

        /// <summary>
        /// Execute command
        /// </summary>
        /// <param name="args">Command context</param>
        public void Execute(CommandContext context)
        {
            Webserver webserver = context.Webserver as Webserver;
            if (webserver != null)
            {
                webserver.Stop();
            }
        }
    }
}
