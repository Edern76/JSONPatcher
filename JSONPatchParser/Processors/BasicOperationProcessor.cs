using JSONPatcherCore.Operations.Base;
using JSONPatchParser.Types;

namespace JSONPatchParser.Processors;

public class BasicOperationProcessor<T> : IProcessor where T : BaseOperation
{
    public IPatchOperation Process(JsonPatchSchema schema)
    {
        return (T)Activator.CreateInstance(typeof(T), new object[] {schema.Path, schema.Priority });
    }
}