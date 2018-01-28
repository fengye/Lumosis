using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!MenuController.instance.isMenuActive) return;
		if (MenuController.instance.currentMenuState != MenuController.MenuState.MAIN_MENU) return;

		if (Input.GetMouseButtonDown(0))
		{
			GameController.instance.controllerType = PlayerInteractor.ControllerType.MOUSE;
			GameController.instance.StartGame();
		}

		if (Input.GetButtonDown("GamepadBoost"))
		{
			GameController.instance.controllerType = PlayerInteractor.ControllerType.GAMEPAD;
			GameController.instance.StartGame();
		}
	}
}
