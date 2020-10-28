using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EGamePlay.Combat;

public sealed class Monster : MonoBehaviour
{
    public UnitCombatManager CombatManager;
    public float MoveSpeed = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        CombatManager = new UnitCombatManager();
        CombatManager.Start();
    }

    // Update is called once per frame
    void Update()
    {
        CombatManager.Update();
    }
}
