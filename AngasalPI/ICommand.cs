/// <summary>
/// Angasal programming interface namespace
/// </summary>
namespace AngasalPI
{
    /// <summary>
    /// Command interface
    /// </summary>
    public interface ICommand
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
        /// <param name="context">Command context</param>
        void Execute(CommandContext context);
    }
}
