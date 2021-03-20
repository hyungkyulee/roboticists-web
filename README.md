# roboticists-web

## a project structure
 - web
   - .git
   - webapp (reactjs frontend app)
   - apis (c#/.net backend apis)
     - RobiticistsApis.sln
     - go.sh (script to build and deploy)
     - src
       - RoboticistsApis.Apis
         - Controllers


 * RoboticistsApis.sln : update project path properly after restructuring the folders
 ```c#
 Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "RoboticistsApis.Apis", "src\RoboticistsApis.Apis\RoboticistsApis.Apis.csproj", "{6982ECC3-0F36-47CB-AEC3-234C46CFAF3E}"
 ```
 * main Project specify the assemblyName, packageId and namespace 
 ```c#
 <PropertyGroup>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <GenerateRuntimeConfigurationFiles>true</GenerateRuntimeConfigurationFiles>
    <AssemblyName>RoboticistsApis.Apis</AssemblyName>
    <PackageId>RoboticistsApis.Apis</PackageId>
    <RootNamespace>RoboticistsApis.Apis</RootNamespace>
  </PropertyGroup>
 ```

## frontend app
```bash
$ cd webapp 
$ npx create-react-app ./
$ npm i
$ npm start
```

## serverless (c# on AWS)
reference : https://www.serverless.com/framework/docs/providers/aws/examples/hello-world/csharp/#hello-world-c-example 

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


## REST apis
### Post
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
* handler path - handler: [assembly name]::[project name].[subfolder name].[file name]::[function name]

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
      var body = JsonConvert.SerializeObject(proxyRequest);

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
