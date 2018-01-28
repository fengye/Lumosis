using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SphereCollider))]
public class SunNodeGravity : MonoBehaviour {

    SphereCollider sphereCollider;
    Vector3 velocity = Vector3.zero;

    public delegate void OnPlayerEnter(PlayerController player);
    public OnPlayerEnter onPlayerEnter;

    public delegate void OnPlayerExit(PlayerController player);
    public OnPlayerExit onPlayerExit;

	public SunNode parentNode;
    public Transform rippleParent;

    public bool gravityEnabled = true;
    const float innerRadiusDiff = 1.5f;
    const float outerRadiusDiff = 3.5f;


    public float innerRadius
    {
        get 
        {
            return nodeRadius + (innerRadiusDiff * (1 + (nodeRadius * 0.06f)));
        }
    }

    public float outerRadius
    {
        get
        {
            // Debug.Log("Out radius: " + (nodeRadius + outerRadiusDiff));
            return nodeRadius + (outerRadiusDiff * (1 + (nodeRadius * 0.06f)));
        }
    }

    public float nodeRadius
    {
        get 
        {
            return parentNode.nodeRadius;
        }
    }

    public float mass
    {
        get 
        {
			if (parentNode.sunType == SunNode.SunType.YELLOW) {
				return 6.0f;
			}
			if (parentNode.sunType == SunNode.SunType.BLUE) {
				return 15.0f;
			}
			if (parentNode.sunType == SunNode.SunType.RED) {
				return 4.0f;
			}
            return 6.0f;
        }
    }

	float enterTime = 0;

    // Use this for initialization
    void Start () {
        sphereCollider = GetComponent<SphereCollider>();
    }

    void Update(){
        float divide = ((transform.lossyScale.x + transform.lossyScale.y + transform.lossyScale.z) * 0.3333f);
        if (divide > 0) // prevent inf.
        {
            sphereCollider.radius = outerRadius / divide;
        }

        if (rippleParent != null)
        {
            float parentScale = 2.0f * outerRadius / divide;
            rippleParent.transform.localScale = new Vector3(parentScale, parentScale, parentScale);
        }
    }

    void OnTriggerEnter(Collider other){
        PlayerController player = other.GetComponent<PlayerController>();
        if (player)
        {
            if (onPlayerEnter != null) onPlayerEnter(player);
        }

        // PlayerAudio playerAudio = other.GetComponent<PlayerAudio>();
        // if (playerAudio != null) {
        //     playerAudio.PlayLitSfx(this.gameObject);
        // }

        PlayerMetrics.instance.MarkPoint(2, null);

		enterTime = Time.time;
    }

    void OnTriggerExit(Collider other){
        PlayerController player = other.GetComponent<PlayerController>();
        if (player)
        {
            if (onPlayerExit != null) onPlayerExit(player);
        }

        PlayerAudio playerAudio = other.GetComponent<PlayerAudio>();
        if (playerAudio != null) {
            playerAudio.StopSpinSfx(this.gameObject);
        }
    }

    void OnTriggerStay(Collider other){
        PlayerPhysics physics = other.GetComponent<PlayerPhysics>();
        PlayerAudio playerAudio = other.GetComponent<PlayerAudio>();
		if (physics && gravityEnabled && parentNode.sunState == SunNode.SunState.ENABLED)
		{
			float sunTypeMultiplier = 1f;
			float timeMultiplier = 1f;
			if(parentNode.sunType == SunNode.SunType.YELLOW) {
				sunTypeMultiplier = 1f;
			}
			else if(parentNode.sunType == SunNode.SunType.BLUE ){
				sunTypeMultiplier = 10;
			}
			else if(parentNode.sunType == SunNode.SunType.RED){
				sunTypeMultiplier = 0.6f;
				timeMultiplier = 2f;
			}


            Vector3 distanceVector = transform.position - other.transform.position;
            distanceVector.y = 0;
            float distance = distanceVector.magnitude;
            if (distance < innerRadius)
                distance = innerRadius;
            distance -= innerRadius;

            float force = 1 / Mathf.Pow(distance, 2);
			if (parentNode.sunState == SunNode.SunState.ENABLED)
            {
				force *= 1 + (((Time.time - enterTime) / 4) * timeMultiplier);
				force *= 1 + (parentNode.nodeRadius / 3);
				force *= 0.05f;
				force *= sunTypeMultiplier;
                force = Mathf.Clamp(force, 0, mass);
				physics.AddSpeed(force * Time.deltaTime);
            }

            if (IsNodeDepleted())
            {
                if (playerAudio != null) {
                    playerAudio.PlayActivateSfx(this.gameObject);
                }

                PlayerMetrics.instance.MarkPoint(10, this.gameObject);
            }
        }
    }

    public bool IsNodeDepleted()
    {
		return parentNode.sunState == SunNode.SunState.DORMANT;
    }

    //void OnDrawGizmosSelected() {
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, outerRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, innerRadius);
    }
}

