using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGamePlay.Combat
{
    public class CombatEntity : Entity
    {
        public CombatListen CombatListen { get; set; }
        public CombatRun CombatRun { get; set; }
        public HealthPoint Health { get; private set; } = new HealthPoint();
        public CombatNumericBox NumericBox { get; private set; } = new CombatNumericBox();
        public DamageAction DamageAction { get; private set; } = new DamageAction();
        public Action<DamageAction> OnReceiveDamage { get; set; }


        public void Initialize()
        {
            NumericBox.Initialize();
            Health.SetMaxValue(99_999);
            Health.Reset();
            DamageAction.Creator = this;
        }

        public void ReceiveDamage(DamageAction damageAction)
        {
            Health.Minus(damageAction.DamageValue);
            OnReceiveDamage?.Invoke(damageAction);
        }

        public void ReceiveCure(int cure)
        {
            Health.Add(cure);
        }
    }
}