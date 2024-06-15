using JSONPatcherCore.Exceptions;
using JSONPatcherCore.Operations;
using Newtonsoft.Json.Linq;

namespace JSONPatcherTest;

public class ReplaceOperationTest
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
    public void Test_ReplaceOperationSuccess()
    {
      JObject json = JObject.Parse(TestJson);
      string target = ".data.addresses[0].street";
      string value = "789 Elm St";
      ReplaceOperation op = new ReplaceOperation(target, value);
      op.Apply(ref json);
      Assert.Equal(value, json["data"]["addresses"][0]["street"].ToString());
    }
    
    [Fact]
    public void Test_ReplaceOperationSuccess2()
    {
      JObject json = JObject.Parse(TestJson);
      string target = ".data.age";
      int value = 31;
      ReplaceOperation op = new ReplaceOperation(target, value);
      op.Apply(ref json);
      Assert.Equal(value, json["data"]["age"]);
    }

    [Fact]
    public void Test_ReplaceOperationFailureWrongPath()
    {
      JObject json = JObject.Parse(TestJson);
      string target = ".data.locations";
      string value = "789 Elm St";
      ReplaceOperation op = new ReplaceOperation(target, value);
      Assert.Throws<JsonPatchTargetNotFoundException>(() => op.Apply(ref json));
    }
}