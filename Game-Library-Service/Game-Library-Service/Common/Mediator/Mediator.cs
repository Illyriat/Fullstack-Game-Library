using Game_Library_Service.Common.Mediator.Interfaces;

namespace Game_Library_Service.Common.Mediator
{
    /// <summary>
    /// Mediator implementation for handling CQRS queries and commands
    /// </summary>
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<Mediator> _logger;

        /// <summary>
        /// Creates a new mediator instance.
        /// </summary>
        /// <param name="serviceProvider">The service provider for resolving handlers.</param>
        /// <param name="logger">The logger instance.</param>
        public Mediator(IServiceProvider serviceProvider, ILogger<Mediator> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        /// <summary>
        /// Creates a scope and resolves the handler to ensure proper scoped service resolution
        /// The scope will be disposed when the handler completes
        /// </summary>
        private (T handler, IServiceScope scope) GetHandlerWithScope<T>() where T : notnull
        {
            var scope = _serviceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<T>();
            return (handler, scope);
        }

        /// <summary>
        /// Sends a command and returns the result.
        /// </summary>
        /// <typeparam name="TCommand">The command type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="command">The command to send.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The command result.</returns>
        public async Task<TResult> SendCommandAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : ICommand<TResult>
        {
            var (handler, scope) = GetHandlerWithScope<ICommandHandler<TCommand, TResult>>();

            _logger.LogDebug("Mediator: Sending command {command}", typeof(TCommand));

            try
            {
                return await handler.HandleAsync(command, cancellationToken);
            }
            finally
            {
                scope.Dispose();
            }
        }

        /// <summary>
        /// Sends a command that does not return a result.
        /// </summary>
        /// <param name="command">The command to send.</param>
        /// <param name="cancellationToken"></param>
        public async Task SendCommandAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : ICommand
        {
            var (handler, scope) = GetHandlerWithScope<ICommandHandler<TCommand>>();

            _logger.LogDebug("Mediator: Sending command {command}", typeof(TCommand));

            try
            {
                await handler.HandleAsync(command, cancellationToken);
            }
            finally
            {
                scope.Dispose();
            }
        }

        /// <summary>
        /// Sends a query and returns the result.
        /// </summary>
        /// <typeparam name="TQuery">The query type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="query">The query to send.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The query result.</returns>
        public async Task<TResult> SendQueryAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
            where TQuery : IQuery<TResult>
        {
            var (handler, scope) = GetHandlerWithScope<IQueryHandler<TQuery, TResult>>();

            _logger.LogDebug("Mediator: Sending query {query}", typeof(TQuery));

            try
            {
                return await handler.HandleAsync(query, cancellationToken);
            }
            finally
            {
                scope.Dispose();
            }
        }
    }
}
