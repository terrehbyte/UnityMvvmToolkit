﻿using System.Collections.Generic;

namespace UnityMvvmToolkit.SourceGenerators.Models;

public class BindableElementInfo
{
    public BindableElementInfo(string classIdentifier)
    {
        ClassIdentifier = classIdentifier;
        Attributes = new List<KeyValuePair<string, string>>();
    }

    public string ClassIdentifier { get; }
    public List<KeyValuePair<string, string>> Attributes { get; }
}