using System;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoboticistsApis.Models.Contract;
using RoboticistsApis.Models.Options;
using RoboticistsApis.Services.Repositories;

namespace RoboticistsApis.Apis
{
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
}