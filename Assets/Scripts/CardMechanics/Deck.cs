using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

///*
///Deck Behaviour Script
///Made 09/18/2019
///by Connery Ray
///Edits made 09/19/19
///made changes to the test methods removing items that weren't needed
///also marked items that were for testing purposes
///09/25/19
///Removed the rest of the test items


public class Deck : MonoBehaviour
{
    [SerializeField] private List<CardData> deck = new List<CardData>();
    [SerializeField] private List<CardData> discardPile = new List<CardData>();

    [SerializeField] private TMP_Text deckCount;
    public HandManager myHand;

    /// <summary>
    /// updates deck UI at the start of the game
    /// </summary>
    void Awake()
    {
        UpdateDeckUI();
    }

    void Update()
    {

        ////shuffles the deck using fisher yates when space is pressed
        //if(Input.GetKeyUp(KeyCode.Space))
        //{
        //    Shuffle();
        //}

        ////returns the first card in hand to the top of deck
        //if(Input.GetKeyUp(KeyCode.W))
        //{
        //    ReturnToTop(myHand[0]);    //myHand[0] is a test method for now
        //}

        ////returns the first card in hand to the bottom of deck
        //if(Input.GetKeyUp(KeyCode.S))
        //{
        //    ReturnToBottom(myHand[0]);   //myHand[0] is a test method for now
        //}

        ////takes the first card in hand and adds it to the discard pile
        //if(Input.GetKeyUp(KeyCode.D))
        //{
        //    AddToDiscard(myHand[0]);         //myHand[0] is a test method for now
        //}
    }

    /// <summary>
    /// uses the fisher yates shuffle to shuffle cards
    /// </summary>
    void Shuffle()
    {
        int size = deck.Count;

        while (size > 1)
        {
            size--;
            int r = Random.Range(0, size);
            CardData swap = deck[r];
            deck[r] = deck[size];
            deck[size] = swap;
        }
    }


    /// <summary>
    /// Allows drawing of cards from the first space in the deck
    /// </summary>
    /// <returns>returns null if discard pile is empty</returns>
    public CardData DrawCard()
    {
        if (deck.Count == 0)  // checks if deck has cards
        {
            
            deck.AddRange(discardPile); //add discard pile back to deck
            Shuffle();                  //runs shuffle algorithm
            UpdateDeckUI();
            
            discardPile = new List<CardData>(); //resets discard pile

            if (discardPile.Count == 0 && deck.Count == 0) //makes sure deck and hand aren't empty to allow card draw with reshuffle
            {
                return null;
            }
            else
            {
                CardData card = deck[0];
                deck.RemoveAt(0);
                UpdateDeckUI();

                return card;
            }

        }
        else
        { 
    
            CardData card = deck[0];       //draws top card of the deck
            deck.RemoveAt(0);
            UpdateDeckUI();

            return card;
        }

    }

    //takes the card passed in and returns it to the top of the deck
    public bool ReturnToTop(CardData card)
    {

        if (card != null)
        {
            deck.Insert(0, card);
            UpdateDeckUI();

            return true;
        }
        else
        {
            return false;
        }
    }

    //takes the card passed in and returns it to the bottom of the deck
    public bool ReturnToBottom(CardData card)
    {

        if (card != null)
        {
            deck.Add(card);
            UpdateDeckUI();

            return true;
        }
        else
        {
            return false;
        }
    }

    //takes the passed in card and adds it to the discard pile
    public CardData AddToDiscard(CardData card)
    {
        if(card != null)
        {
            discardPile.Add(card);

            return null;
        }
        else
        {
            return null;
        }
    }

    void UpdateDeckUI()
    {

        //updates the deck count UI
        deckCount.text = deck.Count.ToString();

    }
}
