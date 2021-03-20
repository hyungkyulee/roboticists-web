using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

[assembly:LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace RoboticistsApis.Apis.Controllers
{
    public class PostsController
    {
        public PostsController()
        {
        }

        public async Task<APIGatewayProxyResponse> Post(APIGatewayProxyRequest proxyRequest)
        {
            var statusCode = (proxyRequest != null)
                ? HttpStatusCode.OK
                : HttpStatusCode.InternalServerError;
            
            // await with repository
            var body = JsonConvert.SerializeObject(proxyRequest.Body);
            
            var response = new APIGatewayProxyResponse()
            {
                StatusCode = (int)statusCode,
                Body = body,
                Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "application/json"},
                    {"Access-Control-Allow-Origin", "*"}
                }
            };

            return response;
        }
    }
}
