using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SharedClientServer
{
    public class XmlMessageProtocol : Protocol<XDocument>
    {
        protected override XDocument Decode(byte[] message)
        {
            var xmlData = Encoding.UTF8.GetString(message);
            var xmlReader = XmlReader.Create(new StringReader(xmlData), new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore });
            return XDocument.Load(xmlReader);
        }

        protected override byte[ ] EncodeBody<T>(T message)
        {
            var sb = new StringBuilder();
            var sw = new StringWriter();
            var xs = new XmlSerializer(typeof(T));
            xs.Serialize(sw, message);

            return Encoding.UTF8.GetBytes(sw.ToString());
        }
    }
}
