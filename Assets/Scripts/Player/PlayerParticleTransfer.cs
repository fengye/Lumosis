using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleTransfer : MonoBehaviour {
    
	public GameObject prefab;

	Node target;
	GameObject current;

	void Start(){
	}

	void Update(){
        if(target == null) return;

        if (target.remainingEnergy <= 0)
        {
            RemoveCurrent();
        }
	}

	public void setTarget(Node newTarget){
		if (target != newTarget) {
            RemoveCurrent();

            if (newTarget != null) {
				current = Instantiate (prefab);
				current.GetComponent<particleAttractorLinear> ().target = newTarget.transform;
				current.transform.SetParent (gameObject.transform);
				current.transform.localPosition = Vector3.zero;
			}
		}
		target = newTarget;
	}

    public void RemoveCurrent(){
        if (current != null) {
            ParticleSystem particleSystem = current.GetComponent<ParticleSystem> ();
            ParticleSystem.EmissionModule emission = particleSystem.emission;
            emission.enabled = false;
            GameObject.Destroy (current, 2.0f);
            current = null;
        }
    }

}
