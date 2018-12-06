/// <summary>
/// Angasal deamon namespace
/// </summary>
namespace Angasald
{
    /// <summary>
    /// Command arguments
    /// </summary>
    internal class CommandArguments
    {
        /// <summary>
        /// Raw arguments
        /// </summary>
        private string rawArguments;

        /// <summary>
        /// Arguments
        /// </summary>
        private string[] arguments;

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
                    }
                }
                return arguments;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rawArguments">Raw arguments</param>
        public CommandArguments(string rawArguments)
        {
            this.rawArguments = rawArguments;
        }
    }
}
