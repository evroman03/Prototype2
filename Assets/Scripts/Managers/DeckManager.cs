using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeckManager : MonoBehaviour
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

    //Declares variables
    [SerializeField]
    private int numMoveCards, numJumpCards, numTurnRightCards, numTurnLeftCards,
                       numBackToItCards, numSwitchCards, numClearCards;
    private int totalCards;
    [SerializeField] Card card;
    [SerializeField] Transform cardFolder;

    Card spawnCard = null;
    List<Card> deck;
    List<Card> dealtCards;
    List<Card> playedCards;

    // Start is called before the first frame update
    void Start()
    {
        //Initializes variables
        totalCards = numMoveCards + numJumpCards + numTurnRightCards + numTurnLeftCards
                     + numBackToItCards + numSwitchCards + numClearCards;
        deck = new List<Card>();
        dealtCards = new List<Card>();
        playedCards = new List<Card>();

        BuildDeck();

        /**
        --Testing--

        DealCard();
        DealCard();
        DealCard();
        print("DEALT CARDS:");
        PrintDealtCards();

        PlayDealtCard(2);
        print("DEALT CARDS:");
        PrintDealtCards();

        ReturnDealtCards();
        print("DECK:");
        PrintDeck();
        */
    }

    /**
 * Resets the deck and remakes the deck for the start of the game.
 */
    public void BuildDeck()
    {
        deck = new List<Card>();
        CreateDeck();
        ShuffleDeck();
        PrintDeck();
    }

    /**
     * Creates empty objects of every card
     */
    private void CreateDeck()
    {
        //Creates all Move cards
        for(int i = 0; i < numMoveCards; i++)
        {
            //Instaniates card
            spawnCard = Instantiate(card);

            //Puts card inside the card folder for organization
            spawnCard.gameObject.transform.SetParent(cardFolder);

            //Changes attributes of the card
            spawnCard.name = "Move Card";
            spawnCard.setClicked(false);

            //Adds card into the deck
            deck.Add(spawnCard);
        }
        //Creates all Jump Cards
        for (int i = 0; i < numJumpCards;i++)
        {
            //Instaniates card
            spawnCard = Instantiate(card);

            //Puts card inside the card folder for organization
            spawnCard.gameObject.transform.SetParent(cardFolder);

            //Changes attributes of the card
            spawnCard.name = "Jump Card";
            spawnCard.setClicked(false);

            //Adds card into the deck
            deck.Add(spawnCard);
        }
        //Creates all Turn Right Cards
        for (int i = 0; i < numTurnRightCards;i++)
        {
            //Instaniates card
            spawnCard = Instantiate(card);

            //Puts card inside the card folder for organization
            spawnCard.gameObject.transform.SetParent(cardFolder);

            //Changes attributes of the card
            spawnCard.name = "Turn Right Card";
            spawnCard.setClicked(false);

            //Adds card into the deck
            deck.Add(spawnCard);
        }
        //Creates all Turn Left Cards
        for (int i = 0; i < numTurnLeftCards;i++)
        {
            //Instaniates card
            spawnCard = Instantiate(card);

            //Puts card inside the card folder for organization
            spawnCard.gameObject.transform.SetParent(cardFolder);

            //Changes attributes of the card
            spawnCard.name = "Turn Left Card";
            spawnCard.setClicked(false);

            //Adds card into the deck
            deck.Add(spawnCard);
        }
        //Creates all Back To It Cards
        for (int i = 0; i < numBackToItCards; i++)
        {
            //Instaniates card
            spawnCard = Instantiate(card);

            //Puts card inside the card folder for organization
            spawnCard.gameObject.transform.SetParent(cardFolder);

            //Changes attributes of the card
            spawnCard.name = "Back To It Card";
            spawnCard.setClicked(false);

            //Adds card into the deck
            deck.Add(spawnCard);
        }
        //Creates all Switch Cards
        for (int i = 0; i < numSwitchCards;i++)
        {
            //Instaniates card
            spawnCard = Instantiate(card);

            //Puts card inside the card folder for organization
            spawnCard.gameObject.transform.SetParent(cardFolder);

            //Changes attributes of the card
            spawnCard.name = "Switch Card";
            spawnCard.setClicked(false);

            //Adds card into the deck
            deck.Add(spawnCard);
        }
        //Creates all Clear Cards
        for (int i = 0; i < numClearCards;i++)
        {
            //Instaniates card
            spawnCard = Instantiate(card);

            //Puts card inside the card folder for organization
            spawnCard.gameObject.transform.SetParent(cardFolder);

            //Changes attributes of the card
            spawnCard.name = "Clear Card";
            spawnCard.setClicked(false);

            //Adds card into the deck
            deck.Add(spawnCard);
        }
    }

    /**
     * Shuffles the deck
     */
    private void ShuffleDeck()
    {
        //Shuffles the deck 10-15 times.
        int numberOfShuffles = Random.Range(10, 16);

        int repeatShuffle = 0;
        for (int i = 0; i < numberOfShuffles; i++)
        {
            //Repeats the Over Hand Shuffle 5-10 times
            repeatShuffle = Random.Range(5, 11);
            for (int j = 0; j < repeatShuffle; j++)
            {
                OverHandShuffle();
            }

            //Repeats the Riffle Shuffle 5-10 times
            repeatShuffle = Random.Range(5, 11);
            for (int j = 0; j < repeatShuffle; j++)
            {
                RiffleShuffle();
            }

            //Cuts the deck
            Cut(Random.Range(3, totalCards - 3));
        }

        //Reorder the hierarchy to match the shuffled list

        //Destroy all children
        foreach (Transform child in cardFolder.transform)
        {
            Destroy(child.gameObject);
        }

        //Recreate all children
        for (int i = 0; i < totalCards; i++)
        {
            spawnCard = Instantiate(card);

            //Puts card inside the card folder for organization
            spawnCard.gameObject.transform.SetParent(cardFolder);

            //Changes attributes of the card
            spawnCard.name = deck[i].name;
            spawnCard.setClicked(deck[i].getClicked());
        }
    }

    #region Shuffle Helper Methods
    /**
     * Performs the Riffle Shuffle on the deck
     */
    private void RiffleShuffle()
    {
        List<Card> topHalf = new List<Card>();
        List<Card> bottomHalf = new List<Card>();

        //Splits the deck into two packets.
        for (int i = 0; i < deck.Count / 2; i++)
        {
            topHalf.Add(deck[i]);
        }
        for (int i = deck.Count / 2; i < deck.Count; i++)
        {
            bottomHalf.Add(deck[i]);
        }
        //Removes all cards from the deck
        deck.Clear();

        //Adds cards back into the deck
        for (int i = totalCards / 2 - 1; i > -1; i--)
        {
            deck.Add(bottomHalf[i]);
            deck.Add(topHalf[i]);
        }
    }

    /**
     * Performs the Over Hand Shuffle
     */
    private void OverHandShuffle()
    {
        //Makes a random size starting packet
        int startingIndex = Random.Range(1, 6);
        int deckSize = deck.Count;

        List<Card> bottomHalf = new List<Card>();

        //Splits the deck into two packets.
        for (int i =  startingIndex; i < deckSize; i++)
        {
            bottomHalf.Add(deck[startingIndex]);
            deck.RemoveAt(startingIndex);
        }

        //Sets starting variables
        int inBottomPacket = totalCards - startingIndex;
        int cardsToAddBackIn = 0;

        //Loops until all cards are placed back into the deck
        while (inBottomPacket > 0)
        {
            //If there is 5 or less cards left to be put into the deck, add them all in
            if (inBottomPacket < 5)
                cardsToAddBackIn = inBottomPacket;
            else
                cardsToAddBackIn = Random.Range(1, 6);

            //Reverses the card order to add back into the deck
            for (int i = cardsToAddBackIn - 1; i >= 0;i--)
            {
                deck.Add(bottomHalf[i]);
                bottomHalf.RemoveAt(i);
            }

            //Decrements inBottomPacket by the amount taken out
            inBottomPacket -= cardsToAddBackIn;
        }
    }

    /**
     * Performs a Cut on the deck
     */
    private void Cut(int index)
    {
        //Keeps index in bounds
        if (index > 0 && index < totalCards)
        {
            List<Card> bottomHalf = new List<Card>();

            //Splits the deck into two packets
            for (int i = 0; i < index; i++)
            {
                bottomHalf.Add(deck[0]);
                deck.RemoveAt(0);
            }

            //Puts the top packet at the bottom of the deck
            for (int i = 0; i < index; i++)
            {
                deck.Add(bottomHalf[i]);
            }
        }
    }
    #endregion

    /**
     * Deals the top card from the deck.
     * Removes the card from the deck list and adds it to the dealtCards list.
     */
    public void DealCard()
    {
        dealtCards.Add(deck[0]);
        deck.Remove(deck[0]);
        totalCards--;
    }

    /**
     * Plays the dealt card and removes it from the dealtCards list.
     */
    public void PlayDealtCard(int cardPlacementNum)
    {
        print("PLAYED " + dealtCards[cardPlacementNum - 1]);
        playedCards.Add(deck[cardPlacementNum - 1]);
        dealtCards.RemoveAt(cardPlacementNum - 1);
    }

    /**
     * Returns all cards in the dealtCards List to the bottom of the deck list.
     */
    public void ReturnDealtCards()
    {
        int dealtCardsSize = dealtCards.Count;
        print(dealtCardsSize);
        for (int i = 0; i < dealtCardsSize; i++)
        {
            deck.Add(dealtCards[0]);
            dealtCards.RemoveAt(0);
            totalCards++;
        }
    }

    /**
     * Removes the first card in the play deck
     */
    public void RemoveFirstPlayed()
    {
        //Checks to make sure there is at least one card in the played deck
        if (playedCards.Count > 0)
            playedCards.RemoveAt(0);
        else
            print("ERROR: FAILED TO REMOVE FIRST CARD");
    }

    /**
     * Removes the last card in the play deck
     */
    public void RemoveLastPlayed()
    {
        //Checks to make sure there is at least one card in the played deck
        if (playedCards.Count > 0)

            playedCards.RemoveAt(playedCards.Count - 1);
        else
            print("ERROR: FAILED TO REMOVE LAST CARD");
    }

    /**
     * Swaps two cards in the play deck
     * @Param indexSwap1 - the first card's index to swap with
     * @Param indexSwap2 - the second card's index to swap with
     */
    public void SwapTwoCards(int indexSwap1, int indexSwap2)
    {
        //Checks to make sure there is at least one card in the played deck
        if (playedCards.Count > 0)
        {
            Card temp = playedCards[indexSwap1];
            playedCards[indexSwap1] = playedCards[indexSwap2];
            playedCards[indexSwap2] = temp;
        }
    }

    /**
     * Returns the list of all cards played
     */
    public List<Card> GetDeck()
    {
        return deck;
    }

    /**
     * Returns the list of all cards played
     */
    public List<Card> GetDealtCards()
    {
        return dealtCards;
    }

    /**
     * Clears all data from dealt cards deck
     */
    public void ClearDealtCards()
    {
        dealtCards = new List<Card>();
    }

    /**
     * Returns the list of all cards played
     */
    public List<Card> GetPlayedCards()
    {
        return playedCards;
    }

    /**
     * Clears all data from the played cards deck
     */
    public void ClearPlayedCards()
    {
        playedCards = new List<Card>();
    }

    /**
     * Prints all cards in the deck list into the console.
     */
    private void PrintDeck()
    {
        for (int i = 0; i < totalCards; i++)
        {
            print(deck[i].name);
        }
    }

    /**
     * Prints the dealt cards
     */
    private void PrintDealtCards()
    {
        int dealtCardsSize = dealtCards.Count;
        for (int i = 0; i < dealtCardsSize; i++)
        {
            print(dealtCards[i].name);
        }
    }
}
