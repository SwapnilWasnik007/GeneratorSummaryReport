using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GeneratorSummary.Common.Utils.XmlReader
{
    public static class XmlReader
    {
        public static async Task<XmlDocument> ReadXmlAsync(string filePath)
        {
            await Task.Delay(1);
            return new XmlDocument();
        }
    }
}
