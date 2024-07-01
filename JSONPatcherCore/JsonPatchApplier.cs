using JSONPatcherCore.Operations.Base;
using Newtonsoft.Json.Linq;

namespace JSONPatcherCore;

public class JsonPatchApplier
{
    private JObject PatchedObject { get; set; }
    public List<IPatchOperation> Operations { get; set; }

    public JsonPatchApplier(JObject patchedObject, List<IPatchOperation> operations)
    {
        PatchedObject = patchedObject;
        Operations = operations;
    }

    public JObject Apply()
    {
        JObject tempObject = (JObject)PatchedObject.DeepClone(); // Ensures immutability of the original object
        foreach (IPatchOperation operation in Operations.OrderByDescending(x => x.Priority))
        {
            operation.Apply(ref tempObject);
        }

        return tempObject;
    }
}