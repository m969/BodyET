using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGamePlay.Combat
{
    public class CombatEntity : Entity
    {
        public CombatListen CombatListen { get; set; }
        public CombatRun CombatRun { get; set; }
        public int Health { get; private set; }
        public CombatNumericBox NumericBox { get; private set; } = new CombatNumericBox();


        public void Awake()
        {
            NumericBox.Initialize();
        }

        public void ReceiveDamage(int damage)
        {
            Health -= damage;
        }
    }
}