using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace EGamePlay.Combat
{
    /// <summary>
    /// 赋给效果行为
    /// </summary>
    public class AssignAffectAction : CombatAction
    {
        //效果类型
        public AffectType AffectType { get; set; }
        //效果数值
        public string AffectValue { get; set; }


        private void BeforeAssign()
        {

        }

        public void ApplyAssignAction()
        {
            BeforeAssign();

            AfterAssign();
        }

        private void AfterAssign()
        {

        }
    }

    public enum AffectType
    {
        DamageAffect = 1,
        NumericModify = 2,
        StatusAttach = 3,
        BuffAttach = 4,
    }
}