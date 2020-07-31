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

public class IdNameTableObject : ScriptableObject
{
    [SerializeField]
    public List<IdName> IdNames;
}