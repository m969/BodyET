using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EGamePlay.Combat;

public class Hero : MonoBehaviour
{
    public CombatManager CombatManager;
    public float MoveSpeed = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        CombatManager = new CombatManager();
        CombatManager.Start();
    }

    // Update is called once per frame
    void Update()
    {
        var h = Input.GetAxis("Horizontal") * MoveSpeed;
        var v = Input.GetAxis("Vertical") * MoveSpeed;
        var p = transform.position;
        transform.position = new Vector3(p.x + h, 0, p.z + v);
        CombatManager.Update();
    }
}
