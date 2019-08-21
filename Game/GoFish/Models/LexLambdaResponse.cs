using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Game.GoFish.Models {

    public class LexLambdaButtons {
        
        [JsonProperty("text")]
        public string Text;
        
        [JsonProperty("value")]
        public string Value;
    }

    public class LexLambdaGenericAttachment {

        [JsonProperty("title")]
        public string Title;
        
        [JsonProperty("subTitle")]
        public string SubTitle;
        
        [JsonProperty("imageUrl")]
        public string ImageUrl;
        
        [JsonProperty("attachmentLinkUrl")]
        public string AttachmentLinkUrl;
        
        [JsonProperty("buttons")]
        public List<LexLambdaButtons> Buttons;

    }

    public class LexLambdaResponseCard {

        [JsonProperty("version")]
        public string Version;

        [DefaultValue("application/vnd.amazonaws.card.generic")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string ContentType;

        [JsonProperty("genericAttachments")]
        public List<LexLambdaGenericAttachment> GenericAttachments;
    }

    public class LexLambdaResponseMessage {
        
        [JsonProperty("contentType")]
        public string ContentType;
        
        [JsonProperty("content")]
        public string Content;
    }

    public class LexLambdaResponse {

        [JsonProperty("sessionAttributes")]
        public Dictionary<string, string> SessionAttributes;
        
    }

}