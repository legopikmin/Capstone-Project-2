using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
/*
    /// <summary>
    /// This script is going off the assumption that this script will be attached to the car itself.
    /// IF that is not the case, adjustments will need to be made to the script, mostly around the death method.
    /// 
    /// Created on: 10/30/19
    /// By: Matthew Myers, Alejandro Muros, Jordan Olson
    /// 
    /// Edited on: 11/06/19
    /// By: Alejandro Muros and Scott P.
    /// Removed unneeded methods and added in the ability for this script to give a reference of itself to the card manager and call a method in the card manager
    /// </summary>
*/

    private enum PlayerID { P1 = 1, P2 = 2, P3 = 3, P4 = 4 }//ID for controls.
    public enum PlayerState { Mulligan, Waiting, Playing, LocalPause, GlobalPause, Dead }//The status of the player.
    //public enum ColorAffinity { Red, Black, Blue, White, Green }//What color is the player weak too and strong against.

    //public enum PlayerStat { NullStat, MaxHealth, MaxShield, Mass, AngularDrag, Drag, TopSpeed, DriftingTraction }


    [Header("Player Settings")]
    [SerializeField] private PlayerID id;
    public PlayerState currentState;
    
    //[SerializeField] List<Buff> activeBuffs; //Contains all of the active buffs and debuffs on the player
    //List<Buff> buffsToRemove; // A list of buffs to remove on FixedUpdate, this is used since buffs could not be removed while iterating through them

    public bool[] checkpointsCleared;

    //[Header("Color Affinity")]
    //[SerializeField] ColorAffinity colorAdvantage;
    //[SerializeField] ColorAffinity colorDisadvantage;

    public TMP_Text winnerMessage;


		//sets the player ID, called by GameManager.cs		-JAM
	public void SetPlayerID(int myID){
		id = myID;
	}
		//returns the player ID, called upon by other scripts	-JAM
	public int GetPlayerID(){
		return id;
	}

	void Update(){
		if(Input.GetButtonDown("Start" + id))	//-JAM
		{
			Debug.Log("Player ID: " + id);
		}
	}

    void FixedUpdate()
    {
        //Check if there are any buffs left to remove
        if (buffsToRemove.Count > 0)
        {
            foreach (Buff b in buffsToRemove)
            {
                //Remove the buff from our active buff list
                activeBuffs.Remove(b);
                b.OnRemoveBuff(this);
            }
            //Clears the list or buffsToRemove so that we don't try and remove the same buff again
            buffsToRemove.Clear();
        }

        //Update each of the buffs and debuffs on the player
        foreach (Buff b in activeBuffs)
        {
            b.Update();
        }

        if(Input.GetButtonDown("Start" + cardManager.playerControllerID))
        {
            vMan.transform.position = GameManager.gameManager.initialSpawnPoints[((int)id)-1].position;
            vMan.transform.rotation = Quaternion.identity;
        }
    }

    public void Initialize(/*Add paramerters once game setup is designed*/ int playerNum /*Game Manager passes in the number of the player when instantiating the player manager*/) //this takes the place of start
    {
        currentState = PlayerState.Mulligan;

        checkpointsCleared = new bool[GameManager.gameManager.checkpoints.Count];
        

        cardManager.playerManager = this; //gives the card manager a reference to itself
        cardManager.Initialize(); //calls the initialize method in the card manager

        //vMan.playerManager = this;
        //vMan.Initialize();

        //weaponManager.playerManager = this;
        //weaponManager.Initialize();

        buffsToRemove = new List<Buff>();
		
		camSetup.SetupCamera(id.ToString());
    }

    public void ClearCheckpoint(int checkpointID)
    {
        if (!checkpointsCleared[checkpointID])
        {
            checkpointsCleared[checkpointID] = true;

            //Check if we cleared each of the checkpoints
            bool allCheckpointsCleared = true;
            foreach(bool b in checkpointsCleared)
            {
                if(!b)
                {
                    allCheckpointsCleared = false;
                    break;
                }
            }

            //If we did call CompleteLap
            if (allCheckpointsCleared)
            {
                CompleteLap(checkpointID);
            }
        }

    }

    private void CompleteLap(int finalCheckpointCleared)
    {
        for (int i = 0; i < checkpointsCleared.Length; i++) //sets the checkPointsCleared bool to false for all the check points that have been crossed
        {
            checkpointsCleared[i] = false;
        }

        checkpointsCleared[finalCheckpointCleared] = true; //Sets the last passed checkpoint checkPointsCleared bool to true so it can't be passed again

        GameManager.gameManager.ResetCheckpoints((int) id);

        //Draw a card
        cardManager.DrawCard();

        //Increase max energy
        if (currentMaxEnergy < absoluteMaxEnergy)
        {
            currentMaxEnergy++;
        }

        if (currentMaxEnergy > absoluteMaxEnergy)
        {
            currentMaxEnergy = absoluteMaxEnergy;
        }

        energyBar.value = (1.0f * currentEnergy / currentMaxEnergy); //Multiplying by a float to ensure that the number results in a float
        energyText.text = currentEnergy.ToString();
    }

/*
    #region Buff/Debuff and Stats
    /// <summary>
    /// Gets the base value of the passed in stat without any modifications
    /// </summary>
    /// <param name="stat">The stat to get the value of</param>
    /// <returns></returns>
    public float GetBaseStatValue(PlayerStat stat)
    {
        switch(stat)
        {
            case PlayerStat.MaxHealth:
                return vehicleData.maxHealth;

            case PlayerStat.MaxShield:
                return vehicleData.maxShield;

            case PlayerStat.Mass:
                return vehicleData.mass;

            case PlayerStat.AngularDrag:
                return vehicleData.angularDrag;

            case PlayerStat.Drag:
                return vehicleData.drag;

            case PlayerStat.TopSpeed:
                return vehicleData.topSpeed;

            case PlayerStat.DriftingTraction:
                return vehicleData.driftingTraction;
            
            default:
                Debug.Log("Stat: " + stat.ToString() + " not defined");
                return -1;
        }
    }

    /// <summary>
    /// Gets the current value of the passed in stat after any modifications from buffs and debuffs
    /// </summary>
    /// <param name="stat">The stat to get the value of</param>
    /// <returns></returns>
    public float GetCurrentStatValue(PlayerStat stat)
    {
        //Get the base value of the stat
        float value = GetBaseStatValue(stat);

        //Pass that value through each of the buffs on this player
        foreach(Buff b in activeBuffs)
        {
            value = b.ModifyStatValue(stat, value);
        }

        //Return the final value
        return value;
    }

    /// <summary>
    /// Adds the buff to the players list of sctive buffs and initializes it
    /// </summary>
    /// <param name="b">The buff to apply to the player</param>
    public void ApplyBuff(Buff b)
    {
        //Make a copy of the buff instead of just applying the base one
        Buff copy = b.GetShallowCopy();

        //Add the buff to the list and initialize it
        activeBuffs.Add(copy);
        copy.Initialize(this);

        copy.OnApplyBuff(this);
    }

    /// <summary>
    /// Marks the passed in buff for removal. It will be removed during the next fixed update
    /// </summary>
    /// <param name="b">The buff to remove</param>
    public void RemoveBuff(Buff b)
    {
        buffsToRemove.Add(b);
    }
    #endregion
*/
}

