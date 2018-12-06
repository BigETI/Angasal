using System.Net;

/// <summary>
/// Angasal programming interface namespace
/// </summary>
namespace AngasalPI
{
    /// <summary>
    /// Angasal plugin interface
    /// </summary>
    public interface IAngasalPlugin
    {
        /// <summary>
        /// On load
        /// </summary>
        void OnLoad();

        /// <summary>
        /// On request
        /// </summary>
        /// <param name="context"></param>
        void OnRequest(HttpListenerContext context);

        /// <summary>
        /// On unload
        /// </summary>
        void OnUnload();
    }
}
