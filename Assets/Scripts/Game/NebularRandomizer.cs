using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class NebularRandomizer : MonoBehaviour {


	Vector3 initPosition;
	Vector3 driftVel;
	float initIntensity;

	MeshRenderer meshRenderer;
	float randomSeed;

	// Use this for initialization
	void Start () {

		meshRenderer = GetComponent<MeshRenderer>();
		// only works in Particles/Additive Intensify shader
		initIntensity = Random.Range(0.6f, 1.3f);
		meshRenderer.materials[0].SetFloat("_Glow", initIntensity);

		initPosition = transform.position;

		randomSeed = Random.RandomRange(0.0f, 100.0f);

		RandomizeDriftVelocity();
	}

	void RandomizeDriftVelocity()
	{
		driftVel = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
		driftVel.Normalize();
	}
	
	// Update is called once per frame
	void Update () {
		
		meshRenderer.materials[0].SetFloat("_Glow", initIntensity + Perlin.Noise(Time.time * 0.15f + randomSeed) * 0.25f);

		if ((transform.position - initPosition).magnitude > 10.0f)
		{
			RandomizeDriftVelocity();
			Vector3 diff =(transform.position - initPosition);
			diff.Normalize();
			diff *= 9.99f;
			transform.position = initPosition + diff;
		}
		else
		{
			transform.position += driftVel * Time.deltaTime;
		}
	}
}
