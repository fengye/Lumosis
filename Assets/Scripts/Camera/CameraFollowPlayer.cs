using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollowPlayer : MonoBehaviour {
    
    public GameObject defaultTarget;

    public float fovAddition;
    private float baseFov;

    public float heightAddition;
    private float baseHeight;

    private Camera thisCamera;

    private Vector3 velocity = Vector3.zero;

    PlayerPhysics target;

    void Start(){
        thisCamera = GetComponent<Camera>();
        baseHeight = transform.position.y;
        baseFov = thisCamera.fieldOfView;
    }

    // Update is called once per frame
    void LateUpdate () {
        target = FindObjectOfType<PlayerPhysics>();

        if (target != null)
        {
            Vector3 targetPosition = target.transform.position;
            targetPosition.y = transform.position.y;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.25f);

            float targetHeight = baseHeight + (heightAddition * (target.currentSpeed / target.maximumSpeed));
            targetHeight = Mathf.Lerp(transform.position.y, targetHeight, Time.deltaTime);
            transform.position = new Vector3(transform.position.x, targetHeight, transform.position.z);

            float targetFov = baseFov + (fovAddition * (target.currentSpeed / target.maximumSpeed));
            targetFov = Mathf.Lerp(thisCamera.fieldOfView, targetFov, Time.deltaTime);
            thisCamera.fieldOfView = targetFov;

            transform.position = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        }
        else if(defaultTarget != null)
        {
            Vector3 targetPosition = defaultTarget.transform.position;
            targetPosition.y = transform.position.y;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);

            float targetHeight = baseHeight;
            targetHeight = Mathf.Lerp(transform.position.y, targetHeight, Time.deltaTime);
            transform.position = new Vector3(transform.position.x, targetHeight, transform.position.z);

            float targetFov = baseFov;
            targetFov = Mathf.Lerp(thisCamera.fieldOfView, targetFov, Time.deltaTime);
            thisCamera.fieldOfView = targetFov;
        }
    }
}
