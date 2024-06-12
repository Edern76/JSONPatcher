using JSONPatcherCore.Operations.Base;
using Newtonsoft.Json.Linq;

namespace JSONPatcherCore;

public class JsonPatchApplier
{
    private JObject PatchedObject { get; set; }
    private List<IPatchOperation> Operations { get; set; }
    
    public JsonPatchApplier(JObject patchedObject, List<IPatchOperation> operations)
    {
        PatchedObject = patchedObject;
        Operations = operations;
    }

    public JObject Apply()
    {
        JObject tempObject = (JObject)PatchedObject.DeepClone(); // Ensures immutaibility of the original object
        foreach (IPatchOperation operation in Operations.OrderByDescending(x => x.Priority))
        {
            operation.Apply(ref tempObject);
        }
        return PatchedObject;
    }
}