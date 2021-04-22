using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RoboticistsApis.Infrastructure;
using RoboticistsApis.Models.Api;
using RoboticistsApis.Models.Contract;
using RoboticistsApis.Models.Domain;
using RoboticistsApis.Models.Wrapper;

[assembly:LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace RoboticistsApis.Apis.Controllers
{
    public class PostsController
    {
        private readonly IPostRepository _postRepository;

        public PostsController()
        {
            var services = Startup.Build();
            _postRepository = services.GetService<IPostRepository>();
        }

        public async Task<APIGatewayProxyResponse> Post(APIGatewayProxyRequest proxyRequest)
        {
            var statusCode = (proxyRequest != null)
                ? HttpStatusCode.OK
                : HttpStatusCode.BadRequest;

            var (request, requestError) = proxyRequest.Body.Deserialize<CreatePostRequest>();
            if (requestError != null)
            {
                statusCode = HttpStatusCode.BadRequest;
            }
            
            var (post, postError) = BlogPost.Create(new PostId(Guid.NewGuid()),
                request.Category,
                request.Title,
                request.Content);
            if (postError != null)
            {
                statusCode = HttpStatusCode.BadRequest;
            }

            var (result, message) = await _postRepository.Save(post);
            
            var response = new APIGatewayProxyResponse()
            {
                StatusCode = (int)result,
                Headers = new Dictionary<string, string>
                {
                    {"Access-Control-Allow-Origin", "*"},
                    {"Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accepted"},
                    {"Content-Type", "application/json"}
                },
                Body = message.ToJson()
            };

            return response;
        }

        public HttpStatusCode List(APIGatewayProxyRequest proxyRequest)
        {
            return HttpStatusCode.OK;
        }
    }
}
