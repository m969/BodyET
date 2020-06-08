using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF
{
    //This abstract camera input class serves as a base for all other camera input classes;
    //The 'CameraController' component will access this script at runtime to get input for the camera's rotation;
    //By extending this class, it is possible to implement custom camera input;
    public abstract class CameraInput : MonoBehaviour
    {
        public abstract float GetHorizontalCameraInput();
        public abstract float GetVerticalCameraInput();
    }
}
