using System.Net;

/// <summary>
/// Angasal programming interface namespace
/// </summary>
namespace AngasalPI
{
    /// <summary>
    /// Plugin interface
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Author
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Version
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Commands
        /// </summary>
        ICommand[] Commands { get; }

        /// <summary>
        /// On load
        /// </summary>
        /// <param name="webServer">Web server</param>
        void OnLoad(IWebServer webServer);

        /// <summary>
        /// On request
        /// </summary>
        /// <param name="context">Listener context</param>
        void OnRequest(ListenerContext context);

        /// <summary>
        /// On unload
        /// </summary>
        /// <param name="webServer">Web server</param>
        void OnUnload(IWebServer webServer);
    }
}
