using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour {

	public AudioSource ambientTrack;
	public AudioSource gameMainTrack;
	public AudioSource gameGoodTrack;
	public AudioSource gameBadTrack;
	public PlayerMetrics playerMetrics;
	// Use this for initialization

	static TrackManager _instance;
	public static TrackManager instance 
	{
		get { return _instance; }
	}
	void Start () {
		_instance = this;

		if (ambientTrack == null ||
			gameMainTrack == null ||
			gameGoodTrack == null ||
			gameBadTrack == null)
		{
			Debug.LogError("Sound track not assigned!");
		}

		ambientTrack.Play();
	}

	public void StartGameMusic()
	{
		gameMainTrack.Play();

		gameGoodTrack.volume = 0;
		gameGoodTrack.Play();
		gameBadTrack.volume= 0;
		gameBadTrack.Play();
	}

	public void StopGameMusic()
	{
		gameMainTrack.Stop();
		gameGoodTrack.Stop();
		gameBadTrack.Stop();
	}
	
	// Update is called once per frame
	void Update () {
		
		float score = 0;
		if (playerMetrics == null)
		{
			playerMetrics = PlayerMetrics.instance;
		}
		
		if (playerMetrics != null)
		{
			score = playerMetrics.IsDoingGreat();

			gameGoodTrack.volume = Mathf.Max(0, score);
			gameBadTrack.volume = Mathf.Min(0, score) * -1.0f;
		}
	}
}
