using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use
        public string playerControllerID = "P1"; // player id string to be added to end of input name to change it to the proper player input

        public void Initialize()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }


        private void FixedUpdate()
        {
            // pass the input to the car!
            float h = CrossPlatformInputManager.GetAxis("LeftAnalogHorizontal" + playerControllerID);
            float v = CrossPlatformInputManager.GetAxis("RightTrigger" + playerControllerID);
            float reverse = CrossPlatformInputManager.GetAxis("LeftTrigger" + playerControllerID);
#if !MOBILE_INPUT
            float handbrake = CrossPlatformInputManager.GetAxis("LeftBumper" + playerControllerID);
            m_Car.Move(h, v, reverse, handbrake);
            //Debug.Log(handbrake);
            //Debug.Log("Accel" + v);
            //Debug.Log("Decel" + reverse);

#else
            m_Car.Move(h, v, v, 0f);
#endif
           
        }

        //method to be called by Vehicle Manager in order to pass controller the proper player id
        public void SetCarControllerID(string id)
        {
            playerControllerID = id;
        }
    }
}
