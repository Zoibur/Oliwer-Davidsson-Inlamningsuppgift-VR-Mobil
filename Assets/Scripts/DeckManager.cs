using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<Card> deck = new List<Card>();  
    public List<Card> playerHand = new List<Card>(); 
    public List<Card> dealerHand = new List<Card>();
    
    public Card[] allCards; 
    
    void Start()
    {
        InitializeDeck();
        ShuffleDeck();
    }
    void InitializeDeck()
    {
        deck.Clear();
        foreach (var card in allCards)
        {
            deck.Add(card);
        }
    }
    void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }
    public Card DealCard()
    {
        if (deck.Count == 0) InitializeDeck(); // If the deck is empty, reset it
        Card dealtCard = deck[0];
        deck.RemoveAt(0);
        return dealtCard;
    }
    public void StartGame()
    {
        playerHand.Clear();
        dealerHand.Clear();

        // Deal two cards to the player and dealer
        playerHand.Add(DealCard());
        dealerHand.Add(DealCard());
        playerHand.Add(DealCard());
        dealerHand.Add(DealCard());
    }
    
    public int CalculateHandValue(List<Card> hand)
    {
        int value = 0;
        int aceCount = 0;

        foreach (var card in hand)
        {
            value += card.blackjackValue;
            if (card.cardValue == 1) aceCount++; // If the card is an Ace
        }

        // Adjust for Aces (they can be worth 1 or 11)
        while (value <= 11 && aceCount > 0)
        {
            value += 10;
            aceCount--;
        }

        return value;
    }

   
}
