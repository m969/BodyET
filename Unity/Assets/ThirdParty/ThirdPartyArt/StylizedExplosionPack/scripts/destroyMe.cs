using UnityEngine;
using System.Collections;

public class destroyMe : MonoBehaviour{

    float timer;
    public float deathtimer = 10;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;

        if(timer >= deathtimer)
        {
            Destroy(gameObject);
        }
	
	}
}
