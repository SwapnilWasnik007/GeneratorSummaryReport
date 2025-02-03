using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorSummary.Common.Configurations
{
    public class XmlFileConfig
    {
        public string InputXmlFolderPath { get; set; } = null!;
        public string OutputXmlFolderPath { get; set; } = null!;
        public string ReceivedXmlFolderPath { get; set; } = null!;
        public string ReferenceDataFilePath { get; set; } = null!;
    }
}
