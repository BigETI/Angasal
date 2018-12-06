/// <summary>
/// Angasal daemon namespace
/// </summary>
namespace Angasald
{
    /// <summary>
    /// Command interface
    /// </summary>
    internal interface ICommand
    {
        /// <summary>
        /// Keys
        /// </summary>
        string[] Keys { get; }

        /// <summary>
        /// Description
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Full description
        /// </summary>
        string FullDescription { get; }

        /// <summary>
        /// Execute command
        /// </summary>
        /// <param name="args">Command arguments</param>
        void Execute(CommandArguments args);
    }
}
