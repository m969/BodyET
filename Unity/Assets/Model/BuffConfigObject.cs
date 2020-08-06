using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[CreateAssetMenu()]
[LabelText("Buff配置")]
public class BuffConfigObject : SerializedScriptableObject
{
    [LabelText("BuffID")]
    public int ID;
    [LabelText("Buff名称")]
    public string Name;
    [LabelText("是否在Buff状态栏显示")]
    public bool ShowInBuffList;

}
