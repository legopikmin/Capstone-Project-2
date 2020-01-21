using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TerrainAlterationBySector : MonoBehaviour
{
    public GameObject[] terrains;

    public GameObject currentTerrain;
    
    public enum VehicleType
    {
        Base,
        Blue,
        Red,
        Green,
        White,
        Black
    }
    public VehicleType type;

    // Start is called before the first frame update
    void Start()
    {
        type = VehicleType.Base;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeTerrain();
        }
    }

    public void ChangeTerrain()
    {
        switch (type)
        {
            case (VehicleType.Base):
                currentTerrain = terrains[0];
                break;

            case (VehicleType.Blue):
                currentTerrain = terrains[1];
                break;

            case (VehicleType.Red):
                currentTerrain = terrains[2];
                break;

            case (VehicleType.Green):
                currentTerrain = terrains[3];
                break;

            case (VehicleType.White):
                currentTerrain = terrains[4];
                break;

            case (VehicleType.Black):
                currentTerrain = terrains[5];
                break;
        }
        foreach(GameObject ob in terrains)
        {
            if(ob != currentTerrain)
            {
                ob.SetActive(false);
            }
        }
        currentTerrain.SetActive(true);
    }
}
