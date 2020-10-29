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
        public NumericBox NumericBox { get; private set; } = new NumericBox();


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