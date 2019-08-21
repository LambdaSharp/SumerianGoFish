using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace Game.GoFish.Library {
    public interface IDependencyProvider {
        Task DynamoDbPutItemAsync(Dictionary<string, AttributeValue> items);
        Task<GetItemResponse> DynamoDbGetItemAsync(Dictionary<string, AttributeValue> items);
        Task DynamoDbDeleteItemAsync(Dictionary<string, AttributeValue> items);
    }

    public class DependencyProvider : IDependencyProvider {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private readonly string _tableName;

        public DependencyProvider(IAmazonDynamoDB dynamoDbClient, string tableName) {
            _dynamoDbClient = dynamoDbClient;
            _tableName = tableName;
        }
        
        Task IDependencyProvider.DynamoDbPutItemAsync(Dictionary<string, AttributeValue> items) => _dynamoDbClient.PutItemAsync(_tableName, items);
        Task<GetItemResponse> IDependencyProvider.DynamoDbGetItemAsync(Dictionary<string, AttributeValue> items) => _dynamoDbClient.GetItemAsync(_tableName, items, true);
        Task IDependencyProvider.DynamoDbDeleteItemAsync(Dictionary<string, AttributeValue> items) => _dynamoDbClient.DeleteItemAsync(_tableName, items);
    }
}