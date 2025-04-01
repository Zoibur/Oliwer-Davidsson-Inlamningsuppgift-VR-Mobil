using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<Card> deck = new List<Card>();  
    public List<Card> playerHand = new List<Card>(); 
    public List<Card> dealerHand = new List<Card>();
    private bool isPlayerTurn = true;
    public Transform playerHandTransform; // Where the player's cards will be placed (in 3D space)
    public Transform dealerHandTransform; // Where the dealer's cards will be placed (in 3D space)
    public GameObject cardPrefab;
    

    private float offset = 0.2f;
    
    private Vector3 currentCardPosition;
    private Vector3 currentCardPositionDealer;
    
    
    public Card[] allCards; 
    
    void Start()
    {
        InitializeDeck();
        ShuffleDeck();
        
        
    }
    void Update()
    {
        // Check for player actions if it's their turn
        
        if (Input.GetKeyDown(KeyCode.R)) // Hit
        {
            
            DestroyAllCards();
            InitializeDeck();
            ShuffleDeck();
            
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            StartGame();
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

    private void SpawnCard(Card card, Vector3 spawnPosition, bool isPlayer, bool isFlipped)
    {
        if (card != null && card.cardPrefab != null)
        {
            
            Quaternion roation = isPlayer ? Quaternion.Euler(90, 0, 0) : isFlipped? Quaternion.Euler(90,0, 0):Quaternion.Euler(-90,0,0);
            
            GameObject cardObject = Instantiate(card.cardPrefab, spawnPosition, roation);
            
            FontTexutreChanger front = cardObject.GetComponent<FontTexutreChanger>();

            if (front != null)
            {
                front.textureData = card; 
                front.UpdateCardSprite();
            }
        }
    }
    
    void InitializeDeck()
    {
        deck.Clear();
        foreach (var card in allCards)
        {
            deck.Add(card);
        }
        
        currentCardPosition = playerHandTransform.position;
        
        currentCardPositionDealer = dealerHandTransform.position;
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

   
    public Card DealCard(bool isPlayer)
    {
        if (deck.Count == 0) InitializeDeck(); // If the deck is empty, reset it
        Card dealtCard = deck[0];
        deck.RemoveAt(0);
        

        if (isPlayer)
        {
            
            SpawnCard(dealtCard, currentCardPosition,true,false);
            currentCardPosition.x += offset;
        }
        else
        {
            if (dealerHand.Count == 1)
            {
                SpawnCard(dealtCard, currentCardPositionDealer, false,false);
              
            }
            else
            {
                SpawnCard(dealtCard, currentCardPositionDealer, true,false);
               
            }
            currentCardPositionDealer.x += offset;
            
        }
        
        return dealtCard;
    }
    public void StartGame()
    {
        playerHand.Clear();
        dealerHand.Clear();

        // Deal two cards to the player and dealer
       playerHand.Add(DealCard(true));
       
       
       dealerHand.Add(DealCard(false));
      

       playerHand.Add(DealCard(true));
      
       
       dealerHand.Add(DealCard(false));
      
       

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

    public void PlayerHit()
    {
        if (isPlayerTurn)
        {
            Card drawnCard = DealCard(true);
            Debug.Log(drawnCard + "Player");
            playerHand.Add(drawnCard);
            
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
        while (CalculateHandValue(dealerHand) < 17)
        {
            Card drawnCard = DealCard(false);
            dealerHand.Add(drawnCard);
            
            
            Debug.Log(drawnCard + "Dealer");
            // Optionally, display the new card in the dealer's hand
            //UpdateHandValueDisplay();
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
