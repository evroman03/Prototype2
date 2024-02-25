using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    //Makes Class a Singleton Class.
    #region Singleton
    private static UIManager instance;
    private GameManager gameManager;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindAnyObjectByType(typeof(UIManager)) as UIManager;
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    #endregion

    //Get data
    [SerializeField] private Image dealtCard1, dealtCard2, dealtCard3, dealtCard4, storedCard;
    [SerializeField] private Sprite moveCardSprite, JumpCardSprite, TurnRightCardSprite,TurnLeftCardSprite,
                                    BackToItCardSprite, SwitchCardSprite, ClearCardSprite;
    [SerializeField] DeckManager deckManager;
    [SerializeField] private TextMeshProUGUI readyText;
    
    //Delcare Variables
    List<Card> dealtCards;
    Card storedCardData;


    /**
     * Initializes variables for UIManager
     */
    public void InitUIManager()
    {
        //Initiate Variables
        gameManager = GameManager.Instance;
        dealtCards = new List<Card>();
    }

    /**
     * Enables and Disables the card images depending if the card slot is null or not
     */
    public void CheckDealtCards()
    {
        int dealtCardsCount = dealtCards.Count;
        storedCardData = deckManager.GetStoredCard();

        //If there is at least 1 card in the dealtCards deck, show the first dealt card
        if (dealtCardsCount > 0)
            dealtCard1.enabled = true;
        else
            dealtCard1.enabled = false;

        //If there is at least 2 cards in the dealtCards deck, show the second dealt card
        if (dealtCardsCount > 1)
            dealtCard2.enabled = true;
        else
            dealtCard2.enabled = false;

        //If there is at least 3 cards in the dealtCards deck, show the second dealt card
        if (dealtCardsCount > 2)
            dealtCard3.enabled = true;
        else
            dealtCard3.enabled = false;

        //If there is at least 4 cards in the dealtCards deck, show the second dealt card
        if (dealtCardsCount > 3)
            dealtCard4.enabled = true;
        else
            dealtCard4.enabled = false;

        //If there is a stored card, show the stored card
        if (storedCardData != null)
        {
            storedCard.enabled = true;
            readyText.enabled = true;
        }
        else
        {
            storedCard.enabled = false;
            readyText.enabled = false;
        }
    }

    /**
     * Plays the selected card
     */
    public void PlayCard()
    {
        if (gameManager.gameState == GameManager.STATE.ChooseCards)
        {
            if (storedCardData != null && storedCardData.GetClicked() && deckManager.GetStoredCardWait() < 1)
            {
                deckManager.PlayDealtCard(-1);

                CheckDealtCards();
                gameManager.ChangeGameState(GameManager.STATE.Lv1);
            }

            int dealtCardsCount = dealtCards.Count;
            for (int i = 0; i < dealtCardsCount; i++)
            {
                if (dealtCards[i].GetClicked())
                {
                    deckManager.PlayDealtCard(i);
                    dealtCardsCount--;

                    CheckDealtCards();
                    gameManager.ChangeGameState(GameManager.STATE.Lv1);
                    return;
                }
            }
        }
    }

    /**
     * Stores the selected card
     */
    public void StoreCard()
    {
        if (gameManager.gameState == GameManager.STATE.ChooseCards)
        {
            int dealtCardsCount = dealtCards.Count;
            for (int i = 0; i < dealtCardsCount; i++)
            {
                if (dealtCards[i].GetClicked())
                {
                    //Checks to see if the stored card is not the same as the one attempting to be stored
                    if (storedCardData == null || deckManager.GetStoredCardWait() < 1)
                    {
                        deckManager.SetStoredCard(dealtCards[i]);
                        dealtCardsCount--;
                        CheckDealtCards();
                        UpdateReadyText();
                        break;
                    }
                }
            }
        }
    }

    /**
     * If the Card is clicked, automatically calls this method.
     */
    public void DealtCardClicked(int cardClicked)
    {
        if (gameManager.gameState == GameManager.STATE.ChooseCards)
        {
            //Checks if the stored card was clicked
            if (cardClicked == -1)
            {
                if (storedCardData != null)
                    storedCardData.SetClicked(true);
                return;
            }
            else if (storedCardData != null)
                storedCardData.SetClicked(false);

            int dealtCardsCount = dealtCards.Count;

            //Sets all cards to not clicked
            for (int i = 0; i < dealtCardsCount; i++)
            {
                dealtCards[i].SetClicked(false);
            }

            //Sets the card that was clicked to clicked
            dealtCards[cardClicked - 1].SetClicked(true);
        }
    }

    public void PlayedCardClicked()
    {
        if (gameManager.gameState == GameManager.STATE.SwitchCards) {
            //TODO
        }
    }

    public void UpdateDealtCards()
    {
        dealtCards = deckManager.GetDealtCards();
    }

    /**
     * Updates the Dealt Card Sprites
     */
    public void UpdateImages()
    {
        //Gathers stored card data
        storedCardData = deckManager.GetStoredCard();

        //Checks if the data is not null
        if (dealtCards[0] != null)
        {
            UpdateImageSprite(dealtCard1, 0);
        }
        if (dealtCards[1] != null)
        {
            UpdateImageSprite(dealtCard2, 1);
        }
        if (dealtCards[2] != null)
        {
            UpdateImageSprite(dealtCard3, 2);
        }
        if (dealtCards[3] != null)
        {
            UpdateImageSprite(dealtCard4, 3);
        }
        if (storedCardData != null)
        {
            //Checks between the possible card names
            switch (storedCardData.name)
            {
                //If Move Card is present
                case "Move Card":
                    storedCard.sprite = moveCardSprite;
                    break;
                //If Jump Card is present
                case "Jump Card":
                    storedCard.sprite = JumpCardSprite;
                    break;
                //If Turn Right Card is present
                case "Turn Right Card":
                    storedCard.sprite = TurnRightCardSprite;
                    break;
                //If Turn Left Card is present
                case "Turn Left Card":
                    storedCard.sprite = TurnLeftCardSprite;
                    break;
                //If Back To It Card is present
                case "Back To It Card":
                    storedCard.sprite = BackToItCardSprite;
                    break;
                //If Switch Card is present
                case "Switch Card":
                    storedCard.sprite = SwitchCardSprite;
                    break;
                //If Clear Card is present
                case "Clear Card":
                    storedCard.sprite = ClearCardSprite;
                    break;
                //If Card name is invalid
                default:
                    print("ERROR: FAILED TO CHANGE DEALT CARD 1 SPRITE");
                    break;
            }
        }
    }

    /**
     * Helper Method for UpdateImages
     * 
     * Checks the possible names for the cards and then updates the sprites
     */
    private void UpdateImageSprite(Image dealtCardImage, int dealtCardIndex)
    {
        //Checks between possible card names
        switch (dealtCards[dealtCardIndex].name)
        {
            //If Move Card is present
            case "Move Card":
                dealtCardImage.sprite = moveCardSprite;
                break;
            //If Jump Card is present
            case "Jump Card":
                dealtCardImage.sprite = JumpCardSprite;
                break;
            //If Turn Right Card is present
            case "Turn Right Card":
                dealtCardImage.sprite = TurnRightCardSprite;
                break;
            //If Turn Left Card is present
            case "Turn Left Card":
                dealtCardImage.sprite = TurnLeftCardSprite;
                break;
            //If Back To It Card is present
            case "Back To It Card":
                dealtCardImage.sprite = BackToItCardSprite;
                break;
            //If Switch Card is present
            case "Switch Card":
                dealtCardImage.sprite = SwitchCardSprite;
                break;
            //If Clear Card is present
            case "Clear Card":
                dealtCardImage.sprite = ClearCardSprite;
                break;
            //If card name is invalid
            default:
                print("ERROR: FAILED TO CHANGE DEALT CARD 1 SPRITE");
                break;
        }
    }

    public void UpdateReadyText()
    {
        readyText.text = "Ready In: " + deckManager.GetStoredCardWait();
    }
}
