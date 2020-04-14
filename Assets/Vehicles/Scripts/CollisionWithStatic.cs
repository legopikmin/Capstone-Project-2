using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class CollisionWithStatic : MonoBehaviour
{
    public PlayerManager playerManager;//reference to the stats script
    float currentSpeed;
    bool canCollide;
    void Start()
    {
        canCollide = true;
    }
    private void Update()
    {
        currentSpeed = playerManager.vMan.carController.CurrentSpeed;
    }

    private IEnumerator WaitTime()//Set this up because we were taking double damage when going at a high enough speed. Game will now wait 750 milisecond before trying to apply damage again. 
    {
        canCollide = false;
        if (currentSpeed < 25)
        {
            //No damage will happen, left here incase we wanted something to happen
        }
        else if (currentSpeed > 25 && currentSpeed <= 50)
        {
            playerManager.ApplyDamage(2.5f);
        }
        else if (currentSpeed > 50 && currentSpeed <= 75)
        {
            playerManager.ApplyDamage(5f);
        }
        else if (currentSpeed > 75 && currentSpeed <= 100)
        {
            playerManager.ApplyDamage(7.5f);
        }
        else if (currentSpeed > 100)
        {
            playerManager.ApplyDamage(10f);
        }
        else//Spits this out if there is no speed found. Should never see this, however, can never be too sure.
        {
            Debug.Log("Code Error, speed not found or speed is outside the bounderies set. Check IEnumerator for Obstacle");
        }
        yield return new WaitForSecondsRealtime(.75f);
        canCollide = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Obstacle")//If the player collides with an object, they will take damage depending how fast they were going before colliding with said object.
        {
            if(canCollide)
            {
                StartCoroutine(WaitTime());
            }
        }
    }
}
