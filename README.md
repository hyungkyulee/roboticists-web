# roboticists-web

roboticists
 - roboticists-web
   - 
 - roboticists-apis
   - RobiticistsApis.sln
   - src
     - RoboticistsApis.Apis
       - Controllers


 * RoboticistsApis.sln : update project path properly after restructuring the folders
 ```c#
 Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "RoboticistsApis.Apis", "src\RoboticistsApis.Apis\RoboticistsApis.Apis.csproj", "{6982ECC3-0F36-47CB-AEC3-234C46CFAF3E}"
 ```


## serverless (c# on AWS)
reference : https://www.serverless.com/framework/docs/providers/aws/examples/hello-world/csharp/#hello-world-c-example 

### install serverless and nuget packages
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

Install Nuget Packages
- AWSSDK.Core
- Amazon.Lambda.Core
- Amazon.Lambda.Serialization.SystemTextJson
- 

### build the folder structure
```bash

```


