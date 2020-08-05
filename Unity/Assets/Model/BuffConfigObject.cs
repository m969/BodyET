using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[CreateAssetMenu(/*"Tools/Buff编辑器/BuffObject"*/)]
public class BuffConfigObject : SerializedScriptableObject
{



    protected override void OnBeforeSerialize()
    {
        base.OnBeforeSerialize();
        BuffHelper.Init();
    }
}
