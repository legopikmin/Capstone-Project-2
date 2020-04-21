using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Alejandro Muros, Matt M, and Joey
//4/21/2020
/*
 * This script was recreated
 * New stuff added includes over time and score checker
 * Way for playerID to be given to players needs to be added in here at some point and method to get the player score needs to the PlayerManager so this script can get player scores
 */

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject cameraPrefab;
    [SerializeField] private float matchLength; //this determines how long the match will run
    [SerializeField] private float overTimeLength; //this determines how long the overTime will last for
    [SerializeField] private float buttonDelay; //this determines how long the buttons won't effect the win screen
    [SerializeField]
    private enum GameStatus
    { settingUp, inProgress, finished };
    [SerializeField] private GameStatus status; //used to keep track of the status of the game
    [SerializeField] private Transform[] initialSpawnPoints; //the initial spawn points for the players 

    private int playerCount; //used to determine how many players to spawn
    private int playerID; //used to set the PlayerID enum in the PlayerManager script
    private List<PlayerManager> players = new List<PlayerManager>(); //this stores the players
    private List<Camera> cameras = new List<Camera>(); //this stores the cameras

    private bool canEndMatch = false; //this is used to add a delay for when the player's can press any button to finish the game
    private bool overTimeActive; //this is used to determine if overtime should be active

    // Start is called before the first frame update
    void Start()
    {
        playerCount = PlayerPrefs.GetInt("PlayerCount");
        playerID = 1; //sets player id to equal one. this is here to ensure no player has there pNum in the player controller script equal 0 or more then 4.

        status = GameStatus.settingUp;

        //used only during the set up of the game
        if (status == GameStatus.settingUp)
        {
            SpawnPlayers();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (status == GameStatus.inProgress) //this is to make sure the timers only run when the game has started
        {
            if(matchLength > 0) //used as long as the matchLength is greater than 0
            {
                matchLength -= Time.deltaTime;

                if (matchLength <= 0) //used when the matchLength is less than 0
                {
                    CheckScores(); //calls a method that will determine whether to go to over time
                }
            }

            if(overTimeActive == true && overTimeLength > 0) //used if overTimeActive is true and if overTimeLength is greater than 0
            {
                overTimeLength -= Time.deltaTime;

                if (overTimeLength <= 0) //used when the matchLength is less than 0
                {
                    status = GameStatus.finished; //our game state is set to finished
                    TurnOffDelay(buttonDelay);
                }
            }
        }

        //If the match is done
        if (status == GameStatus.finished)
        {
            if (Input.anyKeyDown && canEndMatch == true)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    //this method is used to spawn in the players and set there id enum in there PlayerManager script
    public void SpawnPlayers()
    {
        int playersSpawned = 0;

        while (playersSpawned < playerCount)
        {
            GameObject playerClone = Instantiate(playerPrefab, initialSpawnPoints[playersSpawned].position, initialSpawnPoints[playersSpawned].transform.rotation); //spawns the player at the starting spawn points
            players.Add(playerClone.GetComponent<PlayerManager>()); //adds the newly created player to the players list
            players[playersSpawned].GetComponent<PlayerManager>().Initialize(playerID); //insert new method to give the player there ID in here. The current one here is for the old player manager. Erase this  comment once the new method has been added and is called here. Make sure to add a new comment.
            GameObject cameraClone = Instantiate(cameraPrefab);
            cameras.Add(cameraClone.GetComponent<Camera>()); //adds the camera to the cameras list
            playerID += 1;
            playersSpawned += 1;
        }

        CameraSetUp();
    }

    public void CameraSetUp()
    {
        //these set up the cameras depending on the size of the cameras list. Its only able to set up a max of 4 cameras.
        if (cameras.Count == 1)
        {
            cameras[0].GetComponent<Camera>().rect = new Rect(0, 0, 1, 1);
        }

        if (cameras.Count == 2)
        {
            cameras[0].GetComponent<Camera>().rect = new Rect(0, .5f, 1, .5f);
            cameras[1].GetComponent<Camera>().rect = new Rect(0, 0, 1, .5f);
        }

        if (cameras.Count == 3)
        {
            cameras[0].GetComponent<Camera>().rect = new Rect(0, .5f, .5f, .5f);
            cameras[1].GetComponent<Camera>().rect = new Rect(.5f, .5f, .5f, .5f);
            cameras[2].GetComponent<Camera>().rect = new Rect(0, 0, 1, .5f);
        }

        if (cameras.Count == 4)
        {
            cameras[0].GetComponent<Camera>().rect = new Rect(0, .5f, .5f, .5f);
            cameras[1].GetComponent<Camera>().rect = new Rect(.5f, .5f, .5f, .5f);
            cameras[2].GetComponent<Camera>().rect = new Rect(0, 0, .5f, .5f);
            cameras[3].GetComponent<Camera>().rect = new Rect(.5f, 0, .5f, .5f);
        }

        status = GameStatus.inProgress;
    }

    //this handles turning off the player depending on the passed in playerID
    public void RemovePlayer(int playerID)
    {
        players[playerID].gameObject.SetActive(false); //turns off the player
        cameras[playerID].gameObject.SetActive(false); //turns off the player

        playerCount--; //subtracts the playerCount by 1

        if (playerCount == 1)
        {
            status = GameStatus.finished;
            TurnOffDelay(buttonDelay);
        }
    }

    //this'll check whether the game goes to overtime
    private void CheckScores()
    {
        List<PlayerManager> highestScoredPlayers = new List<PlayerManager>(); //stores the players with the highest scores

        for(int i = 0; i < players.Count; i++) //loops through all the players in the players list
        {
            if(players[i].gameObject.activeSelf == true) //checks if the player is active
            {
                if(highestScoredPlayers.Count == 0) //checks if the highestScoredPlayers list count is equal to 0
                {
                    highestScoredPlayers.Add(players[i]); //if so it will add the player to the highestScoredPlayers list
                }

                else if(players[i].GetKillScore() == highestScoredPlayers[0].GetKillScore()) //checks to see if the currently iterated player has the same score as the first player in the highestScoredPlayers list
                {
                    highestScoredPlayers.Add(players[i]); //if so the player will be added the highestScoredPlayers list
                }

                else if(players[i].GetKillScore() > highestScoredPlayers[0].GetKillScore()) //checks to see if the currently iterated player has a higher score compared to the first player in the highestScoredPlayers list
                {
                    highestScoredPlayers.Clear(); //if so the highestScoredPlayers list is cleared out
                    highestScoredPlayers.Add(players[i]); //then the player will be added to the highestScoredPlayers list
                }
            }
        }

        if(highestScoredPlayers.Count >= 2) //checks if the highestScoredPlayers list size is greater than or equal to 2
        {
            overTimeActive = true; //activates over time
        }

        else //used if its not greater than or equal to 2
        {
            status = GameStatus.finished; //our game state is set to finished
            TurnOffDelay(buttonDelay);
        }
    }

    private IEnumerator TurnOffDelay(float time)
    {
        yield return new WaitForSeconds(time);
        canEndMatch = true;
    }
}