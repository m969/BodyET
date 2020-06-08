using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF
{
    //This camera input class is an example of how to get input from joysticks/gamepads using Unity's default input system;
    //It also comes with a dead zone threshold setting to bypass any unwanted joystick "jitter";
    public class CameraJoystickInput : CameraInput
    {
        //Mouse input axes;
        public string joystickHorizontalAxis = "Joystick X";
        public string joystickVerticalAxis = "Joystick Y";

        //Invert input options;
		public bool invertHorizontalInput = false;
		public bool invertVerticalInput = false;

        //If any input falls below this value, it is set to '0';
        //Use this to prevent any unwanted small movements of the joysticks ("jitter"); 
		public float deadZoneThreshold = 0.2f;

	    public override float GetHorizontalCameraInput()
        {
            //Get input;
			float _horizontalInput = Input.GetAxisRaw(joystickHorizontalAxis);

			//Set any input values below threshold to '0';
			if(Mathf.Abs(_horizontalInput) < deadZoneThreshold)
				_horizontalInput = 0f;

            //Handle inverted inputs;
            if(invertHorizontalInput)
                return _horizontalInput *= (-1f);
            else
                return _horizontalInput;
        }

        public override float GetVerticalCameraInput()
        {
            //Get input;
			float _verticalInput = Input.GetAxisRaw(joystickVerticalAxis);

			//Set any input values below threshold to '0';
			if(Mathf.Abs(_verticalInput) < deadZoneThreshold)
				_verticalInput = 0f;

            //Handle inverted inputs;
            if(invertVerticalInput)
                return _verticalInput;
            else
                return _verticalInput *= (-1f);
        }
    }
}

