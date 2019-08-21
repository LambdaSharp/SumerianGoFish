  using System.Collections.Generic;
    using Newtonsoft.Json;

    namespace Game.GoFish.Models {
    public class LexLambdaSlotResolution {

    [JsonProperty("value")]
    public string Value;
    }

    public class LexLambdaSlotDetail {

    [JsonProperty("resolutions")]
    public List<LexLambdaSlotResolution> Resolutions;

    [JsonProperty("originalValue")]
    public string OriginalValue;

    }

    public class LexLambdaCurrentIntent {

    [JsonProperty("name")]
    public string Name;

    [JsonProperty("slots")]
    public Dictionary<string, string> Slots;

    [JsonProperty("slotDetails")]
    public Dictionary<string, LexLambdaSlotDetail> SlotDetails;

    [JsonProperty("confirmationStatus")]
    public string ConfirmationStatus;
    }

    public class LexLambdaBot {

    [JsonProperty("name")]
    public string Name;

    [JsonProperty("alias")]
    public string Alias;

    [JsonProperty("version")]
    public string Version;
    }

    public class LexLambdaInputEvent {

    [JsonProperty("currentIntent")]
    public LexLambdaCurrentIntent CurrentIntent;

    [JsonProperty("bot")]
    public LexLambdaBot Bot;

    [JsonProperty("userId")]
    public string UserId;

    [JsonProperty("inputTranscript")]
    public string InputTranscript;

    [JsonProperty("invocationSource")]
    public string InvocationSource;

    [JsonProperty("outputDialogMode")]
    public string OutputDialogMode;

    [JsonProperty("messageVersion")]
    public string MessageVersion;

    [JsonProperty("sessionAttributes")]
    public Dictionary<string, string> SessionAttributes;

    [JsonProperty("requestAttributes")]
    public Dictionary<string, string> RequestAttributes;
    }
  }