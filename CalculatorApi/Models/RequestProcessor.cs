using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;

namespace CalculatorApi.Models
{
    // Currently supported request types
    // Public visibility for controller to return correct response type
    public enum RequestType
    {
        XML,
        JSON
    }


    // Utility class to process (Parse) maths requests to JSON objects
    public static class ProcessRequest
    {
        // Serializes XML request into JSON format and parses into a JSON object
        private static JObject ProcessXML(string request)
        {
            // Serialize XML request into JSON format
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(request);
            request = JsonConvert.SerializeXmlNode(xmlDocument);

            // Create JObject from the JSON request
            JObject mathObject = JObject.Parse(request);
            if (mathObject["?xml"] != null)
                mathObject.Remove("?xml");

            return mathObject;
        }


        // Parses JSON request into JSON object
        private static JObject ProcessJSON(string request)
        {
            return JObject.Parse(request);
        }


        // Based on request format, its request type is determined
        public static RequestType GetRequestType(string request)
        {
            if (request.TrimStart().StartsWith("<"))
            {
                return RequestType.XML;
            }
            return RequestType.JSON;
        }


        // Based on the request type, the given request is parsed into a JSON object
        public static JObject GetMathsObject(string request, RequestType requestType)
        {
            if (requestType is RequestType.XML)
            {
                return ProcessXML(request);
            }
            return ProcessJSON(request);
        }
    }
}