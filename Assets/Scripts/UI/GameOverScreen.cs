using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour {

	int maximumStars = 0;
	public RectTransform targetProgress; 
	public Text targetProgressText; 


	// Update is called once per frame
	void Update () {
		if (!MenuController.instance.isMenuActive) return;
		if (MenuController.instance.currentMenuState != MenuController.MenuState.GAME_OVER) {
			maximumStars = 0;
			return;
		}

		if (maximumStars == 0) {
			maximumStars = GameObject.FindObjectsOfType<SunNode> ().Length;
		}

		PlayerMetrics metrics = GameController.instance.activePlayer.GetComponent<PlayerMetrics> ();
		if(metrics!=null){
			targetProgress.localScale = new Vector3 ((float)metrics.completedPlanets / (float)maximumStars, 1f);

			targetProgressText.text = ((int) (((float) metrics.completedPlanets / (float)maximumStars) * 100)) + "%";
		}
	}
}
