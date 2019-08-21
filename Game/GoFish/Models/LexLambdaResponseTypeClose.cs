using System.Collections.Generic;
using System.ComponentModel;

using Newtonsoft.Json;

namespace Game.GoFish.Models {

    public class LexLambdaResponseDialogActionTypeClose {
        
        [JsonProperty("type")]
        public string Type;
        
        [JsonProperty("fulfillmentState")]
        public string FulfillmentState;
        
        [JsonProperty("message")]
        public LexLambdaResponseMessage Message;
        
        [JsonProperty("responseCard")]
        public LexLambdaResponseCard ResponseCard;
    }
    
    public class LexLambdaResponseTypeClose : LexLambdaResponse {
        
        [JsonProperty("dialogAction")]
        public LexLambdaResponseDialogActionTypeClose DialogAction;

    }
    
}