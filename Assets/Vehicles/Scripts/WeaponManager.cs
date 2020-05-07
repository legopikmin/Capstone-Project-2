using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

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
    [SerializeField] private float shotOverheatLimit;
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
        OverheatTimerCheck();
    }

    public void Initialize()
    {
        canShoot = true;
        cc = GetComponent<CarController>();

        playerControllerID = playerManager.id.ToString();
    }

    //Controls the overheat or cooldown of the shooting
    private void OverheatTimerCheck()
    {
        if (Input.GetButton("RightBumper" + playerControllerID) && canShoot == true)
        {
            FireProjectile(projectilePrefab, damage, projectileLifetime, projectileSpeed);
            shotOverheatLimit += increaseRate * Time.deltaTime;
            if (shotOverheatLimit >= maxShootTimer)
            {
                canShoot = false;
            }
        }
        else if (canShoot == false)
        {
            Mathf.Clamp(shotOverheatLimit, 0, maxShootTimer);
            shotOverheatLimit -= decreaseRate * Time.deltaTime;
            if (shotOverheatLimit <= 0)
            {
                canShoot = true;
            }
        }
        else if (canShoot == true && shotOverheatLimit > 0)
        {
            shotOverheatLimit -= decreaseRate * Time.deltaTime;
        }
    }


    public void FireProjectile(GameObject prefab, float damage, float lifespan, float speed)
    {
        if (Time.time >= fire)
        {
            fire = Time.time + fireDelay;
            GameObject newObject = Instantiate(prefab, fireLocation.position, fireLocation.rotation);
            if (newObject.GetComponent<Projectile>() != null)
            {
                Projectile newProjectile = newObject.GetComponent<Projectile>();

                //Setup all the parameters for the projectile
                newProjectile.Initialize(playerManager, damage, projectileLifetime);
                //Actually 'shoot' the projectile
                newObject.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward * (projectileSpeed + cc.CurrentSpeed));
            }

            if (newObject.GetComponent<LockOn>() != null)
            {
                newObject.GetComponent<LockOn>().Initialize(transform, damage, lifespan, speed);
            }
        }
    }
}

