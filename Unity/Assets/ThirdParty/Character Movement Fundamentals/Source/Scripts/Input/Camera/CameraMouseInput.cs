using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF
{
    //This camera input class is an example of how to get input from a connected mouse using Unity's default input system;
    //It also includes an optional mouse sensitivity setting;
    public class CameraMouseInput : CameraInput
    {
        //Mouse input axes;
        public string mouseHorizontalAxis = "Mouse X";
        public string mouseVerticalAxis = "Mouse Y";

        //Invert input options;
		public bool invertHorizontalInput = false;
		public bool invertVerticalInput = false;

        //Use this sensitivity setting to fine-tune mouse movement;
        //Mouse input will be multiplied by this value;
        public float mouseSensitivity = 1.0f;

	    public override float GetHorizontalCameraInput()
        {
            //Return mouse input based on mouse sensitivity and invert input setting;
            if(invertHorizontalInput)
                return -Input.GetAxisRaw(mouseHorizontalAxis) * mouseSensitivity;
            else
                return Input.GetAxisRaw(mouseHorizontalAxis) * mouseSensitivity;
        }

        public override float GetVerticalCameraInput()
        {
            //Return mouse input based on mouse sensitivity and invert input setting;
            if(invertVerticalInput)
                return Input.GetAxisRaw(mouseVerticalAxis) * mouseSensitivity;
            else
                return -Input.GetAxisRaw(mouseVerticalAxis) * mouseSensitivity;
        }
    }
}
