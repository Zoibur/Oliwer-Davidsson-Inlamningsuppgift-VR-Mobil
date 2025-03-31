using System;
using UnityEditor.U2D;
using UnityEngine;

public class FontTexutreChanger : MonoBehaviour
{
    public Card textureData;  
    public Material frontMaterial;  
      

    void Start()
    {
      // UpdateCardSprite();
    }

   

    public void UpdateCardSprite()
    {
        if (textureData != null && textureData.cardSprite != null)
        {
            
            MeshRenderer renderer = GetComponent<MeshRenderer>();

            if (renderer != null)
            {
                // Apply the materials to the MeshRenderer
                Material[] materials = new Material[1];
                materials[0] = Instantiate(frontMaterial);  
                renderer.materials = materials;
                
                materials[0].mainTexture = textureData.cardSprite;
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




}
    
 

    

