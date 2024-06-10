using Newtonsoft.Json.Linq;

namespace JSONPatcherCore.Operations.Base;

public interface IPatchOperation
{
    public string TargetPath { get;}
    public int Priority { get; }
    
    public void Apply(JObject patchedObject);
}