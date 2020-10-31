using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EGamePlay.Combat;
using ETModel;
using UnityEngine.UIElements;

public sealed class Hero : MonoBehaviour
{
    public CombatEntity CombatEntity;
    public float MoveSpeed = 0.2f;
    public float AnimTime = 0.05f;
    public GameTimer AnimTimer = new GameTimer(0.1f);
    public GameObject AttackPrefab;


    // Start is called before the first frame update
    void Start()
    {
        CombatEntity = new CombatEntity();
        CombatEntity.Initialize();
        AnimTimer.MaxTime = AnimTime;
    }

    // Update is called once per frame
    void Update()
    {
        AnimTimer.UpdateAsFinish(Time.deltaTime);
        if (!AnimTimer.IsFinished)
        {
            //return;
        }

        var h = Input.GetAxis("Horizontal") * MoveSpeed;
        var v = Input.GetAxis("Vertical") * MoveSpeed;
        var p = transform.position;
        transform.position = new Vector3(p.x + h, 0, p.z + v);

        if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
        {
            AnimTimer.Reset();
            var monster = GameObject.Find("/Monster");
            var attackEffect = GameObject.Instantiate(AttackPrefab);
            attackEffect.transform.position = Vector3.up;
            attackEffect.GetComponent<LineRenderer>().SetPosition(0, transform.position);
            attackEffect.GetComponent<LineRenderer>().SetPosition(1, monster.transform.position);
            GameObject.Destroy(attackEffect, 0.05f);
            CombatEntity.DamageAction.Target = monster.GetComponent<Monster>().CombatEntity;
            CombatEntity.DamageAction.DamageValue = RandomHelper.RandomNumber(100, 999);
            CombatEntity.DamageAction.ApplyDamage();
        }
    }
}
