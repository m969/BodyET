using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace CMF
{
	//This script is used in the showcase scene to control the controller selection/settings menu;
	public class DemoMenu : MonoBehaviour {

		//The menu is shown/hidden using this key;
		public KeyCode menuKey = KeyCode.C;

		//References to secondary components;
		DisableShadows disableShadows;
		FPSCounter fpsCounter;

		//Reference to game object containing the demo menu UI;
		public GameObject demoMenuObject;

		//List of controllers to choose from;
		public List<GameObject> controllers = new List<GameObject>();
		//List of controller selection buttons in the menu UI;
		public List<Button> buttons = new List<Button>();

		//Toggle in the menu UI to control scene shadows;
		public Toggle shadowToggle;

		//Reference to the two separate areas in the showcase scene;
		public GameObject regularArea;
		public GameObject topDownArea;

		//Color of currently selected controller prefab button;
		public Color activeButtonColor = Color.cyan;

		void Start () {

			//Get references;
			disableShadows = GetComponent<DisableShadows>();
			fpsCounter = GetComponent<FPSCounter>();

			//Hide menu;
			SetMenuEnabled(false);

			//Enable/Disable shadows based on last player selection;
			disableShadows.SetShadows(PlayerData.enableShadows);
			shadowToggle.isOn = PlayerData.enableShadows;

			//Deactivate all controller presets in the scene;
			for(int i = 0; i < controllers.Count; i++)
			{
				controllers[i].SetActive(false);
			}

			//Activate the correct controller preset based on the preset index;
			controllers[PlayerData.controllerIndex].SetActive(true);

			//Activate the correct level area based on preset index;
			if(PlayerData.controllerIndex >= 4)
			{
				regularArea.SetActive(false);
			}
			else
			{
				topDownArea.SetActive(false);
			}

			//Colorize the correct button based on controller index;
			ColorBlock c = buttons[PlayerData.controllerIndex].colors;
			c.normalColor = activeButtonColor;
			c.highlightedColor = activeButtonColor;
			c.pressedColor = activeButtonColor;
			buttons[PlayerData.controllerIndex].colors = c;
			
		}
		
		// Update is called once per frame
		void Update () {

			//Hide/show demo menu;
			if(Input.GetKeyDown(menuKey))
			{
				SetMenuEnabled(!demoMenuObject.activeSelf);
			}

			//If scene was built as a Windows executable, also hide/show demo menu when 'Escape' is pressed;
			#if UNITY_STANDALONE_WIN
			if(Input.GetKeyDown(KeyCode.Escape))
			{
				SetMenuEnabled(!demoMenuObject.activeSelf);
			}
			#endif

			//If left mouse button is pressed and the menu is hidden, lock cursor;
			if(Input.GetMouseButtonDown(0) && !demoMenuObject.activeSelf)
				Cursor.lockState = CursorLockMode.Locked;
		}

		//Reload scene;
		public void RestartScene()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		//Quit game;
		public void QuitGame()
		{
			Application.Quit();
		}	

		//This event is called whenever a new controller preset is chosen in the menu;
		public void OnControllerPresetChosen(int _presetIndex)
		{
			//Save new preset index;
			PlayerData.controllerIndex = _presetIndex;

			//Restart scene;
			RestartScene();
		}

		//Hide/show menu;
		public void SetMenuEnabled(bool _isEnabled)
		{
			demoMenuObject.SetActive(_isEnabled);
			if(_isEnabled)
				Cursor.lockState = CursorLockMode.None;
			else
				Cursor.lockState = CursorLockMode.Locked;
		}

		//Enable/disable scene shadows;
		public void SetShadowsEnabled(bool _isEnabled)
		{
			disableShadows.SetShadows(_isEnabled);
			PlayerData.enableShadows = _isEnabled;
		}

		//Show/Hide framerate counter;
		public void SetFrameRateEnabled(bool _isEnabled)
		{
			fpsCounter.enabled = _isEnabled;
		}
	}
}
