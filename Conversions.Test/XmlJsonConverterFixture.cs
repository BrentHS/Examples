using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Conversions;
namespace Conversions.Test
{
    [TestFixture]
    public class XmlJsonConverterFixture
    {

        [Test]
        public void JsonToXml()
        {
            var jsonString = "{\"request\":{\"?xml\":{\"@version\":\"1.0\",\"@encoding\":\"UTF-8\"},\"soapenv:Envelope\":{\"@xmlns:soapenv\":\"http://schemas.xmlsoap.org/soap/envelope/\",\"soapenv:Body\":{\"rpc\":{\"@message-id\":1,\"@nodename\":\"NTWK-Sayre 2\",\"@username\":\"udpuser\",\"@sessionid\":\"36\",\"edit-config\":{\"target\":{\"running\":null},\"config\":{\"top\":{\"object\":{\"@operation\":\"create\",\"@get-config\":\"true\",\"type\":\"EthSvc\",\"id\":{\"ont\":8829486,\"ontslot\":3,\"ontethany\":1,\"ethsvc\":1},\"admin\":\"enabled\",\"tag-action\":{\"type\":\"SvcTagAction\",\"id\":{\"svctagaction\":133}},\"bw-prof\":{\"type\":\"BwProf\",\"id\":{\"bwprof\":\"68\"}},\"sec\":{\"type\":\"EthSecProf\",\"id\":{\"ethsecprof\":\"2\"}}}}}}}}}}}";

            var goo = XmlJsonConverter.JsonToXml(jsonString);
        }
    }
}
