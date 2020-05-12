using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour
{
    [Header("Splash Damage Settings")]
    [Tooltip("If checkmarked, the projectile will do splash damage")]
    [SerializeField] private bool splashDamage = false; //determines whether this projectile will be an AOE
    [SerializeField] private LayerMask targetLayerMask; //sets which layer of the colliders to store when splash damage is active
    [SerializeField] private float splashDamageRange = 10; //sets how far the splash damage aspect of the projectile can reach players
    [Header("Other Settings")]
    [SerializeField] private float damage = 10; //this is the damage for the projectile no matter if it has splash damage active or not
    [Tooltip("Used to set the initial value of nearestTarget")]
    [SerializeField] private float targetRange = 5000; //used to set the initial value of nearestTarget
    [Tooltip("Sets how far forward the raycast will be from the user")]
    [SerializeField] private float forwardDistance = 4; //sets how far forward the raycast will be from the user
    [Header("Projectile Destroy Settings")]
    [SerializeField] private AudioClip sfxProjectileEndlife; //sound that plays when the bullet collides with something
    [SerializeField] private ParticleSystem projectileEndLife; //affect that plays when the bullet collides with something

    private float moveSpeed;
    private float maxDuration = 45; //how long the bullet will be active for
    private Transform userPlayerPawn; //stores who shot the projectile
    private Transform targetPlayerPawn; //stores who will be hit by the projectile
    private bool readyToMove; //this will determine whether the projectile will follow a player or not
    private bool failedToFindTarget; //this will determine whether the projectile act like a normal projectile
    private float nearestValue; //stores the closest distance of a target. will be used to compare the distance of one target to another in order to find the closest one. also will be used as a max distance for when it has no targets yet.
    private List<float> playerDistanceMap = new List<float>(); //stores the distance value of the targets
    private int nearestTarget; //used to determine which player to set as the target
    public List<TempVehicleManager> potentialTargets = new List<TempVehicleManager>(); //stores every player except the one who shot the projectile

    public void Initialize(Transform user, float projectileDamage, float lifeSpan, float speed)
    {
        userPlayerPawn = user; //sets userPlayerPawn to equal the player that shot this projectile
        damage = projectileDamage; //enables damage to be set by the value passed in by the ProjectileEffect script
        maxDuration = lifeSpan; //enables maxDuration to be set by the value passed in by the ProjectileEffect script
        moveSpeed = speed; //enables maxSpeed to be set by the value passed in by the ProjectileEffect script
        nearestValue = targetRange * 2; //makes nearestValue equal targetRange multiplied 2
        Invoke("FindClosestTarget", 0.05f); //adds a slight delay in calling the FindClosestTarget method
    }

    private void Update()
    {
        /*Vector3 startPoint = userPlayerPawn.position + new Vector3(0, .5f, 0) + transform.forward * forwardDistance;
        Vector3 direction = targetPlayerPawn.position - userPlayerPawn.position;
        Debug.DrawRay(startPoint, direction); */

        if (failedToFindTarget == true) //used if a target was not found
        {
            transform.position += transform.forward * Time.fixedDeltaTime * moveSpeed; //moves the projectile forward
        }

        else if (readyToMove == true) //used if a target was found
        {
            transform.position += transform.forward * Time.fixedDeltaTime * moveSpeed; //moves the projectile forward

            Vector3 targetDirection = targetPlayerPawn.position - transform.position; //calculates the target direction the projectile will go by using the targetPlayerPawn.position and the projectiles own position

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, moveSpeed * Time.fixedDeltaTime, 0.0f); //calculates the direction the projectile will go by using the projectiles forward axis and targetDirection

            transform.rotation = Quaternion.LookRotation(newDirection); //used to rotate the projectile itself so its facing its target
        }
    }

    private void FindClosestTarget()
    {
        List<TempVehicleManager> foundPlayers = new List<TempVehicleManager>(); //stores every player found

        foundPlayers.AddRange(FindObjectsOfType<TempVehicleManager>()); //finds all the players and adds them to a list

        for (int i = 0; i < foundPlayers.Count; i++)
        {
            targetPlayerPawn = foundPlayers[i].transform; //sets targetPlayerPawn to equal the current iterated player in foundPlayers

            if (targetPlayerPawn != userPlayerPawn) //checks if targetPlayerPawn does not equal userPlayerPawn
            {
                RaycastHit hit = new RaycastHit(); //enables the raycast to give information on the object hit
                Vector3 startPoint = userPlayerPawn.position + new Vector3(0, .5f, 0) + transform.forward * forwardDistance;
                Vector3 direction = targetPlayerPawn.position - userPlayerPawn.position;

                if (Physics.Raycast(startPoint, direction, out hit))
                {
                    if(hit.collider.GetComponentInParent<TempVehicleManager>() != null) //checks if the hit object parent has the TempVehicleManager script
                    {
                        if (hit.collider.GetComponentInParent<TempVehicleManager>().transform == targetPlayerPawn) //used if the hit object is the targetPlayerPawn
                        {
                            playerDistanceMap.Add(hit.distance); //adds the value of the distance between the origin and the hit point to the playerDistanceMap list
                            potentialTargets.Add(targetPlayerPawn.GetComponent<TempVehicleManager>()); //adds the current iterated player in foundPlayers to potentialTargets
                        }
                        Debug.Log(hit.collider); //debugs in the console what object was hit
                    }
                }
            }
        }

        FindNearestPlayer();
    }

    //used to find the closest target 
    private void FindNearestPlayer()
    {
        bool noTargetFound = true; //used to determine whether a target has been found
   
        for (int i = 0; i < playerDistanceMap.Count; i++)
        {
            if (playerDistanceMap[i] != 0) //checks if the current iterated playerDistanceMap value is not equal to zero
            {
                if (playerDistanceMap[i] < nearestValue) //checks if the current iterated playerDistanceMap value is less than nearestValue
                {
                    nearestValue = playerDistanceMap[i]; //sets nearestValue to equal the value of the current iterated playerDistanceMap
                    nearestTarget = i; //sets nearestTarget to equal the current iterated playerDistanceMap index value
                    noTargetFound = false; //used to set that a target has been found
                }
            }
        }

        if(noTargetFound == true) //used if no target has been found
        {
            failedToFindTarget = true;
        }

        else //used if a target has been found
        {
            LockOntoTargetPlayer();
        }
    }

    private void LockOntoTargetPlayer()
    {
        targetPlayerPawn = potentialTargets[nearestTarget].transform; //sets targetPlayerSpawn to equal one of the potentialTargets with a index that equals nearestTarget 
        Destroy(this, maxDuration); //sets when to destroy the projectile
        readyToMove = true; //used to tell the projectile it can start to move
    }

    private void OnTriggerEnter(Collider other)
    {
        if (readyToMove == true || failedToFindTarget == true)
        {
            if (other.GetComponentInParent<TempVehicleManager>() != null) //checks if the collided object's parent has the TempVehicleManager script
            {
                if (other.GetComponentInParent<TempVehicleManager>().transform != userPlayerPawn) //checks if the collided object's parent with the TempVehicleManager script transform is not equal to the userPlayerPawn transform variable
                {
                    if (splashDamage == true) //calls the ApplyRadialDamage method if splashDamage is true
                    {
                        ApplyRadialDamage();
                    }

                    else //used if splashDamage is false
                    {
                        other.GetComponentInParent<TempVehicleManager>().playerManager.ApplyDamage(damage); //uses the TempVehicleManager script to call a method from the PlayerManager script
                    }

                    DestroyProjectile();
                }
            }

            else //used if the collided object does not have the TempVehicleManager script
            {
                if (splashDamage == true) //calls the ApplyRadialDamage method if splashDamage is true
                {
                    ApplyRadialDamage();
                }

                DestroyProjectile();
            }
        }
    }

    //applies damage to other players that are in range of the overlapSphere
    private void ApplyRadialDamage()
    {
        Collider[] splashGroup = Physics.OverlapSphere(transform.position, splashDamageRange, targetLayerMask); //stores every player in range of the OverlapSphere

        foreach (Collider s in splashGroup) //applies damage to each player in splashGroup
        {
            s.GetComponentInParent<TempVehicleManager>().playerManager.ApplyDamage(damage); //uses the TempVehicleManager script to call a method from the PlayerManager script
        }
    }

    //used to play certain affects when the projectile is destroyed
    private void DestroyProjectile()
    {
        //Instantiate(projectileEndLife, transform.position, transform.rotation);

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (splashDamage == true) //used only if splashDamage is set to true
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, splashDamageRange); //draws a sphere that represents the range of the splashDamage
        }
    }
}