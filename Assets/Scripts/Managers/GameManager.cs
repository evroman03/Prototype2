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
            if (Instance == null)
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
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 playerStartingLocation;

    // Start is called before the first frame update
    void Start()
    {
        //Sets the game state to menu
        gameState = STATE.Menu;
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

        //Resets player position
        player.transform.position = playerStartingLocation;
        player.transform.rotation = Quaternion.identity;
        player.transform.rotation *= Quaternion.Euler(0, 0, 0);

        //Resets Game State
        changeGameState(STATE.Menu);

    }

    //Changes the game state and activates methods if needed
    public void changeGameState(STATE state)
    {
        switch (state)
        {
            case STATE.Menu:
                gameState = STATE.Menu;
                //Add method here if needed
                break;
            case STATE.ChooseCards:
                gameState = STATE.ChooseCards;
                //Add method here if needed
                break;
            case STATE.Lv1:
                gameState = STATE.Lv1;
                //Add method here if needed
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

    //All possible game states.
   public enum STATE {
        Menu,
        ChooseCards,
        Lv1,
        End
    }
}