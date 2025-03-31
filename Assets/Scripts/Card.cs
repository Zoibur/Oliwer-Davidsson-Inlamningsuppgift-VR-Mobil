using UnityEngine;


[CreateAssetMenu(fileName = "New Card", menuName = "Card", order = 1)]
public class Card : ScriptableObject
{
   public enum Suit {Hearts, Spades, Diamonds, Clubs}
   
   public Suit cardSuit;
   public string cardName;
   public int cardValue;
   public int blackjackValue; 
   public GameObject cardPrefab;
   public Texture2D cardSprite;
   
}
