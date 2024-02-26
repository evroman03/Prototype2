using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    //Makes Class a Singleton Class.

    #region Singleton
    private static DeckManager instance;
    public static DeckManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindAnyObjectByType(typeof(DeckManager)) as DeckManager;
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    #endregion
    GameManager gameManager;
    UIManager uiManager;

    //Declares variables
    [SerializeField]
    private int numMoveCards, numJumpCards, numTurnRightCards, numTurnLeftCards,
                       numBackToItCards, numSwitchCards, numClearCards;
    private int totalCards;
    [SerializeField] Card card;
    [SerializeField] Transform cardFolder;

    private Card spawnCard = null;
    public List<Card> deck, dealtCards, playedCards;

    [SerializeField] Card storedCard;
    private int storedCardWait;

    //Init Variables
    public void InitDeckManager()
    {
        gameManager = GameManager.Instance;
        uiManager = UIManager.Instance;
        totalCards = numMoveCards + numJumpCards + numTurnRightCards + numTurnLeftCards
                     + numBackToItCards + numSwitchCards + numClearCards;

        deck = new List<Card>();
        dealtCards = new List<Card>();
        playedCards = new List<Card>();

        storedCard = null;
        storedCardWait = 2;
    }

    /**
 * Resets the deck and remakes the deck for the start of the game.
 */
    public void BuildDeck()
    {
        deck = new List<Card>();
        CreateDeck();
        ShuffleDeck();
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
            spawnCard.SetClicked(false);

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
            spawnCard.SetClicked(false);

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
            spawnCard.SetClicked(false);

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
            spawnCard.SetClicked(false);

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
            spawnCard.SetClicked(false);

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
            spawnCard.SetClicked(false);

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
            spawnCard.SetClicked(false);

            //Adds card into the deck
            deck.Add(spawnCard);
        }
    }

    /**
     * Shuffles the deck
     */
    public void ShuffleDeck()
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
    }

    #region Shuffle Helper Methods
    /**
     * Performs the Riffle Shuffle on the deck
     */
    private void RiffleShuffle()
    {
        List<Card> topHalf = new List<Card>();
        List<Card> bottomHalf = new List<Card>();
        Card tempCard = null;
        //Checks for uneven amount of cards
        if (totalCards % 2 == 1)
        {
            tempCard = deck[deck.Count - 1];
        }
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

        //Adds in tempCard if it is not null
        if (tempCard != null)
            deck.Add(tempCard);

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
            List<Card> topHalf = new List<Card>();

            //Splits the deck into two packets
            for (int i = 0; i < index; i++)
            {
                bottomHalf.Add(deck[i]);
            }
            for (int i = index; i  < totalCards; i++)
            {
                topHalf.Add(deck[i]);
            }
            deck.Clear();

            //Puts both packets back into the deck with the top half at the bottom
            deck.AddRange(topHalf);
            deck.AddRange(bottomHalf);
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
        totalCards = dealtCards.Count;
    }

    /**
     * Plays the dealt card and removes it from the dealtCards list.
     */
    public void PlayDealtCard(int cardPlacementNumIndex)
    {
        //Checks if the stored card was clicked
        if (cardPlacementNumIndex == -1)
        {
            storedCard.SetClicked(false);
            playedCards.Add(storedCard);
            storedCard = null;
            uiManager.UpdatePlayedCardsImage();
            return;
        }

        //If any other dealt card was clicked
        dealtCards[cardPlacementNumIndex].SetClicked(false);
        playedCards.Add(dealtCards[cardPlacementNumIndex]);
        dealtCards.RemoveAt(cardPlacementNumIndex);
        uiManager.UpdatePlayedCardsImage();
    }

    /**
     * Returns all cards in the dealtCards List to the bottom of the deck list.
     */
    public void ReturnDealtCards()
    {
        int dealtCardsSize = dealtCards.Count;
        for (int i = 0; i < dealtCardsSize; i++)
        {
            deck.Add(dealtCards[0]);
            dealtCards.RemoveAt(0);
        }
        totalCards = deck.Count;
    }

    /**
     * Removes the first card in the play deck
     */
    public void RemoveFirstPlayed()
    {
        //Checks to make sure there is at least one card in the played deck
        if (playedCards.Count > 0)
            playedCards.RemoveAt(0);
    }

    /**
     * Removes the last card in the play deck
     */
    public void RemoveLastPlayed()
    {
        //Checks to make sure there is at least one card in the played deck
        if (playedCards.Count > 0)

            playedCards.RemoveAt(playedCards.Count - 1);
    }

    /**
     * Swaps two cards in the play deck
     * @Param indexSwap1 - the first card's index to swap with
     * @Param indexSwap2 - the second card's index to swap with
     */
    public void SwapTwoCards()
    {
        int numOfSelectedCards = 0;
        int playedCardsCount = playedCards.Count;

        //Searches for two selected cards
        for (int i = 0; i < playedCardsCount; i++)
        {
            if (playedCards[i].GetClicked())
            {
                numOfSelectedCards++;
            }
        }

        //Checks if two cards were selected
        if (numOfSelectedCards == 2)
        {
            int index1 = -1;
            int index2 = -1;
            int i = 0;

            //Finds first instance of selected card
            for (i = 0; i < playedCardsCount; i++)
            {
                if (playedCards[i].GetClicked())
                {
                    index1 = i;
                    break;
                }
            }

            //Finds second instance of a clicked card
            for (i += 1;  i < playedCardsCount; i++)
            {
                if (playedCards[i].GetClicked())
                {
                    index2 = i;
                    break;
                }
            }

            //Swaps the cards
            Card tempCard = playedCards[index1];
            playedCards[index1] = playedCards[index2];
            playedCards[index2] = tempCard;

            //Sets the played cards' clicked to false
            playedCards[index1].SetClicked(false);
            playedCards[index2].SetClicked(false);

            //Returns back to the game
            uiManager.ReturnToGameUI();
            gameManager.ChangeGameState(GameManager.STATE.Lv1);
            
        }
    }

    /**
     * Puts a card in the stored card slot
     */
    public void SetStoredCard(Card card)
    {
        if (storedCard != null)
        {
            int dealtCardsCount = dealtCards.Count;
            Card tempCard = storedCard;
            for (int i = 0; i < dealtCardsCount; i++)
            {
                if (dealtCards[i].GetClicked())
                {
                    storedCard = dealtCards[i];
                    dealtCards[i] = tempCard;
                    dealtCards.Remove(storedCard);

                    storedCardWait = 2;
                    return;
                }
            }
        }
        if (storedCard == null)
        {
            storedCard = card;
            dealtCards.Remove(card);
            storedCardWait = 2;
        }
    }

    /**
     * Returns the stored card
     */
    public Card GetStoredCard()
    {
        return storedCard;
    }

    public void UpdateStoredCardWait()
    {
        if (storedCard != null && storedCardWait > 0)
        {
            storedCardWait--;
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
     * Gets the wait countdown
     */
    public int GetStoredCardWait()
    {
        if (storedCard == null)
            return 2;
        return storedCardWait;
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