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
    /// Web server class
    /// </summary>
    public class WebServer : IWebServer, IDisposable
    {
        /// <summary>
        /// HTTP listener
        /// </summary>
        private HttpListener httpListener;

        /// <summary>
        /// Plugins
        /// </summary>
        private IPlugin[] plugins;

        /// <summary>
        /// Commands
        /// </summary>
        private Dictionary<string, ICommand> commandsDictionary = new Dictionary<string, ICommand>();

        /// <summary>
        /// Commands
        /// </summary>
        private ICommand[] commands;

        /// <summary>
        /// Standard output
        /// </summary>
        private TextWriter standardOutput;

        /// <summary>
        /// Error output
        /// </summary>
        private TextWriter errorOutput;

        /// <summary>
        /// Is web server running
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return (httpListener != null);
            }
        }

        /// <summary>
        /// Plugins
        /// </summary>
        public IPlugin[] Plugins
        {
            get
            {
                if (plugins == null)
                {
                    plugins = new IPlugin[0];
                }
                return plugins.Clone() as IPlugin[];
            }
        }

        /// <summary>
        /// Commands
        /// </summary>
        public ICommand[] Commands
        {
            get
            {
                if (commands == null)
                {
                    commands = new ICommand[0];
                }
                return commands.Clone() as ICommand[];
            }
        }

        /// <summary>
        /// Standard output
        /// </summary>
        public TextWriter StandardOutput
        {
            get
            {
                if (standardOutput == null)
                {
                    standardOutput = Console.Out;
                }
                return standardOutput;
            }
        }

        /// <summary>
        /// Error output
        /// </summary>
        public TextWriter ErrorOutput
        {
            get
            {
                if (errorOutput == null)
                {
                    errorOutput = Console.Error;
                }
                return errorOutput;
            }
        }

        /// <summary>
        /// Is web server listening
        /// </summary>
        public bool IsListening
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
        /// <param name="standardOutput">Standard output</param>
        /// <param name="errorOutput">Error output</param>
        private WebServer(HttpListener httpListener, TextWriter standardOutput, TextWriter errorOutput)
        {
            this.httpListener = httpListener;
            this.standardOutput = standardOutput;
            this.errorOutput = errorOutput;
        }

        /// <summary>
        /// Add command
        /// </summary>
        /// <param name="command">Command</param>
        /// <param name="commands">Commands</param>
        private void AddCommand(ICommand command, List<ICommand> commands)
        {
            if (command != null)
            {
                foreach (string key in command.Keys)
                {
                    string k = key.Trim().ToLower();
                    if (commandsDictionary.ContainsKey(k))
                    {
                        ErrorOutput.WriteLine("Duplicate key \"" + k + "\" in \"" + command.GetType().FullName + "\". Registered command class: \"" + commandsDictionary[k].GetType().FullName + "\"");
                    }
                    else
                    {
                        commandsDictionary.Add(k, command);
                    }
                }
                commands.Add(command);
            }
        }

        /// <summary>
        /// Reload plugins
        /// </summary>
        public void ReloadPlugins()
        {
            List<IPlugin> plugins = new List<IPlugin>();
            List<ICommand> commands = new List<ICommand>();
            commandsDictionary.Clear();
            try
            {
                string plugins_directory = Path.Combine(Environment.CurrentDirectory, "plugins");
                UnloadPlugins();
                if (Directory.Exists(plugins_directory))
                {
                    string[] files = Directory.GetFiles(plugins_directory, "*.dll");
                    if (files != null)
                    {
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
                                                    if (type.IsClass && typeof(IPlugin).IsAssignableFrom(type))
                                                    {
                                                        IPlugin plugin = Activator.CreateInstance(type) as IPlugin;
                                                        if (plugin != null)
                                                        {
                                                            ICommand[] plugin_commands = plugin.Commands;
                                                            if (plugin_commands != null)
                                                            {
                                                                foreach (ICommand plugin_command in plugin_commands)
                                                                {
                                                                    AddCommand(plugin_command, commands);
                                                                }
                                                            }
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
                                    ErrorOutput.WriteLine(e);
                                }
                            }
                        }
                    }
                }
                try
                {
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
                                        AddCommand(command, commands);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorOutput.WriteLine(e);
                }
            }
            catch (Exception e)
            {
                ErrorOutput.WriteLine(e);
            }
            this.plugins = plugins.ToArray();
            plugins.Clear();
            this.commands = commands.ToArray();
            commands.Clear();
            foreach (IPlugin plugin in this.plugins)
            {
                plugin.OnLoad(this);
            }
        }

        /// <summary>
        /// Unload plugins
        /// </summary>
        private void UnloadPlugins()
        {
            if (plugins != null)
            {
                foreach (IPlugin plugin in plugins)
                {
                    plugin.OnUnload(this);
                }
                plugins = null;
            }
            ClearCommands();
        }

        /// <summary>
        /// Start web server
        /// </summary>
        /// <param name="port">Port</param>
        /// <param name="allowHTTPS">Allow HTTPS</param>
        /// <param name="standardOutput">Standard output</param>
        /// <param name="errorOutput">Error output</param>
        /// <returns>Web server if successful, otherwise "null"</returns>
        public static WebServer Start(ushort port, bool allowHTTPS, TextWriter standardOutput, TextWriter errorOutput)
        {
            WebServer ret = null;
            if (standardOutput == null)
            {
                standardOutput = Console.Out;
            }
            if (errorOutput == null)
            {
                errorOutput = Console.Error;
            }
            try
            {
                if (HttpListener.IsSupported)
                {
                    HttpListener http_listener = new HttpListener();
                    standardOutput.WriteLine("Initializing web server...");
                    http_listener.Prefixes.Add("http://*:" + port + "/");
                    if (allowHTTPS)
                    {
                        http_listener.Prefixes.Add("https://*:" + port + "/");
                    }
                    http_listener.Start();
                    ret = new WebServer(http_listener, standardOutput, errorOutput);
                    http_listener.BeginGetContext(new AsyncCallback(ret.ContextReceivedCallback), null);
                    standardOutput.WriteLine("Loading plugins...");
                    ret.ReloadPlugins();
                    standardOutput.WriteLine("Finished loading plugins!");
                    standardOutput.WriteLine("Finished initializing web server!");
                }
                else
                {
                    errorOutput.WriteLine("Listening to HTTP requests is not supported in this platform.");
                }
            }
            catch (Exception e)
            {
                if (ret != null)
                {
                    ret.Dispose();
                    ret = null;
                }
                errorOutput.WriteLine(e);
            }
            return ret;
        }

        /// <summary>
        /// Start web server
        /// </summary>
        /// <param name="port">Port</param>
        /// <param name="allowHTTPS">Allow HTTPS</param>
        /// <returns>Web server if successful, otherwise "null"</returns>
        public static WebServer Start(ushort port, bool allowHTTPS)
        {
            return Start(port, allowHTTPS, Console.Out, Console.Error);
        }

        /// <summary>
        /// Start web server
        /// </summary>
        /// <param name="port">Port</param>
        /// <returns>Web server if successful, otherwise "null"</returns>
        public static WebServer Start(ushort port)
        {
            return Start(port, false, Console.Out, Console.Error);
        }

        /// <summary>
        /// Context recieved callback
        /// </summary>
        /// <param name="asyncResult">Asynchronous result</param>
        private void ContextReceivedCallback(IAsyncResult asyncResult)
        {
            try
            {
                if (IsListening)
                {
                    HttpListenerContext context = httpListener.EndGetContext(asyncResult);
                    httpListener.BeginGetContext(new AsyncCallback(ContextReceivedCallback), null);
                    using (Stream stream = context.Response.OutputStream)
                    {
                        foreach (IPlugin plugin in plugins)
                        {
                            ListenerContext listener_context = new ListenerContext(context, StandardOutput, ErrorOutput);
                            plugin.OnRequest(listener_context);
                            stream.Write(listener_context.ResponseBytes, 0, listener_context.ResponseBytes.Length);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorOutput.WriteLine(e);
            }
        }

        /// <summary>
        /// Parse command
        /// </summary>
        /// <param name="command">Command</param>
        public void ParseCommand(string command)
        {
            string[] args = command.Split(' ');
            if (args != null)
            {
                if (args.Length > 0)
                {
                    string arg = args[0];
                    ICommand cmd = GetCommandByKey(arg);
                    if (cmd != null)
                    {
                        cmd.Execute(new CommandContext(this, command.Substring((command.Length > arg.Length) ? arg.Length + 1 : arg.Length), StandardOutput, ErrorOutput));
                    }
                    else
                    {
                        ErrorOutput.WriteLine("Unknown command \"" + arg.Trim().ToLower() + "\"");
                    }
                }
            }
        }

        /// <summary>
        /// Get command by key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Command if successful, otherwise "null"</returns>
        public ICommand GetCommandByKey(string key)
        {
            ICommand ret = null;
            if (key != null)
            {
                string k = key.Trim().ToLower();
                if (commandsDictionary.ContainsKey(k))
                {
                    ret = commandsDictionary[k];
                }
            }
            return ret;
        }

        /// <summary>
        /// Clear commands
        /// </summary>
        private void ClearCommands()
        {
            commandsDictionary.Clear();
            commands = null;
        }

        /// <summary>
        /// Stop web server
        /// </summary>
        public void Stop()
        {
            UnloadPlugins();
            if (IsRunning)
            {
                StandardOutput.WriteLine("Stopping web server...");
                httpListener.Close();
                httpListener = null;
            }
        }

        /// <summary>
        /// Dispose web server
        /// </summary>
        public void Dispose()
        {
            Stop();
        }
    }
}
