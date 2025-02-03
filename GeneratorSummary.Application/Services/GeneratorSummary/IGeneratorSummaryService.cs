using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorSummary.Application.Services.GeneratorSummaryService
{
    public interface IGeneratorSummaryService
    {
        Task ExtractGeneratorSummary();
    }
}
