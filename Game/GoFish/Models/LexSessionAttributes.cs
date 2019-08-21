using System.Collections.Generic;
using Newtonsoft.Json;

namespace Game.GoFish.Models {

    public class LexSessionAttributes {
        
        [JsonProperty("game_id")]
        public string GameId;     
        
        
        [JsonProperty("game_start_date")]
        public string GameStartDate;


        public Dictionary<string, string> ToDictionary() {
            return new Dictionary<string, string> {
                ["game_id"] = GameId,
                ["game_start_date"] = GameStartDate
            };
        }

        public static LexSessionAttributes GoFishLexSession(string gameId, string gameStartDate) {
            return new LexSessionAttributes {
                GameId = gameId, 
                GameStartDate = gameStartDate
            };
        }

    }
   
}