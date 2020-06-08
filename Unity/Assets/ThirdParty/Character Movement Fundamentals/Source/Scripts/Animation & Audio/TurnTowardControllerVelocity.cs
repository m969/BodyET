using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF
{
	//This script turns a gameobject toward the target controller's velocity direction;
	public class TurnTowardControllerVelocity : MonoBehaviour {

		//Target controller;
		public Controller controller;

		//Speed at which this gameobject turns toward the controller's velocity;
		public float turnSpeed = 500f;

		Transform parentTransform;
		Transform tr;

		//Current (local) rotation around the (local) y axis of this gameobject;
		float currentYRotation = 0f;

		//If the angle between the current and target direction falls below 'fallOffAngle', 'turnSpeed' becomes progressively slower (and eventually approaches '0f');
		//This adds a smoothing effect to the rotation;
		float fallOffAngle = 90f;

		//Whether the current controller momentum should be ignored when calculating the new direction;
		public bool ignoreControllerMomentum = false;

		//Setup;
		void Start () {
			tr = transform;
			parentTransform = tr.parent;

			//Throw warning if no controller has been assigned;
			if(controller == null)
			{
				Debug.LogWarning("No controller script has been assigned to this 'TurnTowardControllerVelocity' component!", this);
				this.enabled = false;
			}	
		}

		void LateUpdate () {

			//Get controller velocity;
			Vector3 _velocity;
			if(ignoreControllerMomentum)
				_velocity = controller.GetMovementVelocity();
			else
				_velocity = controller.GetVelocity();

			//Project velocity onto a plane defined by the 'up' direction of the parent transform;
			_velocity = Vector3.ProjectOnPlane(_velocity, parentTransform.up);

			float _magnitudeThreshold = 0.001f;

			//If the velocity's magnitude is smaller than the threshold, return;
			if(_velocity.magnitude < _magnitudeThreshold)
				return;

			//Normalize velocity direction;
			_velocity.Normalize();

			//Get current 'forward' vector;
			Vector3 _currentForward = tr.forward;

			//Calculate (signed) angle between velocity and forward direction;
			float _angleDifference = VectorMath.GetAngle(_currentForward, _velocity, parentTransform.up);

			//Calculate angle factor;
			float _factor = Mathf.InverseLerp(0f, fallOffAngle, Mathf.Abs(_angleDifference));

			//Calculate this frame's step;
			float _step = Mathf.Sign(_angleDifference) * _factor * Time.deltaTime * turnSpeed;

			//Clamp step;
			if(_angleDifference < 0f && _step < _angleDifference)
				_step = _angleDifference;
			else if(_angleDifference > 0f && _step > _angleDifference)
				_step = _angleDifference;

			//Add step to current y angle;
			currentYRotation += _step;

			//Clamp y angle;
			if(currentYRotation > 360f)
				currentYRotation -= 360f;
			if(currentYRotation < -360f)
				currentYRotation += 360f;

			//Set transform rotation using Quaternion.Euler;
			tr.localRotation = Quaternion.Euler(0f, currentYRotation, 0f);

		}

		void OnDisable()
		{ 
		}

		void OnEnable()
		{
			currentYRotation = transform.localEulerAngles.y;
		}
	}
}
