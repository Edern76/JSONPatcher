﻿using Newtonsoft.Json.Linq;

namespace JSONPatcherCore.Operations.Base;

public abstract class BaseOperation : IPatchOperation
{
    public string TargetPath { get; private set; }
    public int Priority { get; private set; }
    
    protected BaseOperation(string targetPath, int priority=1)
    {
        TargetPath = targetPath;
        Priority = priority;
    }

    public abstract void Apply(JObject patchedObject);
}