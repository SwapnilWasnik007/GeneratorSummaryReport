using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GeneratorSummary.Data.Repositories.GeneratorSummary
{
    public interface IXmlOperationRepository
    {
        Task<XmlDocument> ReadXmlDocument(string filePath);
    }
}
