using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Node : MonoBehaviour {

    NodeMutator _nodeMutator;
    NodeMutator nodeMutator
    {
        get { 
            return _nodeMutator;
        }
        set { 
            if (value != null)
            {
                _nodeMutator = value;
                _nodeMutator.AddedToNode(this);
            }
            else
            {
                if (_nodeMutator != null)
                {
                    _nodeMutator.RemovedFromNode(this);
                    _nodeMutator = value;
                }
            }
        }
    }

    public NodeGravity nodeGravity;

    [Range(1.0f, 50.0f)]
    public float nodeRadius = 1.0f;
    Coroutine nodeGravityEnabler;

    float _remainingEnergy;
    public float remainingEnergy
    {
        get
        {
            return _remainingEnergy;
        }
        set
        {
            _remainingEnergy = value;
        }
    }
    public float initEnergy
    {
        get 
        {
            return _initEnergy;
        }
    }
    float _initEnergy;


    void Start(){
        if (nodeGravity == null)
        {
            nodeGravity = GetComponentInChildren<NodeGravity>();
        }
        
        nodeGravity.onPlayerEnter = OnPlayerEnter;
        nodeGravity.onPlayerExit = OnPlayerExit;

        _initEnergy = Mathf.Pow(nodeRadius, 0.5f);
        remainingEnergy = _initEnergy;
        Debug.Log(remainingEnergy);
    }

	// Update is called once per frame
	void Update () {
        if (nodeMutator != null)
        {
            nodeMutator.UpdateAffectedNode(this, Time.deltaTime);
        }

        transform.localScale = new Vector3(nodeRadius * 2, nodeRadius * 2, nodeRadius * 2);
	}

    void OnPlayerEnter(PlayerController player){
        if (nodeMutator == null)
        {
            nodeMutator = new NodeCapturedMutator(this.gameObject.name + "_Mutator");
        }
    }

    void OnPlayerExit(PlayerController player){
        if (nodeMutator != null)
        {
            // Debug.Log("Node mutator removed: " + ((NodeCapturedMutator)nodeMutator).name);
            nodeMutator = null;
        }
    }

    public void NodeCompleted(){
        PlayerMetrics playerMetrics = GameObject.FindObjectOfType<PlayerMetrics>();
        playerMetrics.boostLeft += .4f;
        playerMetrics.completedPlanets++;
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
}
