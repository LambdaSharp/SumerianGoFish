using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Game.GoFish.Models {

    public class Card {
        
        [JsonProperty("card_id")]
        public string Id;
        
        [JsonProperty("card_name")]
        public string Name;
        
        [JsonProperty("card_image", NullValueHandling = NullValueHandling.Ignore)]
        public string CardImage;
        
        // [JsonConverter(typeof(StringEnumConverter))]
        // [JsonProperty("card_game_status")]
        // public CardGameStatus GameStatus;
    }
    public enum CardGameStatus {
        
        [EnumMember(Value = "STUB")] 
        Stub,
        [EnumMember(Value = "PLAYER")] 
        Player,
        [EnumMember(Value = "FIELD")] 
        Field,
        [EnumMember(Value = "UNDEALT")] 
        Undealt,
        [EnumMember(Value = "DISCARD")] // no longer in the game, out until new game
        Discard
    }
}