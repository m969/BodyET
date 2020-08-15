using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.IO;
using Sirenix.Utilities.Editor;

namespace UnityGame.Combat
{
    [CreateAssetMenu(fileName = "技能特效配置", menuName = "技能|状态/技能特效配置")]
    [LabelText("技能特效配置")]
    public class SkillEffectObject : SerializedScriptableObject
    {
        [LabelText("技能ID")]
        [DelayedProperty]
        public uint ID;
        [LabelText("技能名称")]
        [DelayedProperty]
        public string Name;
    }
}