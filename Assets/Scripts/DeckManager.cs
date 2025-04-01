using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<Card> deck = new List<Card>();  
    public List<CardView> playerHand = new List<CardView>(); 
    public List<CardView> dealerHand = new List<CardView>();
    private bool isPlayerTurn = true;
    public Transform playerHandTransform; // Where the player's cards will be placed (in 3D space)
    public Transform dealerHandTransform; // Where the dealer's cards will be placed (in 3D space)
    public GameObject cardPrefab;
    public CardDataBase cardDatabase;

    private float offset = 0.2f;
    
    private Vector3 currentCardPosition;
    private Vector3 currentCardPositionDealer;
    
    
    public Card[] allCards; 
    
    void Start()
    {
        InitializeDeck();
    }
    
    void Update()
    {
        // Check for player actions if it's their turn
        
        if (Input.GetKeyDown(KeyCode.R)) // Hit
        {
            
            DestroyAllCards();
            InitializeDeck();
            
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(StartGame());
            isPlayerTurn = true;
        }
        
        
        if (isPlayerTurn)
        {
            if (Input.GetKeyDown(KeyCode.H)) // Hit
            {
                PlayerHit();
            }
            if (Input.GetKeyDown(KeyCode.S)) // Stand
            {
                PlayerStand();
            }
        }
    }

    private Card GetRandomCardFromDeck()
    {
        var cardIndex = Random.Range(0, deck.Count);
        var card = deck[cardIndex];
        deck.RemoveAt(cardIndex);
        return card;
    }

    public void DestroyAllCards()
    {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Card");

        foreach (GameObject obj in allObjects)
        {
            if(obj.tag == "Card")
            {
                Destroy(obj);
            }
        }
    }

    private bool SpawnCard(Card cardData, Vector3 spawnPosition, bool isPlayer, out CardView cardView)
    {
        cardView = null;
        if (cardData != null  )
        {
            Quaternion rotation = isPlayer ? Quaternion.Euler(0,0, 0):Quaternion.Euler(0,0,180);
            cardView = Instantiate(cardDatabase.cardViewPrefab);
            cardView.transform.position = spawnPosition;
            cardView.transform.rotation = rotation;
            cardView.SetCardData(cardData);

            return true;
        }

        return false;
    }
    
    void InitializeDeck()
    {
        deck = new List<Card>(cardDatabase.cards);
        currentCardPosition = playerHandTransform.position;
        currentCardPositionDealer = dealerHandTransform.position;
    }
    
    private void DealCard(bool isPlayer, List<CardView> hand)
    {
        if (deck.Count == 0) InitializeDeck(); // If the deck is empty, reset it
        Card dealtCard = GetRandomCardFromDeck();
        
        if (isPlayer)
        {
            if (SpawnCard(dealtCard, currentCardPosition, true, out var cardView))
            {
                hand.Add(cardView);
            }
            currentCardPosition.x += offset;
        }
        else
        {
            if (dealerHand.Count == 1)
            {
                if(SpawnCard(dealtCard, currentCardPositionDealer, false, out var cardView))
                {
                    hand.Add(cardView);
                };
            }
            else
            {
                if (SpawnCard(dealtCard, currentCardPositionDealer, true, out var cardView))
                {
                    hand.Add(cardView);
                }
               
            }
            currentCardPositionDealer.x += offset;
        }
    }
    
    public IEnumerator StartGame()
    {
        playerHand.Clear();
        dealerHand.Clear();
        
        DealCard(true,playerHand);
        yield return new WaitForSeconds(1.0f);
        DealCard(false,dealerHand);
        yield return new WaitForSeconds(1.0f);
        DealCard(true,playerHand);
        yield return new WaitForSeconds(1.0f);
        DealCard(false,dealerHand);
    }
    
    public int CalculateHandValue(List<CardView> hand)
    {
        int value = 0;
        int aceCount = 0;

        foreach (var card in hand)
        {
            value += card.CardData.blackjackValue;
            if (card.CardData.cardValue == 1) aceCount++; // If the card is an Ace
        }

        // Adjust for Aces (they can be worth 1 or 11)
        while (value <= 11 && aceCount > 0)
        {
            value += 10;
            aceCount--;
        }

        return value;
    }

    public void PlayerHit()
    {
        if (isPlayerTurn)
        {
            DealCard(true, playerHand);
            // playerHand.Add(drawnCard);
            
            // Check if the player has busted (hand value > 21)
            int playerHandValue = CalculateHandValue(playerHand);
            if (playerHandValue > 21)
            {
                Debug.Log("Player Busted!");
                isPlayerTurn = false;
                EndTurn(); // End player's turn if busted
            }
        }
    }
    
    public void PlayerStand()
    {
        isPlayerTurn = false;
        StartCoroutine(DealerTurn()); // Start dealer's turn after the player stands
    }
    
    IEnumerator DealerTurn()
    {
        var cardInstance = dealerHand[^1];
        var continueCoroutine = false;
        
        Sequence flipSequence = DOTween.Sequence();
        var cardInstanceStartPos = cardInstance.transform.position;
        flipSequence.Append(cardInstance.transform.DOMoveY(cardInstanceStartPos.y + 0.25f, 0.5f))
            .Append(cardInstance.transform.DORotate(new Vector3(0, 0, 180), 0.5f, RotateMode.LocalAxisAdd))
            .Append(cardInstance.transform.DOMoveY(cardInstanceStartPos.y, 0.5f)).OnComplete(() =>
            {
                continueCoroutine = true;
            });
        
        // cardInstance.transform.DORotate(new Vector3(0, 0, 180), 1f,RotateMode.WorldAxisAdd).OnComplete(() =>
        // {
        //     continueCoroutine = true;
        // });
        yield return new WaitUntil(() => continueCoroutine);
        
        while (CalculateHandValue(dealerHand) < 17)
        {
            DealCard(false, dealerHand);
          
            yield return new WaitForSeconds(1); // Delay to simulate the dealer drawing cards
        }

        // Now determine the outcome of the game
        int playerHandValue = CalculateHandValue(playerHand);
        int dealerHandValue = CalculateHandValue(dealerHand);

        // Check the outcome
        if (dealerHandValue > 21)
        {
            Debug.Log("Dealer Busted! Player Wins!");
        }
        else if (dealerHandValue > playerHandValue)
        {
            Debug.Log("Dealer Wins!");
        }
        else if (dealerHandValue < playerHandValue)
        {
            Debug.Log("Player Wins!");
        }
        else
        {
            Debug.Log("It's a Draw!");
        }

       
    }
    
    void EndTurn()
    {
        // Optionally, do something after the player's turn ends (e.g., show a message or reset state)
    }
}
