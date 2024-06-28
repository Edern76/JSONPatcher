using JSONPatcherCore.Operations;
using JSONPatcherCore.Operations.Base;
using JSONPatchParser;

namespace JSONPatcherTest.JSONPatchParser.Parsing.Single;

public class RemoveParsingTest
{
    public const string TestJson1 = @"{
    ""type"": ""RemoveOperation"",
    ""path"": ""/data/name"",
}";

    public const string TestJson2 = @"{
    ""type"": ""RemoveOperation"",
    ""path"": ""/data/age"",
    ""priority"": 10,
}";

    [Fact]
    public void Test_ParseSingle_RemoveOperation_WithoutPriority()
    {
        List<IPatchOperation> result = JsonPatchParser.ParseUnsafe(TestJson1);
        Assert.Single(result);
        IPatchOperation op = result[0];
        Assert.IsType<RemoveOperation>(op);
        RemoveOperation removeOp = (RemoveOperation)op;
        Assert.Equal("/data/name", removeOp.TargetPath);
        Assert.Equal(1, removeOp.Priority);
    }

    [Fact]
    public void Test_ParseSingle_RemoveOperation_WithPriority()
    {
        List<IPatchOperation> result = JsonPatchParser.ParseUnsafe(TestJson2);
        Assert.Single(result);
        IPatchOperation op = result[0];
        Assert.IsType<RemoveOperation>(op);
        RemoveOperation removeOp = (RemoveOperation)op;
        Assert.Equal("/data/age", removeOp.TargetPath);
        Assert.Equal(10, removeOp.Priority);
    }
}