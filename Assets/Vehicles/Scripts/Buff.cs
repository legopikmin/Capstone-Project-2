using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buff", menuName = "Buff")]
public class Buff : ScriptableObject
{
    [Header("Stat Change")]
    public PlayerManager.PlayerStat statModified;
    public float amount;

    [Header("Duration")]
    public bool hasDuration;
    public float duration;
    private float durationRemaining;

    private PlayerManager player;
    //WARNING: If an object is added as a variable here the PlayerManager's ApplyBuff method may lead to unexpected behavior unless we implement a deep copy

    public void Initialize(PlayerManager player)
    {
        this.player = player;

        if(hasDuration)
        {
            durationRemaining = duration;
        }
    }

    public void Update()
    {
        if(hasDuration)
        {
            durationRemaining -= Time.deltaTime;

            if(durationRemaining<=0)
            {
                player.RemoveBuff(this);
            }
        }
    }

    public Buff GetShallowCopy()
    {
        return (Buff) MemberwiseClone();
    }

    public float ModifyStatValue(PlayerManager.PlayerStat stat, float currentValue)
    {
        if(stat == statModified)
        {
            return currentValue + amount;
        }
        else
        {
            return currentValue;
        }
    }

    public void OnApplyBuff(PlayerManager player)
    {
        //Check if this stat is one that requires a change to the rigidbody or car controller
        switch(statModified)
        {
            //If the stat is mass, drag, angularDrag, topSpeed, or driftingTraction update the value on the rigidBody to what the current value should be
            case PlayerManager.PlayerStat.Mass:
            case PlayerManager.PlayerStat.Drag:
            case PlayerManager.PlayerStat.AngularDrag:
            case PlayerManager.PlayerStat.DriftingTraction:
            case PlayerManager.PlayerStat.TopSpeed:
                player.vMan.UpdateVehicleStats();
                break;
        }
    }

    public void OnRemoveBuff(PlayerManager player)
    {
        //Check if this stat is one that requires a change to the rigidbody or car controller
        switch (statModified)
        {
            //If the stat is mass, drag, angularDrag, topSpeed, or driftingTraction update the value on the rigidBody to what the current value should be
            case PlayerManager.PlayerStat.Mass:
            case PlayerManager.PlayerStat.Drag:
            case PlayerManager.PlayerStat.AngularDrag:
            case PlayerManager.PlayerStat.DriftingTraction:
            case PlayerManager.PlayerStat.TopSpeed:
                player.vMan.UpdateVehicleStats();
                break;
        }
    }
}
