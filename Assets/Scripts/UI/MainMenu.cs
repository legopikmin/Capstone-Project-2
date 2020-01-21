using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Alejandro Muros
//10/2/2019
public class MainMenu : MonoBehaviour
{
    public EventSystem eventSystem; //stores the event system

    [Header("Levels")]
    public GameObject levelImageDisplay; //stores the image game object that will display the level image
    public string[] levels; //stores the levels
    public Sprite[] levelImage; //used to display the image for each level

    [Header("UI objects")]
    public GameObject menu; //stores the main menu UI
    public GameObject optionsMenu; //stores the options menu UI
    public GameObject playerSelection; //stores the player selection UI
    public GameObject trackSelection; //stores the track selection UI

    [Header("First Selected Game Object")]
    public GameObject menuObject; //used to store one of the menu button's
    public GameObject playerSelectionObject; //used to store one of the playerSelection button's
    public GameObject optionsObject; //used to store one of the option's buttton's
    public GameObject tracksObject; //used to store one of the track options

    [Header("Buttons")] //stores the buttons for each UI gameobject
    public GameObject[] menuButtons; //used to store the menu buttons in an array
    public GameObject[] playerSelectButtons; //used to store the player select buttons in an array
    public GameObject[] trackSelectButtons; //used to store the track select buttons in an array

    [HideInInspector] public int playerCount; //used to set how many players will be in the game
    private string selectedLevel; //used to set which level to load
    private GameObject lastSelectedGameObject; //used to make sure the menu doesn't break when a mouse is clicked

    void Start()
    {
        //sets the images for the levels on the track selection menu
        for(int i = 0; i < trackSelectButtons.Length; i++)
        {
            trackSelectButtons[i].GetComponent<Image>().sprite = levelImage[i];
        }

        playerCount = 2; //defaults the playerCount to 2
        eventSystem.firstSelectedGameObject = menuObject;
        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
    }

    public void Update()
    {
        ChangeInput();
        SetCurrentSelectedGameObject();

        if (Input.GetButtonDown("ButtonYP1"))
        {
            CloseMenu();
        }

        //sets the image for the levelDisplayImage
        if (trackSelection.activeSelf == true)
        {
            SetLevelImageDisplay();
        }
    }

    private void ChangeInput()
    {
        //used to set the EventSystem verticalAxis to use the d pad to move vertically
        if (Input.GetAxis("DPadVertical") > 0.2 && eventSystem.GetComponent<StandaloneInputModule>().verticalAxis != "DPadVertical" || Input.GetAxis("DPadVertical") < -0.2 && eventSystem.GetComponent<StandaloneInputModule>().verticalAxis != "DPadVertical")
        {
            eventSystem.GetComponent<StandaloneInputModule>().verticalAxis = "DPadVertical";
        }

        //used to set the EventSystem verticalAxis to use the joystick to move vertically
        if (Input.GetAxis("LeftAnalogVerticalP1") > 0.2 && eventSystem.GetComponent<StandaloneInputModule>().verticalAxis != "LeftAnalogVerticalP1" || Input.GetAxis("LeftAnalogVerticalP1") < -0.2 && eventSystem.GetComponent<StandaloneInputModule>().verticalAxis != "LeftAnalogVerticalP1")
        {
            eventSystem.GetComponent<StandaloneInputModule>().verticalAxis = "LeftAnalogVerticalP1";
        }

        //used to set the EventSystem horizontalAxis to use the d pad to move horizontally
        if (Input.GetAxis("DPadHorizontal") > 0.2 && eventSystem.GetComponent<StandaloneInputModule>().horizontalAxis != "DPadHorizontal" || Input.GetAxis("DPadHorizontal") < -0.2 && eventSystem.GetComponent<StandaloneInputModule>().horizontalAxis != "DPadHorizontal")
        {
            eventSystem.GetComponent<StandaloneInputModule>().horizontalAxis = "DPadHorizontal";
        }

        //used to set the EventSystem horizontalAxis to use the joystick to move horizontally
        if (Input.GetAxis("LeftAnalogHorizontalP1") > 0.2 && eventSystem.GetComponent<StandaloneInputModule>().horizontalAxis != "LeftAnalogHorizontalP1" || Input.GetAxis("LeftAnalogHorizontalP1") < -0.2 && eventSystem.GetComponent<StandaloneInputModule>().horizontalAxis != "LeftAnalogHorizontalP1")
        {
            eventSystem.GetComponent<StandaloneInputModule>().horizontalAxis = "LeftAnalogHorizontalP1";
        }
    }

    private void SetCurrentSelectedGameObject()
    {
        //used to store the last selected gameobject when the currentSelectedGameObject is not null
        if (eventSystem.currentSelectedGameObject != null)
        {
            lastSelectedGameObject = eventSystem.currentSelectedGameObject;
        }

        //used to make sure the currentSelectedGameObject is never null
        if (eventSystem.currentSelectedGameObject == null)
        {
            eventSystem.SetSelectedGameObject(lastSelectedGameObject);
        }
    }

    private void SetLevelImageDisplay()
    {
        for (int i = 0; i < trackSelectButtons.Length; i++)
        {
            if (eventSystem.currentSelectedGameObject == trackSelectButtons[i])
            {
                levelImageDisplay.GetComponent<Image>().sprite = trackSelectButtons[i].GetComponent<Image>().sprite;
            }
        }
    }

    private void CloseMenu()
    {
        //closes out the options menu when the cancel button is pressed
        if (optionsMenu.activeSelf == true)
        {
            TurnOnMenu(menuButtons);
            TurnOffUIElements(optionsMenu, menuObject);
        }

        //closes out the player select menu when the cancel button is pressed
        if (playerSelection.activeSelf == true && trackSelection.activeSelf == false)
        {
            TurnOnMenu(menuButtons);
            TurnOffUIElements(playerSelection, menuObject);
        }

        //closes out the track selection menu when the cancel button is pressed
        if (playerSelection.activeSelf == true && trackSelection.activeSelf == true)
        {
            TurnOnMenu(playerSelectButtons);
            TurnOffUIElements(trackSelection, playerSelectionObject);
        }
    }

    //  used for the main menu buttons
    public void OpenPlayerSelect()
    {
        TurnOffMenu(menuButtons);
        TurnOnUIElements(playerSelection, playerSelectionObject);
    }

    public void OpenOptionsMenu()
    {
        TurnOffMenu(menuButtons);
        TurnOnUIElements(optionsMenu, optionsMenu);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    //  end of main menu buttons


    //   used for the option's menu buttons

    //  end of options buttons


    //  used for the player select buttons
    public void TwoPlayers()
    {
        playerCount = 2;
        PlayerPrefs.SetInt("PlayerCount", playerCount);
        TurnOffMenu(playerSelectButtons);
        TurnOnUIElements(trackSelection, tracksObject);
    }

    public void ThreePlayers()
    {
        playerCount = 3;
        PlayerPrefs.SetInt("PlayerCount", playerCount);
        TurnOffMenu(playerSelectButtons);
        TurnOnUIElements(trackSelection, tracksObject);
    }

    public void FourPlayers()
    {
        playerCount = 4;
        PlayerPrefs.SetInt("PlayerCount", playerCount);
        TurnOffMenu(playerSelectButtons);
        TurnOnUIElements(trackSelection, tracksObject);
    }
    //  end of player select buttons

    //Methods that are used to turn stuff on or off in the UI
    //set the interactable bool in the Buttons component to false for each object in one of the buttons array
    private void TurnOffMenu(GameObject[] buttons)
    {
        foreach (GameObject gameObject in buttons)
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
    }

    //set the interactable bool in the Buttons component to true for each object in one of the buttons array
    private void TurnOnMenu(GameObject[] buttons)
    {
        foreach (GameObject gameObject in buttons)
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
    }

    //used to turn on another menu and set a new firstselectobject for it
    public void TurnOnUIElements(GameObject uiMenu, GameObject firstSelectedObj)
    {
        uiMenu.SetActive(true);
        eventSystem.firstSelectedGameObject = firstSelectedObj;
        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
    }

    //used to turn off another menu and set a new firstselectobject for the previous menu
    public void TurnOffUIElements(GameObject uiMenu, GameObject firstSelectedObj)
    {
        eventSystem.firstSelectedGameObject = firstSelectedObj;
        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
        uiMenu.SetActive(false);
    }
    //end of methods used for the UI

    //  used for the track select buttons
    public void LevelOptionOne()
    {
        selectedLevel = levels[0];
        SceneManager.LoadScene(selectedLevel);
    }

    public void LevelOptionTwo()
    {
        selectedLevel = levels[1];
        SceneManager.LoadScene(selectedLevel);
    }

    public void LevelOptionThree()
    {
        selectedLevel = levels[2];
        SceneManager.LoadScene(selectedLevel);
    }
    //  end of track select buttons
}