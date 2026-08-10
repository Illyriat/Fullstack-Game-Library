namespace Game_Library_Service.Common.Mediator.Interfaces
{
    /// <summary>
    /// Marker interface for queries in the CQRS pattern.
    /// </summary>
    /// <typeparam name="TResult">The type of result returned by the query.</typeparam>
    public interface IQuery<TResult>
    {
    }
}
