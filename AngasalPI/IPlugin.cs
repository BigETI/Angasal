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
        /// <param name="webserver">Webserver</param>
        void OnLoad(IWebserver webserver);

        /// <summary>
        /// On request
        /// </summary>
        /// <param name="context">Listener context</param>
        void OnRequest(ListenerContext context);

        /// <summary>
        /// On unload
        /// </summary>
        /// <param name="webserver">Webserver</param>
        void OnUnload(IWebserver webserver);
    }
}
