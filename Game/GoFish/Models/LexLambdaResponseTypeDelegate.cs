using System.Collections.Generic;
using System.ComponentModel;

using Newtonsoft.Json;

namespace Game.GoFish.Models {


    public class LexLambdaResponseDialogActionTypeDelegate {
        
        [JsonProperty("type")]
        public string Type;
        
        [JsonProperty("slots", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Slots;
    }
    
    public class LexLambdaResponseTypeDelegate : LexLambdaResponse {
        
        [JsonProperty("dialogAction")]
        public LexLambdaResponseDialogActionTypeDelegate DialogAction;

    }
    
}