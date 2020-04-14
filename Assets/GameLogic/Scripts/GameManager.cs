using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager; //this is used so other scripts can access it
    public Transform[] initialSpawnPoints; //the initial spawn points for the players. 
    public GameObject playerPrefab;
    public GameObject[] groupOfHazards; //used to set a group of hazards to activate
    public float[] hazardTriggerTimes; //used to set when to activate hazards
    public float[] energyIncreaseTimes; //used to set which times to increase the amount of energy you can get

    public List<Checkpoint> checkpoints;
    public List<EnergyPickUp> energyPickUps;


    private List<GameObject> players = new List<GameObject>();
    private List<Camera> cameras = new List<Camera>();
    [HideInInspector]
    public enum GameStatus
    {
        settingUp, currentlyPlaying, finished
    }
    [HideInInspector] public GameStatus status; //used to keep track of the status of the game
    private int playerCount; //used to determine how many players to spawn
    private int playerID; //used to set the PlayerID enum in the PlayerManager script
    [HideInInspector] public int energyIncrease; //this is used to add extra energy to the player when they pick an energy pick up
    private int hazardGroup; //used to keep track of which group of hazards to activate
    private float matchTime; //this keeps track of the time for the match

    public bool canEndMatch = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = this;
        playerCount = PlayerPrefs.GetInt("PlayerCount");
        playerID = 1; //sets player id to equal one. this is here to ensure no player has there pNum in the player controller script equal 0 or more then 4.

        status = GameStatus.settingUp;

        //Get all the checkpoints from the map
        checkpoints = new List<Checkpoint>(FindObjectsOfType<Checkpoint>());
        int i = 0;
        foreach(Checkpoint c in checkpoints)
        {
            c.checkpointID = i++;
        }

        //Get all the energy spawns
        energyPickUps = new List<EnergyPickUp>(FindObjectsOfType<EnergyPickUp>());

        //used only during the set up of the game
        if (status == GameStatus.settingUp)
        {
            SpawnPlayers();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //If the match is done
        if (status == GameStatus.finished)
        {
            if(Input.anyKeyDown && canEndMatch == true)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
        if (status == GameStatus.currentlyPlaying) //this is to make sure the time only increases when the game has started
        {
            matchTime += Time.deltaTime;
        }

        if (hazardGroup < hazardTriggerTimes.Length) //this will only be when the int hazardGroup is less than the hazardTriggerTimes.Length
        {
            ActivateHazards();
        }

        if (energyIncrease < energyIncreaseTimes.Length) //this will only be when the int energyIncrease is less than the energyIncreaseTimes.Length
        {
            IncreaseEnergy();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }

    }
    public void ResetCheckpoints(int playerID)
    {
        checkpoints.ForEach(c => c.lightBeams[playerID].SetActive(true));
    }

    //this method is used to spawn in the players and set there id enum in there PlayerManager script
    public void SpawnPlayers()
    {
        int playersSpawned = 0;
        while (playersSpawned < playerCount)
        {
            GameObject playerClone = Instantiate(playerPrefab, initialSpawnPoints[playersSpawned].position, initialSpawnPoints[playersSpawned].transform.rotation); //spawns the player at the starting spawn points
            playerClone.GetComponent<PlayerManager>().id = (PlayerManager.PlayerID)playerID; //sets the player's num that will help differentiate it from other players
            players.Add(playerClone); //adds the newly created player to the players list
            players[playersSpawned].GetComponent<PlayerManager>().Initialize(playerID);
            cameras.Add(playerClone.GetComponentInChildren<Camera>()); //adds the camera to the cameras list
            playerID += 1;
            playersSpawned += 1;
            Debug.Log(playersSpawned);
        }

        CameraSetUp();
    }

    public void CameraSetUp()
    {
        //these set up the cameras depending on the size of the cameras list. Its only able to set up a max of 4 cameras.
        if (cameras.Count == 1)
        {
            cameras[0].GetComponent<Camera>().rect = new Rect(0, 0, 1, 1);
            status = GameStatus.currentlyPlaying;
        }

        if (cameras.Count == 2)
        {
            cameras[0].GetComponent<Camera>().rect = new Rect(0, .5f, 1, .5f);
            cameras[1].GetComponent<Camera>().rect = new Rect(0, 0, 1, .5f);
            status = GameStatus.currentlyPlaying;
        }

        if (cameras.Count == 3)
        {
            cameras[0].GetComponent<Camera>().rect = new Rect(0, .5f, .5f, .5f);
            cameras[1].GetComponent<Camera>().rect = new Rect(.5f, .5f, .5f, .5f);
            cameras[2].GetComponent<Camera>().rect = new Rect(0, 0, 1, .5f);
            status = GameStatus.currentlyPlaying;
        }

        if (cameras.Count == 4)
        {
            cameras[0].GetComponent<Camera>().rect = new Rect(0, .5f, .5f, .5f);
            cameras[1].GetComponent<Camera>().rect = new Rect(.5f, .5f, .5f, .5f);
            cameras[2].GetComponent<Camera>().rect = new Rect(0, 0, .5f, .5f);
            cameras[3].GetComponent<Camera>().rect = new Rect(.5f, 0, .5f, .5f);
            status = GameStatus.currentlyPlaying;
        }
    }

    public void RemovePlayer(GameObject player)
    {
        GameManager.gameManager.players.Remove(player);

        if(players.Count < 2)
        {
            status = GameStatus.finished;
            players[0].GetComponent<PlayerManager>().winnerMessage.gameObject.SetActive(true);
            StartCoroutine(TurnOffDelay(3));
        }
    }

    private IEnumerator TurnOffDelay(float time)
    {
        yield return new WaitForSeconds(time);
        canEndMatch = true;
    }

    //used to activate the hazards as the match goes on
    private void ActivateHazards()
    {
        if (matchTime > hazardTriggerTimes[hazardGroup]) //used when the match time is greater than the current value being checked for hazardTriggerTimes
        {
            groupOfHazards[hazardGroup].SetActive(true); //used to activate a group of hazard corresponding with the current hazardTriggerTimes value being checked
            hazardGroup++;
        }
    }

    //used to increase the value that gives the players extra energy as the match goes on
    private void IncreaseEnergy()
    {
        if (matchTime > energyIncreaseTimes[energyIncrease]) //used when the match time is greater than the current value being checked for energyIncreaseTimes
        {
            energyIncrease++; //increse the energy by 1 and used to get the next value in the energyIncrease array for the if statement

            energyPickUps.ForEach(p => p.addedEnergy++);
        }
    }
}