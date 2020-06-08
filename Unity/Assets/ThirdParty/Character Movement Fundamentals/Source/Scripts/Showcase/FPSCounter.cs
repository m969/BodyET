using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF
{
	//This script calculates the average framerate and displays it in the upper right corner of the screen;
	public class FPSCounter : MonoBehaviour {

		//Framerate is calculated using this interval;
		public float checkInterval = 1f;

		//Variables to keep track of passed time and frames;
		int currentPassedFrames = 0;
		float currentPassedTime = 0f;

		//Current framerate;
		public float currentFrameRate = 0f;
		string currentFrameRateString = "";
		
		// Update;
		void Update () {

			//Increment passed frames;
			currentPassedFrames ++;

			//Increment passed time;
			currentPassedTime += Time.deltaTime;

			//If passed time has reached 'checkInterval', recalculate framerate;
			if(currentPassedTime >= checkInterval)
			{
				//Calculate frame rate;
				currentFrameRate = (float)currentPassedFrames/currentPassedTime;

				//Reset counters;
				currentPassedTime = 0f;
				currentPassedFrames = 0;

				//Clamp to two digits behind comma;
				currentFrameRate *= 100f;
				currentFrameRate = (int)currentFrameRate;
				currentFrameRate /= 100f;

				//Calculate framerate string to display later;
				currentFrameRateString = currentFrameRate.ToString();
			}
		}

		//Render framerate in the upper right corner of the screen;
		void OnGUI()
		{
			GUI.contentColor = Color.black;

			float labelSize = 40f;
			float offset = 2f;

			GUI.Label(new Rect(Screen.width - labelSize + offset, offset, labelSize, 30f), currentFrameRateString);

			GUI.contentColor = Color.white;

			GUI.Label(new Rect(Screen.width - labelSize, 0f, labelSize, 30f), currentFrameRateString);
		}
	}
}