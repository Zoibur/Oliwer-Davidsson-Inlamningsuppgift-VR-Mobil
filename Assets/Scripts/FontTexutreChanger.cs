using UnityEditor.U2D;
using UnityEngine;

public class FontTexutreChanger : MonoBehaviour
{
    public Card textureData;  
    public Material frontMaterial;  
    public Material backMaterial;   

    void Start()
    {
        if (textureData != null && textureData.cardSprite != null)
        {
            
            MeshRenderer renderer = GetComponent<MeshRenderer>();

            if (renderer != null)
            {
                // Apply the materials to the MeshRenderer
                Material[] materials = new Material[1];
                materials[0] = frontMaterial;  
                
                renderer.materials = materials;

               
                frontMaterial.mainTexture = textureData.cardSprite;
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
    
 

    

