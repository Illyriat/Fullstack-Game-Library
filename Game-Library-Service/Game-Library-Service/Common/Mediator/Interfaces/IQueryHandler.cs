namespace Game_Library_Service.Common.Mediator.Interfaces
{
    /// <summary>
    /// Base interface for Query handlers in the CQRS pattern.
    /// </summary>
    /// <typeparam name="TQuery">The query type.</typeparam>
    /// <typeparam name="TResult">The result type.</typeparam>
    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        /// <summary>
        /// Handles the query and returns the result.
        /// </summary>
        /// <param name="query">The query to handle.</param>
        /// <param name="token"></param>
        /// <returns>The query result.</returns>
        Task<TResult> HandleAsync(TQuery query, CancellationToken token);
    }
}
