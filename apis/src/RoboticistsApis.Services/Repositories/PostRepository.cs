using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using RoboticistsApis.Models.Constants;
using RoboticistsApis.Models.Contract;
using RoboticistsApis.Models.Domain;

namespace RoboticistsApis.Services.Repositories
{
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
                    DatabaseKey.BlogId, 
                    new AttributeValue{S = blogPost.BlogId.ToString()}
                }
            };
            
            var updatedValues = new Dictionary<string, AttributeValueUpdate>
            {
                {
                    DatabaseKey.Category, 
                    new AttributeValueUpdate{Action = AttributeAction.PUT, Value = new AttributeValue{S = blogPost.Category}}
                },
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
}