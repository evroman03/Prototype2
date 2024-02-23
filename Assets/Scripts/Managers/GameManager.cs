using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        //Sets the game state to menu
        ChangeGameState(STATE.Menu);

        ChangeGameState(STATE.ChooseCards);
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
        deckManager.DealCard();
        deckManager.DealCard();
        deckManager.DealCard();
        deckManager.DealCard();

        //uiManager.UpdateImages();
    }

    private void RunPlaySequence()
    {
        deckManager.ReturnDealtCards();
        deckManager.ShuffleDeck();
        
        List<Card> playedCards = deckManager.GetPlayedCards();

        int playedCardsSize = playedCards.Count;
        for (int i = 0; i < playedCardsSize; i++) {
            switch (playedCards[i].name) {
                case "Move Card":
                    //TODO - Call PlayerMovement Move Method Here!
                    break;
                case "Jump Card":
                    //TODO - Call PlayerMovement Jump Method Here!
                    break;
                case "Turn Right Card":
                    //TODO - Call PlayerMovement Turn Right Method Here!
                    break;
                case "Turn Left Card":
                //TODO - Call PlayerMovement Turn Left Method Here!
                case "Back To It Card":
                    //TODO - Call PlayerMovement Back To It Method Here!
                    break;
                case "Switch Card":
                    //TODO - Call PlayerMovement Switch Method Here!
                    break;
                case "Clear Card":
                    //TODO - Call PlayerMovement Clear Method Here!
                    break;
                default:
                    print("ERROR: ATTEMPTED TO DO INVALID ACTION FROM INVALID CARD NAME");
                    break;
            }
        }
    }

    //All possible game states.
   public enum STATE {
        Menu,
        ChooseCards,
        Lv1,
        End
    }
}