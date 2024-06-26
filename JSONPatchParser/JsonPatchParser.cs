﻿using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using JSONPatcherCore.Exceptions;
using JSONPatcherCore.Operations;
using JSONPatcherCore.Operations.Base;
using JSONPatchParser.Processors;
using JSONPatchParser.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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


    public static List<IPatchOperation> ParseUnsafe(string patches)
    {
        List<string> errors = new List<string>();
        List<IPatchOperation> result = Parse(patches, ref errors);
        if (errors.Count > 0)
        {
            if (errors.Count == 1)
            {
                throw new InvalidOperationException(errors[0]);
            }
            else
            {
                throw new InvalidOperationException($"Multiple errors: {string.Join(", ", errors)}");
            }
        }

        return result;
    }

    public static List<IPatchOperation> Parse(string patches, ref List<string> errors)
    {
        JToken? token = JToken.Parse(patches);
        List<IPatchOperation> result = new List<IPatchOperation>();
        switch (token)
        {
            case JArray array:
                result = ParseMultiple(array, ref errors);
                break;
            case JObject obj:
                try
                {
                    result.Add(ParseSingle(obj));
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                    errors.Add(e.Message);
                }

                break;
            default:
                throw new InvalidOperationException("Invalid JSON Patch root");
        }

        return result;
    }

    public static List<IPatchOperation> ParseFolder(string path, bool recursive = true)
    {
        if (!Directory.Exists(path))
        {
            throw new DirectoryNotFoundException($"Directory {path} does not exist");
        }

        IEnumerable<string> files = Directory
            .GetFiles(path, "*.json", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
            .Where(p =>
            {
                string fileName = Path.GetFileName(p);
                return fileName.StartsWith("Patch_") || fileName.StartsWith("Patches_");
            });
        Dictionary<string, List<string>> errorsMap = new Dictionary<string, List<string>>();
        List<IPatchOperation> result = files.SelectMany(file =>
        {
            List<string> errors = new List<string>();
            List<IPatchOperation> operations = Parse(File.ReadAllText(file), ref errors);
            if (errors.Count > 0)
            {
                errorsMap.Add(file, errors);
            }

            return operations;
        }).ToList();
        if (errorsMap.Count > 0)
        {
            string errors = string.Join("\n",
                errorsMap.Select(pair => $"- {pair.Key}:\n{string.Join("\n", pair.Value)}"));
            throw new JsonPatchException($"Errors while parsing JSON Patches:\n{errors}");
        }

        return result;
    }

    private static IPatchOperation ParseSingle(JObject patch)
    {
        JsonPatchSchema? schema = patch.ToObject<JsonPatchSchema>();
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

    private static List<IPatchOperation> ParseMultiple(string patches)
    {
        List<string> errors = new List<string>();
        return ParseMultiple(JArray.Parse(patches), ref errors);
    }

    private static List<IPatchOperation> ParseMultiple(JArray patches, ref List<string> errors)
    {
        List<IPatchOperation> result = new List<IPatchOperation>();
        foreach (JToken token in patches.Children())
        {
            try
            {
                if (token is JObject obj)
                {
                    result.Add(ParseSingle(obj));
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                errors.Add(e.Message);
            }
        }

        return result;
    }
}