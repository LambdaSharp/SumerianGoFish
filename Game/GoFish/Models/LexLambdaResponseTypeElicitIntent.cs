using System.Collections.Generic;
using System.ComponentModel;

using Newtonsoft.Json;

namespace Game.GoFish.Models {

    public class LexLambdaResponseDialogActionTypeElicitIntent {
        
        [JsonProperty("type")]
        public string Type;
        
        [JsonProperty("message")]
        public LexLambdaResponseMessage Message;
        
        [JsonProperty("responseCard")]
        public LexLambdaResponseCard ResponseCard;
    }
    
    public class LexLambdaResponseTypeElicitIntent : LexLambdaResponse {
        
        [JsonProperty("dialogAction")]
        public LexLambdaResponseDialogActionTypeElicitIntent DialogAction;

    }
    
}