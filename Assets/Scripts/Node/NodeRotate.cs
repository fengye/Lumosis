using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeRotate : MonoBehaviour {


    Vector3 axis;
    float rotationAmount;

	// Use this for initialization
	void Start () {
        transform.rotation = Random.rotation;
        axis = Random.onUnitSphere;
        rotationAmount = Random.Range(1, 4f);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(axis, rotationAmount * Time.deltaTime);
	}
}
