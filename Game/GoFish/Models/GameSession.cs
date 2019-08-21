using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Game.GoFish.Models {

    public class GameSession {
        
        [JsonProperty("total_players")]
        public int TotalPlayers;     
        
        [JsonProperty("stub_cards")]
        public List<Card> StubCards;     
        
        [JsonProperty("game_start_date")]
        public DateTime GameStartDate;
        
        [JsonProperty("uri_to_s3_bucket")]
        public string UriToS3Bucket;

        [JsonProperty("next_turn")]
        public Player NextTurn;
        
        [JsonProperty("game_id")]
        public string GameId;

        [JsonProperty("players")]
        public List<Player> Players;
        
        public Player CurrentPlayer() {
            return NextTurn;
        }
        public Player OpponentPlayer() {
            return Players.FirstOrDefault(x => x.Id != NextTurn.Id);;
        }
    }
    
    public class Player {

        [JsonProperty("player_id")]
        public string Id;

        [JsonProperty("is_player_a_bot")]
        public bool IsABot;

        [JsonProperty("last_intent")]
        public string LastIntent;
        
        [JsonProperty("cards")]
        public List<Card> Cards;
        
        [JsonProperty("matched_cards")]
        public List<Card> MatchedCards;
        
    }
}