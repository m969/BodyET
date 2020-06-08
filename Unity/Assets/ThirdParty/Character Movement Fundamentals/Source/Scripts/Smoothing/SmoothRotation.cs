using UnityEngine;
using System.Collections;

namespace CMF
{
	//This script smoothes the rotation of a gameobject;
	public class SmoothRotation : MonoBehaviour {

		//The target transform, whose rotation values will be copied and smoothed;
		public Transform target;
		Transform tr;

		Quaternion currentRotation;

		//Speed that controls how fast the current rotation will be smoothed toward the target rotation;
		public float smoothSpeed = 20f;

		//Whether rotation values will be extrapolated to compensate for delay caused by smoothing;
		public bool extrapolateRotation = false;

		//'UpdateType' controls whether the smoothing function is called in 'Update' or 'LateUpdate';
		public enum UpdateType
		{
			Update,
			LateUpdate
		}
		public UpdateType updateType;

		//Awake;
		void Awake () {

			//If no target has been selected, choose this transform's parent as target;
			if(target == null)
				target = this.transform.parent;

			tr = transform;
			currentRotation = transform.rotation;
		}

		//OnEnable;
		void OnEnable()
		{
			//Reset current rotation when gameobject is re-enabled to prevent unwanted interpolation from last rotation;
			ResetCurrentRotation();
		}

		void Update () {
			if(updateType == UpdateType.LateUpdate)
				return;
			SmoothUpdate();
		}

		void LateUpdate () {
			if(updateType == UpdateType.Update)
				return;
			SmoothUpdate();
		}

		void SmoothUpdate()
		{
			//Smooth current rotation;
			currentRotation = Smooth (currentRotation, target.rotation, smoothSpeed);

			//Set rotation;
			tr.rotation = currentRotation;
		}

		//Smooth a rotation toward a target rotation based on 'smoothTime';
		Quaternion Smooth(Quaternion _currentRotation, Quaternion _targetRotation, float _smoothSpeed)
		{
			//If 'extrapolateRotation' is set to 'true', calculate a new target rotation;
			if (extrapolateRotation && Quaternion.Angle(_currentRotation, _targetRotation) < 90f) {
				Quaternion difference = _targetRotation * Quaternion.Inverse (_currentRotation);
				_targetRotation *= difference;
			}

			//Slerp rotation and return;
			return Quaternion.Slerp (_currentRotation, _targetRotation, Time.deltaTime * _smoothSpeed);
		}

		//Reset stored rotation and rotate this gameobject to macth the target's rotation;
		//Call this function if the target has just been rotatedand no interpolation should take place (instant rotation);
		public void ResetCurrentRotation()
		{
			currentRotation = target.rotation;
		}
								
	}
}
