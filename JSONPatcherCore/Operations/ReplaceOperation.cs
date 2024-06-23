using JSONPatcherCore.Exceptions;
using JSONPatcherCore.Operations.Base;
using Newtonsoft.Json.Linq;

namespace JSONPatcherCore.Operations;

public class ReplaceOperation : BaseOperationWithValue
{
    public ReplaceOperation(string targetPath, JToken value, int? priority = 1) : base(targetPath, value, priority)
    {
    }

    public override void Apply(ref JObject patchedObject)
    {
        JToken? parent = patchedObject.SelectToken(TargetPath);
        if (parent is null)
        {
            throw new JsonPatchTargetNotFoundException($"Target path {TargetPath} not found", patchedObject);
        }
        parent.Replace(Value);
    }
}