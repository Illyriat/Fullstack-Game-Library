namespace Game_Library_Service.Common.Mediator.Interfaces
{
    /// <summary>
    /// Marker interface for commands that return a result in the CQRS pattern.
    /// </summary>
    /// <typeparam name="TResult">The type of result returned by the command.</typeparam>
    public interface ICommand<TResult>
    {
    }

    /// <summary>
    /// Marker interface for commands that do not return a result in the CQRS pattern.
    /// </summary>
    public interface ICommand
    {
    }
}
