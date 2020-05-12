﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

//edited 5/12/20 by Jordan

public class WeaponManager : MonoBehaviour
{
    //Bullet prefab
    [SerializeField] private GameObject projectilePrefab;
    //Where bullet shoots from
    [SerializeField] private Transform fireLocation;
    [SerializeField] private float damage;
    //Delay between shots
    [SerializeField] private float fireDelay;
    //How fast the bullet travels
    [SerializeField] private float projectileSpeed;
    //How long before bullet destroys itself
    [SerializeField] private float projectileLifetime;

    //If it reaches this number, it must reset to zero before you can fire again.
    [SerializeField] private float overheatAmount;
    //How much shootTimer increase
    [Range(0, 1)]
    [SerializeField] private float increaseRate;
    //How much shootTimer decreases
    [Range(0, 1)]
    [SerializeField] private float decreaseRate;
    //Maxiumum amount of time until player can no longer shoot
    [SerializeField] private float maxShootTimer;
    //TO DO: Make use of this when card effects are fleshed out
    [SerializeField] private float weaponSwapDelay;
    //Must be -1.0f, DO NOT CHANGE  
    private float fire = -1.0f;
    //Reference to Car Controller
    private CarController cc;
    //Controls if you can shoot or not.
    private bool canShoot;

    [HideInInspector] public PlayerManager playerManager;
    [HideInInspector] public string playerControllerID;

    void Update()
    {
        //constantly check if gun can shoot
        OverheatTimerCheck();
    }

    //initialize should be start but playermanager hasnt been fixed so cant be changed yet
    public void Initialize()
    {
        //gun should be able to shoot at start of game
        canShoot = true;
        //get car controller so that car velocity can be matched
        cc = GetComponent<CarController>();
        
        //set controller id
        playerControllerID = playerManager.id.ToString();
    }

    //Controls the overheat or cooldown of the shooting
    private void OverheatTimerCheck()
    {
        //check if fire button is pressed while shooting enabled
        if(Input.GetButton("RightBumper" + playerControllerID) && canShoot == true)
        {
            //if shooting is nabled and button is pressed, spawn projectile
            FireProjectile(projectilePrefab, damage, projectileLifetime, projectileSpeed);
            overheatAmount += increaseRate * Time.deltaTime;
            //check overheat amount to see if gun can keep shooting, if not, disable shooting
            if(overheatAmount >= maxShootTimer)
            {
                canShoot = false;
            }
        }
        //if not actively shooting, check if shooting is disabled
        else if(canShoot == false)
        {
            //if shooting is disabled, slowly reduced overheat amount to 0, then enable shooting
            Mathf.Clamp(overheatAmount, 0, maxShootTimer);
            overheatAmount -= decreaseRate * Time.deltaTime;
            if (overheatAmount <= 0)
            {
                canShoot = true;
            }
        }
        //if not actively shooting and shooting is not disabled, check if overheat amount is more than 0
        else if(canShoot == true && overheatAmount > 0)
        {
            //if overheat amount is more than 0, slowly reduce it untill its <= 0
            overheatAmount -= decreaseRate * Time.deltaTime;
        }
    }

    
    public void FireProjectile(GameObject prefab, float damage, float lifespan, float speed)
    {
        //check if enough time has passed between shots
        if(Time.time >= fire)
        {
            //reset the delay between shots
            fire = Time.time + fireDelay;
            
            //create new projectile to be fired
            GameObject newObject = Instantiate(prefab, fireLocation.position, fireLocation.rotation);
            Projectile newProjectile = newObject.GetComponent<Projectile>();

            //Setup all the parameters for the projectile
            newProjectile.Initialize(playerManager, damage, projectileLifetime);
            //Actually 'shoot' the projectile
            newObject.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward * (projectileSpeed + cc.CurrentSpeed));
        }
    } 
}
