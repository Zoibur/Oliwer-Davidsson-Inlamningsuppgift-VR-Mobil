using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "CardDataBase", menuName = "Casino/CardDataBase")]
public class CardDataBase : ScriptableObject
{
   public CardView cardViewPrefab;
   public List<Card> cards = new List<Card>();
}

[CustomEditor(typeof(CardDataBase))]
public class CardDatabaseEditor : Editor
{
   public override void OnInspectorGUI()
   {
      base.OnInspectorGUI();
      if (GUILayout.Button("Find Cards"))
      {
         var dataBase = (CardDataBase)target;
         
         List<Card> result = new List<Card>();
         string[] guids = AssetDatabase.FindAssets("t:Card");

         foreach (string guid in guids)
         {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Card obj = AssetDatabase.LoadAssetAtPath<Card>(path);
            if (obj != null)
            {
               result.Add(obj);
            }
         }

         dataBase.cards = result;
      }
   }
}
