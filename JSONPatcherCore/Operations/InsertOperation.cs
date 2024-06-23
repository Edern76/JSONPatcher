using JSONPatcherCore.Exceptions;
using JSONPatcherCore.Operations.Base;
using Newtonsoft.Json.Linq;

namespace JSONPatcherCore.Operations;

public class InsertOperation : BaseOperationWithValue
{
    public InsertOperation(string targetPath, JToken value, int? priority = 1) : base(targetPath, value, priority)
    {
    }

    public override void Apply(ref JObject patchedObject)
    {
        JToken? parent = patchedObject.SelectToken(TargetPath);
        switch (parent)
        {
            case null:
                throw new JsonPatchTargetNotFoundException($"Target path {TargetPath} not found", patchedObject);
            case JArray array:
                array.Add(Value);
                break;
            case JObject obj:
                if (Value is JProperty property)
                {
                    obj.Add(property);
                }
                else
                {
                    throw new JsonPatchException($"Trying to insert non JProperty object into a JObject", patchedObject);
                }
                break;
            default:
                throw new JsonPatchException($"Target path {TargetPath} is not an insertable type", patchedObject);
        }
    }
}