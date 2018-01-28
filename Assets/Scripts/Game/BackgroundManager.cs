using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BackgroundManager : MonoBehaviour {

	public int width = 1000;
	public int height = 1000;
	[Range(0.001f, 0.01f)]
	public float neabularDensity = 0.005f;

	[Range(100.0f, 1000.0f)]
	public float nebularAverageSize = 200.0f;

	[Range(30.0f, 100.0f)]
	public float nebularSizeDelta = 100.0f;

	public bool generateBackground = false;

	public GameObject[] nebularPrefabs;


	[Range(0.001f, 0.01f)]
	public float starDensity = 0.005f;

	[Range(50.0f, 200.0f)]
	public float starAverageSize = 100.0f;

	[Range(20.0f, 40.0f)]
	public float starSizeDelta = 30.0f;


	public GameObject[] starPrefabs;

	// Use this for initialization
	void Start () {
		
	}

	public void RandomiseBackground()
	{
		Debug.Log("RandomiseBackground");

		// remove all children
		foreach (Transform child in transform) {
			GameObject.DestroyImmediate(child.gameObject);
		}

		int count = (int)(width * height * neabularDensity / nebularAverageSize);
		Debug.Log(count);
		for(int i = 0; i < count; ++i)
		{
			int idx = Random.Range(0, nebularPrefabs.Length);
			GameObject nebular = Instantiate(nebularPrefabs[idx]);
			nebular.transform.parent = this.transform;

			float randomSize = nebularAverageSize + Random.Range(-nebularSizeDelta, nebularSizeDelta);
			nebular.transform.localScale = new Vector3(randomSize, randomSize, randomSize);

			float randomX = Random.Range(width * -0.5f, width * 0.5f);
			float randomY = Random.Range(height * -0.5f, height * 0.5f);
			nebular.transform.localPosition = new Vector3(randomX, 0, randomY);

			nebular.transform.localRotation = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.up);
		}

		count = (int)(width * height * starDensity / starAverageSize);
		Debug.Log(count);
		for(int i = 0; i < count; ++i)
		{
			int idx = Random.Range(0, starPrefabs.Length);
			GameObject star = Instantiate(starPrefabs[idx]);
			star.transform.parent = this.transform;

			float randomSize = starAverageSize + Random.Range(-starSizeDelta, starSizeDelta);
			star.transform.localScale = new Vector3(randomSize, randomSize, randomSize);

			float randomX = Random.Range(width * -0.5f, width * 0.5f);
			float randomY = Random.Range(height * -0.5f, height * 0.5f);
			star.transform.localPosition = new Vector3(randomX, 0, randomY);

			star.transform.localRotation = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.up);
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, 1.0f, height));
	}
	
	// Update is called once per frame

	void Update () {

		if (generateBackground)
		{
			RandomiseBackground();
			generateBackground = false;
		}
		
	}
}
