using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instantiateEffectCaller : MonoBehaviour
{
    [System.NonSerialized]
    public bool fired = false;
    float timer;
    public float timeLimit;
    [System.Serializable]
    public class chainEffect
    {
        [System.NonSerialized]
        public bool isPlayed = false;
        public float activateTimer;
        public GameObject Effect;
        public Transform effectLocator;
    }
    public chainEffect[] chainEffectList;

    void Start()
    {
        //  print(chainEffectList.Length);
    }

    // Update is called once per frame
    void Update()
    {
            timer += Time.deltaTime;
            CheckTimer();
    }
    void CheckTimer()
    {

        for (int i = 0; i < chainEffectList.Length; i++)
        {
            if (timer >= chainEffectList[i].activateTimer && chainEffectList[i].isPlayed == false)
            {
                Instantiate(chainEffectList[i].Effect, chainEffectList[i].effectLocator.transform.position, chainEffectList[i].effectLocator.transform.rotation);
                chainEffectList[i].isPlayed = true;
            }
        }
        if (timer >= timeLimit)
        {
            fired = false;
            ResetTimers();
        }
    }


    public void ResetTimers()
    {
        for (int i = 0; i < chainEffectList.Length; i++)
        {
            chainEffectList[i].isPlayed = false;
        }
        timer = 0;
    }
}
