using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public enum VehicleType { Light, Medium, Heavy }

[CreateAssetMenu(fileName = "New Vehicle", menuName = "Vehicle")]
public class VehicleData : ScriptableObject
{
    [Header ("Vehicle Base Stats")]
    public VehicleType vehicleType;
    public float maxHealth;//
    public float maxShield;//

    public float mass;
    public float angularDrag;
    public float drag;
    public float topSpeed;
    public float driftingTraction;

    //carController = gameObject.GetComponent<CarController>();

    //if(vehicleType == VehicleType.Light)
    //{

    //    maxHealth = 75;
    //    currentHealth = maxHealth;
    //    carController.m_Topspeed = 125;
    //    carController.NoOfGears = 5;
    //    currentShield = 0;
    //    maxShield = 0;
    //    gameObject.GetComponent<Rigidbody>().mass = 950;
    //    gameObject.GetComponent<Rigidbody>().drag = 0.1f;
    //    gameObject.GetComponent<Rigidbody>().angularDrag = 0.05f;
    //    carController.driftingTraction = 3.5f;

    //}
    //else if(vehicleType == VehicleType.Medium)
    //{
    //    maxHealth = 100;
    //    currentHealth = maxHealth;
    //    carController.m_Topspeed = 100;
    //    carController.NoOfGears = 5;
    //    currentShield = 0;
    //    maxShield = 0;
    //    gameObject.GetComponent<Rigidbody>().mass = 1100;
    //    gameObject.GetComponent<Rigidbody>().drag = 0.2f;
    //    gameObject.GetComponent<Rigidbody>().angularDrag = 0.05f;
    //    carController.driftingTraction = 3.5f;
    //}
    //else if(vehicleType == VehicleType.Heavy)
    //{
    //    maxHealth = 125;
    //    currentHealth = maxHealth;
    //    carController.m_Topspeed = 75;
    //    carController.NoOfGears = 5;
    //    currentShield = 0;
    //    maxShield = 0;
    //    gameObject.GetComponent<Rigidbody>().mass = 1500;
    //    gameObject.GetComponent<Rigidbody>().drag = 0.45f;
    //    gameObject.GetComponent<Rigidbody>().angularDrag = 0.05f;
    //    carController.driftingTraction = 3.5f;
    //}
}
