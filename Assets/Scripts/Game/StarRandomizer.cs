using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class StarRandomizer : MonoBehaviour {

	float initIntensity;

	MeshRenderer meshRenderer;
	float randomSeed;

	// Use this for initialization
	void Start () {

		meshRenderer = GetComponent<MeshRenderer>();
		// only works in Particles/Additive Intensify shader
		initIntensity = Random.Range(0.3f, 1.0f);
		meshRenderer.materials[0].SetFloat("_Glow", initIntensity);

		randomSeed = Random.RandomRange(0.0f, 100.0f);
	}

	// Update is called once per frame
	void Update () {
		
		meshRenderer.materials[0].SetFloat("_Glow", initIntensity + Perlin.Noise(Time.time * 0.15f + randomSeed) * 0.35f);
	}
}
