using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace JSONPatcherCore.Exceptions;

public class JsonPatchException :Exception
{
    public JObject? PatchedObject { get; private set; }
    
    // Default constructor
    public JsonPatchException(JObject? patchedObject=null) : base()
    {
        PatchedObject = patchedObject;
    }

    // Constructor with a custom message
    public JsonPatchException(string message, JObject? patchedObject=null)
        : base(message)
    {
        PatchedObject = patchedObject;
    }

    // Constructor with a custom message and an inner exception
    public JsonPatchException(string message, Exception innerException, JObject? patchedObject=null)
        : base(message, innerException)
    {
        PatchedObject = patchedObject;
    }

    // Constructor for serialization
    protected JsonPatchException(SerializationInfo info, StreamingContext context, JObject? patchedObject=null)
        : base(info, context)
    {
        PatchedObject = patchedObject;
    }
}