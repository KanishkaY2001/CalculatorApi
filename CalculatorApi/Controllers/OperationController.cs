using CalculatorApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace CalculatorApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OperationController : ControllerBase
    {
        // Creates varies API responses based on request type and un/successful operation calculation
        private ContentResult ResponseHandler(RequestType requestType, bool success, string value)
        {
            // If successful, returns calculated value. Otherwise, returns error message
            string property = success ? "Result" : "Error";
            switch (requestType)
            {
                case RequestType.XML:
                    return Content(new XElement(property, value).ToString(), "application/xml");

                case RequestType.JSON:
                    return Content(JsonConvert.SerializeObject(new JObject { [property] = value }), "application/json");

                default:
                    return Content("Internal server issue.");
            }
        }


        // Handles API POST request for calculating operations
        [HttpPost(Name = "PostOperation")]
        public IActionResult Post(string request)
        {
            // Get the type of the request for response handling
            RequestType requestType = ProcessRequest.GetRequestType(request);

            try
            {
                // Create a Maths JObject based on request type
                JObject? mathObject = ProcessRequest.GetMathsObject(request, requestType);
                if (mathObject == null)
                    throw new NullReferenceException("Request could not be parsed, invalid formatting.");

                // Attempt to deserialize the request
                Maths maths = RequestSerializer.Deserialize(mathObject).Maths;

                // Calculate the result and return response (true)
                double mathsResult = maths.Operation.Calculate();
                return ResponseHandler(requestType, true, mathsResult.ToString());
            }
            catch (Exception exception)
            {
                // Return error message (false)
                return ResponseHandler(requestType, false, exception.Message);
            }
        }
    }
}