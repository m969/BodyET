using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EGamePlay.Combat;
using ETModel;

public sealed class Hero : MonoBehaviour
{
    public CombatEntity CombatEntity;
    public float MoveSpeed = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        CombatEntity = new CombatEntity();
        CombatEntity.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        var h = Input.GetAxis("Horizontal") * MoveSpeed;
        var v = Input.GetAxis("Vertical") * MoveSpeed;
        var p = transform.position;
        transform.position = new Vector3(p.x + h, 0, p.z + v);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CombatEntity.DamageAction.Target = GameObject.Find("/Monster").GetComponent<Monster>().CombatEntity;
            CombatEntity.DamageAction.DamageValue = RandomHelper.RandomNumber(100, 999);
            CombatEntity.DamageAction.ApplyDamage();
        }
    }
}
