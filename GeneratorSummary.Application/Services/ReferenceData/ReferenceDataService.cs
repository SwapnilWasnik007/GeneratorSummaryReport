using GeneratorSummary.Application.Constants;
using GeneratorSummary.Common.Models;
using GeneratorSummary.Data.Repositories.GeneratorSummary;
using System.Xml;

namespace GeneratorSummary.Application.Services.ReferenceData
{
    public static class ReferenceDataService
    {
        private static ReferenceDataView referenceData = new();

        public static void InitializeReferenceData(string filePath)
        {
            XmlDocument referenceDoc = new();
            referenceDoc.Load(filePath);

            ReferenceDataView result = new ReferenceDataView();

            XmlNodeList? valueNodes = referenceDoc.SelectNodes(XmlConstants.ValueFactorNode);
            if (valueNodes != null)
            {
                foreach (XmlNode node in valueNodes)
                {
                    result.ValueFactors[node.Name] = double.Parse(node.InnerText);
                }
            }

            XmlNodeList? emissionNodes = referenceDoc.SelectNodes(XmlConstants.EmissionsFactorNode);
            if (emissionNodes != null)
            {
                foreach (XmlNode node in emissionNodes)
                {
                    result.EmissionFactors[node.Name] = double.Parse(node.InnerText);
                }
            }

            referenceData = result;
        }

        public static ReferenceDataView GetReferenceData()
        {
            return referenceData;
        }
    }
}
