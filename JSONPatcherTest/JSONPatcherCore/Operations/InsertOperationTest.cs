using JSONPatcherCore.Exceptions;
using JSONPatcherCore.Operations;
using Newtonsoft.Json.Linq;

namespace JSONPatcherTest;

public class InsertOperationTest
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
    public void Test_InsertOperationSuccess()
    {
        JObject json = JObject.Parse(TestJson);
        string target = ".data.addresses";
        JToken value = JObject.Parse(@"{
        ""street"": ""789 Elm St"",
        ""city"": ""Anytown"",
        ""state"": ""CA"",
        ""zip"": 12345
      }");
        InsertOperation op = new InsertOperation(target, value);
        op.Apply(ref json);
        Assert.Equal(3, json["data"]["addresses"].Count());
        Assert.Equal("123 Elm St", json["data"]["addresses"][0]["street"].ToString());
        Assert.Equal("456 Main St", json["data"]["addresses"][1]["street"].ToString());
        Assert.Equal("789 Elm St", json["data"]["addresses"][2]["street"].ToString());
    }

    [Fact]
    public void Test_InsertOperationSuccess2()
    {
        JObject json = JObject.Parse(TestJson);
        string target = ".data";
        InsertOperation op = new InsertOperation(target, new JProperty("job", JToken.Parse(@"""Software Engineer""")));
        op.Apply(ref json);
        Assert.Equal("Software Engineer", json["data"]!["job"].ToString());
        Assert.Equal("John Doe", json["data"]!["name"].ToString());
    }

    [Fact]
    public void Test_InsertOperationFailureWrongPath()
    {
        JObject json = JObject.Parse(TestJson);
        string target = ".data.locations";
        JToken value = JObject.Parse(@"{
        ""street"": ""789 Elm St"",
        ""city"": ""Anytown"",
        ""state"": ""CA"",
        ""zip"": 12345
      }");
        InsertOperation op = new InsertOperation(target, value);
        Assert.Throws<JsonPatchTargetNotFoundException>(() => op.Apply(ref json));
    }

    [Fact]
    public void Test_InsertOperationFailureWrongType()
    {
        JObject json = JObject.Parse(TestJson);
        string target = ".data.name";
        JToken value = JObject.Parse(@"{
        ""street"": ""789 Elm St"",
        ""city"": ""Anytown"",
        ""state"": ""CA"",
        ""zip"": 12345
      }");
        InsertOperation op = new InsertOperation(target, value);
        Assert.Throws<JsonPatchException>(() => op.Apply(ref json));
    }

    [Fact]
    public void Test_InsertOperationFailureNonInsertableType()
    {
        JObject json = JObject.Parse(TestJson);
        string target = ".data";
        InsertOperation op = new InsertOperation(target, "Software Engineer");
        Assert.Throws<JsonPatchException>(() => op.Apply(ref json));
    }
}