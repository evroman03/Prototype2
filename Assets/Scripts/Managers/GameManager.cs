using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 playerStartingLocation;
    [SerializeField] private int cardsToDeal = 4;

    [SerializeField] private Button removeFirst, removeLast;
    private PlayerController pC;
    // Start is called before the first frame update
    void Start()
    {
        pC = PlayerController.Instance;
        removeFirst.gameObject.SetActive(false);
        removeLast.gameObject.SetActive(false);

        //Sets the game state to menu
        ChangeGameState(STATE.Menu);
        ChangeGameState(STATE.ChooseCards);
    }

    private void Update()
    {
        if (gameState == STATE.SwitchCards)
        {
            deckManager.SwapTwoCards();
        }
    }

    /**
     * Resets the game back to its starting conditions
     */
    private void Reset()
    {
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
        ChangeGameState(STATE.Menu);

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
    }

    private void DealCards()
    {
        int deckSize = deckManager.GetDeck().Count;
        for (int i = 0; i < cardsToDeal; i++)
        {
            if (deckSize > 0)
                deckManager.DealCard();
        }
        deckManager.UpdateStoredCardWait();
        uiManager.UpdateReadyText();

        uiManager.UpdateDealtCards();
        uiManager.CheckDealtCards();
        //uiManager.UpdateImages();
    }

    private void RunPlaySequence()
    {
        deckManager.ReturnDealtCards();
        deckManager.ShuffleDeck();
        
        List<Card> playedCards = deckManager.GetPlayedCards();

        //If Clear Card was Played
        if (playedCards.Count > 0 && playedCards[playedCards.Count - 1].name == "Clear Card")
        {
            print("CEARED ACTION");
            ClearAction();
            return;
        }

        //If Back To It Card was played
        if (playedCards.Count > 0 && playedCards[playedCards.Count - 1].name == "Back To It Card")
        {
            print("BACK TO IT ACTION");
            deckManager.RemoveLastPlayed();
        }

        //If Switch Card was played
        if (playedCards.Count > 0 && playedCards[playedCards.Count - 1].name == "Switch Card")
        {
            print("SWITCH ACTION");
            SwitchAction();
            return;
        }

        //Plays the sequence of cards in order
        int playedCardsSize = playedCards.Count;
        for (int i = 0; i < playedCardsSize; i++) {
            switch (playedCards[i].name) {
                case "Move Card":
                    pC.Action(playedCards[i].name);
                    print("MOVED");                   
                    //TODO - Call PlayerMovement Move Method Here!
                    break;
                case "Jump Card":
                    pC.Action(playedCards[i].name);
                    print("JUMPED");                    
                    //TODO - Call PlayerMovement Jump Method Here!
                    break;
                case "Turn Right Card":
                    print("TURNED RIGHT");
                    pC.Action(playedCards[i].name);
                    //TODO - Call PlayerMovement Turn Right Method Here!
                    break;
                case "Turn Left Card":
                    pC.Action(playedCards[i].name);
                    print("TURNED LEFT");

                    //TODO - Call PlayerMovement Turn Left Method Here!
                    break;
                //TODO - Call PlayerMovement Turn Left Method Here!
                default:
                    print("ERROR: ATTEMPTED TO DO INVALID ACTION FROM INVALID CARD NAME");
                    break;
            }
        }
        ChangeGameState(STATE.ChooseCards);
    }

    //Sets up game to clear a card
    private void ClearAction()
    {
        deckManager.RemoveLastPlayed();
        removeFirst.gameObject.SetActive(true);
        removeLast.gameObject.SetActive(true);
        ChangeGameState(STATE.ChooseClear);
    }

    //Sets up game to switch two cards
    private void SwitchAction()
    {
        deckManager.RemoveLastPlayed();
        ChangeGameState(STATE.SwitchCards);
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