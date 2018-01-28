using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCapturedMutator : NodeMutator {

	bool inCapture;
	public string name;

	GameObject captureTransfer;
	GameObject captureTransferTarget;

	public NodeCapturedMutator(string name)
	{
		this.name = name;
	}

    public void AddedToNode(Node node){
        // Renderer renderer = node.gameObject.GetComponent<Renderer>();
        // if (renderer)
        // {
        //     Material capturedMaterial = Resources.Load<Material>("CapturedNodeMaterial");
        //     renderer.material = capturedMaterial;
        // }
        if (node.remainingEnergy > 0)
        {
            inCapture = true;
        }

		GameObject player = GameObject.FindGameObjectsWithTag ("Player")[0];
		PlayerParticleTransfer transfer = player.GetComponentInChildren<PlayerParticleTransfer> ();
		transfer.setTarget(node);
    }

    public void RemovedFromNode(Node node) {
        inCapture = false;

        GameObject player = GameObject.FindGameObjectsWithTag ("Player")[0];
        PlayerParticleTransfer transfer = player.GetComponentInChildren<PlayerParticleTransfer> ();
        transfer.setTarget(null);
    }

    public void UpdateAffectedNode(Node node, float deltaTime){
    	Renderer renderer = node.gameObject.GetComponent<Renderer>();
	    if (renderer)
	    {
	    	if (inCapture)
	    	{

		    	if (node.remainingEnergy > 0)
		    	{
//                    float remaining = 1.0f - node.remainingEnergy / node.initEnergy;
//
//                    float c = remaining * 0.25f + 0.5f;
//                    renderer.material.color = new Color(c,c,c);
//                    float e = remaining * 0.25f;
//                    renderer.material.SetColor("_EmissionColor", new Color(e,e,e));
		    	}
		    	else
		    	{
                    inCapture = false;

                    node.remainingEnergy = 0;
                    float c = 0.75f;
                    renderer.material.color = new Color(c,c,c);
                    float e = 2f;
                    renderer.material.SetColor("_EmissionColor", new Color(e,e,e));

                    node.NodeCompleted();
		    	}

//		    	Debug.Log("Remaining: " + node.remainingEnergy.ToString());

		    	// Debug.Log(node.remainingEnergy);

		    }
	    }
    }
}
