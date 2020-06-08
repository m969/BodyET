using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF
{
	//This script controls whether a scene light casts shadows;
	//It is used by the 'DemoMenu' script to toggle the shadows;
	public class DisableShadows : MonoBehaviour {

		//Whether shadows are cast;
		bool shadowsAreActive = true;

		//Reference to light component;
		public Light sceneLight;

		//Start;
		void Start () {
			sceneLight = this.GetComponent<Light>();
		}

		//This function is called by an external script to disable/enable shadows in the scene;
		public void SetShadows(bool _isActivated)
		{
			shadowsAreActive = _isActivated;
			if(!shadowsAreActive)
				sceneLight.shadows = LightShadows.None;
			else 
				sceneLight.shadows = LightShadows.Hard;
		}
	}
}
