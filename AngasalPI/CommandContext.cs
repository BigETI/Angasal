using System;
using System.IO;
using System.Text;

/// <summary>
/// Angasal programming interface namespace
/// </summary>
namespace AngasalPI
{
    /// <summary>
    /// Command context class
    /// </summary>
    public class CommandContext
    {
        /// <summary>
        /// Webserver
        /// </summary>
        private IWebserver webserver;

        /// <summary>
        /// Raw arguments
        /// </summary>
        private string rawArguments;

        /// <summary>
        /// Arguments
        /// </summary>
        private string[] arguments;

        /// <summary>
        /// Standard output
        /// </summary>
        private TextWriter standardOutput;

        /// <summary>
        /// Error output
        /// </summary>
        private TextWriter errorOutput;

        /// <summary>
        /// Webserver
        /// </summary>
        public IWebserver Webserver => webserver;

        /// <summary>
        /// Raw arguments
        /// </summary>
        public string RawArguments
        {
            get
            {
                if (rawArguments == null)
                {
                    rawArguments = "";
                }
                return rawArguments;
            }
        }

        /// <summary>
        /// Arguments
        /// </summary>
        public string[] Arguments
        {
            get
            {
                if (arguments == null)
                {
                    arguments = RawArguments.Split(' ');
                    if (arguments == null)
                    {
                        arguments = new string[0];
                    }
                    else
                    {
                        for (int i = 0; i < arguments.Length; i++)
                        {
                            arguments[i] = arguments[i].Trim();
                        }
                        if (arguments.Length == 1)
                        {
                            if (arguments[0] == "")
                            {
                                arguments = new string[0];
                            }
                        }
                    }
                }
                return arguments;
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
        /// Standard error output
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
        /// Constructor
        /// </summary>
        /// <param name="webserver">Webserver</param>
        /// <param name="rawArguments">Raw arguments</param>
        public CommandContext(IWebserver webserver, string rawArguments)
        {
            this.webserver = webserver;
            this.rawArguments = rawArguments;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="webserver">Webserver</param>
        /// <param name="rawArguments">Raw arguments</param>
        /// <param name="standardOutput">Standard output</param>
        /// <param name="errorOutput">Error output</param>
        public CommandContext(IWebserver webserver, string rawArguments, TextWriter standardOutput, TextWriter errorOutput)
        {
            this.webserver = webserver;
            this.rawArguments = rawArguments;
            this.standardOutput = standardOutput;
            this.errorOutput = errorOutput;
        }
    }
}
