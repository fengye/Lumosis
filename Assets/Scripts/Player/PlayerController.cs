using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerPhysics))]
[RequireComponent(typeof(PlayerInteractor))]
[RequireComponent(typeof(PlayerMetrics))]
public class PlayerController : MonoBehaviour {

	const bool boostEnabled = false;

	public float baseTurnRate = 0.25f;
    private Vector3 velocity = Vector3.zero;

    PlayerPhysics physics;
    PlayerInteractor interactor;
    PlayerMetrics metrics;

    ParticleSystem system;
    TrailRenderer trail;

    bool finished = false;
    float createdTime = 0f;

    public PlayerPowerup powerup;

    // Use this for initialization
    void Start () {
        physics = GetComponent<PlayerPhysics>();
        interactor = GetComponent<PlayerInteractor>();
        metrics = GetComponent<PlayerMetrics>();

        system = GetComponentInChildren<ParticleSystem>();
        trail = GetComponentInChildren<TrailRenderer>();

        createdTime = Time.time;
    }

	// Update is called once per frame
    void Update () {
        if (Time.time - createdTime < 0.2f) return;
		if (finished) return;

        if (physics.currentSpeed < 3 && !finished){
            metrics.boostLeft = 0.0f;
            physics.ResetSpeed();
            GameController.instance.EndGame();
            finished = true;
        }

        transform.forward = Vector3.SmoothDamp (transform.forward, interactor.getInputForwardDirection(), ref velocity, baseTurnRate * physics.turnRateMultiplier);


        float scale = physics.currentSpeed;
        ParticleSystem.ShapeModule shape = system.shape;
        shape.scale = new Vector3(2, 0, scale);
        system.transform.localPosition = (Vector3.forward * -scale) * 0.75f;

		trail.time = 0.1f + (physics.currentSpeed / physics.maximumSpeed) * 2f;

		ParticleSystem.EmissionModule emission = system.emission;
		emission.rateOverTimeMultiplier = scale * 0.5f;

		if (interactor.getInputBoosting() && boostEnabled)
        {
            float boostAmount = Time.deltaTime * 20.0f;
            if (metrics.boostLeft > Time.deltaTime)
            {
                metrics.boostLeft -= Time.deltaTime;
                physics.AddSpeed(boostAmount);
            }
            else if (metrics.boostLeft > 0)
            {
                metrics.boostLeft = 0;
                physics.AddSpeed(boostAmount);
            }
            else
            {
                metrics.boostLeft = 0;
            }
        }
	}
}
