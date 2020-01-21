using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [Header("Script References")]
    Deck deckReference;
    HandManager handReference;

    [HideInInspector] public PlayerManager playerManager;
    [HideInInspector] public string playerControllerID; //this is used to determine which player this script is for

    // Gets called by the PlayerManager script once it gives the CardManager a reference to it
    public void Initialize()
    {
        playerControllerID = playerManager.id.ToString();
        deckReference = gameObject.GetComponentInChildren<Deck>();

        handReference = gameObject.GetComponentInChildren<HandManager>();
        handReference.cardManager = this;
        handReference.InitializeHand();
    }

    // Update is called once per frame
    void Update()
    {
        ////draws a card when A is pressed
        if (Input.GetKeyUp(KeyCode.A))
        {
            handReference.AddCard(deckReference.DrawCard());
        }
    }

    public void DrawCard()
    {
        if (handReference.CanDrawCard())
        {
            handReference.AddCard(deckReference.DrawCard());
        }
    }
}