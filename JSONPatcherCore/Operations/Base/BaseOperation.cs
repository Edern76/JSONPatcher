using Newtonsoft.Json.Linq;

namespace JSONPatcherCore.Operations.Base;

public abstract class BaseOperation : IPatchOperation
{
    public string TargetPath { get; private set; }
    public int Priority { get; private set; }
    
    protected BaseOperation(string targetPath, int? priority=1)
    {
        TargetPath = targetPath;
        Priority = priority ?? 1;
    }

    public abstract void Apply(ref JObject patchedObject);
}