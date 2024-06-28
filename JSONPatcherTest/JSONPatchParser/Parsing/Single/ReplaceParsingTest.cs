using JSONPatcherCore.Operations;
using JSONPatcherCore.Operations.Base;
using JSONPatchParser;
using JSONPatchParser.Utils;
using Newtonsoft.Json.Linq;

namespace JSONPatcherTest.JSONPatchParser.Parsing.Single;

public class ReplaceParsingTest
{
    public const string TestJson1 = @"{
        ""type"": ""ReplaceOperation"",
        ""path"": ""/data/name"",
        ""value"": {""firstName"" : ""test"", ""lastName"" : ""icle""}
    }";

    public const string TestJson2 = @"{
        ""type"": ""ReplaceOperation"",
        ""path"": ""/data"",
        ""priority"": 10,
        ""value"": {
            ""hobbies"" : [""coding"", ""reading""]
        }
    }";

    [Fact]
    public void Test_ParseSingle_ReplaceOperation_WithoutPriority()
    {
        List<IPatchOperation> result = JsonPatchParser.ParseUnsafe(TestJson1);
        Assert.Single(result);
        IPatchOperation op = result[0];
        Assert.IsType<ReplaceOperation>(op);
        ReplaceOperation replaceOp = (ReplaceOperation)op;
        Assert.Equal("/data/name", replaceOp.TargetPath);
        Assert.Equal(1, replaceOp.Priority);
        JToken? value = AccessUtils.AccessNonPublicProperty<JToken>(replaceOp, "Value");
        Assert.IsType<JObject>(value);
        JObject obj = (JObject)value!;
        Assert.Equal("test", obj["firstName"].ToString());
        Assert.Equal("icle", obj["lastName"].ToString());
    }

    [Fact]
    public void Test_ParseSingle_ReplaceOperation_WithPriorityAndParseAsProperty()
    {
        List<IPatchOperation> result = JsonPatchParser.ParseUnsafe(TestJson2);
        Assert.Single(result);
        IPatchOperation op = result[0];
        Assert.IsType<ReplaceOperation>(op);
        ReplaceOperation replaceOp = (ReplaceOperation)op;
        Assert.Equal("/data", replaceOp.TargetPath);
        Assert.Equal(10, replaceOp.Priority);
        JToken? value = AccessUtils.AccessNonPublicProperty<JToken>(replaceOp, "Value");
        Assert.IsType<JObject>(value);
        JObject obj = (JObject)value!;
        Assert.NotNull(obj.Properties().First());
        JProperty property = obj.Properties().First();
        Assert.Equal("hobbies", property.Name);
        Assert.IsType<JArray>(property.Value);
        JArray array = (JArray)property.Value;
        Assert.Equal(2, array.Count);
        Assert.Equal("coding", array[0].ToString());
        Assert.Equal("reading", array[1].ToString());
    }
}