using AngasalPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;

/// <summary>
/// Angasal namespace
/// </summary>
namespace Angasal
{
    /// <summary>
    /// Webserver class
    /// </summary>
    public class Webserver : IDisposable
    {
        /// <summary>
        /// HTTP listener
        /// </summary>
        private HttpListener httpListener;

        /// <summary>
        /// Plugins
        /// </summary>
        private IAngasalPlugin[] plugins;

        /// <summary>
        /// Is webserver running
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return ((httpListener == null) ? false : httpListener.IsListening);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpListener">HTTP listener</param>
        /// <param name="plugins">Plugins</param>
        private Webserver(HttpListener httpListener, IAngasalPlugin[] plugins)
        {
            this.httpListener = httpListener;
            this.plugins = plugins;
        }

        /// <summary>
        /// Start webserver
        /// </summary>
        /// <returns>Webserver if successful, otherwise "null"</returns>
        public static Webserver Start()
        {
            Webserver ret = null;
            try
            {
                if (HttpListener.IsSupported)
                {
                    string plugins_directory = Path.Combine(Environment.CurrentDirectory, "plugins");
                    List<IAngasalPlugin> plugins = new List<IAngasalPlugin>();
                    if (Directory.Exists(plugins_directory))
                    {
                        string[] files = Directory.GetFiles(plugins_directory, "*.dll");
                        if (files != null)
                        {
                            Console.WriteLine("Loading plugins...");
                            foreach (string file in files)
                            {
                                if (file != null)
                                {
                                    try
                                    {
                                        Assembly assembly = Assembly.LoadFile(file);
                                        if (assembly != null)
                                        {
                                            Type[] types = assembly.GetExportedTypes();
                                            if (types != null)
                                            {
                                                foreach (Type type in types)
                                                {
                                                    if (type != null)
                                                    {
                                                        if (type.IsClass && typeof(IAngasalPlugin).IsAssignableFrom(type))
                                                        {
                                                            IAngasalPlugin plugin = (IAngasalPlugin)(Activator.CreateInstance(type));
                                                            if (plugin != null)
                                                            {
                                                                plugins.Add(plugin);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Console.Error.WriteLine(e);
                                    }
                                }
                            }
                        }
                    }
                    HttpListener http_listener = new HttpListener();
                    {
                        Console.WriteLine("Initializing webserver...");
                        http_listener.Prefixes.Add("http://localhost:80/");
                        http_listener.Start();
                        ret = new Webserver(http_listener, plugins.ToArray());
                        http_listener.BeginGetContext(new AsyncCallback(ret.ContextReceivedCallback), null);
                        foreach (IAngasalPlugin plugin in plugins)
                        {
                            plugin.OnLoad();
                        }
                        Console.WriteLine("Finished initializing webserver!");
                    }
                }
                else
                {
                    Console.Error.WriteLine("Listening to HTTP requests is not supported in this platform.");
                }
            }
            catch (Exception e)
            {
                if (ret != null)
                {
                    ret.Dispose();
                    ret = null;
                }
                Console.Error.WriteLine(e);
                Console.ReadLine();
            }
            return ret;
        }

        /// <summary>
        /// Context recieved callback
        /// </summary>
        /// <param name="asyncResult">Asynchronous result</param>
        private void ContextReceivedCallback(IAsyncResult asyncResult)
        {
            try
            {
                if (httpListener.IsListening)
                {
                    HttpListenerContext context = httpListener.EndGetContext(asyncResult);
                    httpListener.BeginGetContext(new AsyncCallback(ContextReceivedCallback), null);
                    foreach (IAngasalPlugin plugin in plugins)
                    {
                        plugin.OnRequest(context);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }

        /// <summary>
        /// Close webserver
        /// </summary>
        public void Close()
        {
            if (IsRunning)
            {
                Console.WriteLine("Closing webserver...");
                foreach (IAngasalPlugin plugin in plugins)
                {
                    plugin.OnUnload();
                }
                httpListener.Close();
            }
        }

        /// <summary>
        /// Dispose webserver
        /// </summary>
        public void Dispose()
        {
            Close();
        }
    }
}
