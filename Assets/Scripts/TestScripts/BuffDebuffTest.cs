using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDebuffTest : MonoBehaviour
{
    //public PlayerManager.PlayerStat stat;
    //public PlayerManager player;
    //public Buff buff;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            //Debug.Log("Base value:" + player.GetBaseStatValue(stat));
            //Debug.Log("Current value:" + player.GetCurrentStatValue(stat));
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            //player.ApplyBuff(buff);
            //Debug.Log("Buff Applied");
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            //player.RemoveBuff(buff);
            //Debug.Log("Buff Removed");
        }

    }
}
