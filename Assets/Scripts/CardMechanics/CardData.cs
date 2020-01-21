using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType {OneShot, Ongoing }
public enum CardColor {Red, Black, White, Green, Blue, Colorless }

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : ScriptableObject
{
    [Header ("Card Identifiers")]
    public string idCode;   //Stores unique identifier for the card
    public string cardName;   //Name of the card

    [Header ("General Info")]
    public int energyCost;   //How much energy the card costs
    [TextArea(2, 3)]
    public string cardDescription;   //contains the cards body text
    public CardType type;  //what type of card is this
    public CardColor cardColor;   //defines the color of the card

    [Header("Images")]
    public Sprite cardImage;  //image for the card

    [Header("Effect Parameters")]
    public List<Effect> onCardPlayedEffects = new List<Effect>();
    public List<Effect> onSlotActivatedEffects = new List<Effect>();
}
