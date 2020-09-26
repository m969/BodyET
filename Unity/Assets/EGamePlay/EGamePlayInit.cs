using EGamePlay.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EGamePlayInit : MonoBehaviour
{
    public CombatManager CombatManager;

    // Start is called before the first frame update
    void Start()
    {
        CombatManager = new CombatManager();
        CombatManager.Start();
    }

    // Update is called once per frame
    void Update()
    {
        CombatManager.Update();
    }
}
