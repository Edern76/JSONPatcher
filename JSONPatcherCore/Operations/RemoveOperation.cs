using JSONPatcherCore.Exceptions;
using JSONPatcherCore.Operations.Base;
using Newtonsoft.Json.Linq;

namespace JSONPatcherCore.Operations;

public class RemoveOperation : BaseOperation
{
    public RemoveOperation(string targetPath, int? priority = 1) : base(targetPath, priority)
    {
    }

    public override void Apply(ref JObject patchedObject)
    {
        JToken? token = patchedObject.SelectToken(TargetPath);
        switch (token)
        {
            case null:
                throw new JsonPatchTargetNotFoundException($"Target path {TargetPath} not found", patchedObject);
            case JProperty property:
                property.Remove();
                break;
            case JValue value:
            {
                JProperty? parent = value.Parent as JProperty;
                if (parent is null)
                {
                    throw new JsonPatchException($"Target path {TargetPath} is not a property", patchedObject);
                }
                parent.Remove();
                break;
            }
            default:
                token.Remove();
                break;
        }
    }
}