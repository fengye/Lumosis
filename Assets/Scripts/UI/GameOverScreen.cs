using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour {

	public int maximumStars;
	public RectTransform targetProgress; 
	public Text targetProgressText; 

	// Update is called once per frame
	void Update () {

		if (!MenuController.instance.isMenuActive) return;
		if (MenuController.instance.currentMenuState != MenuController.MenuState.GAME_OVER) return;

		PlayerMetrics metrics = GameController.instance.activePlayer.GetComponent<PlayerMetrics> ();
		if(metrics!=null){
			targetProgress.localScale = new Vector3 ((float) metrics.completedPlanets / (float) maximumStars, 1f, 1f);
			Debug.Log ((float)metrics.completedPlanets / (float)maximumStars);

			targetProgressText.text = ((int)((float)metrics.completedPlanets / (float)maximumStars)) + "%";
		}
	}
}
