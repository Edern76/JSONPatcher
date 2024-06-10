using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace JSONPatcherCore.Exceptions;

public class JsonPatchTargetNotFoundException : JsonPatchException
{
    public JsonPatchTargetNotFoundException(JObject? patchedObject=null) : base(patchedObject)
    {
    }

    // Constructor with a custom message
    public JsonPatchTargetNotFoundException(string message, JObject? patchedObject=null)
        : base(message, patchedObject)
    {
    }

    // Constructor with a custom message and an inner exception
    public JsonPatchTargetNotFoundException(string message, Exception innerException, JObject? patchedObject=null)
        : base(message, innerException, patchedObject)
    {
    }

    // Constructor for serialization
    protected JsonPatchTargetNotFoundException(SerializationInfo info, StreamingContext context, JObject? patchedObject=null)
        : base(info, context, patchedObject)
    {
    }
}