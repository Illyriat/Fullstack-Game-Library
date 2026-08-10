using Microsoft.AspNetCore.OpenApi;

namespace Game_Library_Service.Common.OpenApi
{
    /// <summary>
    /// Extension methods for OpenApi doc generation
    /// </summary>
    public static class OpenApiExtensions
    {
        /// <summary>
        /// Uses fully qualified names, including namespaces, for types to avoid issues with Command/Query/Result sharing the same name across classes
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static OpenApiOptions UseFullTypeNamesForSchemaIds(this OpenApiOptions options)
        {
            options.CreateSchemaReferenceId = jsonTypeInfo =>
            {
                var type = jsonTypeInfo.Type;

                var defaultId = OpenApiOptions.CreateDefaultSchemaReferenceId(jsonTypeInfo);
                if (string.IsNullOrEmpty(defaultId))
                {
                    return defaultId;
                }

                if (string.IsNullOrEmpty(type.Namespace))
                {
                    return defaultId;
                }

                var ns = type.Namespace;

                var fullName = type.FullName ?? $"{ns}.{defaultId}";
                return fullName.Replace("+", ".", StringComparison.OrdinalIgnoreCase);
            };

            return options;
        }
    }
}
