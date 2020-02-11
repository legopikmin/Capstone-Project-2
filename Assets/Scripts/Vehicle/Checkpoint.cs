using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointID;

    public List<GameObject> lightBeams;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnTriggerEnter(Collider other)
    {
        TempVehicleManager tempVehicleManager = other.GetComponentInParent<TempVehicleManager>();
        //Checks for tempVehicleManager then takes player's PlayerManager to add energy.
        if (tempVehicleManager != null)
        {
            PlayerManager playerManager = tempVehicleManager.playerManager;

            playerManager.ClearCheckpoint(checkpointID);
            lightBeams[(int)playerManager.id].SetActive(false);

        }
    }
}
