using JSONPatcherCore.Exceptions;
using JSONPatcherCore.Operations;
using Newtonsoft.Json.Linq;

namespace JSONPatcherTest;

public class RemoveOperationTest
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
    public void Test_RemoveOperationSuccess()
    {
      JObject json = JObject.Parse(TestJson);
      string target = ".data.addresses[0]";
      RemoveOperation op = new RemoveOperation(target);
      op.Apply(ref json);
      Assert.Single(json["data"]!["addresses"]!);
      Assert.Equal("456 Main St", json["data"]!["addresses"]![0]["street"].ToString());
    }

    [Fact]
    public void Test_RemoveOperationSuccess2()
    {
      JObject json = JObject.Parse(TestJson);
      string target = ".data.name";
      Assert.True((((JObject)json["data"]!)!).ContainsKey("name"));
      RemoveOperation op = new RemoveOperation(target);
      op.Apply(ref json);
      Assert.False((((JObject)json["data"]!)!).ContainsKey("name"));
    }

    [Fact]
    public void Test_RemoveOperationFailureWrongPath()
    {
      JObject json = JObject.Parse(TestJson);
      string target = ".data.locations";
      RemoveOperation op = new RemoveOperation(target);
      Assert.Throws<JsonPatchTargetNotFoundException>(() => op.Apply(ref json));
    }
}