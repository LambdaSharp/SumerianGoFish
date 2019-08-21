using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.Lambda.Core;
using Game.GoFish.Library;
using Game.GoFish.Models;
using LambdaSharp;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Game.GoFish {

    public class Function : ALambdaFunction<LexLambdaInputEvent, LexLambdaResponse> {
        private IDependencyProvider _dependencyProvider;
        private ProcessIntent _processIntent;

        //--- Methods ---
        public override async Task InitializeAsync(LambdaConfig config) {
            var tableName = AwsConverters.ConvertDynamoDBArnToName(config.ReadText("GoFishSessions"));
            var accountRegion = config.ReadText("AccountRegion");
            var bucketName = AwsConverters.ConvertBucketArnToName(config.ReadText("GoFishSumerianBucket"));
            var uriToCardImage = $"http://{bucketName}.s3-website-{accountRegion}.amazonaws.com";
            _dependencyProvider = new DependencyProvider(new AmazonDynamoDBClient(), tableName);
            _processIntent = new ProcessIntent(_dependencyProvider, Logger, uriToCardImage);
        }

        public override async Task<LexLambdaResponse> ProcessMessageAsync(LexLambdaInputEvent lexInputEvent) {
            
            // print event
            LogInfo($"lexInputEvent {JsonConvert.SerializeObject(lexInputEvent)}");
            var response = await _processIntent.Run(lexInputEvent);
            return response;
        }


    }
}
