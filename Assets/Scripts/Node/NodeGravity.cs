using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SphereCollider))]
public class NodeGravity : MonoBehaviour {

    SphereCollider sphereCollider;
    Vector3 velocity = Vector3.zero;

    public delegate void OnPlayerEnter(PlayerController player);
    public OnPlayerEnter onPlayerEnter;

    public delegate void OnPlayerExit(PlayerController player);
    public OnPlayerExit onPlayerExit;

    public Node parentNode;
    public Transform rippleParent;

    public bool gravityEnabled = true;
    const float innerRadiusDiff = 1.25f;
    const float outerRadiusDiff = 3.0f;

    public float innerRadius
    {
        get 
        {
            return nodeRadius + (innerRadiusDiff * (1 + (nodeRadius * 0.03f)));
        }
    }

    public float outerRadius
    {
        get
        {
            // Debug.Log("Out radius: " + (nodeRadius + outerRadiusDiff));
            return nodeRadius + (outerRadiusDiff * (1 + (nodeRadius * 0.03f)));
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
            return 6.0f;
        }
    }

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

            if (parentNode.remainingEnergy <= 0)
            {
                rippleParent.gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider other){
        PlayerController player = other.GetComponent<PlayerController>();
        if (player)
        {
            if (onPlayerEnter != null) onPlayerEnter(player);
        }

        PlayerAudio playerAudio = other.GetComponent<PlayerAudio>();
        if (playerAudio != null) {
            //playerAudio.PlayLitSfx(this.gameObject);

            PlayerMetrics.instance.MarkPoint(2, null);
        }
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
        if (physics && gravityEnabled)
        {
            Vector3 distanceVector = transform.position - other.transform.position;
            distanceVector.y = 0;
            float distance = distanceVector.magnitude;
            if (distance < innerRadius)
                distance = innerRadius;
            distance -= innerRadius;

            float force = 1 / Mathf.Pow(distance, 2);
            parentNode.remainingEnergy -= Time.deltaTime;
            if (parentNode.remainingEnergy > 0)
            {
                force = Mathf.Clamp(force, 0, mass);
                physics.AddSpeed(force * Time.deltaTime * 3);
            }

            if (IsNodeDepleted())
            {
                if (playerAudio != null) {
                    playerAudio.PlayActivateSfx(this.gameObject);
                }

                PlayerMetrics.instance.MarkPoint(10, this.gameObject);
            }
        }

        
        if (playerAudio != null) {
            playerAudio.PlaySpinSfx(this.gameObject);
        }
    }

    public bool IsNodeEverTouched()
    {
        if (parentNode != null && parentNode.remainingEnergy < parentNode.initEnergy)
        {
            return true;
        }

        return false;
    }

    public bool IsNodeDepleted()
    {
        if (parentNode != null && parentNode.remainingEnergy <= 0)
        {
            return true;
        }

        return false;
    }

    //void OnDrawGizmosSelected() {
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, outerRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, innerRadius);
    }
}

