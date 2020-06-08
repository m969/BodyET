using UnityEngine;
using System.Collections;

namespace CMF
{
	//This script smoothes the position of a gameobject;
	public class SmoothPosition : MonoBehaviour {

		//The target transform, whose position values will be copied and smoothed;
		public Transform target;
		Transform tr;

		Vector3 currentPosition;
		
		//Speed that controls how fast the current position will be smoothed toward the target position when 'Lerp' is selected as smoothType;
		public float lerpSpeed = 20f;

		//Time that controls how fast the current position will be smoothed toward the target position when 'SmoothDamp' is selected as smoothType;
		public float smoothDampTime = 0.02f;

		//Whether position values will be extrapolated to compensate for delay caused by smoothing;
		public bool extrapolatePosition = false;

		//'UpdateType' controls whether the smoothing function is called in 'Update' or 'LateUpdate';
		public enum UpdateType
		{
			Update,
			LateUpdate
		}
		public UpdateType updateType;

		//Different smoothtypes use different algorithms to smooth out the target's position; 
		public enum SmoothType
		{
			Lerp,
			SmoothDamp, 
		}

		public SmoothType smoothType;

		//Local position offset at the start of the game;
		Vector3 localPositionOffset;

		Vector3 refVelocity;
		
		//Awake;
		void Awake () {
			
			//If no target has been selected, choose this transform's parent as the target;
			if(target == null)
				target = this.transform.parent;

			tr = transform;
			currentPosition = transform.position;

			localPositionOffset = tr.localPosition;
		}

		//OnEnable;
		void OnEnable()
		{
			//Reset current position when gameobject is re-enabled to prevent unwanted interpolation from last position;
			ResetCurrentPosition();
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
			//Smooth current position;
			currentPosition = Smooth (currentPosition, target.position, lerpSpeed);

			//Set position;
			tr.position = currentPosition;
		}

		Vector3 Smooth(Vector3 _start, Vector3 _target, float _smoothTime)
		{
			//Convert local position offset to world coordinates;
			Vector3 _offset = tr.localToWorldMatrix * localPositionOffset;

			//If 'extrapolateRotation' is set to 'true', calculate a new target position;
			if (extrapolatePosition) {
				Vector3 difference = _target - (_start - _offset);
				_target += difference;
			}

			//Add local position offset to target;
			_target += _offset;

			//Smooth (based on chosen smoothType) and return position;
			switch (smoothType)
			{
				case SmoothType.Lerp:
					return Vector3.Lerp (_start, _target, Time.deltaTime * _smoothTime);
				case SmoothType.SmoothDamp:
					return Vector3.SmoothDamp (_start, _target, ref refVelocity, smoothDampTime);
				default:
					return Vector3.zero;
			}
		}

		//Reset stored position and move this gameobject directly to the target's position;
		//Call this function if the target has just been moved a larger distance and no interpolation should take place (teleporting);
		public void ResetCurrentPosition()
		{
			//Convert local position offset to world coordinates;
			Vector3 _offset = tr.localToWorldMatrix * localPositionOffset;
			//Add position offset and set current position;
			currentPosition = target.position + _offset;
		}
	}
}