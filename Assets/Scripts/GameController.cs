using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController instance;

    public GameObject selectedLevel;
    public GameObject activeLevel;

    public GameObject playerPrefab;
    public GameObject activePlayer;

	public PlayerInteractor.ControllerType controllerType = PlayerInteractor.ControllerType.MOUSE;

    public void Awake(){
        if (instance != null) Destroy(instance);
        instance = this;
    }

    public void Start(){
        ClearGame();
        TrackManager.instance.StartGameMusic();
    }

	public void StartGame(){
        GameObject player = Instantiate(playerPrefab);
        player.transform.position = new Vector3(0, 0, -4f);
        player.gameObject.SetActive(true);
        MenuController.instance.HideMenu();

		player.GetComponent<PlayerInteractor> ().controllerType = controllerType;

        activePlayer = player;
    }

    public void EndGame(){
        MenuController.instance.ShowMenu(MenuController.MenuState.GAME_OVER);
    }

    public void ClearGame(){
        Destroy(activePlayer);
        activePlayer = null;

        if (activeLevel != null) Destroy(activeLevel);
        activeLevel = Instantiate(selectedLevel);
        activeLevel.transform.position = new Vector3(0, 0, 0f);
        activeLevel.gameObject.SetActive(true);
    }
}
