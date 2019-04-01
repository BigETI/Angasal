using System.IO;
/// <summary>
/// Angasal programming interface namespace
/// </summary>
namespace AngasalPI
{
    /// <summary>
    /// Web server interface
    /// </summary>
    public interface IWebServer
    {
        /// <summary>
        /// Is running
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Plugins
        /// </summary>
        IPlugin[] Plugins { get; }

        /// <summary>
        /// Commands
        /// </summary>
        ICommand[] Commands { get; }

        /// <summary>
        /// Standard output
        /// </summary>
        TextWriter StandardOutput { get; }

        /// <summary>
        /// Error output
        /// </summary>
        TextWriter ErrorOutput { get; }

        /// <summary>
        /// Parse command
        /// </summary>
        /// <param name="command">Command</param>
        void ParseCommand(string command);
    }
}
