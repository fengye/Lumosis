using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public TrackManager trackManager;
	// Use this for initialization
	void Start () {

		if (trackManager == null)
		{
			Debug.LogError("TrackManager is null!");
		}

		// TODO: start at a proper time
		trackManager.StartGameMusic();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
