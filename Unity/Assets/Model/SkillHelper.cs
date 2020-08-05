using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Sirenix.OdinInspector;

public class SkillHelper
{

}

[LabelText("技能释放方式")]
public enum SkillSpellType
{
    [LabelText("主动")]
    Initiative,
    [LabelText("被动")]
    Passive,
}

[LabelText("技能类型")]
public enum SkillType
{
    [LabelText("指向性技能")]
    Toward,
    [LabelText("非指向性技能")]
    Untoward,
}

[LabelText("目标选择方式")]
public enum SkillTargetSelectType
{
    [LabelText("自动")]
    Auto,
    [LabelText("其他")]
    Other,
}

[LabelText("作用对象")]
public enum SkillAffectTargetType
{
    [LabelText("自身")]
    Self = 0,
    [LabelText("己方")]
    SelfTeam = 1,
    [LabelText("敌方")]
    EnemyTeam = 2,
}

[LabelText("目标类型")]
public enum SkillTargetType
{
    [LabelText("单体目标")]
    Single = 0,
    [LabelText("群体目标")]
    Multiple = 1,
}

[LabelText("伤害类型")]
public enum DamageType
{
    [LabelText("(空)")]
    None = 0,
    [HideLabel]
    [LabelText("物理伤害")]
    Physic = 1,
    [LabelText("魔法伤害")]
    Magic = 2,
    [LabelText("真实伤害")]
    Real = 3,
}

//[Flags]
//[LabelText("伤害类型")]
//public enum DamageType
//{
//    [LabelText("(空)")]
//    None = 0,
//    [HideLabel]
//    [LabelText("物理伤害")]
//    Physic = 1,//2-0
//    [LabelText("魔法伤害")]
//    Magic = 1 << 1,//2-10
//    [LabelText("真实伤害")]
//    Real = 1 << 2,//2-100
//    [LabelText("暴击伤害")]
//    Critical = 1 << 3,//2-1000
//    [LabelText("物理暴击伤害")]
//    PhysicCritical = Physic | Critical,
//}