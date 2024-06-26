using JSONPatcherCore.Operations.Base;
using JSONPatchParser.Types;
using Newtonsoft.Json.Linq;

namespace JSONPatchParser.Processors;

public class ValueOperationProcessor<T> : IProcessor where T : BaseOperationWithValue
{
    public IPatchOperation Process(JsonPatchSchema schema)
    {
        bool parseAsProperty = schema.ParseAsProperty ?? false;
        JToken value = parseAsProperty ? ParseAsProperty(schema.Value) : schema.Value;
        return (T)Activator.CreateInstance(typeof(T), new object[] { schema.Path, value, schema.Priority });
    }

    private static JProperty ParseAsProperty(JObject obj)
    {
        if (obj is null)
        {
            throw new InvalidOperationException("Trying to parse null object as property");
        }

        IEnumerable<JProperty> properties = obj.Properties();
        if (properties.Count() != 1)
        {
            throw new InvalidOperationException(
                "Trying to parse object with more or less than one property as property");
        }

        return obj.Properties().First();
    }
}