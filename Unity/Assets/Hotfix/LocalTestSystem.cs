using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETHotfix;
using ETModel;

public class LocalTestSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Unit.LocalUnit = ETModel.EntityFactory.Create<Unit>(ETModel.Game.Scene);
        //ETHotfix.Game.Scene.AddComponent<ETHotfix.OperaComponent>().Awake();
    }

    // Update is called once per frame
    void Update()
    {
        //ETHotfix.Game.Scene.GetComponent<ETHotfix.OperaComponent>().Update();
    }
}
