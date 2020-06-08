using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CMF
{
	//This script rotates all gameobjects inside the attached trigger collider around a central axis (the forward axis of this gameobject);
	//In combination with a tube-shaped collider, this script can be used to let a player walk around on the inside walls of a tunnel;
	public class GravityTunnel : MonoBehaviour {

		//List of rigidbodies inside the attached trigger;
		List<Rigidbody> rigidbodies = new List<Rigidbody>();

		void FixedUpdate ()
		{
			for(int i = 0; i < rigidbodies.Count; i++)
			{
				//Calculate center position based on rigidbody position;
				Vector3 _center = 
					Vector3.Project((rigidbodies[i].transform.position - transform.position) ,((transform.position + transform.forward) - transform.position)) + transform.position;
				
				RotateRigidbody(rigidbodies[i].transform, (_center - rigidbodies[i].transform.position).normalized);
			}
		}

		void OnTriggerEnter(Collider col)
		{
			Rigidbody _rigidbody = col.GetComponent<Rigidbody>();
			if(!_rigidbody)
				return;
			
			//Make sure that the entering collider is actually a character;
			if(col.GetComponent<Mover>() == null)
				return;

			rigidbodies.Add(_rigidbody);
		}

		void OnTriggerExit(Collider col)
		{
			Rigidbody _rigidbody = col.GetComponent<Rigidbody>();
			if(!_rigidbody)
				return;

			//Make sure that the leaving collider is actually a character;
			if(col.GetComponent<Mover>() == null)
				return;

			rigidbodies.Remove(_rigidbody);

			RotateRigidbody(_rigidbody.transform, Vector3.up);

			//Reset rigidbody rotation;
			Vector3 _eulerAngles = _rigidbody.rotation.eulerAngles;

			_eulerAngles.z = 0f;
			_eulerAngles.x = 0f;

			_rigidbody.MoveRotation(Quaternion.Euler(_eulerAngles));
		}

		void RotateRigidbody(Transform _transform, Vector3 _targetDirection)
		{
			//Get rigidbody component of transform;
			Rigidbody _rigidbody = _transform.GetComponent<Rigidbody>();
			
			_targetDirection.Normalize();

			//Calculate rotation difference;
			Quaternion _rotationDifference = Quaternion.FromToRotation(_transform.up, _targetDirection);

			//Save start and end rotation;
			Quaternion _startRotation = _transform.rotation;
			Quaternion _endRotation = _rotationDifference * _transform.rotation;

			//Rotate rigidbody;
			_rigidbody.MoveRotation(_endRotation);
		}

		//Calculate a counter rotation from a rotation;
		Quaternion GetCounterRotation(Quaternion _rotation)
		{
			Vector3 _axis;
			float _angle;

			_rotation.ToAngleAxis(out _angle, out _axis);
			Quaternion _rotationAdd = Quaternion.AngleAxis( Mathf.Sign(_angle) * 180f, _axis);

			return _rotation * Quaternion.Inverse(_rotationAdd);
		}
	}
}
