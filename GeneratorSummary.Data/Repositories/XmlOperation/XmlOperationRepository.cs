using System.Xml;

namespace GeneratorSummary.Data.Repositories.GeneratorSummary
{
    public class XmlOperationRepository : IXmlOperationRepository
    {
        public async Task<XmlDocument> ReadXmlDocument(string filePath)
        {
            return await Task.Run(() =>
            {
                XmlDocument xmlDoc = new();
                xmlDoc.Load(filePath);
                return xmlDoc;
            });
        }
    }
}
