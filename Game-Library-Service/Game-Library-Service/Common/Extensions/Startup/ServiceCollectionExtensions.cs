using Game_Library_Service.Common.Mediator.Interfaces;

namespace Game_Library_Service.Common.Extensions.Startup
{
    /// <summary>
    /// Extension methods for configuring services in the dependency injection container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures the mediator pattern and registers all CQRS handlers.
        /// Register each new query/command handler here as features are added, e.g.:
        /// <code>services.AddScoped&lt;IQueryHandler&lt;GetGameById.Query, GetGameById.Result&gt;, GetGameById.Handler&gt;();</code>
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void ConfigureMediatorAndHandlers(this IServiceCollection services)
        {
            services.AddSingleton<IMediator, Mediator.Mediator>();
        }
    }
}
