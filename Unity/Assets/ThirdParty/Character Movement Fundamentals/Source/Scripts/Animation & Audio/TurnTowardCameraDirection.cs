using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF
{
	//This script turns a gameobject toward the look direction of a chosen 'CameraController' component;
	public class TurnTowardCameraDirection : MonoBehaviour {

		public CameraController cameraController;
		Transform tr;

		//Setup;
		void Start () {
			tr = transform;

			if(cameraController == null)
				Debug.LogWarning("No camera controller reference has been assigned to this script.", this);
		}
		
		//Update;
		void LateUpdate () {

			if(!cameraController)
				return;

			//Calculate up and forwward direction;
			Vector3 _forwardDirection = cameraController.GetFacingDirection();
			Vector3 _upDirection = cameraController.GetUpDirection();

			//Set rotation;
			tr.rotation = Quaternion.LookRotation(_forwardDirection, _upDirection);
		}
	}
}
