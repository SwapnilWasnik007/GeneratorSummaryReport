using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorSummary.Application.Constants
{
    public static class XmlConstants
    {
        public const string ValueFactorNode = "//Factors/ValueFactor/*";
        public const string EmissionsFactorNode = "//Factors/EmissionsFactor/*";
        public const string GeneratorNode = "//{0}Generator";
        public const string LocationNode = "./Location";
        public const string EmissionsRatingNode = "./EmissionsRating";
        public const string GenerationDayNode = ".//Generation/Day";
        public const string NameNode = "./Name";
        public const string EnergyNode = "./Energy";
        public const string PriceNode = "./Price";
        public const string DateNode = "./Date";
        public const string TotalHeatInputNode = "./TotalHeatInput";
        public const string ActualNetGenerationNode = "./ActualNetGeneration";
        public const string OutputRootNode = "GenerationOutput";
        public const string OuputFileSuffix = "-Result.xml";
        public const string HeaderXsiUrl = "http://www.w3.org/2001/XMLSchema-instance";
        public const string HeaderXsdUrl = "http://www.w3.org/2001/XMLSchema";
    }
}
