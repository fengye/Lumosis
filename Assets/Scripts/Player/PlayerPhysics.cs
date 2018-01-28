using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour {

	public float speedMaximum = 80f;
	public float speedDecay = 0.25f;
	public float turnRateMinimumMultiplier = 0.1f;

    float speed = 8;

    private float speedVelocity;

    public float maximumSpeed { 
        get { 
            return speedMaximum;
        } 
    }

    public float currentSpeed { 
        get { 
            return speed;
        } 
    }

    public float turnRateMultiplier {
        get { 
            return (((1 - (currentSpeed / maximumSpeed)) * (1 - turnRateMinimumMultiplier)) + turnRateMinimumMultiplier);
        }
    }

    // Update is called once per frame
    void Update () {
        speed = Mathf.Lerp(speed, 0, speedDecay * Time.deltaTime);
        speed = Mathf.Clamp(speed, 0, speedMaximum);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void AddSpeed(float add){
        speed += add; 
    }

    public void ResetSpeed(){
        speed = 0f;
    }
}
