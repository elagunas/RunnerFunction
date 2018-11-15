
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using System.Threading.Tasks;
using Alexa.NET.Response;
using Alexa.NET;

namespace RunnerFunction
{
    public static class Function1
    {
        [FunctionName("Alexa")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            string json = await req.ReadAsStringAsync();
            var skillRequest = JsonConvert.DeserializeObject<SkillRequest>(json);

            var requestType = skillRequest.GetRequestType();

            SkillResponse response = null;

            if (requestType == typeof(LaunchRequest))
            {
                response = ResponseBuilder.Tell("Bienvenido al calendario de runners");
                response.Response.ShouldEndSession = false;
            }

            return new OkObjectResult(response);
        }
    }
}
