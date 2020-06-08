using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF
{
	//This script provides simple mouse cursor locking functionality;
	public class MouseCursorLock : MonoBehaviour {

		//Whether to lock the mouse cursor at the start of the game;
		public bool lockCursorAtGameStart = true;

		//Key used to unlock mouse cursor;
		public KeyCode unlockKeyCode = KeyCode.Escape;

		//Key used to lock mouse cursor;
		public KeyCode lockKeyCode = KeyCode.Mouse0;

		//Start;
		void Start () {

			if(lockCursorAtGameStart)
				Cursor.lockState = CursorLockMode.Locked;
		}
		
		//Update;
		void Update () {

			if(Input.GetKeyDown(unlockKeyCode))
				Cursor.lockState = CursorLockMode.None;

			if(Input.GetKeyDown(lockKeyCode))
				Cursor.lockState = CursorLockMode.Locked;
		}
	}
}
