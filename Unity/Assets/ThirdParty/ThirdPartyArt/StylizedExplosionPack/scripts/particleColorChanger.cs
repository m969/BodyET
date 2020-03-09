using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class particleColorChanger : MonoBehaviour {

    [System.Serializable]
    public class colorChange
    {
        public string Name;
        public ParticleSystem[] colored_ParticleSystem;
        public Gradient customer_Gradient;
        
    }
    public colorChange[] colorChangeList;


    void Update()
     {
         for (int i = 0; i < colorChangeList.Length; i++)
         {
            
            for(int a = 0; a < colorChangeList[i].colored_ParticleSystem.Length; a++)
            {
                var col = colorChangeList[i].colored_ParticleSystem[a].colorOverLifetime;
                col.color = colorChangeList[i].customer_Gradient;

            }        
         }
     }
  
}
