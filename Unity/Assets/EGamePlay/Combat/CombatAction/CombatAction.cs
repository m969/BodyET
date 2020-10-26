using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EGamePlay;

namespace EGamePlay.Combat
{
    /// <summary>
    /// 战斗行为概念，造成伤害、治疗英雄、附加状态都属于战斗行为，需要继承自CombatAction
    /// </summary>
    public class CombatAction
    {
        public Entity ActionCreatorEntity { get; set; }
        public List<Entity> ActionTargetEntities { get; set; }
    }
}