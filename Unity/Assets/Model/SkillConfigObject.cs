using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;

[CreateAssetMenu(/*"Tools/Buff编辑器/BuffObject"*/)]
public class SkillConfigObject : SerializedScriptableObject
{
    [LabelText("技能ID")]
    public int ID;
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
    public string ColdGroupTitle = "冷却时间";
    [ToggleGroup("Cold")]
    [LabelText("冷却时间")]
    [SuffixLabel("毫秒", true)]
    public int ColdTime;

    //[ToggleGroup("CauseDamage", "$DamageGroupTitle")]
    //public bool CauseDamage = false;
    //[ToggleGroup("CauseDamage")]
    //[HideInInspector]
    //public string DamageGroupTitle = "造成伤害";
    //[ToggleGroup("CauseDamage")]
    //public DamageType DamageType;

    [Toggle("Enabled")]
    [LabelText("造成伤害")]
    public DamageToggleGroup DamageToggleGroup = new DamageToggleGroup();

    [Toggle("Enabled")]
    [LabelText("治疗英雄")]
    public CureToggleGroup CureToggleGroup = new CureToggleGroup();


    protected override void OnBeforeSerialize()
    {
        BuffHelper.Init();
        base.OnBeforeSerialize();
    }

    protected override void OnAfterDeserialize()
    {
        base.OnAfterDeserialize();
    }
}

[Serializable]
public class MyToggleObject
{
    public bool Enabled;
}

[Serializable]
public class CureToggleGroup : MyToggleObject
{
    [LabelText("治疗参数")]
    public string CureValue;
}

[Serializable]
public class DamageToggleGroup : MyToggleObject
{
    public DamageType DamageType;
    [LabelText("伤害参数")]
    public string DamageValue;
}

//// Simple Toggle Group
//[ToggleGroup("MyToggle")]
//public bool MyToggle;

//[ToggleGroup("MyToggle")]
//public float A;

//[ToggleGroup("MyToggle")]
//[HideLabel, Multiline]
//public string B;

//// Toggle for custom data.
//[ToggleGroup("EnableGroupOne", "$GroupOneTitle")]
//public bool EnableGroupOne = true;

//[ToggleGroup("EnableGroupOne")]
//public string GroupOneTitle = "One";

//[ToggleGroup("EnableGroupOne")]
//public float GroupOneA;

//[ToggleGroup("EnableGroupOne")]
//public float GroupOneB;

//// Toggle for individual objects.
//[Toggle("Enabled")]
//public MyToggleObject Three = new MyToggleObject();

//[Toggle("Enabled")]
//public MyToggleA Four = new MyToggleA();

//[Toggle("Enabled")]
//public MyToggleB Five = new MyToggleB();

//public MyToggleC[] ToggleList = new MyToggleC[]
//{
//    new MyToggleC(){ Test = 2f, Enabled = true, },
//    new MyToggleC(){ Test = 5f, },
//    new MyToggleC(){ Test = 7f, },
//};

//[Serializable]
//public class MyToggleObject
//{
//    public bool Enabled;

//    [HideInInspector]
//    public string Title;

//    public int A;
//    public int B;
//}

//[Serializable]
//public class MyToggleA : MyToggleObject
//{
//    public float C;
//    public float D;
//    public float F;
//}

//[Serializable]
//public class MyToggleB : MyToggleObject
//{
//    public string Text;
//}

//[Serializable]
//public class MyToggleC
//{
//    [ToggleGroup("Enabled", "$Label")]
//    public bool Enabled;

//    public string Label { get { return this.Test.ToString(); } }

//    [ToggleGroup("Enabled")]
//    public float Test;
//}