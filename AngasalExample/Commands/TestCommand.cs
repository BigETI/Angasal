using AngasalPI;

/// <summary>
/// Angasal example commands namespace
/// </summary>
namespace AngasalExample.Commands
{
    /// <summary>
    /// Test command class
    /// </summary>
    internal class TestCommand : ICommand
    {
        /// <summary>
        /// Keys
        /// </summary>
        public string[] Keys => new string[] { "test", "example" };

        /// <summary>
        /// Description
        /// </summary>
        public string Description => "Test command";

        /// <summary>
        /// Full description
        /// </summary>
        public string FullDescription => "This is just a test command";

        /// <summary>
        /// Execute command
        /// </summary>
        /// <param name="context">Command context</param>
        public void Execute(CommandContext context)
        {
            context.StandardOutput.WriteLine("It works!");
        }
    }
}
