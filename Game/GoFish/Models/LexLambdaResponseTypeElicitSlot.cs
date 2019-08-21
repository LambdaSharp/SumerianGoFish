using System.Collections.Generic;
using System.ComponentModel;

using Newtonsoft.Json;

namespace Game.GoFish.Models {


    public class LexLambdaResponseDialogActionTypeElicitSlot {
        
        [JsonProperty("type")]
        public string Type;
        
        [JsonProperty("slots")]
        public KeyValuePair<string, string> Slots;
        
        [JsonProperty("message")]
        public LexLambdaResponseMessage Message;
        
        [JsonProperty("intentName")]
        public string IntentName;
        
        [JsonProperty("slotToElicit")]
        public string SlotToElicit;
        
        [JsonProperty("responseCard")]
        public LexLambdaResponseCard ResponseCard;
    }
    
    public class LexLambdaResponseTypeElicitSlot : LexLambdaResponse {
        
        [JsonProperty("dialogAction")]
        public LexLambdaResponseDialogActionTypeConfirmIntent DialogAction;

    }
    
}