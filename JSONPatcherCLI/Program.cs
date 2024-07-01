using JSONPatcherCore;
using JSONPatcherCore.Operations.Base;
using JSONPatchParser;
using Newtonsoft.Json.Linq;

namespace JSONPatcherCLI;

class Program
{
    static void Main(string[] args)
    {
        string inputPath = args[0];
        string patchPath = args[1];
        string outputPath = args[2];

        JObject patchableJSON = JObject.Parse(File.ReadAllText(inputPath));
        List<IPatchOperation> patchOperations = JsonPatchParser.ParseFolder(patchPath);
        JsonPatchApplier applier = new JsonPatchApplier(patchableJSON, patchOperations);
        JObject patchedJSON = applier.Apply();
        File.WriteAllText(outputPath, patchedJSON.ToString());
    }
}