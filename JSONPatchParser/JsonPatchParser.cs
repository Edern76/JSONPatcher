using JSONPatcherCore.Operations;
using JSONPatcherCore.Operations.Base;
using JSONPatchParser.Processors;
using JSONPatchParser.Types;
using Newtonsoft.Json;

namespace JSONPatchParser;

public static class JsonPatchParser
{
    private static readonly Dictionary<string, IProcessor> Processors = new Dictionary<string, IProcessor>();

    static JsonPatchParser()
    {
        RegisterProcessor("RemoveOperation", new BasicOperationProcessor<RemoveOperation>());
        RegisterProcessor("InsertOperation", new ValueOperationProcessor<InsertOperation>());
        RegisterProcessor("ReplaceOperation", new ValueOperationProcessor<ReplaceOperation>());
    }

    public static void RegisterProcessor(string name, IProcessor processor)
    {
        Processors.Add(name, processor);
    }

    public static void UnregisterProcessor(string name)
    {
        Processors.Remove(name);
    }

    public static IPatchOperation ParseSingle(string patch)
    {
        JsonPatchSchema? schema = JsonConvert.DeserializeObject<JsonPatchSchema>(patch);
        if (schema == null)
        {
            throw new InvalidOperationException("Invalid JSON Patch");
        }

        if (Processors.TryGetValue(schema.Type, out IProcessor? processor))
        {
            return processor.Process(schema);
        }

        throw new InvalidOperationException($"Unknown JSON Patch type: {schema.Type}");
    }
}