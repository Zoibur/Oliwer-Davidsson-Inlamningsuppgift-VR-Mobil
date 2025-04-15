using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
