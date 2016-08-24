using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Conversions;

namespace Api.Controllers
{
    public class ConversionsController : ApiController
    {

        [HttpPost]
        public HttpResponseMessage JosnToXmlRequest(HttpRequestMessage req)
        {
            string jsonString = req.Content.ReadAsStringAsync().Result;
            var xml = XmlJsonConverter.JsonToXml(jsonString);

            return Request.CreateResponse(HttpStatusCode.OK, xml);
        }
    }
}
