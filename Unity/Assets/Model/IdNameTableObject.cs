using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;


[Serializable]
public class IdName
{
    [HorizontalGroup]
    [HideLabel]
    public int Id;
    [HorizontalGroup]
    [HideLabel]
    public string Name;
}

[Serializable]
public class MapData
{
    [DictionaryDrawerSettings]
    Dictionary<string, int> Maps;
}

public class IdNameTableObject : SerializedScriptableObject
{
    [SerializeField]
    public string[] Names;
    //[SerializeField]
    //Dictionary<string, string> Maps = new Dictionary<string, string>() {
    //    {"ss", "ss" }
    //};
}