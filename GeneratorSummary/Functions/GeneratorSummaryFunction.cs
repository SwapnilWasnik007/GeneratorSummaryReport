using GeneratorSummary.Application.Constants;
using GeneratorSummary.Application.Services.GeneratorSummaryService;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GeneratorSummary.Functions
{
    public class GeneratorSummaryFunction
    {
        private readonly IGeneratorSummaryService generatorSummaryService;

        public GeneratorSummaryFunction(
            IGeneratorSummaryService generatorSummaryService)
        {
            this.generatorSummaryService = generatorSummaryService;
        }

        [FunctionName(nameof(GeneratorSummaryFunction))]
        public async Task Run([TimerTrigger(CronExpressionConstants.GeneratorSummaryCron)] TimerInfo myTimer, ILogger log)
        {
            try
            {
                await generatorSummaryService.ExtractGeneratorSummary();
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An error occurred: {Message}", ex.Message);
            }
        }
    }
}
