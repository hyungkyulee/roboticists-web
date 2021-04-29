# WEBAPP-API Project
---

## a project structure
---
 - web
   - .git
   - webapp (reactjs frontend app)
   - apis (c#/.net backend apis)
     - RobiticistsApis.sln
     - go.sh (script to build and deploy)
     - src
       - RoboticistsApis.Apis
         - Controllers


> RoboticistsApis.sln : update project path properly after restructuring the folders
 ```c#
 Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "RoboticistsApis.Apis", "src\RoboticistsApis.Apis\RoboticistsApis.Apis.csproj", "{6982ECC3-0F36-47CB-AEC3-234C46CFAF3E}"
 ```
 
 > main Project specify the assemblyName, packageId and namespace 
 ```c#
 <PropertyGroup>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <GenerateRuntimeConfigurationFiles>true</GenerateRuntimeConfigurationFiles>
    <AssemblyName>RoboticistsApis.Apis</AssemblyName>
    <PackageId>RoboticistsApis.Apis</PackageId>
    <RootNamespace>RoboticistsApis.Apis</RootNamespace>
  </PropertyGroup>
 ```

---
## backend API on serverless platform (c# on AWS)
reference : https://www.serverless.com/framework/docs/providers/aws/examples/hello-world/csharp/#hello-world-c-example 
---

### install serverless framework create lambda projec with c# template
``` bash
(base) ➜  src $ npm install -g serverless
(base) ➜  src $ cd RoboticcistsApis.Apis
(base) ➜  RoboticistsApis.Apis $ sls create --template aws-csharp          
Serverless: Generating boilerplate...
 _______                             __
|   _   .-----.----.--.--.-----.----|  .-----.-----.-----.
|   |___|  -__|   _|  |  |  -__|   _|  |  -__|__ --|__ --|
|____   |_____|__|  \___/|_____|__| |__|_____|_____|_____|
|   |   |             The Serverless Application Framework
|       |                           serverless.com, v2.30.3
 -------'

Serverless: Successfully generated boilerplate for template: "aws-csharp"
Serverless: NOTE: Please update the "service" property in serverless.yml with your service name
```

### make a script to build and deploy
```bash
#!/bin/bash

#install zip on debian OS, since microsoft/dotnet container doesn't have zip by default
if [ -f /etc/debian_version ]
then
  apt -qq update
  apt -qq -y install zip
fi

pushd src/RoboticistsApis.Apis/
dotnet restore
dotnet tool install -g Amazon.Lambda.Tools --framework netcoreapp3.1
dotnet lambda package --configuration Release --framework netcoreapp3.1 --output-package bin/Release/netcoreapp3.1/package.zip
sls deploy
popd
```

```bash
./go.sh
```

### Nuget Package for c#/.net examples on AWS (optional because build will install the package by cli)
Install Nuget Packages
- AWSSDK.Core
- Amazon.Lambda.Core
- Amazon.Lambda.Serialization.SystemTextJson

AWS Lambda는 C# 함수를 위한 다음 라이브러리를 제공합니다.

Amazon.Lambda.Core – 이 라이브러리는 정적 Lambda 로거, 직렬화 인터페이스 및 콘텍스트 객체를 제공합니다. Context 객체(AWS Lambda 컨텍스트 객체(C#))는 Lambda 함수에 대한 런타임 정보를 제공합니다.
Amazon.Lambda.Serialization.Json – Amazon.Lambda.Core에 직렬화 인터페이스를 구현한 것입니다.
Amazon.Lambda.Logging.AspNetCore – ASP.NET에서의 로깅을 위해 라이브러리를 제공합니다.
몇 가지 AWS 서비스를 위한 이벤트 객체(POCO)로는 다음과 같은 것들이 있습니다.
Amazon.Lambda.APIGatewayEvents
Amazon.Lambda.CognitoEvents
Amazon.Lambda.ConfigEvents
Amazon.Lambda.DynamoDBEvents
Amazon.Lambda.KinesisEvents
Amazon.Lambda.S3Events
Amazon.Lambda.SQSEvents
Amazon.Lambda.SNSEvents


## APIs (RESTful)
---
### POST (basic without auth/repository connection)
---
#### update serverless yml function to create API gateway and lambda
```yml
functions:
  createPost:
    handler: RoboticistsApis.Apis::RoboticistsApis.Apis.Controllers.PostsController::Post
    package:
      artifact: bin/Release/netcoreapp3.1/package.zip
    events:
      - http:
          path: posts
          method: post
          cors: true
```
> handler path - handler: [assembly name]::[project name].[subfolder name].[file name]::[function name]

#### create file and class signature
file structure
```
- src
  - RoboticistsApis.Apis
    - Controllers
      - PostsController.cs
```

Set the namespace of the Controller file
- the namesapce should be a concatenation of [project name].[subfolder name]
- class name and constructor updated with the filename
```c#
[assembly:LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace RoboticistsApis.Apis.Controllers
{
    public class PostsController
    {
        public PostsController()
        {
        }
    }
}
```

#### create async handler function for REST Api
(* ref: https://aws.amazon.com/blogs/compute/developing-net-core-aws-lambda-functions/)
- prerequisite : install NuGet
  ```c#
  using Amazon.Lambda.Core;
  using Amazon.Lambda.TestUtilities;
  using Amazon.Lambda.APIGatewayEvents;
  
  using Newtonsoft.Json;
  ```
  
- handler signature
  ```c#public async Task<APIGatewayProxyResponse> Post(APIGatewayProxyRequest proxyRequest)```
  
- APIGatewayProxyResponse/Request
  ```c#
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
  ```
  
- Build and Test
  - ./go.sh
  - test with postman 
    - endpoint: https://u3fnac9d51.execute-api.eu-west-1.amazonaws.com/dev/posts
    - method: POST
    - no-auth
    - body : raw - json
      {
        "title": "Test Title",
        "content": "Pilot Content ",
        "category": "diary"
      }


#### Json operators
- Json serializer and deserializer Tool
- make it an 'extension method'

```c#
namespace RoboticistsApis.Infrastructure
{
    public static class JsonExtensions
    {
        public static (T entity, Exception error) Deserialize<T>(this string payload)
        {
            if (string.IsNullOrEmpty(payload))
            {
                return (default, new Exception("A payload cannot be empty"));
            }

            var entity = JsonSerializer.Deserialize<T>(payload, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            
            Console.WriteLine(entity.ToJson());

            return (entity, null);
        }

        public static string ToJson(this object entity)
        {
            return JsonSerializer.Serialize(entity, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}
```
> extension mehod:'this' keyword, should be a static class and method
> import System.Text.Json to use JsonSerializer library
> to match c# annotation with the json body, set JsonNamingPolicy.CamelCase
> tupple used to handle the multiple returns at the same time.

#### Models
- Api : arrange a request and a response to REST API
- Domain : handle a repository resource

```c#
public class CreatePostRequest
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string Category { get; set; }
}
```

```c#
public class BlogPost
{
    public PostId BlogId { get; }
    public string Category { get; }
    public string Title { get; }
    public string Content { get; }
    public DateTime DateTimestamp { get; }

    public BlogPost(PostId blogBlogId, 
        string category, 
        string title, 
        string content,
        DateTime dateTimestamp)
    {
        BlogId = blogBlogId;
        Category = category;
        Title = title;
        Content = content;
        DateTimestamp = dateTimestamp;
    }

    public static (BlogPost post, ArgumentException error) Create(PostId blogId, 
        string category, 
        string title, 
        string content)
    {
        var errorMessage = string.Empty;
        if (blogId.Value == Guid.Empty) errorMessage += $"{nameof(blogId)}";
        if (string.IsNullOrEmpty(category)) errorMessage += $"{nameof(category)}";
        if (string.IsNullOrEmpty(title)) errorMessage += $"{nameof(category)}";
        if (string.IsNullOrEmpty(content)) errorMessage += $"{nameof(category)}";

        if (!string.IsNullOrEmpty(errorMessage))
        {
            return (null, new ArgumentException($"Key data: ${errorMessage}, cannot be empty."));
        }

        return (new BlogPost(blogId, category, title, content, DateTime.Now), null);
    }
}
```

---
### POST (connected with repository service)
---
file structure
```
- src
  - RoboticistsApis.Apis
    Startup.cs
    appsettings.json
    - Controllers
      - PostsController.cs
  - RoboticistsApis.Models
    - Api
      - CreatePostRequest.cs
    - Constants
      - DatabaseKey.cs
      - DatabaseTables.cs
    - Contract
      - IPostRepository.cs
    - Domain
      - BlogPost.cs
    - Options
      - AwsOptions.cs
  - RoboticistsApis.Services
    - Repositories
      - PostRepository.cs
```
> appsettings including an aws credential should ingnore in git. 
> Alarm message example from AWS when it opens to the public : 
  ```text
  Hi there,
  We are following-up regarding re-securing your account.
  As previously communicated, we have detected an abnormal pattern in your AWS account that matches unauthorized activity. 
  Your security is important to us and this exposure of your account’s IAM credentials poses a security risk to your AWS account, could lead to excessive charges from unauthorized activity, and violates the AWS Customer Agreement or other agreement with us governing your use of our Services.
  To re-secure your account, please follow the below steps and then respond to this notification within five days. 
  ```

#### Dependency Injection of services
A dependency is an object that another object depends on. Startup class with Services collections that other classes (e.g. repository) depend on.

```c#
public static class Startup
{
    private static IConfiguration _configuration;
    private static readonly IServiceCollection Services = new ServiceCollection();

    public static IServiceProvider Build()
    {
        return ConfigureServices().BuildServiceProvider();
    }

    private static IServiceCollection ConfigureServices()
    {
        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddEnvironmentVariables();
        configBuilder.AddJsonFile("appsettings.json", false, true);
        _configuration = configBuilder.Build();

        var awsOptions = new AwsOptions();
        _configuration.GetSection("aws").Bind(awsOptions);

        Services.AddSingleton<AWSCredentials>(x =>
            new BasicAWSCredentials(awsOptions.AccessKey, awsOptions.SecretKey));
        Services.AddSingleton<IAmazonDynamoDB, AmazonDynamoDBClient>();
        Services.AddSingleton<IPostRepository, PostRepository>();

        return Services;
    }

}
```

Let's look at the updated PostsController implementing the constructor to invoke Startup class's 'Build' method which adds and links related services such as AWS Credential, DynamoDBClient, instance of Repository Interface.
```c#
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

        // var body = JsonConvert.SerializeObject(proxyRequest.Body);

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
}
```

#### AWS Credential configuraiton
- create setting file 'appsettings.json' on the same location as Main service configuration file (e.g. Startup.cs)
```json
{
  "aws": {
    "accessKey": "AKIA*********",
    "secretKey": "X3OYi/5uRAHQR3ur**************"
  }
}
```
- declare the appsettings.json path on project setting (.csproj)
```c#
:
  <ItemGroup>
    <None Update="appsettings.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
  </ItemGroup>
:
```
> DynamoDBClient is requiring a AWS Credential configuration. If it's not binded correctly, error shows as follows :
> The security token included in the request is invalid.: AmazonDynamoDBException
   at Amazon.Runtime.Internal.HttpErrorResponseExceptionHandler.HandleExceptionStream(IRequestContext requestContext, IWebResponseData httpErrorResponse, HttpErrorResponseException exception, Stream responseStream)
   at Amazon.Runtime.Internal.HttpErrorResponseExceptionHandler.HandleExceptionAsync(IExecutionContext executionContext, HttpErrorResponseException exception)


#### Role of repository
Controller <-> Repository <-> Context (e.g. database, memory, etc) 
Repository is in charge of handling a domain object for Controller (Requests/Responses)
```c#
public interface IPostRepository
{
    Task<(HttpStatusCode result, string message)> Save(BlogPost blogPost);
}
```

```c#
public class PostRepository : IPostRepository
{
    private readonly IAmazonDynamoDB _dynamoDb;

    public PostRepository(IAmazonDynamoDB dynamoDb)
    {
        _dynamoDb = dynamoDb;
    }

    public async Task<(HttpStatusCode result, string message)> Save(BlogPost blogPost)
    {
        var key = new Dictionary<string, AttributeValue>
        {
            {
                DatabaseKey.Category, 
                new AttributeValue{S = blogPost.Category}
            },
            {
                DatabaseKey.BlogId, 
                new AttributeValue{S = blogPost.BlogId.ToString()}
            }
        };

        var updatedValues = new Dictionary<string, AttributeValueUpdate>
        {
            {
                DatabaseKey.Title, 
                new AttributeValueUpdate{Action = AttributeAction.PUT, Value = new AttributeValue{S = blogPost.Title}}
            },
            {
                DatabaseKey.Content, 
                new AttributeValueUpdate{Action = AttributeAction.PUT, Value = new AttributeValue{S = blogPost.Content}}
            },
            {
                DatabaseKey.DateTimestamp, 
                new AttributeValueUpdate{Action = AttributeAction.PUT, Value = new AttributeValue{S = blogPost.DateTimestamp.ToString("u")}}
            }
        };

        await _dynamoDb.UpdateItemAsync(DatabaseTables.BlogPosts, key, updatedValues);
        return (HttpStatusCode.Created, "Success - A new post has been created.");
    }
}
```

---
### GET
---

```yml
listCategoryPosts:
   handler: RoboticistsApis.Apis::RoboticistsApis.Apis.Controllers.PostsController::List
   package:
     artifact: bin/Release/netcoreapp3.1/package.zip
   events:
     - http:
         path: categoryposts/{category}
         method: get
         request:
           parameters:
             paths:
               category: true
```

Handler Skeleton
```c#
public async Task<APIGatewayProxyResponse> List(APIGatewayProxyRequest proxyRequest)
{
    Console.WriteLine("Get List..");

    var message = ">> List Hanlder ...";
    var response = new APIGatewayProxyResponse()
    {
        StatusCode = (int) HttpStatusCode.OK,
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
```

---
---
## frontend app

---
### Basic Reactjs Webapp development environment
---
#### Get started
```bash
$ cd webapp 
$ npx create-react-app ./
$ npm i
$ npm start
```

#### Basic packages (React bootstrap)
```bash
npm install react-bootstrap bootstrap
```
[index.js]
```js
import 'bootstrap/dist/css/bootstrap.min.css'
import './index.css'
```
> import the bootstrap.min.css before the index.css

#### Basic packages (Router)
```bash
npm install react-router-dom
```
[app.js]
```js
import {
  BrowserRouter as Router,
  Route,
  Switch,
} from "react-router-dom"

:
function App() {
  return (
    <Router>
      <div>App</div>
    </Router>
  );
}
:
```

---
### UserPool(AWS Congnito) Configuration
---
#### Basic concept of AWS Cognito
AWS Congnito UserPool 
 - a managed user directory
 - an identity provider
 - a set of basic auth features inclusive (login, signup, email verification, password recovery, etc)
 - JWT as a outcome of a login activity
 - extension with Lambda Authorizer

#### Create a UserPool
```yml
resources:
  Resources:
    blogUsersPool:                         -> UserPool to handle authenticated users
      Type: AWS::Cognito::UserPool         
      Properties:
        MfaConfiguration: OFF
        UserPoolName: BlogUsers
        UsernameAttributes:
          - email
        AutoVerifiedAttributes:
          - email
        Policies:
          PasswordPolicy:
            MinimumLength: 6
            RequireLowercase: False
            RequireNumbers: False
            RequireSymbols: False
            RequireUppercase: False
    blogUsersPoolClient:                   -> UserPool to handle un-authenticated users
      Type: AWS::Cognito::UserPoolClient
      Properties:
        ClientName: BlogUsersClient
        GenerateSecret: False
        AllowedOAuthFlows:
          - implicit
        AllowedOAuthFlowsUserPoolClient: true
        AllowedOAuthScopes:
          - phone
          - email
          - openid
          - profile
          - aws.cognito.signin.user.admin
        UserPoolId:
          Ref: blogUsersPool
        CallbackURLs:
          - http://localhost:3000/signedin
          - <host url>/signedin
        ExplicitAuthFlows:
          - ALLOW_CUSTOM_AUTH
          - ALLOW_USER_SRP_AUTH
          - ALLOW_REFRESH_TOKEN_AUTH
        SupportedIdentityProviders:
          - COGNITO
    blogUsersPoolDomain:
      Type: AWS::Cognito::UserPoolDomain
      Properties:
        UserPoolId:
          Ref: blogUsersPool
        Domain: roboticists
        
```

#### url for a hosted UI
This can be checked in AWS Congnito colsole -> sidebar -> App Integration -> App client settings -> Hosted UI
[example]
https://<cognito hosted ui URI>/login?client_id=<client id>&response_type=token&scope=aws.cognito.signin.user.admin+email+openid+phone+profile&redirect_uri=http://localhost:3000/signedin

> After successfully signing in or registering, you’ll be redirected to https://<callback uri>/#id_token=<123*****>&expires_in=3600&token_type=Bearer
> The id_token will be used in a API call for an authrization

#### Token Overview
[Session Auth]
Send a "login info"  -> Save session to DB 
Save Session Id      <- Return Cookie (Session Id)

Send Auth request for API with Cookie (Session Id) -> Check Session Id with the stored session info from DB
Handle API Response                                <- Return Response

> Session can be only handled with Web Browser. Token-based Auth is required for other apps

[Token Auth]
Send a "Login info" -> Create JWT
Save Token          <- Return JWT

Send Auth request for API with JWT in Header -> Validate JWT
Handle API Response                          <- Return Response

JWT location on Client side according to platform :
 - Browser: Local Storage
 - IOS: Keychain
 - Android: SharedPreferences

JWT Overview
(ref: https://bezkoder.com/jwt-json-web-token/)
- JWT includes Header, Payload, Signature
  - Header
    ```json
    {
      "typ": "JWT",
      "alg": "HS256"
    }
    ```
   - Payload
     ```json
     {
       "userId": "abcd12345ghijk",
       "username": "bezkoder",
       "email": "contact@bezkoder.com",
       // standard fields
       "iss": "zKoder, author of bezkoder.com",
       "iat": 1570238918,
       "exp": 1570238992
     }
     ```
     > iss: issuer, iat: issed at, exp: expired at
   - Header
     ```js
     const data = Base64UrlEncode(header) + '.' + Base64UrlEncode(payload);
     const hashedData = Hash(data, secret);
     const signature = Base64UrlEncode(hashedData);
     
     const JWT = encodedHeader + "." + encodedPayload + "." + signature;
     ```

---
### POST (with Authentication)
---





