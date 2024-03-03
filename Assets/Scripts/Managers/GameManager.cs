using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    //Makes Class a Singleton Class.
    #region Singleton
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindAnyObjectByType(typeof(GameManager)) as GameManager;
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    #endregion

    //Current game state
    public STATE gameState;
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private PlayerEndTrigger endTrigger;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 playerStartingLocation;
    [SerializeField] private int cardsToDeal = 4;
    [SerializeField] private int numOfShuffles = 3;

    [SerializeField] private Button removeFirst, removeLast;
    private PlayerController playerController;

    private List<Card> dealtCards, playedCards;
    [SerializeField] private List<Card> tempPlayedCards;
    private List<Card> tempBeforeBackToItCards, tempAfterBackToItCards;
    private int lastBackToItIndex;
    // Start is called before the first frame update
    void Start()
    {
        playerController = PlayerController.Instance;
        removeFirst.gameObject.SetActive(false);
        removeLast.gameObject.SetActive(false);
        playedCards = new List<Card>();
        tempPlayedCards = new List<Card>();

        tempBeforeBackToItCards = new List<Card>();
        tempAfterBackToItCards = new List<Card>();
        lastBackToItIndex = -1;

        //Sets the game state to menu
        ChangeGameState(STATE.Menu);
        ChangeGameState(STATE.ChooseCards);
    }

    /**
     * Resets the game back to its starting conditions
     */
    public void Reset()
    {
        /*
        //Resets the decks
        deckManager.BuildDeck();
        deckManager.ClearDealtCards();
        deckManager.ClearPlayedCards();
        deckManager.SetStoredCard(null);

        //Resets player position
        player.transform.position = playerStartingLocation;
        player.transform.rotation = Quaternion.identity;
        player.transform.rotation *= Quaternion.Euler(0, 0, 0);

        //Resets Game State
        ChangeGameState(STATE.ChooseCards);
        */
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    //Changes the game state and activates methods if needed
    public void ChangeGameState(STATE state)
    {
        switch (state)
        {
            case STATE.Menu:
                gameState = STATE.Menu;
                StartGame();
                break;
            case STATE.ChooseCards:
                gameState = STATE.ChooseCards;
                DealCards();
                break;
            case STATE.Lv1:
                gameState = STATE.Lv1;
                RunPlaySequence();
                break;
            case STATE.ChooseClear:
                gameState = STATE.ChooseClear;
                break;
            case STATE.SwitchCards:
                gameState = STATE.SwitchCards;
                break;
            case STATE.End:
                gameState = STATE.End;
                //Add method here if needed
                break;
            default:
                print("ERROR: FAILED TO SWITCH GAME STATE.");
                break;
        }
    }

    //Calls "Start" Functions for other Managers
    private void StartGame()
    {
        deckManager.InitDeckManager();
        uiManager.InitUIManager();
        deckManager.BuildDeck();
        endTrigger.InitTrigger();
        player.transform.position = playerStartingLocation;
    }

    private void DealCards()
    {
        //If there are no cards played
        if (playedCards.Count < 1) {
            bool isValid = false;
            //Make sure dealt cards do not include Switch or Clear cards
            while (!isValid)
            {
                deckManager.ReturnDealtCards();
                deckManager.ShuffleDeck();

                isValid = true;
                int deckSize = deckManager.GetDeck().Count;
                for (int i = 0; i < cardsToDeal; i++)
                {
                    if (deckSize > i)
                        deckManager.DealCard();
                }

                //Checks if any card is a Swap or Clear card
                dealtCards = deckManager.GetDealtCards();
                int dealtCardsSize = dealtCards.Count;
                for (int i = 0; i < dealtCardsSize; i++)
                {
                    if (dealtCards[i].name == "Clear Card" || dealtCards[i].name == "Switch Card")
                    {
                        isValid = false;
                        break;
                    }
                }
            }

            //Updates UI and other variables
            deckManager.UpdateStoredCardWait();
            uiManager.UpdateReadyText();

            uiManager.UpdateDealtCards();
            uiManager.CheckDealtCards();
            uiManager.UpdateDealtCardsImages();

            playedCards = deckManager.GetPlayedCards();

        } else if (playedCards.Count == 1) {
            bool isValid = false;
            //Make sure dealt cards do not include the Switch Card
            while (!isValid)
            {
                deckManager.ReturnDealtCards();
                deckManager.ShuffleDeck();

                isValid = true;
                int deckSize = deckManager.GetDeck().Count;
                for (int i = 0; i < cardsToDeal; i++)
                {
                    if (deckSize > i)
                        deckManager.DealCard();
                }

                //Checks if any card is a Switch Card
                dealtCards = deckManager.GetDealtCards();
                int dealtCardsSize = dealtCards.Count;
                for (int i = 0; i < dealtCardsSize; i++)
                {
                    if (dealtCards[i].name == "Switch Card")
                    {
                        isValid = false;
                        break;
                    }
                }
            }

            //Updates UI and other variables
            deckManager.UpdateStoredCardWait();
            uiManager.UpdateReadyText();

            uiManager.UpdateDealtCards();
            uiManager.CheckDealtCards();
            uiManager.UpdateDealtCardsImages();

            playedCards = deckManager.GetPlayedCards();

        } else {
            //Draw normally
            int deckSize = deckManager.GetDeck().Count;
            for (int i = 0; i < cardsToDeal; i++)
            {
                if (deckSize > i)
                    deckManager.DealCard();
            }
            deckManager.UpdateStoredCardWait();
            uiManager.UpdateReadyText();

            uiManager.UpdateDealtCards();
            uiManager.CheckDealtCards();
            uiManager.UpdateDealtCardsImages();

            playedCards = deckManager.GetPlayedCards();
        }
    }

    public void ShuffleDeck()
    {
        if (numOfShuffles > 0)
        {
            deckManager.ReturnDealtCards();
            deckManager.ShuffleDeck();
            ChangeGameState(STATE.ChooseCards);
            numOfShuffles--;
            uiManager.UpdateShuffleText();
        }
    }

    private void RunPlaySequence()
    {
        deckManager.ReturnDealtCards();
        deckManager.ShuffleDeck();
        lastBackToItIndex = -1;
        
        playedCards = deckManager.GetPlayedCards();

        //Clears the temporary played cards
        tempPlayedCards.Clear();

        int playedCardsCount = playedCards.Count;

        //Adds copies of the cards into a temporary list
        for (int i = 0; i < playedCardsCount ; i++)
        {
            tempPlayedCards.Add(playedCards[i]);
        }

        //If Clear Card was Played
        if (playedCards.Count > 0 && playedCards[playedCards.Count - 1].name == "Clear Card")
        {
            deckManager.RemoveLastPlayed();
            print("CEARED ACTION");
            if (deckManager.GetPlayedCards().Count > 0)
                ClearAction();
            else
                ChangeGameState(STATE.Lv1);

            return;
        }

        //If Switch Card was played
        if (playedCards.Count > 0 && playedCards[playedCards.Count - 1].name == "Switch Card")
        {
            deckManager.RemoveLastPlayed();
            print("SWITCH ACTION");
            if (deckManager.GetPlayedCards().Count > 1)
                SwitchAction();
            else
                ChangeGameState(STATE.Lv1);
            
            return;
        }
    }
    public void PlaySequence()
    {
        if(tempPlayedCards.Count < 1) {
            ChangeGameState(STATE.ChooseCards);
            return;
        }
        switch (tempPlayedCards[0].name)
        {
            case "Move Card":
                playerController.Action(tempPlayedCards[0].name);
                //print("MOVED");
                break;
            case "Jump Card":
                playerController.Action(tempPlayedCards[0].name);
                //print("JUMPED");
                break;
            case "Turn Right Card":
                //print("TURNED RIGHT");
                playerController.Action(tempPlayedCards[0].name);
                break;
            case "Turn Left Card":
                playerController.Action(tempPlayedCards[0].name);
                //print("TURNED LEFT");
                break;
            case "Back To It Card":
                //print("BACKED TO IT);
                BackToItAction();
                break;
            default:
                print("ERROR: ATTEMPTED TO DO INVALID ACTION FROM INVALID CARD NAME");
                break;
        }
        print("PLAYED");
        tempPlayedCards.RemoveAt(0);
    }

    //Sets up game to clear a card
    private void ClearAction()
    {
        removeFirst.gameObject.SetActive(true);
        removeLast.gameObject.SetActive(true);
        ChangeGameState(STATE.ChooseClear);
    }

    //Sets up Back To It Card function
    private void BackToItAction()
    {
        tempBeforeBackToItCards.Clear();
        tempAfterBackToItCards.Clear();
        //Gets index of BackToIt card
        int playedCardsSize = playedCards.Count;

        //If this is first instance of BackToIt in playedCards
        if (lastBackToItIndex == -1)
        {
            //Gets the first instance of BackToIt's index in playedCards
            for (int i = 0; i < playedCardsSize; i++)
            {
                if (playedCards[i].name == "Back To It Card")
                {
                    lastBackToItIndex = i;
                    break;
                }
            }
        }
        //If this is the second or greater instance of BackToIt in playedCards
        else
        {
            //Gets the next instance of BackToIt's index in playedCards
            for (int i = lastBackToItIndex + 1; i < playedCardsSize; i++)
            {
                if (playedCards[i].name == "Back To It Card")
                {
                    lastBackToItIndex = i;
                    break;
                }
            }
        }

        //Copies all cards before BackToIt Card
        for (int i = 0; i < lastBackToItIndex; i++)
        {
            //Ignores previous Back To It Cards
            if (playedCards[i].name != "Back To It Card")
            {
                print("FOUND BEFORE " + playedCards[i]);
                tempBeforeBackToItCards.Add(playedCards[i]);
            }
        }
        //Copies all cards after BackToIt Card
        for (int i = lastBackToItIndex + 1; i < playedCardsSize; i++)
        {
            print("FOUND AFTER " + playedCards[i]);
            tempAfterBackToItCards.Add(playedCards[i]);
        }

        //Clears the tempPlayedCardsList
        tempPlayedCards.Clear();

        //Adds card to be removed at front of list ( due to RemoveAt(0) at the end of PlaySequence() )
        tempPlayedCards.Add(playedCards[lastBackToItIndex]);

        //Adds all cards before Back To It into the list
        int beforeBackToItSize = tempBeforeBackToItCards.Count;
        print("Size = " + beforeBackToItSize);
        for (int i = 0; i < beforeBackToItSize; i++)
        {
            print("CARD ADDED " + tempBeforeBackToItCards[i]);
            tempPlayedCards.Add(tempBeforeBackToItCards[i]);
        }

        //Adds all cards after Back To It into the list
        int afterBackToItSize = tempAfterBackToItCards.Count;
        for (int i = 0;i < afterBackToItSize; i++)
        {
            tempPlayedCards.Add(tempAfterBackToItCards[i]);
        }
    }

    //Sets up game to switch two cards
    private void SwitchAction()
    {
        ChangeGameState(STATE.SwitchCards);
        uiManager.ShowPlayedCards(false);
        uiManager.UpdatePlayedCardsImage();
    }

    //Called if the remove first button is clicked
    public void RemoveFirstClicked()
    {
        deckManager.RemoveFirstPlayed();
        removeFirst.gameObject.SetActive(false);
        removeLast.gameObject.SetActive(false);
        ChangeGameState(STATE.Lv1);
    }

    //Called if the remove last button is clicked
    public void RemoveLastClicked()
    {
        deckManager.RemoveLastPlayed();
        removeFirst.gameObject.SetActive(false);
        removeLast.gameObject.SetActive(false);
        ChangeGameState(STATE.Lv1);
    }

    public int GetNumOfShuffles()
    {
        return numOfShuffles;
    }

    //All possible game states.
   public enum STATE {
        Menu,
        ChooseCards,
        ChooseClear,
        SwitchCards,
        Lv1,
        End
    }
}