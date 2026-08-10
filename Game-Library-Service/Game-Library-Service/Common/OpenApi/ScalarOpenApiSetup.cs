using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;

namespace Game_Library_Service.Common.OpenApi
{
    /// <summary>
    /// Scalar documentation setup. Registers the OpenAPI documents that Scalar's UI navigates between.
    /// </summary>
    public static class ScalarOpenApiSetup
    {
        /// <summary>
        /// Registers the "v1" API document and the "entities" document used for EF Core entity schema browsing.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddScalarOpenApi(this IServiceCollection services)
        {
            services.AddOpenApi("v1", options =>
            {
                options.UseFullTypeNamesForSchemaIds();
                options.AddDocumentTransformer((document, _, __) =>
                {
                    document.Info = new()
                    {
                        Title = "Game Library Service API",
                        Version = "v1",
                        Description = "API for managing the game library"
                    };
                    return Task.CompletedTask;
                });

                options.ShouldInclude = d => d.GroupName == "v1";
            });

            services.AddOpenApi("entities", options =>
            {
                options.UseFullTypeNamesForSchemaIds();
                options.AddDocumentTransformer((document, _, __) =>
                {
                    document.Info = new()
                    {
                        Title = "Game Library Service - Entities",
                        Version = "entities",
                        Description = "EF Core entity schemas (documentation only)"
                    };
                    return Task.CompletedTask;
                });

                options.ShouldInclude = d => d.GroupName == "entities";
            });

            return services;
        }

        /// <summary>
        /// Adds the given entity types to the "entities" schema-only document.
        /// </summary>
        public static RouteHandlerBuilder AddEntitySchemas(this RouteHandlerBuilder schemaEndpoint, params Type[] entityTypes)
        {
            foreach (var t in entityTypes)
            {
                schemaEndpoint.WithMetadata(new ProducesResponseTypeAttribute(t, StatusCodes.Status204NoContent));
            }

            return schemaEndpoint;
        }
    }
}
