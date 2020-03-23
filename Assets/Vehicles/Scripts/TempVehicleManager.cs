using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.Vehicles.Car;

//THIS IS A TEMP CLASS MEANT TO PASS INPUTS TO THE CAR CONTROLLER UNTIL THE REAL VEHICLE CONTROLLER IS ADDED
//Created: 11/6/19
//By: Jordan

public class TempVehicleManager : MonoBehaviour
{
    public PlayerManager playerManager;
    public string playerControllerID;
    public CarUserControl car;
    public CarController carController;
    public CollisionWithStatic collisionWithStatic;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize()
    {
        //get car control and pass it the id string
        car = this.gameObject.GetComponent<CarUserControl>();
        collisionWithStatic = this.gameObject.GetComponent<CollisionWithStatic>();
        collisionWithStatic.playerManager = playerManager;
        this.playerControllerID = playerManager.id.ToString();
        car.SetCarControllerID(playerControllerID);
        car.Initialize();
        carController.Initialize();
        UpdateVehicleStats();
    }

    public void UpdateVehicleStats()
    {
        carController.driftingTraction = playerManager.GetCurrentStatValue(PlayerManager.PlayerStat.DriftingTraction);
        carController.m_Topspeed = playerManager.GetCurrentStatValue(PlayerManager.PlayerStat.TopSpeed);

        gameObject.GetComponent<Rigidbody>().mass = Mathf.Clamp(playerManager.GetCurrentStatValue(PlayerManager.PlayerStat.Mass), 250, Mathf.Infinity);
        gameObject.GetComponent<Rigidbody>().drag = playerManager.GetCurrentStatValue(PlayerManager.PlayerStat.Drag);
        gameObject.GetComponent<Rigidbody>().angularDrag = playerManager.GetCurrentStatValue(PlayerManager.PlayerStat.AngularDrag);
    }

    //method to be called by Player Manager in order to pass Vehicle Manager the proper player id
    public void SetVehicleControllerID(string id)
    {
        playerControllerID = id;
    }

    private void Update()
    {
        
    }
} 

