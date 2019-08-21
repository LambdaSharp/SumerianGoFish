using System.Collections.Generic;
using System.ComponentModel;

using Newtonsoft.Json;

namespace Game.GoFish.Models {


    public class LexLambdaResponseDialogActionTypeConfirmIntent {
        
        [JsonProperty("type")]
        public string Type;
        
        [JsonProperty("slots")]
        public KeyValuePair<string, string> Slots;
        
        [JsonProperty("message")]
        public LexLambdaResponseMessage Message;
        
        [JsonProperty("responseCard")]
        public LexLambdaResponseCard ResponseCard;
    }
    
    public class LexLambdaResponseTypeConfirmIntent : LexLambdaResponse {
        
        [JsonProperty("dialogAction")]
        public LexLambdaResponseDialogActionTypeConfirmIntent DialogAction;

    }
    
}