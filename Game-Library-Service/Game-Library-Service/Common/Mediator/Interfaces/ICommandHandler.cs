namespace Game_Library_Service.Common.Mediator.Interfaces
{
    /// <summary>
    /// Base interface for Command handlers, that return a value, in the CQRS pattern.
    /// </summary>
    /// <typeparam name="TCommand">The command type.</typeparam>
    /// <typeparam name="TResult">The result type.</typeparam>
    public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
    {
        /// <summary>
        /// Handles the command and returns the result.
        /// </summary>
        /// <param name="command">The command to handle.</param>
        /// <param name="token"></param>
        /// <returns>The command result.</returns>
        Task<TResult> HandleAsync(TCommand command, CancellationToken token);
    }

    /// <summary>
    /// Base interface for Command handlers, that don't return a value, in the CQRS pattern.
    /// </summary>
    /// <typeparam name="TCommand">The command type.</typeparam>
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        /// <summary>
        /// Handles the command without returning a result.
        /// </summary>
        /// <param name="command">The command to handle.</param>
        /// <param name="token"></param>
        Task HandleAsync(TCommand command, CancellationToken token);
    }
}
