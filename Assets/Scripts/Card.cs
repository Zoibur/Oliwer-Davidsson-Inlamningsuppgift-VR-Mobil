using UnityEngine;


[CreateAssetMenu(fileName = "New Card", menuName = "Card", order = 1)]
public class Card : ScriptableObject
{
   public enum Suit {Heard, Spades, Diamonds, Clubs}
   
   public Suit cardSuit;
   public string cardName;
   public int cardValue;
   
   public Sprite cardSprite;
}
