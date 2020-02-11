using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class HandManager : MonoBehaviour
{
    #region Variables

    //Mulligan is while the player is choosing which cards to mulligan, Ready is after they mulligan but before match start, and Match is during the match
    public enum HandState { Mulligan, Ready, Match };


    [Header("Hand References")]
    public List<CardObject> cardsInHand = new List<CardObject>();
    public int initialHandSize = 3;
    [SerializeField] int maxHandSize = 4;

    [Header("Timer References")]
    public float discardHoldTimer = 2.0f;
    public float discardHoldDuration = 2.0f;
    public float cardCycleTimer = 1.0f;
    public float cardCycleDelay = 0.1f;
    public bool canDiscard = true;

    [Header("Slotting References")]
    [SerializeField] private CardData[] slots = new CardData[3];//Array of slots for the card data to be stored and called upon.
    [SerializeField] Transform[] slotPositions;//These will be the positions the cards end up in when we slot them.
    [SerializeField] private CardObject[] storedObject = new CardObject[3];//Array for the stored gameobjects that is needed to delete the gameobject when we replace slotted cards.
    [SerializeField] private CardData toBeSlotted;//stored area for a card to be slotted
    [SerializeField] private bool slottingInProgress = false;
    //[SerializeField] private float playerEnergy = 10;//Energy placeholder to test code, may need to be changed for the final project.
    public GameObject slottingUI; //Alex; made this to control the slotting UI that contains the slotting message. (11/13/2019)
    [SerializeField][Range(0, 0.4f)]
    private float slotCardScale = .3f;//The scale for card objects once they are slotted


    [Header("Other References")]
    [SerializeField] Deck myDeck;
    public int selectedCardIndex;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform[] cardPositions;
    public HandState currentState = HandState.Mulligan;
    public CardManager cardManager;

    [Header("Mulligan Variables")]
    public bool[] cardSelectedForMulligan;
    [SerializeField] int maxMulliganAmount = 1;
    [SerializeField] float mulliganDuration = 30; //THIS SHOULD BE MOVED TO MATCH MANAGER ONCE IT IS MADE
    public float mulliganCountdown; //THIS SHOULD BE MOVED TO MATCH MANAGER ONCE IT IS MADE
    public TMP_Text mulliganCountdownText;
    public TMP_Text mulliganInstructionText;

    #endregion


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        cardCycleTimer += Time.deltaTime;

        //If they are not pressing the dpad left or right, set the timer high enough so that when they do press a direction it cycles immediately
        if (Input.GetAxis("DPadHorizontal" + cardManager.playerControllerID) == 0)
        {
            cardCycleTimer = cardCycleDelay;
        }

        //If they are pressing the dpad left or right, cycle between selected cards
        if (cardCycleTimer >= cardCycleDelay)
        {
            if (Input.GetAxis("DPadHorizontal" + cardManager.playerControllerID) == 1 && slottingInProgress == false)
            {
                SelectNextCard();
                cardCycleTimer = 0;
                canDiscard = false;
            }
            else if (Input.GetAxis("DPadHorizontal" + cardManager.playerControllerID) == -1 && slottingInProgress == false)
            {
                SelectPreviousCard();
                cardCycleTimer = 0;
                canDiscard = false;
            }
        }

        switch (currentState)
        {
            case HandState.Mulligan:
                if (Input.GetButtonDown("ButtonA" + cardManager.playerControllerID))
                {
                    ToggleMulliganSelection();
                }

                if (Input.GetAxis("DPadVertical" + cardManager.playerControllerID) == -1)
                {
                    if (canDiscard)
                    {
                        discardHoldTimer += Time.deltaTime;
                        if (discardHoldTimer > discardHoldDuration)
                        {
                            //Discard all of the selected cards
                            for (int i = initialHandSize - 1; i >= 0; i--)
                            {
                                if (cardSelectedForMulligan[i])
                                {
                                    cardSelectedForMulligan[i] = false;
                                    DiscardCard(i);
                                }
                            }

                            //Draw cards to replace the discarded ones
                            for (int i = cardsInHand.Count; i < initialHandSize; i++)
                            {
                                AddCard(myDeck.DrawCard());
                            }

                            discardHoldTimer = 0;
                            canDiscard = false;

                            currentState = HandState.Ready;
                        }
                    }
                }
                else
                {
                    //They are not pressing 'discard', so reset the timer
                    discardHoldTimer = 0;
                    canDiscard = true;
                }

                //Reduce the countdown
                mulliganCountdown -= Time.deltaTime;
                mulliganCountdownText.text = ((int)mulliganCountdown).ToString();
                //If the countdown is at 0, disable mulligans and start the match
                if (mulliganCountdown <= 0)
                {
                    currentState = HandState.Match;
                    UpdateCardPositions();
                    mulliganCountdownText.gameObject.SetActive(false);
                    mulliganInstructionText.gameObject.SetActive(false);
                }
                break;

            case HandState.Ready:
                //Reduce the countdown
                mulliganCountdown -= Time.deltaTime;
                mulliganCountdownText.text = ((int)mulliganCountdown).ToString();
                //If the countdown is at 0, disable mulligans and start the match
                if (mulliganCountdown <= 0)
                {
                    currentState = HandState.Match;
                    mulliganCountdownText.gameObject.SetActive(false);
                    mulliganInstructionText.gameObject.SetActive(false);
                }
                break;


            case HandState.Match:
                //Check if 'Discard' is currently held down
                if (Input.GetAxis("DPadVertical" + cardManager.playerControllerID) == -1 && slottingInProgress == false)
                {
                    if (canDiscard)
                    {
                        discardHoldTimer += Time.deltaTime;
                        if (discardHoldTimer > discardHoldDuration)
                        {
                            //Discard the selected card, reset discard timer, and prevent players from discarding another card without releasing and repressing the discard button.
                            DiscardCard(selectedCardIndex);
                            discardHoldTimer = 0;
                            canDiscard = false;
                        }
                    }
                }
                else
                {
                    //They are not pressing 'discard', so reset the timer
                    discardHoldTimer = 0;
                    canDiscard = true;
                }

                if (Input.GetButtonDown("ButtonA" + cardManager.playerControllerID) && cardsInHand.Count>0)
                {
                    CardObject co = GetSelectedCard();
                    if (co != null)
                    {
                        CardData c = GetSelectedCard().cardData;

                        if (c.type == CardType.Ongoing && slottingInProgress == false)//if the card can be slotted, save that card to a special area, then promt the player to slot the card
                        {
                            slottingInProgress = true;
                            slottingUI.SetActive(true);
                            toBeSlotted = c;
                        }
                        else if (slottingInProgress == true)//if the player wishes to not slot the card, press the A button again to cancel the slotting
                        {
                            slottingUI.SetActive(false);
                            toBeSlotted = null;
                            slottingInProgress = false;
                        }
                        else//else, just play the card as normal
                        {
                            PlaySelectedCard();
                        }
                    }
                }

                //The three inputs all do the same thing, just change where the card is slotted to.
                if (Input.GetButtonDown("ButtonX" + cardManager.playerControllerID))
                {
                    slottingUI.SetActive(false);
                    if (toBeSlotted != null)
                    {
                        AddCardToSlot(0, toBeSlotted);
                        toBeSlotted = null;
                        slottingInProgress = false;
                    }
                    else if (slots[0] != null)//If there are no cards in the hand, and there is a card in the slot, it will activate the card.
                    {
                        ActivateSlottedCard(0);
                    }
                }

                if (Input.GetButtonDown("ButtonY" + cardManager.playerControllerID))
                {
                    slottingUI.SetActive(false);
                    if (toBeSlotted != null)
                    {
                        AddCardToSlot(1, toBeSlotted);
                        toBeSlotted = null;
                        slottingInProgress = false;
                    }
                    else if (slots[1] != null)
                    {
                        ActivateSlottedCard(1);
                    }
                }

                if (Input.GetButtonDown("ButtonB" + cardManager.playerControllerID))
                {
                    slottingUI.SetActive(false);
                    if (toBeSlotted != null)
                    {
                        AddCardToSlot(2, toBeSlotted);
                        toBeSlotted = null;
                        slottingInProgress = false;
                    }
                    else if (slots[2] != null)
                    {
                        ActivateSlottedCard(2);
                    }

                }
                break;
        }
    }

    /// <summary>
    /// Set the hand up for the start of the match
    /// </summary>
    public void InitializeHand()
    {
        //Initialize Arrays
        cardsInHand = new List<CardObject>();
        cardSelectedForMulligan = new bool[initialHandSize];

        //Draw initial cards from deck
        for (int i = 0; i < initialHandSize; i++)
        {
            AddCard(myDeck.DrawCard());
        }

        //Start mulliganing
        mulliganCountdown = mulliganDuration;
        currentState = HandState.Mulligan;
    }

    /// <summary>
    /// Instantiates a CardObject, assigns it the passed in CardData, and adds it to the hand
    /// </summary>
    /// <param name="c">the card data to </param>
    /// <returns>true if card is added to hand successfully and false if it is not</returns>
    public bool AddCard(CardData c)
    {
        if (c != null)
        {
            //Check if we have space left in the hand for another card
            if (cardsInHand.Count < maxHandSize)
            {
                //Set up the card prefab
                GameObject card = Instantiate(cardPrefab, new Vector3(0, -5, 0), Quaternion.identity);
                CardObject newCard = card.GetComponent<CardObject>();
                newCard.SetLayerRecursively(LayerMask.NameToLayer(cardManager.playerControllerID));
                newCard.cardData = c;
                newCard.ReadCardFromData();

                //Add the CardObject to hand and adjust selected card and positions as needed
                cardsInHand.Insert(0, newCard);
                SelectNextCard();
                UpdateCardPositions();
                HighlightSelectedCard();
                return true;
            }
            else
            {
                //Since the hand was full send the new card to the discard pile
                myDeck.AddToDiscard(c);
            }
        }
        return false;
    }


    public bool CanDrawCard()
    {
        return cardsInHand.Count < maxHandSize;
    }

    #region Discard Methods

    /// <summary>
    /// Remove the CardObject of the card at the given index from the hand and add its CardData to the discard pile
    /// </summary>
    /// <param name="index">The index of the card to discard</param>
    /// <returns>true if the card is discarded successfully and false if it is not</returns>
    public bool DiscardCard(int index)
    {
        //Make sure the index is valid
        if (index >= 0 && index < cardsInHand.Count)
        {
            //If certain cards are discarded we need to adjust the selected card
            if (index < selectedCardIndex || index == cardsInHand.Count - 1)
            {
                SelectPreviousCard();
            }

            //Delete the CardObject and add the CardData to the discard
            CardObject co = cardsInHand[index];
            myDeck.AddToDiscard(co.cardData);
            cardsInHand.Remove(co);
            Destroy(co.gameObject);

            //Adjust selected card and positions as needed
            HighlightSelectedCard();
            UpdateCardPositions();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Remove the CardObject the has the passed in CardData the hand and add that CardData to the discard pile
    /// </summary>
    /// <param name="c">The CardData of the card to attempt to discard</param>
    /// <returns>true if the card is discarded successfully and false if it is not</returns>
    public bool DiscardCard(CardData c)
    {
        int i = 0;

        foreach (CardObject card in cardsInHand)
        {
            if (card.cardData.Equals(c))
            {
                return DiscardCard(i);
            }
            i++;
        }

        return false;
    }

    /// <summary>
    /// Discards the currently selected card from the hand
    /// </summary>
    /// <returns>true if the card is discarded successfully and false if it is not</returns>
    public bool DiscardSelectedCard()
    {
        return DiscardCard(selectedCardIndex);
    }

    /// <summary>
    /// Picks a random card in hand to send to the discard pile
    /// </summary>
    /// <returns>true if the card is discarded successfully and false if it is not</returns>
    public bool DiscardRandomCard()
    {
        int r = Random.Range(0, cardsInHand.Count);

        return DiscardCard(r);
    }

        #endregion

        /// <summary>
        /// Highlight the currently selected card with a glow effect and disable the glow effect on each other card
        /// </summary>
            public void HighlightSelectedCard()
    {
        int i = 0;
        foreach (CardObject card in cardsInHand)
        {
            if (i == selectedCardIndex)
            {
                card.cardFaceGlowImage.enabled = true;
            }
            else
            {
                card.cardFaceGlowImage.enabled = false;
            }
            i++;
        }
    }

    /// <summary>
    /// Update the position of each card in hand so that they are arranged properly in the hand
    /// </summary>
    public void UpdateCardPositions()
    {
        //TODO Update this method to use DoTween or another technique to actually animate cards moving between the deck or slots in hand
        //TODO Update the method so that the card positions are more dynamic
        int i = 0;
        foreach (CardObject card in cardsInHand)
        {
            card.transform.position = cardPositions[i].position;
            card.transform.rotation = cardPositions[i].rotation;
            card.transform.parent = cardPositions[i];
            i++;
        }
    }

    /// <summary>
    /// Select the next card to the right. Wraps around as needed
    /// </summary>
    public void SelectNextCard()
    {
        int previousIndex = selectedCardIndex;

        if (selectedCardIndex >= cardsInHand.Count - 1)
        {
            selectedCardIndex = 0;
        }
        else
        {
            selectedCardIndex++;
        }

        if (previousIndex != selectedCardIndex)
        {
            HighlightSelectedCard();
        }
    }

    /// <summary>
    /// Select the next card to the left. Wraps around as needed
    /// </summary>
    public void SelectPreviousCard()
    {
        int previousIndex = selectedCardIndex;

        if (selectedCardIndex <= 0)
        {
            selectedCardIndex = cardsInHand.Count - 1;
        }
        else
        {
            selectedCardIndex--;
        }

        if (previousIndex != selectedCardIndex)
        {
            HighlightSelectedCard();
        }
    }

    /// <summary>
    /// Returns the CardObject currently selected in hand
    /// </summary>
    /// <returns>The CardObject currently selected in hand or null if no card is selected</returns>
    public CardObject GetSelectedCard()
    {
        if (selectedCardIndex < cardsInHand.Count && selectedCardIndex >= 0)
        {
            return cardsInHand[selectedCardIndex];
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Pay the costs for the selected card and activate any of its effects
    /// </summary>
    /// <returns>true if the card was played successfully and false otherwise</returns>
    public bool PlaySelectedCard()
    {

        if (CanPlaySelectedCard())
        {
            cardManager.playerManager.ReduceEnergy(GetSelectedCard().cardData.energyCost);
            cardsInHand[selectedCardIndex].GetComponent<CardObject>().OnCardPlayed(GetComponentInParent<PlayerManager>());
            DiscardSelectedCard();
        }
        return false;
    }

    /// <summary>
    /// Checks the players energy level versus the card energy level to determine if card is playable
    /// </summary>
    /// <returns>true if the card can currently be played or false otherwise</returns>
    public bool CanPlaySelectedCard()
    {
        return cardManager.playerManager.currentEnergy >= GetSelectedCard().cardData.energyCost;
    }

    #region Card Slots
    public bool AddCardToSlot(int slotID, CardData c)
    {
        //Creates a card object to be stored to delete later on if/when we store a new card in the slot.
        CardObject co = GetSelectedCard();

        if (slots[slotID] != null)//Discards the card currently in the slot and puts the new card in that slot
        {
            myDeck.AddToDiscard(slots[slotID]);

            GetSelectedCard().transform.position = slotPositions[slotID].position;
            GetSelectedCard().transform.localScale = new Vector3(slotCardScale, slotCardScale, slotCardScale);
            GetSelectedCard().transform.parent = slotPositions[slotID];
            slots[slotID] = c;
            Destroy(storedObject[slotID].gameObject);//This will get rid of the game object in the scene to keep clones of card objects to a minimum
            storedObject[slotID] = co;//Sets up the stored object with the new card object
            co.cardFaceGlowImage.enabled = false;//Turns off the highlighting for the slotted card

            //Forces the selected card index to go back to 0 to avoid errors. Also updats the position of the cards to make the UI cleaner.
            cardsInHand.Remove(co);
            SelectPreviousCard();
            UpdateCardPositions();
            return true;
        }
        else if (c != null)//If there is no card already in the slot, set the current selected card as the new card for the slot.
        {
            GetSelectedCard().transform.position = slotPositions[slotID].position;
            GetSelectedCard().transform.localScale = new Vector3(slotCardScale, slotCardScale, slotCardScale);
            GetSelectedCard().transform.parent = slotPositions[slotID];
            slots[slotID] = c;
            storedObject[slotID] = co;
            co.cardFaceGlowImage.enabled = false;

            //Removes the card that was slotted from the hand to keep it from being selected.

            cardsInHand.Remove(co);
            SelectPreviousCard();
            UpdateCardPositions();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool ActivateSlottedCard(int slotID)
    {
        if (CanActivateSlottedCard(slotID))
        {
            cardManager.playerManager.ReduceEnergy(slots[slotID].energyCost);
            storedObject[slotID].GetComponent<CardObject>().OnSlotPlayed(GetComponentInParent<PlayerManager>());
        }
        return true;
    }

    public bool CanActivateSlottedCard(int slotID)
    {
        return cardManager.playerManager.currentEnergy >= slots[slotID].energyCost;
    }
    #endregion

    #region Mulligan Mechanics

    /// <summary>
    /// Switches the currently highlighted card between being mulliganed or not.
    /// </summary>
    public void ToggleMulliganSelection()
    {
        //Make sure we aren't selecting more than the maxMulliganAmount
        if (cardSelectedForMulligan[selectedCardIndex] || cardSelectedForMulligan.Where(b => b == true).Count() < maxMulliganAmount)
        {
            //Toggle whether the card is selected or not, and move it up or down as necessary
            cardSelectedForMulligan[selectedCardIndex] = !cardSelectedForMulligan[selectedCardIndex];
            GetSelectedCard().transform.position = cardPositions[selectedCardIndex].position + (cardSelectedForMulligan[selectedCardIndex] ? new Vector3(0, .5f, 0) : Vector3.zero);
        }
    }

    #endregion
}
