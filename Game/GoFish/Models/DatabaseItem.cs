using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using Castle.Core.Internal;
using Newtonsoft.Json;

namespace Game.GoFish.Models {

    public class DatabaseItem {
        
        [JsonProperty("game_id")]
        public string GameId;     
        
        
        [JsonProperty("game_start_date")]
        public string GameStartDate;     
        
        
        [JsonProperty("game_session")]
        public string GameSession;    
        
        
        [JsonProperty("game_winner")]
        public string GameWinner;

        public Dictionary<string, AttributeValue> ToDictionary() {
            var items = new Dictionary<string, AttributeValue> {
                ["game_id"] = new AttributeValue {
                    S = GameId
                },
                ["game_start_date"] = new AttributeValue {
                    S = GameStartDate
                }
            };
            if (!GameSession.IsNullOrEmpty()) {
                items["game_session"] = new AttributeValue {
                    S = GameSession
                };
            }
            if (!GameWinner.IsNullOrEmpty()) {
                items["game_winner"] = new AttributeValue {
                    S = GameWinner
                };
            }
            return items;
        }
    }
}