using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Connery Ray 05/07/2020
/// Added bounce ability and bounce check
/// </summary>

public class Projectile : MonoBehaviour
{
    //The amount of damage this projectile should deal
    public float damage;
    //How long the projectile should persist before being destroyed
    public float lifespan;
    private float timeElapsed;
	//Angle the projectile should follow
	public float projectileAngle;

    //The player that fired this projectile
    public PlayerManager owner;

    //Whether or not the projectile can hit it's own user
    public bool canHitOwner;

    //Whether or not the projectile can currently collide with things. This is meant to prevent double collisions with the same object
    public bool canCollide;

	//Whether or not the projectile can bounce after hitting something or if it has already bounced and should cause damage
	public bool canBounce;
	[SerializeField] private bool hasBounced;
	public bool hasPulse;

    public void Initialize(PlayerManager owner, float damage, float lifespan)
    {
        this.owner = owner;
        this.damage = damage;
        this.lifespan = lifespan;

        canHitOwner = false;
        canCollide = true;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if(timeElapsed >= lifespan)
        {
            //Destroy the projectile
				//cleaner form of self destroy -Joey
            Destroy(gameObject);
        }
    }
		//changed 'OnCollisionEnter()' to 'OnTriggerEnter()' -Joey
    public void OnTriggerEnter(Collider other)
    {
        if (canCollide)
        {
            //Since the rigidbody is on the car, we will check if we can get the vehicle manager and then the player manager from there
				//changed 'GetComponent<>()' to 'GetComponentInParent<>()' -Joey
            TempVehicleManager vehicle = other.gameObject.GetComponentInParent<TempVehicleManager>();

            if (vehicle != null)
            {
                PlayerManager player = vehicle.playerManager;

                //If we can hit the owner or this is a different player
                if (canHitOwner || !player.Equals(owner))
                {
                    //Apply damage and disable collisions so that it does not hit the same object again this frame before it is destroyed
                    player.ApplyDamage(damage);
                    canCollide = false;
					Destroy(gameObject);	//cleaner form of self destroy -Joey
                }
            }	//destroys the projectile upon collision with anything other than a vehicle 
			if(vehicle == null && canBounce)
			{
				StartCoroutine(BounceDelay());
			}
			else if(hasBounced == true)
			{
				Debug.Log("Bouncing Projectile Destroyed");
				Destroy(gameObject);
			}
			else
			{
				Debug.Log("Destroyed Projectile");
				Destroy(gameObject);
			}
        }
    }
	IEnumerator BounceDelay()
	{
		Debug.Log("Projectile has bounced");
		hasBounced = true;
		yield return new WaitForSeconds(1);
	}
	//commenting out useless method, but leaving it in just in case -Joey
/*     public void DestroyProjectile()
    {
        Destroy(this.gameObject);
    } */
}
