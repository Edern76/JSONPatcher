using JSONPatcherCore.Exceptions;
using JSONPatcherCore.Operations.Base;
using Newtonsoft.Json.Linq;

namespace JSONPatcherCore.Operations;

public class RemoveOperation : BaseOperation
{
    public RemoveOperation(string targetPath, int priority = 1) : base(targetPath, priority)
    {
    }

    public override void Apply(JObject patchedObject)
    {
        JToken? parent = patchedObject.SelectToken(TargetPath);
        if (parent is null)
        {
            throw new JsonPatchTargetNotFoundException($"Target path {TargetPath} not found", patchedObject);
        }
        parent.Remove();
    }
}