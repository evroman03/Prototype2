using System.Collections.Generic;
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
            if (Instance == null)
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
     * If the Card is clicked, automatically calls this method.
     */
    public void CardClicked(int cardClicked)
    {
        deckManager.PlayDealtCard(cardClicked);
        gameManager.ChangeGameState(GameManager.STATE.Lv1);
        gameManager.ChangeGameState(GameManager.STATE.ChooseCards);
    }

    /**
     * Updates the Dealt Card Sprites
     */
    public void UpdateImages()
    {
        //Gathers data from the dealt cards deck and stored card data
        dealtCards = deckManager.GetDealtCards();
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
}
