using AngasalPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

/// <summary>
/// Angasal example namespace
/// </summary>
namespace AngasalExample
{
    /// <summary>
    /// Plugin class
    /// </summary>
    public class Plugin : IPlugin
    {
        /// <summary>
        /// Web server
        /// </summary>
        private IWebServer webServer;

        /// <summary>
        /// Commands
        /// </summary>
        private ICommand[] commands;

        /// <summary>
        /// Name
        /// </summary>
        public string Name => "Example plugin";

        /// <summary>
        /// Author
        /// </summary>
        public string Author => "Ethem Kurt";

        /// <summary>
        /// Version
        /// </summary>
        public string Version
        {
            get
            {
                string ret = null;
                try
                {
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                    ret = fvi.FileVersion;
                }
                catch (Exception e)
                {
                    ((webServer == null) ? Console.Error : webServer.ErrorOutput).WriteLine(e);
                }
                if (ret == null)
                {
                    ret = "Unknown";
                }
                return ret;
            }
        }

        /// <summary>
        /// Commands
        /// </summary>
        public ICommand[] Commands
        {
            get
            {
                try
                {
                    if (commands == null)
                    {
                        List<ICommand> commands = new List<ICommand>();
                        Assembly assembly = Assembly.GetExecutingAssembly();
                        if (assembly != null)
                        {
                            Type[] types = assembly.GetTypes();
                            if (types != null)
                            {
                                foreach (Type type in types)
                                {
                                    if (type != null)
                                    {
                                        if (type.IsClass && typeof(ICommand).IsAssignableFrom(type))
                                        {
                                            ICommand command = Activator.CreateInstance(type) as ICommand;
                                            if (command != null)
                                            {
                                                commands.Add(command);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        this.commands = commands.ToArray();
                        commands.Clear();
                    }
                }
                catch (Exception e)
                {
                    ((webServer == null) ? Console.Error : webServer.ErrorOutput).WriteLine(e);
                }
                if (commands == null)
                {
                    commands = new ICommand[0];
                }
                return commands;
            }
        }

        /// <summary>
        /// On load
        /// </summary>
        /// <param name="webServer">Web server</param>
        public void OnLoad(IWebServer webServer)
        {
            this.webServer = webServer;
            webServer.StandardOutput.WriteLine("");
            webServer.StandardOutput.WriteLine("\t===========================");
            webServer.StandardOutput.WriteLine("\t= Angasal example loaded! =");
            webServer.StandardOutput.WriteLine("\t===========================");
            webServer.StandardOutput.WriteLine("");
        }

        /// <summary>
        /// On request
        /// </summary>
        /// <param name="context">Command context</param>
        public void OnRequest(ListenerContext context)
        {
            context.StatusCode = 200;
            context.ResponseEncoding = Encoding.UTF8;
            context.ContentType = "text/html";
            context.Response = "It works!";
        }

        /// <summary>
        /// On unload
        /// </summary>
        /// <param name="webServer">Web server</param>
        public void OnUnload(IWebServer webServer)
        {
            webServer.StandardOutput.WriteLine("");
            webServer.StandardOutput.WriteLine("\t=============================");
            webServer.StandardOutput.WriteLine("\t= Angasal example unloaded! =");
            webServer.StandardOutput.WriteLine("\t=============================");
            webServer.StandardOutput.WriteLine("");
        }
    }
}
