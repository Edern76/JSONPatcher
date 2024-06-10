using Newtonsoft.Json.Linq;

namespace JSONPatcherCore.Operations.Base;

public abstract class BaseOperationWithValue : BaseOperation
{
    protected JToken Value { get; private set; }

    protected BaseOperationWithValue(string targetPath, JToken value, int priority = 1) : base(targetPath, priority)
    {
        Value = value;
    }
}