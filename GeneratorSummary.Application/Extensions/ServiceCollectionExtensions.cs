using GeneratorSummary.Application.Services.GeneratorSummaryService;
using GeneratorSummary.Application.Services.ReferenceData;
using GeneratorSummary.Data.Repositories.GeneratorSummary;
using Microsoft.Extensions.DependencyInjection;

namespace GeneratorSummary.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IGeneratorSummaryService, GeneratorSummaryService>();
        }

        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddTransient<IXmlOperationRepository, XmlOperationRepository>();
        }
    }
}
