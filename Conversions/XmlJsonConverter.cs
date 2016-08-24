using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;

namespace Conversions
{
    public static class XmlJsonConverter
    {
        public static string JsonToXml(string jsonString)
        {

            try
            {
                XmlDocument doc = JsonConvert.DeserializeXmlNode(jsonString);
                string xml = doc.OuterXml;
                return xml;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
