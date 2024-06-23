using JSONPatcherCore.Operations.Base;
using JSONPatchParser.Types;

namespace JSONPatchParser.Processors;

public interface IProcessor
{
    public IPatchOperation Process(JsonPatchSchema schema);
}