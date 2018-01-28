using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SunNode : MonoBehaviour {

	public SunNodeGravity nodeGravity;

	public enum SunState
	{
		DISABLED,
		ENABLED,
		DORMANT
	}
	public SunState sunState;
	bool completed = false;

    [Range(1.0f, 50.0f)]
    public float nodeRadius = 1.0f;
    Coroutine nodeGravityEnabler;

	ParticleSystem system;

	public enum SunType
	{
		YELLOW,
		RED,
		BLUE
	}
	public SunType sunType = SunType.YELLOW;


    void Start(){
        if (nodeGravity == null)
        {
			nodeGravity = GetComponentInChildren<SunNodeGravity>();
        }

		system = GetComponentInChildren<ParticleSystem> ();
        
        nodeGravity.onPlayerEnter = OnPlayerEnter;
        nodeGravity.onPlayerExit = OnPlayerExit;
    }

	// Update is called once per frame
	void Update () {
        transform.localScale = new Vector3(nodeRadius * 2, nodeRadius * 2, nodeRadius * 2);
	}

    void OnPlayerEnter(PlayerController player){
		if (sunState == SunState.DISABLED && !completed) {
			completed = true;
			sunState = SunState.ENABLED;
			StartCoroutine(SetNodeEnabled ());
		}
    }

    void OnPlayerExit(PlayerController player){
		
    }

    void OnCollisionEnter(Collision collision){
        PlayerPhysics physics = collision.gameObject.GetComponent<PlayerPhysics>();
        if (physics)
        {
            physics.ResetSpeed();
            physics.AddSpeed(3);

            PlayerMetrics.instance.MarkPoint(-5, null);
        }

        if(nodeGravityEnabler != null) StopCoroutine(nodeGravityEnabler);
        nodeGravity.gravityEnabled = false;
    }

    void OnCollisionExit(Collision collision){
        PlayerPhysics physics = collision.gameObject.GetComponent<PlayerPhysics>();
        if (physics)
        {
            physics.ResetSpeed();
            physics.AddSpeed(3);
        }

        if(nodeGravityEnabler != null) StopCoroutine(nodeGravityEnabler);
        nodeGravityEnabler = StartCoroutine(EnableNodeGravity());
    }

    IEnumerator EnableNodeGravity(){
        yield return new WaitForSeconds(0.2f);
        nodeGravity.gravityEnabled = true;
    }

	IEnumerator SetNodeEnabled(){
		PlayerAudio audio = GameController.instance.activePlayer.GetComponent<PlayerAudio> ();
		PlayerMetrics metrics = GameController.instance.activePlayer.GetComponent<PlayerMetrics> ();
		metrics.completedPlanets++;
		metrics.boostLeft += 1.0f;

		sunState = SunState.ENABLED;
		audio.PlayActivateSfx (gameObject);

		Renderer renderer = GetComponent<Renderer> ();
		float c = renderer.material.color.r;
		float e = renderer.material.GetColor("_EmissionColor").r;
		ParticleSystem.EmissionModule emission = system.emission;
		emission.rateOverTime = 25 * nodeRadius;

		while ( e < 2 || c < 1f){
			c = Mathf.Lerp (c, 1.01f, Time.deltaTime * 5f);
			e = Mathf.Lerp (e, 2.02f, Time.deltaTime * 5f);

			renderer.material.color = new Color(c,c,c);
			renderer.material.SetColor("_EmissionColor", new Color(e,e,e));
			yield return null;
		}

		c = 1f;
		e = 2f;
		renderer.material.color = new Color(c,c,c);
		renderer.material.SetColor("_EmissionColor", new Color(e,e,e));

		float timeoutTime = 0;
		if(sunType == SunType.YELLOW) {
			timeoutTime = 1f * Mathf.Pow (nodeRadius, 0.72f);
		}
		else if(sunType == SunType.BLUE ){
			timeoutTime = 0.3f * Mathf.Pow (nodeRadius, 0.72f);
		}
		else if(sunType == SunType.RED){
			timeoutTime = 3f * Mathf.Pow (nodeRadius, 0.72f);
		}

		yield return new WaitForSeconds(timeoutTime);
		sunState = SunState.DORMANT;

		emission = system.emission;
		emission.rateOverTime = 5 * nodeRadius;

		while ( e > 1 || c > 0.9f){
			c = Mathf.Lerp (c, 0.89f, Time.deltaTime * 4f);
			e = Mathf.Lerp (e, 0.89f, Time.deltaTime * 4f);

			renderer.material.color = new Color(c,c,c);
			renderer.material.SetColor("_EmissionColor", new Color(e,e,e));
			yield return null;
		}

		c = 0.9f;
		e = 1f;
		renderer.material.color = new Color(c,c,c);
		renderer.material.SetColor("_EmissionColor", new Color(e,e,e));
	}

}
