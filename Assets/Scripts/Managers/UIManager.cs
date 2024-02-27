using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    //Makes Class a Singleton Class.
    #region Singleton
    private static UIManager instance;
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
    private GameManager gameManager;
    //Get data
    [SerializeField] private Canvas gameUI, playedCardsUI;
    [SerializeField] private int maxNumOfCardsPlayed = 12;
    [SerializeField] private Image dealtCard1, dealtCard2, dealtCard3, dealtCard4, storedCard;
    [SerializeField] private Image[] playedCards;
    [SerializeField] private Sprite moveCardSprite, JumpCardSprite, TurnRightCardSprite,TurnLeftCardSprite,
                                    BackToItCardSprite, SwitchCardSprite, ClearCardSprite;
    [SerializeField] DeckManager deckManager;
    [SerializeField] private Button returnButton;
    [SerializeField] private TextMeshProUGUI readyText;
    [SerializeField] private TextMeshProUGUI shuffleText;
    
    //Delcare Variables
    List<Card> dealtCards;
    List<Card> playedCardsData;
    Card storedCardData;


    /**
     * Initializes variables for UIManager
     */
    public void InitUIManager()
    {
        //Initiate Variables
        gameManager = GameManager.Instance;
        dealtCards = new List<Card>();
        gameUI.enabled = true;
        playedCardsUI.enabled = false;

        for (int i = 0; i < maxNumOfCardsPlayed;  i++)
        {
            playedCards[i].enabled = false;
        }
        returnButton.gameObject.SetActive(false);
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
                return;
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
                        UpdateDealtCardsImages();
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

    /**
     * Called when a played card is clicked
     */
    public void PlayedCardClicked(int cardClicked)
    {
        //Checks the GameState
        if (gameManager.gameState == GameManager.STATE.SwitchCards) {
            playedCardsData = deckManager.GetPlayedCards();
            int playedCardsCount = playedCardsData.Count;

            //Sets the card that was clicked to clicked
            playedCardsData[cardClicked - 1].SetClicked(true);

            //Calls swap method
            deckManager.SwapTwoCards();
        }
    }

    public void UpdateDealtCards()
    {
        dealtCards = deckManager.GetDealtCards();
    }

    /**
     * Updates the Dealt Card Sprites
     */
    public void UpdateDealtCardsImages()
    {
        //Gathers stored card data
        storedCardData = deckManager.GetStoredCard();

        //Gets how many dealt cards there are
        int dealtCardsCount = dealtCards.Count;

        //Checks if the data is not null
        //If it is not null, display the correct image
        if (dealtCardsCount > 0 && dealtCards[0] != null)
        {
            UpdateImageSprite(dealtCard1, 0, true);
        }
        if (dealtCardsCount > 1 && dealtCards[1] != null)
        {
            UpdateImageSprite(dealtCard2, 1, true);
        }
        if (dealtCardsCount > 2 && dealtCards[2] != null)
        {
            UpdateImageSprite(dealtCard3, 2, true);
        }
        if (dealtCardsCount > 3 && dealtCards[3] != null)
        {
            UpdateImageSprite(dealtCard4, 3, true);
        }
        if (storedCardData != null)
        {
            //Check for the stored card
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
     * Updates the played cards sprites
     */
    public void UpdatePlayedCardsImage()
    {
        playedCardsData = deckManager.GetPlayedCards();
        int playerCardCount = playedCardsData.Count;

        //Enables images for the played cards
        for (int i = 0; i < playerCardCount; i++)
        {
            if (playedCardsData[i] != null)
            {
                playedCards[i].enabled = true;
                UpdateImageSprite(playedCards[i], i, false);
            }
            else
                playedCards[i].enabled = false;
        }

        //Disables all other card images
        for (int i = playerCardCount; i < maxNumOfCardsPlayed; i++)
        {
            playedCards[i].enabled = false;
        }
    }

    /**
     * Helper Method for UpdateImages
     * 
     * Checks the possible names for the cards and then updates the sprites
     */
    private void UpdateImageSprite(Image cardImage, int cardIndex, bool changeDealtCards)
    {
        //Changes dealt card images
        if (changeDealtCards) {
            //Checks between possible card names
            switch (dealtCards[cardIndex].name)
            {
                //If Move Card is present
                case "Move Card":
                    cardImage.sprite = moveCardSprite;
                    break;
                //If Jump Card is present
                case "Jump Card":
                    cardImage.sprite = JumpCardSprite;
                    break;
                //If Turn Right Card is present
                case "Turn Right Card":
                    cardImage.sprite = TurnRightCardSprite;
                    break;
                //If Turn Left Card is present
                case "Turn Left Card":
                    cardImage.sprite = TurnLeftCardSprite;
                    break;
                //If Back To It Card is present
                case "Back To It Card":
                    cardImage.sprite = BackToItCardSprite;
                    break;
                //If Switch Card is present
                case "Switch Card":
                    cardImage.sprite = SwitchCardSprite;
                    break;
                //If Clear Card is present
                case "Clear Card":
                    cardImage.sprite = ClearCardSprite;
                    break;
                //If card name is invalid
                default:
                    print("ERROR: FAILED TO CHANGE DEALT CARD 1 SPRITE");
                    break;
            }
        } else {
            //Checks between possible card names
            switch (playedCardsData[cardIndex].name)
            {
                //If Move Card is present
                case "Move Card":
                    cardImage.sprite = moveCardSprite;
                    break;
                //If Jump Card is present
                case "Jump Card":
                    cardImage.sprite = JumpCardSprite;
                    break;
                //If Turn Right Card is present
                case "Turn Right Card":
                    cardImage.sprite = TurnRightCardSprite;
                    break;
                //If Turn Left Card is present
                case "Turn Left Card":
                    cardImage.sprite = TurnLeftCardSprite;
                    break;
                //If Back To It Card is present
                case "Back To It Card":
                    cardImage.sprite = BackToItCardSprite;
                    break;
                //If Switch Card is present
                case "Switch Card":
                    cardImage.sprite = SwitchCardSprite;
                    break;
                //If Clear Card is present
                case "Clear Card":
                    cardImage.sprite = ClearCardSprite;
                    break;
                //If card name is invalid
                default:
                    print("ERROR: FAILED TO CHANGE DEALT CARD 1 SPRITE");
                    break;
            }
        }
    }

    /**
     * Shows all cards that have been played
     */
    public void ShowPlayedCards(bool ableToReturn)
    {
        gameUI.enabled = false;
        playedCardsUI.enabled = true;
        if (ableToReturn)
            returnButton.gameObject.SetActive(true);
        else
            returnButton.gameObject.SetActive(false);
    }

    /**
     * Returns back to the Game UI
     */
    public void ReturnToGameUI()
    {
        gameUI.enabled = true;
        playedCardsUI.enabled = false;
        returnButton.gameObject.SetActive(false);
    }

    /**
     * Updates the text for the storage cooldown
     */
    public void UpdateReadyText()
    {
        readyText.text = "Ready In: " + deckManager.GetStoredCardWait();
    }
    
    /**
     * Updates the shuffle text
     */
    public void UpdateShuffleText()
    {
        shuffleText.text = "Shuffles: " + gameManager.GetNumOfShuffles();
    }
}
