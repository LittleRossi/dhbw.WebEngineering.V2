using System.Text.Json.Serialization;

namespace dhbw.WebEngineering.V2.Api.Extensions
{
    public static class JsonConvertersExtensions
    {
        public static IServiceCollection AddCustomJsonConverters(this IServiceCollection services)
        {
            services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition =
                        JsonIgnoreCondition.WhenWritingNull;
                });

            return services;
        }
    }
}
