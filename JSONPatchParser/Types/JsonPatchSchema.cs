using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JSONPatchParser.Types;

[Serializable]
public class JsonPatchSchema
{
    [JsonProperty("type")]
    public string Type { get; set; }
    [JsonProperty("path")]
    public string Path { get; set; }
    [JsonProperty("priority")]
    public int? Priority { get; set; }
    [JsonProperty("parseAsProperty")]
    public bool? ParseAsProperty { get; set; }
    [JsonProperty("value")]
    public JObject? Value { get; set; }

}