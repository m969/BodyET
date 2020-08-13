using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.IO;

namespace Combat
{
    [CreateAssetMenu()]
    [LabelText("Buff配置")]
    public class BuffConfigObject : SerializedScriptableObject
    {
        [LabelText("BuffID")]
        [DelayedProperty]
        public int ID;
        [LabelText("Buff名称")]
        [DelayedProperty]
        public string Name;
        [LabelText("Buff/Debuff类型")]
        public BuffType BuffType;
        [LabelText("是否在Buff状态栏显示")]
        public bool ShowInBuffList;

        [Toggle("Enabled")]
        public DurationToggleGroup DurationToggleGroup = new DurationToggleGroup();
        
        [Toggle("Enabled")]
        public StateToggleGroup StateToggleGroup = new StateToggleGroup();

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

        [Space(40)]
        [LabelText("Buff特效")]
        public GameObject SkillParticleEffect;

        [LabelText("Buff音效")]
        public AudioClip SkillAudio;


        [OnInspectorGUI]
        private void OnInspectorGUI()
        {
            string[] guids = UnityEditor.Selection.assetGUIDs;
            int i = guids.Length;
            if (i == 1)
            {
                string guid = guids[0];
                string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var fileName = Path.GetFileName(assetPath);
                var newName = $"Buff_{this.ID}_{this.Name}";
                if (!fileName.StartsWith(newName))
                {
                    Debug.Log(assetPath);
                    UnityEditor.AssetDatabase.RenameAsset(assetPath, newName);
                }
            }
        }
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
    
    [Serializable]
    [LabelText("设置状态")]
    public class StateToggleGroup : MyToggleObject
    {
        [LabelText("设置")]
        public StateType StateType;
    }
}