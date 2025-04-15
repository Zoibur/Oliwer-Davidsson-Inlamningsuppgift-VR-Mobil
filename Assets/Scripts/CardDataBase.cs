using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "CardDataBase", menuName = "Casino/CardDataBase")]
public class CardDataBase : ScriptableObject
{
   public CardView cardViewPrefab;
   public List<Card> cards = new List<Card>();
}



