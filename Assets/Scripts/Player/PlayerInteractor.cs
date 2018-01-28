using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class PlayerInteractor : MonoBehaviour {

    public enum ControllerType {
        MOUSE,
        KEYBOARD,
        GAMEPAD,
        LEAPMOTION
    }

    public ControllerType controllerType = ControllerType.MOUSE;

    public Vector3 getInputForwardDirection(){
        Vector3 forwardDirection = Vector3.zero;

        switch (controllerType)
        {
            case ControllerType.MOUSE:
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                Plane playerPlane = new Plane(transform.up, transform.position);
                float planeIntersection;
                playerPlane.Raycast(mouseRay, out planeIntersection);

                Vector3 mousePosition = mouseRay.GetPoint(planeIntersection);
                forwardDirection = (mousePosition - transform.position).normalized;
                break;

            case ControllerType.KEYBOARD:
                // pass
                break;
            
            case ControllerType.GAMEPAD:
                // Method 1: 
               forwardDirection = new Vector3(Input.GetAxis("GamepadHorizontal"), 0, Input.GetAxis("GamepadVertical"));

                // Method 2:
                // float horizontal = Input.GetAxis("GamepadHorizontal");
                // forwardDirection = Quaternion.AngleAxis(90 * horizontal, Vector3.up) * transform.forward;
                break;

            case ControllerType.LEAPMOTION:

                forwardDirection = Vector3.forward;

                Controller controller = HandController.Main.GetLeapController();
                Frame frame = controller.Frame (); // controller is a Controller object
                if(frame.Hands.Count > 0){
                    HandList hands = frame.Hands;
                    Hand firstHand = hands [0];
                    Vector handPos = firstHand.PalmPosition;
                    Vector diff = handPos - new Vector(0, handPos.y, 0);
                    Vector dir = diff.Normalized;
                    forwardDirection = HandController.Main.ToUnityDir(dir);
                }
                
                break;
        }

        return forwardDirection;
    }

    public bool getInputBoosting(){
        bool boosting = false;

        switch (controllerType)
        {
            case ControllerType.MOUSE:
                boosting = Input.GetMouseButton(0);
                break;

            case ControllerType.KEYBOARD:
                // pass
                break;

            case ControllerType.GAMEPAD:
                boosting = Input.GetButton("GamepadBoost");
                break;

            case ControllerType.LEAPMOTION:
                break;
        }

        return boosting;
    }

}
