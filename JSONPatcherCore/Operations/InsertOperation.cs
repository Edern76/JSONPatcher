using JSONPatcherCore.Exceptions;
using JSONPatcherCore.Operations.Base;
using Newtonsoft.Json.Linq;

namespace JSONPatcherCore.Operations;

public class InsertOperation : BaseOperationWithValue
{
    public InsertOperation(string targetPath, JToken value, int priority = 1) : base(targetPath, priority)
    {
    }

    public override void Apply(JObject patchedObject)
    {
        JToken? parent = patchedObject.SelectToken(TargetPath);
        switch (parent)
        {
            case null:
                throw new JsonPatchTargetNotFoundException($"Target path {TargetPath} not found", patchedObject);
            case JArray array:
                array.Add(Value);
                break;
            default:
                throw new JsonPatchException($"Target path {TargetPath} is not an array", patchedObject);
        }
    }
}