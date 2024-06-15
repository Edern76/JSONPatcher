using JSONPatcherCore;
using JSONPatcherCore.Operations;
using JSONPatcherCore.Operations.Base;
using Newtonsoft.Json.Linq;

namespace JSONPatcherTest.JSONPatcherCore.Applier;

public class JsonApplierTest
{
    public const string TestJson = @"{
  ""data"": {
    ""name"": ""John Doe"",
    ""age"": 30,
    ""addresses"": [
      {
        ""street"": ""123 Elm St"",
        ""city"": ""Anytown"",
        ""state"": ""CA"",
        ""zip"": 12345
      },
      {
        ""street"": ""456 Main St"",
        ""city"": ""Anytown"",
        ""state"": ""CA"",
        ""zip"": 12345
      }
    ]
  }
}";

    [Fact]
    public void Test_JsonApplierSuccess()
    {
        JObject json = JObject.Parse(TestJson);
        IPatchOperation op = new InsertOperation(".data.addresses", JToken.Parse(@"{""street"":""789 Elm street""}"));
        IPatchOperation op2 = new ReplaceOperation(".data.name", JToken.Parse(@"""Jane Doe"""));
        IPatchOperation op3 = new RemoveOperation(".data.age");
        JsonPatchApplier applier = new JsonPatchApplier(json, new List<IPatchOperation> { op, op2, op3 });
        JObject result = applier.Apply();

        // Test immutability
        Assert.Equal(2, json["data"]!["addresses"]!.Count());
        Assert.Equal("123 Elm St", json["data"]!["addresses"]![0]["street"].ToString());
        Assert.Equal("456 Main St", json["data"]!["addresses"]![1]["street"].ToString());
        Assert.Equal("John Doe", json["data"]!["name"].ToString());
        Assert.Equal(30, json["data"]!["age"]?.ToObject<int>());

        // Test result
        Assert.Equal(3, result["data"]!["addresses"]!.Count());
        Assert.Equal("123 Elm St", result["data"]!["addresses"]![0]?["street"]?.ToString());
        Assert.Equal("456 Main St", result["data"]!["addresses"]![1]?["street"]?.ToString());
        Assert.Equal("789 Elm street", result["data"]!["addresses"]![2]?["street"]?.ToString());
        Assert.Equal("Jane Doe", result["data"]!["name"]!.ToString());
        Assert.Null(result["data"]!["age"]);
    }

    // Hack to repeat the test 5 times
    // This ensures that the test does not succeed just because we get lucky on a random ordering of operations
#pragma warning disable xUnit1025
#pragma warning disable CA1825
    [Theory, InlineData(), InlineData(), InlineData(), InlineData(), InlineData()]
#pragma warning restore CA1825
#pragma warning restore xUnit1025
#pragma warning disable xUnit1006
    public void Test_JsonApplierPriority()
#pragma warning restore xUnit1006
    {
        JObject json = JObject.Parse(TestJson);
        IPatchOperation op0 = new ReplaceOperation(".data.hobbies[0].name", JToken.Parse(@"""Video Games"""), 1);
        IPatchOperation op1 = new ReplaceOperation(".data.hobbies[1].name", JToken.Parse(@"""Programming"""), 1);
        IPatchOperation op2 = new InsertOperation(".data.hobbies", JToken.Parse(@"{""name"": ""Coding""}"), 10);
        JToken hobbiesToken = JToken.Parse(@"[{""name"": ""Gaming""}]");
        IPatchOperation op3 = new InsertOperation(".data", new JProperty("hobbies", hobbiesToken), 100);

        JsonPatchApplier applier = new JsonPatchApplier(json, new List<IPatchOperation> { op0, op1, op2, op3 });
        JObject result = applier.Apply();

        Assert.Equal(2, result["data"]!["hobbies"]?.Count());
        Assert.Equal("Video Games", result["data"]!["hobbies"]?[0]?["name"]?.ToString());
        Assert.Equal("Programming", result["data"]!["hobbies"]?[1]?["name"]?.ToString());
        Assert.Equal("John Doe", result["data"]!["name"]?.ToString());
        Assert.Equal(30, result["data"]!["age"]?.ToObject<int>());
    }
}