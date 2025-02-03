using GeneratorSummary.Application.Constants;
using GeneratorSummary.Application.Enums;
using GeneratorSummary.Application.Services.ReferenceData;
using GeneratorSummary.Common.Configurations;
using GeneratorSummary.Common.Models;
using GeneratorSummary.Common.Models.OutputXmlResponses;
using GeneratorSummary.Data.Repositories.GeneratorSummary;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Reflection.Emit;
using System.Xml;
using System.Xml.Linq;

namespace GeneratorSummary.Application.Services.GeneratorSummaryService
{
    public class GeneratorSummaryService : IGeneratorSummaryService
    {
        private readonly IXmlOperationRepository generatorSummaryRepository;
        private readonly XmlFileConfig xmlFileConfig;

        public GeneratorSummaryService(
            IXmlOperationRepository generatorSummaryRepository,
            IOptions<XmlFileConfig> xmlFileConfig)
        {
            this.generatorSummaryRepository = generatorSummaryRepository;
            this.xmlFileConfig = xmlFileConfig.Value;
        }

        public async Task ExtractGeneratorSummary()
        {
            string? xmlFilePath = GetXmlFilePath(xmlFileConfig.InputXmlFolderPath);
            if (!string.IsNullOrWhiteSpace(xmlFilePath))
                await PerformGeneratorSummaryExtractionAsync(xmlFilePath);
        }

        private static string? GetXmlFilePath(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine("Folder does not exist.");
                return null;
            }

            return Directory.GetFiles(folderPath, "*.xml").FirstOrDefault();
        }

        private async Task PerformGeneratorSummaryExtractionAsync(string xmlFilePath)
        {
            GenerationOutput generationOutput = new();
            Dictionary<string, Day> emissionAsPerDay = [];
            ReferenceDataView referenceData = ReferenceDataService.GetReferenceData();
            XmlDocument doc = await generatorSummaryRepository.ReadXmlDocument(xmlFilePath);

            XmlNode? generationReportNode = doc.DocumentElement;
            if (generationReportNode != null)
            {
                foreach (XmlNode generatorNode in generationReportNode.ChildNodes)
                {
                    string generatorName = generatorNode.Name;
                    XmlNodeList? generators = generatorNode.SelectNodes(string.Format(XmlConstants.GeneratorNode, generatorName));
                    if (generators == null)
                    {
                        return;
                    }

                    foreach (XmlNode generator in generators)
                    {
                        GenerateReportValues(generator, generatorName, generationOutput, referenceData, emissionAsPerDay);
                    }
                }
                var emissions = emissionAsPerDay.Select(x => new Day
                {
                    Name = x.Value.Name,
                    Date = x.Value.Date,
                    Emission = x.Value.Emission
                }).OrderBy(x => x.Date).ToList();
                generationOutput.MaxEmissionGenerators.Day = emissions;
            }

            GenerateOutputXmlFile(generationOutput, Path.GetFileNameWithoutExtension(xmlFilePath), xmlFileConfig.OutputXmlFolderPath);

            MoveFileToReceivedFolder(xmlFilePath, xmlFileConfig.ReceivedXmlFolderPath);
        }

        private void GenerateReportValues(
            XmlNode generator,
            string generatorName,
            GenerationOutput generationOutput,
            ReferenceDataView referenceData,
            Dictionary<string, Day> maxEmissionPerDay)
        {
            XmlNodeList? generationNodes = generator.SelectNodes(XmlConstants.GenerationDayNode);
            if (generationNodes == null)
            {
                return;
            }

            string name = generator.SelectSingleNode(XmlConstants.NameNode)?.InnerText ?? string.Empty;

            // Retrieving Daily Generation Values and Daily Emissions.
            GetDailyGenerationAndEmission(
                generationNodes,
                generator,
                generatorName,
                referenceData,
                maxEmissionPerDay,
                name,
                generationOutput);

            // Retrieving Actual Heat Rate.
            GetActualHeatRate(
                generator,
                name,
                generationOutput);
        }

        private void GetActualHeatRate(
            XmlNode generator,
            string name,
            GenerationOutput generationOutput)
        {
            if (generator.SelectSingleNode(XmlConstants.TotalHeatInputNode) != null)
            {
                double totalHeatInput = Convert.ToDouble(generator.SelectSingleNode(XmlConstants.TotalHeatInputNode)?.InnerText ?? string.Empty);
                double actualNetGeneration = Convert.ToDouble(generator.SelectSingleNode(XmlConstants.ActualNetGenerationNode)?.InnerText ?? string.Empty);
                ActualHeatRate actualHeatRate = new()
                {
                    Name = name,
                    HeatRate = totalHeatInput / actualNetGeneration
                };
                generationOutput.ActualHeatRates.ActualHeatRate.Add(actualHeatRate);
            }
        }

        private void GetDailyGenerationAndEmission(
            XmlNodeList generationNodes,
            XmlNode generator,
            string generatorName,
            ReferenceDataView referenceData,
            Dictionary<string, Day> maxEmissionPerDay,
            string name,
            GenerationOutput generationOutput)
        {
            string locationName = generator.SelectSingleNode(XmlConstants.LocationNode)?.InnerText ?? string.Empty;
            GeneratorType generatorType = GeneratorType.FromName($"{locationName} {generatorName}".Trim());
            double valueFactor = referenceData.ValueFactors[generatorType.ValueFactor.Name];

            double emissionFactor = 0.0;
            XmlNode? emissionNode = generator.SelectSingleNode(XmlConstants.EmissionsRatingNode);
            bool isEmittableResource = emissionNode != null;
            double emissionRating = Convert.ToDouble(emissionNode?.InnerText);
            if (isEmittableResource)
            {
                emissionFactor = referenceData.EmissionFactors[generatorType.EmissionFactor.Name];
            }
            double totalGeneration = 0.0;
            foreach (XmlNode day in generationNodes)
            {
                double totalEmission = 0.0;
                string energy = day.SelectSingleNode(XmlConstants.EnergyNode)?.InnerText ?? "0";
                string price = day.SelectSingleNode(XmlConstants.PriceNode)?.InnerText ?? "0";
                string date = day.SelectSingleNode(XmlConstants.DateNode)?.InnerText ?? string.Empty;

                if (isEmittableResource)
                {
                    totalEmission = Math.Round(Convert.ToDouble(energy) * emissionRating * emissionFactor, 9);
                    GetMaxEmissionPerDay(maxEmissionPerDay, date, name, totalEmission);
                }

                totalGeneration += Math.Round(Convert.ToDouble(energy) * Convert.ToDouble(price) * valueFactor, 9);
            }

            Generator generatorItem = new()
            {
                Name = name,
                Total = totalGeneration.ToString("F9"),
            };
            generationOutput.Totals.Generator.Add(generatorItem);
        }

        private void GetMaxEmissionPerDay(
            Dictionary<string, Day> maxEmissionPerDay,
            string date,
            string name,
            double totalEmission)
        {
            if (maxEmissionPerDay.ContainsKey(date))
            {
                Day existingDay = maxEmissionPerDay[date];
                if (existingDay.Name == name)
                {
                    existingDay.Emission += totalEmission;
                }
                else
                {
                    if (totalEmission > Convert.ToDouble(existingDay.Emission))
                    {
                        maxEmissionPerDay[date] = new Day
                        {
                            Name = name,
                            Date = date,
                            Emission = totalEmission.ToString("F9")
                        };
                    }
                }
            }
            else
            {
                maxEmissionPerDay.Add(
                    date,
                    new()
                    {
                        Name = name,
                        Date = date,
                        Emission = totalEmission.ToString("F9")
                    });
            }
        }

        private void MoveFileToReceivedFolder(
            string sourceFile,
            string destinationFolder)
        {
            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            File.Move(sourceFile, Path.Combine(destinationFolder, Path.GetFileName(sourceFile)));
        }

        private void GenerateOutputXmlFile(
            GenerationOutput generationOutput,
            string fileName,
            string outputFolder)
        {
            string outputFileName = Path.Combine(outputFolder, $"{fileName}{XmlConstants.OuputFileSuffix}");
            string outputJsonString = JsonConvert.SerializeObject(generationOutput);

            XmlDocument? xmlDocument = JsonConvert.DeserializeXmlNode(outputJsonString, XmlConstants.OutputRootNode);
            if (xmlDocument != null)
            {
                XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDocument.InsertBefore(xmlDeclaration, xmlDocument.DocumentElement);

                XmlElement? root = xmlDocument.DocumentElement;
                if (root != null)
                {
                    root.SetAttribute("xmlns:xsi", XmlConstants.HeaderXsiUrl);
                    root.SetAttribute("xmlns:xsd", XmlConstants.HeaderXsdUrl);
                }
            }
            xmlDocument?.Save(outputFileName);
        }
    }
}
