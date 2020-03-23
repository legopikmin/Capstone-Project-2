using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardObject : MonoBehaviour
{
    public CardData cardData;
    
    [Header("Text Component References")]
    public Text nameText;
    public Text energyCostText;
    public Text descriptionText;
    public Text cardTypeText;

    [Header("Image References")]
    public Image cardTopRibbonImage;
    public Image cardLowRibbonImage;
    public Image cardGraphicImage;
    public Image cardBodyImage;
    public Image cardFaceFrameImage;
    public Image cardFaceGlowImage;

    void Awake()
    {
        if (cardData != null)         //TODO will eventually move the ReadFromCardData to the script where we initialize the cards
            ReadCardFromData();
    }

    public void ReadCardFromData()
    {
        // universal actions for any Card

        // 1) add card name
        nameText.text = cardData.cardName;
        // 2) add mana cost
        energyCostText.text = cardData.energyCost.ToString();
        // 3) add description
        descriptionText.text = cardData.cardDescription;
        // 4) Change the card graphic sprite
        cardGraphicImage.sprite = cardData.cardImage;
        // 5) Change the card color text
        cardTypeText.text = cardData.cardColor.ToString();
    }

    public void OnCardPlayed(PlayerManager player)
    {
        for (int i = 0; i < cardData.onCardPlayedEffects.Count; i++)
        {
            cardData.onCardPlayedEffects[i].ActivateEffect(player, this);
        }
    }

    public void OnSlotPlayed(PlayerManager player)
    {
        for (int i = 0; i < cardData.onSlotActivatedEffects.Count; i++)
        {
            cardData.onSlotActivatedEffects[i].ActivateEffect(player, this);
        }
    }

    public void SetLayerRecursively(int layerID)
    {
        this.gameObject.layer = layerID;

        foreach (Transform child in this.transform)
        {
            SetLayerRecursively(child.gameObject, layerID);
        }
    }
    void SetLayerRecursively(GameObject obj, int layerID)
    {
        obj.layer = layerID;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layerID);
        }
    }
}
