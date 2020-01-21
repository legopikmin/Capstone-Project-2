using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPickUp : MonoBehaviour
{
    //Amount of energy added to player energy
    public int addedEnergy;
    //Amount of time before energy respawns. Set at 20 as per design
    public float respawnDelay = 20f;
    
    public void OnTriggerEnter(Collider other)
    {
            TempVehicleManager tempVehicleManager = other.GetComponentInParent<TempVehicleManager>();
            //Checks for tempVehicleManager then takes player's PlayerManager to add energy.
            if (tempVehicleManager != null)
            {
                PlayerManager playerManager = tempVehicleManager.playerManager;

                playerManager.GainEnergy(addedEnergy);
                StartCoroutine(SpawnDelay());
            }
    }

    //Turns off box collider and renderer then turns back on after a certain amount of time
    private IEnumerator SpawnDelay()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSecondsRealtime(respawnDelay);
        gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

   
}
