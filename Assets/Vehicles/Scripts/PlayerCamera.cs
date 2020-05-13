using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script created by Jordan 3/17/20

public class PlayerCamera : MonoBehaviour
{
    //the object which the camera follows and rotates around aka the car
    [SerializeField]
    private Transform target;
    //the camera
    [SerializeField]
    private Transform cameraTransform;

    //check if camera is moving
    private bool cameraMoving;
    //saved time at which reset happens
    private float timestamp;
    //check if we want the camera to reset after a delay
    [SerializeField]
    private bool resetCameraAfterDelay = true;
    //how much time to add to timestamp
    [SerializeField]
    private float timeToReset = .5f;
    //how fast camera returns to position
    [SerializeField]
    private float resetSpeed = 3f;

    //variables for the point where the camera should stop rotating.
    [SerializeField]
    private float xRotationUpperLimit = 60f;
    [SerializeField]
    private float xRotationLowerLimit = 1f; //values cannot be 0 or 90 because the euler calculation apparently cant tell the difference between those two values and it makes the camera spazz

    //variables for saving the camera rotation values so we can align axes
    private float currentYRotation;
    private float currentXRotation;

    private string playerID;



    // get player id so we control the correct camera
    void Start()
    {
        //playerID = target.GetComponent<PlayerManager>().GetPlayerID(); //the way it should work
        playerID = GetComponent<PlayerManager>().id.ToString();
    } 

    // Update is called once per frame
    void Update()
    {
        FollowTarget(target);
        CamRotation();
    }

    //camera is not childed to the car, so this method makes it follow the car
    private void FollowTarget(Transform targetTransform)
    {
        cameraTransform.position = new Vector3(targetTransform.position.x, targetTransform.position.y, targetTransform.position.z);
    }

    //CamRotation moves the camera around according to joystick input
    private void CamRotation()
    {
        //Get axes input
        float horizontalRotation = Input.GetAxis("RightAnalogHorizontal" + playerID);
        float verticalRotation = Input.GetAxis("RightAnalogVertical" + playerID);
        
        cameraTransform.Rotate(-verticalRotation, 0, 0);
        cameraTransform.Rotate(0, -horizontalRotation, 0);

        //save current y rotation value in case rotation needs to be fixed 
        currentYRotation = cameraTransform.localEulerAngles.y;
         
        //check if new camera rotation exceeds x rotation limits. If so, change the rotation back to its limit value.
        /*if (cameraTransform.localEulerAngles.x > xRotationUpperLimit)
        {
            cameraTransform.localEulerAngles = new Vector3(xRotationUpperLimit, currentYRotation, 0);

        } else if (cameraTransform.localEulerAngles.x < xRotationLowerLimit)
        {
            cameraTransform.localEulerAngles = new Vector3(xRotationLowerLimit, currentYRotation, 0);
        }*/

        //now that we know the x value is correct, save it in case z rotation needs to be fixed
        currentXRotation = cameraTransform.localEulerAngles.x;

        //if the z axis rotates at all, put z axis rotation back to 0. We dont want the camera to roll at all.
        if (cameraTransform.localEulerAngles.z != 0)
        {
            cameraTransform.localEulerAngles = new Vector3(currentXRotation, currentYRotation, 0);
        }


       //check if we want the camera to reset after a delay, and then execute a reset if needed
        if(resetCameraAfterDelay)
        {
            //check to see if player is making camera move. Combine the two input variables because if either is not zero, the camera is moving.
            if (horizontalRotation != 0 || verticalRotation != 0)
            {
                cameraMoving = true;
                timestamp = Time.time + timeToReset;
            }
            else
            {
                //if camera was moving but is not anymore, reset bool and set timestamp the point in the future where camera will reset if camera isnt moved again.
                if (cameraMoving)
                {
                    cameraMoving = false;
                    timestamp = Time.time + timeToReset;
                }
                 //when delayTime has passed, move camera back to its original position
                else if (Time.time >= timestamp && cameraTransform.rotation != target.rotation)
                {
                    cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, target.rotation, resetSpeed * Time.deltaTime);
                }
            } 
        }    
    }
}