using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF
{
	//This script is a slightly more specialized version of the regular 'CameraController' script, intended for games using a third-person camera.
	//By enabling 'turnCameraTowardMovementDirection', the camera will gradually rotate toward the current movement direction of the gameobject it is attached to;
	//The rate and speed of this rotation can be controlled using 'maximumMovementSpeed' and 'cameraTurnSpeed';
	public class ThirdPersonCameraController : CameraController {

		//Whether or not the camera turns towards the player's movement direction;
		public bool turnCameraTowardMovementDirection = true;

		public Controller controller;

		//The maximum expected movement speed of this game object;
		//This value should be set to the maximum movement speed achievable by this gameobject;
		//The closer the current movement speed is to 'maximumMovementSpeed', the faster the camera will turn;
		//As a result, if the gameobject moves slower (i.e. "walking" instead of "running", in case of a character), the camera will turn slower as well.
		public float maximumMovementSpeed = 10f;

		//The general rate at which the camera turns toward the movement direction;
		public float cameraTurnSpeed = 70f;

		protected override void Setup()
		{
			if(controller == null)
				Debug.LogWarning("No controller reference has been assigned to this script.", this.gameObject);
		}

		protected override void HandleCameraRotation ()
		{
			//Execute normal camera rotation code;
			base.HandleCameraRotation ();

			if(controller == null)
				return;

			if(turnCameraTowardMovementDirection && controller != null)
			{
				//Get controller velocity;
				Vector3 _controllerVelocity = controller.GetVelocity();

				RotateTowardsVelocity(_controllerVelocity, cameraTurnSpeed);
			}
		}

		//Rotate camera toward '_direction', at the rate of '_speed', around the upwards vector of this gameobject;
		public void RotateTowardsVelocity(Vector3 _velocity, float _speed)
		{
			//Remove any unwanted components of direction;
			_velocity = VectorMath.RemoveDotVector(_velocity, GetUpDirection());
			
			//Calculate angle difference of current direction and new direction;
			float _angle = VectorMath.GetAngle(GetFacingDirection(), _velocity, GetUpDirection());

			//Calculate sign of angle;
			float _sign = Mathf.Sign (_angle);

			//Calculate final angle difference;
			float _finalAngle =  Time.deltaTime * _speed * _sign * Mathf.Abs(_angle/90f);

			//If angle is greater than 90 degrees, recalculate final angle difference;
			if(Mathf.Abs(_angle) > 90f)
				_finalAngle = Time.deltaTime * _speed * _sign * ((Mathf.Abs (180f - Mathf.Abs(_angle)))/90f);

			//Check if calculated angle overshoots;
			if(Mathf.Abs (_finalAngle) > Mathf.Abs (_angle))
				_finalAngle = _angle;

			//Take movement speed into account by comparing it to 'maximumMovementSpeed';
			_finalAngle *= Mathf.InverseLerp(0f, maximumMovementSpeed, _velocity.magnitude);
			
			RotateCamera(_finalAngle, 0f);
		}	
	}
}
