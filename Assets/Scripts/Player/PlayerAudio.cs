using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {

	public AudioSource[] consectiveLitSfx = new AudioSource[3];
	public AudioSource[] spinSfx = new AudioSource[3];
	public AudioSource[] activateSfx = new AudioSource[9];

	float lastLitTime = -1.0f;
    int litSfxIndex = 0;
    public void PlayLitSfx(GameObject litObject)
    {
        if (Time.time - lastLitTime > 2.5f)
        {
            litSfxIndex = 0;
        }
        else
        {
            litSfxIndex = (litSfxIndex + 1) % 3;
        }


        lastLitTime = Time.time;
        NodeGravity nodeGravity = litObject.GetComponent<NodeGravity>();
        if (nodeGravity != null)
        {
            if (nodeGravity.IsNodeEverTouched())
            {
                litSfxIndex = 0;
            }
            if (nodeGravity.IsNodeDepleted())
            {
                return;
            }
        }

        consectiveLitSfx[litSfxIndex].Play();
        
        
    }

    float lastSpinTime = -1.0f;
    GameObject lastSpinObject = null;
    HashSet<GameObject> spinnedObjects = new HashSet<GameObject>();
    public void StopSpinSfx(GameObject spinObject)
    {
    	lastSpinObject = null;
    }

    public void PlaySpinSfx(GameObject spinObject)
    {
    	if (spinnedObjects.Contains(spinObject))
    		return;

    	if (lastSpinObject != spinObject)
    	{
    		lastSpinTime = Time.time;
    		lastSpinObject = spinObject;
    	}
        else
        {
            if (Time.time - lastSpinTime > 1.0)
            {
            	
            	NodeGravity nodeGravity = spinObject.GetComponent<NodeGravity>();
            	if (nodeGravity.IsNodeDepleted())
            	{
            		return;
            	}

            	int index = 0;
            	if (nodeGravity && nodeGravity.nodeRadius >= 5.0f)
            	{
            		index = 1;
            	}
            	if (nodeGravity && nodeGravity.nodeRadius >= 10.0f)
            	{
            		index = 2;
            	}

            	


            	spinSfx[index].Play();

            	lastSpinTime = Time.time;

            	spinnedObjects.Add(spinObject);
            }
        }

    }

    int activateSfxIndex = 0;
    float lastActivateTime = -1.0f;
    HashSet<GameObject> activatedObjects = new HashSet<GameObject>();
    public void PlayActivateSfx(GameObject activeObject)
    {
    	if (activatedObjects.Contains(activeObject))
    	{
    		return;
    	}

    	/*
    	NodeGravity nodeGravity = activeObject.GetComponent<NodeGravity>();
    	int index = 0;
    	if (nodeGravity && nodeGravity.nodeRadius >= 5.0f)
    	{
    		index = 1;
    	}
    	if (nodeGravity && nodeGravity.nodeRadius >= 10.0f)
    	{
    		index = 2;
    	}

    	activateSfx[index].Play();
    	*/

    	if (Time.time - lastActivateTime > 5.0f)
        {
            activateSfxIndex = 0;

            Debug.Log("Reset activate sound");
        }
        else
        {
            activateSfxIndex = (activateSfxIndex + 1) % 9;
        }


        lastActivateTime = Time.time;
        NodeGravity nodeGravity = activeObject.GetComponent<NodeGravity>();
        if (nodeGravity != null)
        {
            if (nodeGravity.IsNodeEverTouched())
            {
                return;
            }
            if (nodeGravity.IsNodeDepleted())
            {
                return;
            }
        }

        activateSfx[activateSfxIndex].Play();

    	activatedObjects.Add(activeObject);


    }
}
