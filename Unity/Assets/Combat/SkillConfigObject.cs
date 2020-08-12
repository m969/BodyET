using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using Sirenix.OdinInspector.Editor;
using UnityEngine.PlayerLoop;
using Sirenix.Utilities.Editor;


namespace Combat
{
//#if UNITY_EDITOR
//    using UnityEditor;
//    [CustomEditor(typeof(SkillConfigObject))]
//    public class SkillConfigObjectInspector : OdinEditor
//    {
//        public override void OnInspectorGUI()
//        {
//            base.OnInspectorGUI();
//            var skillConfigObject = target as SkillConfigObject;
//            //var rect = SirenixEditorGUI.BeginVerticalList();
//            //EditorGUILayout.PropertyField(serializedObject.FindProperty("MyToggleObjectList"));
//            //SirenixEditorFields.Dropdown(rect, new GUIContent(""), skillConfigObject.MyToggleObjectList[0], skillConfigObject.MyToggleObjectList);
//            //foreach (var item in skillConfigObject.MyToggleObjectList)
//            //{
//            //    var visible = true;
//            //    var isToggle = SirenixEditorGUI.BeginToggleGroup(item, ref item.Enabled, ref visible, "");
//            //    if (isToggle)
//            //    {
//            //        if (item is CureToggleGroup CureToggleGroup)
//            //        {
//            //            EditorGUILayout.TextField(CureToggleGroup.CureValue);
//            //        }
//            //    }
//            //    SirenixEditorGUI.EndToggleGroup();
//            //}
//            //SirenixEditorGUI.EndVerticalList();
//        }
//    }
//#endif


    [CreateAssetMenu()]
    [LabelText("技能配置")]
    public class SkillConfigObject : SerializedScriptableObject
    {
        [LabelText("技能ID")]
        public uint ID;
        [LabelText("技能名称")]
        public string Name;
        public SkillSpellType SkillSpellType;
        public SkillType SkillType;
        public SkillTargetSelectType TargetSelectType;

        [ToggleGroup("TargetSelect", "$TargetGroupTitle")]
        [ReadOnly]
        public bool TargetSelect = true;
        [ToggleGroup("TargetSelect")]
        [HideInInspector]
        public string TargetGroupTitle = "目标限制";
        [ToggleGroup("TargetSelect")]
        public SkillAffectTargetType AffectTargetType;
        [ToggleGroup("TargetSelect")]
        [HideIf("AffectTargetType", SkillAffectTargetType.Self)]
        public SkillTargetType TargetType;

        [ToggleGroup("Cold", "$ColdGroupTitle")]
        public bool Cold = false;
        [ToggleGroup("Cold")]
        [HideInInspector]
        public string ColdGroupTitle = "技能冷却";
        [ToggleGroup("Cold")]
        [LabelText("冷却时间")]
        [SuffixLabel("毫秒", true)]
        public uint ColdTime;

        [Space(10)]
        [LabelText("效果列表")]
        public SkillEffectToggleGroup[] EffectGroupList;

        //public List<MyToggleObject> MyToggleObjectList = new List<MyToggleObject>()
        //{
        //    new CureToggleGroup(),
        //    new DamageToggleGroup(),
        //};

        [Space(40)]
        [LabelText("技能动作")]
        public AnimationClip SkillAnimationClip;

        [LabelText("技能特效")]
        public GameObject SkillParticleEffect;

        [LabelText("技能音效")]
        public AudioClip SkillAudio;
    }


    //[Serializable]
    //public class CureToggleGroup : MyToggleObject
    //{
    //    [LabelText("治疗参数")]
    //    public string CureValue;
    //}

    //[Serializable]
    //public class DamageToggleGroup : MyToggleObject
    //{
    //    public DamageType DamageType;
    //    [LabelText("伤害参数")]
    //    public string DamageValue;
    //}
}

namespace Combat
{
    [Serializable]
    public class SkillEffectToggleGroup
    {
        [ToggleGroup("Enabled", "$Label")]
        public bool Enabled;
        public string Label
        {
            get
            {
                switch (SkillEffectType)
                {
                    case SkillEffectType.None: return "（空）";
                    case SkillEffectType.CauseDamage: return "造成伤害";
                    case SkillEffectType.CureHero: return "治疗英雄";
                    case SkillEffectType.AddBuff: return "施加Buff";
                    case SkillEffectType.RemoveBuff: return "移除Buff";
                    case SkillEffectType.AddShield: return "添加护盾";
                    case SkillEffectType.ChangeNumeric: return "改变数值";
                    //case SkillEffectType.ChangeState:
                    //    {
                    //        switch (StateType)
                    //        {
                    //            case StateType.Vertigo: return $"改变[眩晕]状态";
                    //            case StateType.Silent: return $"改变[沉默]状态";
                    //            case StateType.Poison: return $"改变[中毒]状态";
                    //            default: return "改变状态";
                    //        }
                    //    }
                    default: return "（空）";
                }
            }
        }
        [ToggleGroup("Enabled")]
        [ShowIf("SkillEffectType", SkillEffectType.None)]
        public SkillEffectType SkillEffectType;

        [ToggleGroup("Enabled")]
        //[ShowIf("SkillEffectType", SkillEffectType.AddBuff)]
        public AddEffetTargetType AddBuffTargetType;

        #region 造成伤害
        [ToggleGroup("Enabled")]
        [ShowIf("SkillEffectType", SkillEffectType.CauseDamage)]
        public DamageType DamageType;
        [ToggleGroup("Enabled")]
        [ShowIf("SkillEffectType", SkillEffectType.CauseDamage)]
        [LabelText("伤害参数")]
        public string DamageValue;
        [ToggleGroup("Enabled")]
        [ShowIf("SkillEffectType", SkillEffectType.CauseDamage)]
        [LabelText("是否可以暴击")]
        public bool CanCrit;
        #endregion

        #region 治疗英雄
        [ToggleGroup("Enabled")]
        [ShowIf("SkillEffectType", SkillEffectType.CureHero)]
        [LabelText("治疗参数")]
        public string CureValue;
        #endregion

        #region 施加Buff
        [ToggleGroup("Enabled")]
        [ShowIf("SkillEffectType", SkillEffectType.AddBuff)]
        public BuffConfigObject AddBuff;
        #endregion

        #region 移除Buff
        [ToggleGroup("Enabled")]
        [ShowIf("SkillEffectType", SkillEffectType.RemoveBuff)]
        public BuffConfigObject RemoveBuffConfigObject;
        [ToggleGroup("Enabled")]
        [ShowIf("SkillEffectType", SkillEffectType.RemoveBuff)]
        public AddEffetTargetType RemoveBuffTargetType;
        #endregion

        //#region 改变状态
        //[ToggleGroup("Enabled")]
        //[ShowIf("SkillEffectType", SkillEffectType.ChangeState)]
        //public StateType StateType;
        //[ToggleGroup("Enabled")]
        //[ShowIf("SkillEffectType", SkillEffectType.ChangeState)]
        //[LabelText("状态参数")]
        //public string StateValue;
        //#endregion

        #region 改变数值
        [ToggleGroup("Enabled")]
        [ShowIf("SkillEffectType", SkillEffectType.ChangeNumeric)]
        public NumericType NumericType;
        [ToggleGroup("Enabled")]
        [ShowIf("SkillEffectType", SkillEffectType.ChangeNumeric)]
        [LabelText("数值参数")]
        public string NumericValue;
        #endregion

        #region 添加护盾
        [ToggleGroup("Enabled")]
        [ShowIf("SkillEffectType", SkillEffectType.AddShield)]
        public ShieldType ShieldType;
        [ToggleGroup("Enabled")]
        [ShowIf("SkillEffectType", SkillEffectType.AddShield)]
        [LabelText("护盾值")]
        public uint ShieldValue;
        [ToggleGroup("Enabled")]
        [ShowIf("SkillEffectType", SkillEffectType.AddShield)]
        [LabelText("护盾持续时间")]
        [SuffixLabel("毫秒", true)]
        public uint ShieldDuration;
        #endregion

        //#region 施加中毒效果
        //[ToggleGroup("Enabled")]
        //[ShowIf("SkillEffectType", SkillEffectType.AddShield)]
        //public ShieldType ShieldType;
        //[ToggleGroup("Enabled")]
        //[ShowIf("SkillEffectType", SkillEffectType.AddShield)]
        //[LabelText("护盾值")]
        //public uint ShieldValue;
        //[ToggleGroup("Enabled")]
        //[ShowIf("SkillEffectType", SkillEffectType.AddShield)]
        //[LabelText("中毒持续时间")]
        //[SuffixLabel("毫秒", true)]
        //public uint PoisonDuration;
        //#endregion
    }

    [LabelText("护盾类型")]
    public enum ShieldType
    {
        [LabelText("普通护盾")]
        Shield,
        [LabelText("物理护盾")]
        PhysicShield,
        [LabelText("魔法护盾")]
        MagicShield,
        [LabelText("技能护盾")]
        SkillShield,
    }
}