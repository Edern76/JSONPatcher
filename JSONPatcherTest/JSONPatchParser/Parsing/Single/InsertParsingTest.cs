using System.Reflection;
using JSONPatcherCore.Operations;
using JSONPatcherCore.Operations.Base;
using JSONPatchParser;
using JSONPatchParser.Utils;
using Newtonsoft.Json.Linq;

namespace JSONPatcherTest.JSONPatchParser.Parsing.Single;

public class InsertParsingTest
{
    public const string TestJson1 = @"{
        ""type"": ""InsertOperation"",
        ""path"": ""/data/name"",
        ""value"": {""firstName"" : ""test"", ""lastName"" : ""icle""}
    }";

    public const string TestJson2 = @"{
        ""type"": ""InsertOperation"",
        ""path"": ""/data"",
        ""parseAsProperty"": true,
        ""priority"": 10,
        ""value"": {
            ""hobbies"" : [""coding"", ""reading""]
        }
    }";

    [Fact]
    public void Test_ParseSingle_InsertOperation_WithoutPriority()
    {
        IPatchOperation op = JsonPatchParser.ParseSingle(TestJson1);
        Assert.IsType<InsertOperation>(op);
        InsertOperation insertOp = (InsertOperation)op;
        Assert.Equal("/data/name", insertOp.TargetPath);
        Assert.Equal(1, insertOp.Priority);
        JToken? value = AccessUtils.AccessNonPublicProperty<JToken>(insertOp, "Value");
        Assert.IsType<JObject>(value);
        JObject obj = (JObject)value!;
        Assert.Equal("test", obj["firstName"].ToString());
        Assert.Equal("icle", obj["lastName"].ToString());
    }

    [Fact]
    public void Test_ParseSingle_InsertOperation_WithPriorityAndParseAsProperty()
    {
        IPatchOperation op = JsonPatchParser.ParseSingle(TestJson2);
        Assert.IsType<InsertOperation>(op);
        InsertOperation insertOp = (InsertOperation)op;
        Assert.Equal("/data", insertOp.TargetPath);
        Assert.Equal(10, insertOp.Priority);
        JToken? value = AccessUtils.AccessNonPublicProperty<JToken>(insertOp, "Value");
        Assert.IsType<JProperty>(value);
        JProperty property = (JProperty)value!;
        Assert.Equal("hobbies", property.Name);
        Assert.IsType<JArray>(property.Value);
        JArray array = (JArray)property.Value;
        Assert.Equal(2, array.Count);
        Assert.Equal("coding", array[0].ToString());
        Assert.Equal("reading", array[1].ToString());
    }
}