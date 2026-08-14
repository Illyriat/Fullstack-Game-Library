using Game_Library_Service.Common.Mediator.Interfaces;
using Game_Library_Service.Features.Game_Lib_FE.Logic;

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

            services.AddScoped<IQueryHandler<GetGames.Query, GetGames.Result>, GetGames.Handler>();
            services.AddScoped<IQueryHandler<GetPublishers.Query, GetPublishers.Result>, GetPublishers.Handler>();
            services.AddScoped<IQueryHandler<GetGenres.Query, GetGenres.Result>, GetGenres.Handler>();
        }
    }
}
