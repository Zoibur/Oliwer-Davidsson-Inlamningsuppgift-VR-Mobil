using UnityEngine;

public class CardView : MonoBehaviour
{
   
    public Card CardData{get; private set;}  
    public Material cardFront;  

    
    
    private void UpdateCardSprite()
    {
        if (CardData != null && CardData.cardSprite != null)
        {
            
            MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();

            if (renderer != null)
            {
                // Apply the materials to the MeshRenderer
                Material[] materials = new Material[1];
                materials[0] = Instantiate(cardFront);  
                renderer.materials = materials;
                
                materials[0].mainTexture = CardData.cardSprite;
            }
            else
            {
                Debug.LogWarning("No MeshRenderer found on this GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("TextureData or texture is missing.");
        } 
    }

    public void SetCardData(Card cardData)
    {
        this.CardData = cardData;
        UpdateCardSprite();
    }
}
