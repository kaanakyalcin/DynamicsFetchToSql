using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace Engine
{
    public class Validation
    {
        public bool CheckFetchText(string fetchXML)
        {
            if (fetchXML == "")
            {
                throw new Exception("Please enter fetchXML.");
            }

            if (fetchXML.TrimStart().StartsWith("<") == false)
            {
                throw new Exception("Please enter valid fetchXML.");
            }

            return true;
        }

        public XmlDocument CreateFetchXml(string fetxhText)
        {
            XmlDocument xmldoc = new XmlDocument();
            string path = Environment.CurrentDirectory;
            // Validating fetchXml against schema
            XmlSchema schema = null;
            using (XmlReader reader = XmlReader.Create("https://crmfetch.blob.core.windows.net/files/fetch.xsd"))
            {
                schema = XmlSchema.Read(reader, null);
            }

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.Add(schema);

            byte[] byteArray = Encoding.ASCII.GetBytes(fetxhText);
            MemoryStream stream = new MemoryStream(byteArray);

            using (XmlReader reader = XmlReader.Create(stream, settings))
            {
                xmldoc.Load(reader);
            }

            return xmldoc;

        }
    }
}
