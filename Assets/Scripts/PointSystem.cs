using TMPro;
using UnityEngine;

public class PointSystem : MonoBehaviour
{
    
    public int points = 300;
    
    public TextMeshProUGUI pointsText;
    
    void Start()
    {
        UpdatePointsDisplay(); // Update the display at the start
    }

    // Add points to the current total
    public void AddPoints(int pointsToAdd)
    {
        points += pointsToAdd;
        UpdatePointsDisplay();
    }

    // Subtract points from the current total
    public void SubtractPoints(int pointsToSubtract)
    {
        if (points >= pointsToSubtract)
        {
            points -= pointsToSubtract;
        }
        else
        {
            Debug.Log("Not enough points to subtract!");
        }
        UpdatePointsDisplay();
    }

    public bool HasEnoughtPoints(int requiredPoints)
    {
        return points >= requiredPoints;
    }

    // Update the points display (UI Text)
    private void UpdatePointsDisplay()
    {
        pointsText.text = "Points: " + points.ToString();
    }
    
    
}
