using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Combat
{
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

        [Toggle("Enabled")]
        public DurationToggleGroup DurationToggleGroup = new DurationToggleGroup();

        [Space(10)]
        [LabelText("Buff效果触发机制")]
        public BuffTriggerType BuffTriggerType;
        [ShowIf("BuffTriggerType", BuffTriggerType.Condition)]
        public ConditionType ConditionType;
        [ShowIf("BuffTriggerType", BuffTriggerType.Action)]
        public ActionType ActionType;
        [ShowIf("BuffTriggerType", BuffTriggerType.Interval)]
        [LabelText("间隔时间")]
        [SuffixLabel("毫秒", true)]
        public uint Interval;

        [LabelText("效果列表")]
        public SkillEffectToggleGroup[] EffectGroupList;
    }

    [Serializable]
    public class MyToggleObject
    {
        public bool Enabled;
    }

    [Serializable]
    [LabelText("持续时间")]
    public class DurationToggleGroup : MyToggleObject
    {
        [Tooltip("不勾即代表永久，0也代表永久")]
        [LabelText("持续时间")]
        [SuffixLabel("毫秒", true)]
        public uint Duration;
    } 
}