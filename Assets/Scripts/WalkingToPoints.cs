using UnityEngine;

public class WalkingToPoints : MonoBehaviour
{
    public Transform[] waypoints;  // Array of waypoints
    public float speed = 5f;       // Movement speed
    public float rotationSpeed = 5f; // Rotation speed

    private int currentWaypointIndex = 0;  

    void Update()
    {
        if (waypoints.Length == 0) return;  

       
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = targetWaypoint.position - transform.position;

        
        direction.y = 0;

        
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        
        if (direction != Vector3.zero)  
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);  
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); 
        }

        
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
           
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}
