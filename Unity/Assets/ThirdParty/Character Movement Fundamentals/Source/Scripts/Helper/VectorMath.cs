using UnityEngine;
using System.Collections;

namespace CMF
{
	//This is a static helper class that offers various methods for calculating and modifying vectors (as well as float values);
	public static class VectorMath {

		//Calculate signed angle (ranging from -180 to +180) between '_vector_1' and '_vector_2';
		public static float GetAngle(Vector3 _vector1, Vector3 _vector2, Vector3 _planeNormal)
		{
			//Calculate angle and sign;
			float _angle = Vector3.Angle(_vector1,_vector2);
			float _sign = Mathf.Sign(Vector3.Dot(_planeNormal,Vector3.Cross(_vector1,_vector2)));
			
			//Combine angle and sign;
			float _signedAngle = _angle * _sign;

			return _signedAngle;
		}

		//Returns the length of the part of a vector that points in the same direction as '_direction' (i.e., the dot product);
		public static float GetDotProduct(Vector3 _vector, Vector3 _direction)
		{
			//Normalize vector if necessary;
			if(_direction.sqrMagnitude != 1)
				_direction.Normalize();
				
			float _length = Vector3.Dot(_vector, _direction);

			return _length;
		}
		
		//Remove all parts from a vector that are pointing in the same direction as '_direction';
		public static Vector3 RemoveDotVector(Vector3 _vector, Vector3 _direction)
		{
			//Normalize vector if necessary;
			if(_direction.sqrMagnitude != 1)
				_direction.Normalize();
			
			float _amount = Vector3.Dot(_vector, _direction);
			
			_vector -= _direction * _amount;
			
			return _vector;
		}
		
		//Extract and return parts from a vector that are pointing in the same direction as '_direction';
		public static Vector3 ExtractDotVector(Vector3 _vector, Vector3 _direction)
		{
			//Normalize vector if necessary;
			if(_direction.sqrMagnitude != 1)
				_direction.Normalize();
			
			float _amount = Vector3.Dot(_vector, _direction);
			
			return _direction * _amount;
		}

		//Rotate a vector onto a plane defined by '_planeNormal'; 
		public static Vector3 RotateVectorOntoPlane(Vector3 _vector, Vector3 _planeNormal, Vector3 _upDirection)
		{
			//Calculate rotation;
			Quaternion _rotation = Quaternion.FromToRotation(_upDirection, _planeNormal);

			//Apply rotation to vector;
			_vector = _rotation * _vector;
			
			return _vector;
		}

		//Project a point onto a line defined by '_lineStartPosition' and '_lineDirection';
		public static Vector3 ProjectPointOntoLine(Vector3 _lineStartPosition, Vector3 _lineDirection, Vector3 _point)
		{		
			//Caclculate vector pointing from '_lineStartPosition' to '_point';
			Vector3 _projectLine = _point - _lineStartPosition;
	
			float dotProduct = Vector3.Dot(_projectLine, _lineDirection);
	
			return _lineStartPosition + _lineDirection * dotProduct;
		}

		//Increments a float value from '_currentValue' toward '_targetValue', using '_speed' and '_deltaTime';
		public static float IncrementValueTowardTarget(float _currentValue, float _speed, float _deltaTime, float _targetValue)
		{
			if(_currentValue == _targetValue)
				return _currentValue;

			//Calculate sign and remaining distance;
			float _sign = Mathf.Sign(_targetValue - _currentValue);
			float _remainingDistance = Mathf.Abs(_targetValue - _currentValue);

			if(Mathf.Abs(_speed * Time.deltaTime * _sign) > _remainingDistance)
				return _targetValue;
			else
				return _currentValue + _speed * Time.deltaTime * _sign;
		}

		//Increments a vector's length toward '_targetLength', using '_speed' and '_deltaTime';
		public static Vector3 IncrementVectorLengthTowardTargetLength(Vector3 _currentVector, float _speed, float _deltaTime, float _targetLength)
		{
			//Calculate length;
			float _currentLength = _currentVector.magnitude;
			Vector3 _normalizedVector = _currentVector/_currentLength;

			if(_currentLength == _targetLength)
				return _currentVector;

			//Increment length;
			float _newLength = IncrementValueTowardTarget(_currentLength, _speed, _deltaTime, _targetLength);

			return _normalizedVector * _newLength;
		}


	}
}
