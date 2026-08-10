namespace Game_Library_Service.Common.Mediator.Interfaces
{
    /// <summary>
    /// Mediator interface for handling CQRS queries and commands.
    /// </summary>
    public interface IMediator
    {
        /// <summary>
        /// Sends a query and returns the result.
        /// </summary>
        /// <typeparam name="TQuery">The query type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="query">The query to send.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The query result.</returns>
        Task<TResult> SendQueryAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken) where TQuery : IQuery<TResult>;

        /// <summary>
        /// Sends a command and returns the result.
        /// </summary>
        /// <typeparam name="TCommand">The command type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="command">The command to send.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The command result.</returns>
        Task<TResult> SendCommandAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken) where TCommand : ICommand<TResult>;

        /// <summary>
        /// Sends a command that does not return a result.
        /// </summary>
        /// <param name="command">The command to send.</param>
        /// <param name="cancellationToken"></param>
        Task SendCommandAsync<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : ICommand;
    }
}
