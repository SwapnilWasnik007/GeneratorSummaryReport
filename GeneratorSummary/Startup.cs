using GeneratorSummary;
using GeneratorSummary.Application.Constants;
using GeneratorSummary.Application.Extensions;
using GeneratorSummary.Application.Services.ReferenceData;
using GeneratorSummary.Common.Configurations;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

[assembly: FunctionsStartup(typeof(Startup))]
namespace GeneratorSummary
{
    public class Startup : FunctionsStartup
    {
        private XmlFileConfig XmlFileConfig { get; set; } = null!;

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();
            builder.ConfigurationBuilder.Sources.Clear();

            builder.ConfigurationBuilder.AddJsonFile(
            Path.Combine(context.ApplicationRootPath, $"appsettings.json"), optional: false, reloadOnChange: false);
            builder.ConfigurationBuilder.AddJsonFile(
                Path.Combine(context.ApplicationRootPath, $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json"), optional: false, reloadOnChange: false);
            builder.ConfigurationBuilder.AddEnvironmentVariables();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = builder.GetContext().Configuration;
            XmlFileConfig = new XmlFileConfig();
            configuration.GetSection(AppConfigurationConstants.XmlFileConfig).Bind(XmlFileConfig);
            builder.Services.Configure<XmlFileConfig>((setting) => configuration.GetSection(AppConfigurationConstants.XmlFileConfig).Bind(setting));
            builder.Services.RegisterServices();
            builder.Services.RegisterRepositories();

            ReferenceDataService.InitializeReferenceData(XmlFileConfig.ReferenceDataFilePath);
        }
    }
}
