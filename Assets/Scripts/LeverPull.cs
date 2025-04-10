using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class LeverPull : MonoBehaviour
{
   public XRGrabInteractable grabInteractable; // Reference to the XRGrabInteractable component
    private Quaternion initialRotation; // Initial rotation of the lever
    private float lastRotationZ; // The last known Z rotation
    private float currentRotationZ; // The current Z rotation

    public float minRotationZ = 0f; // Minimum Z rotation (0 degrees)
    public float maxRotationZ = 90f; // Maximum Z rotation (90 degrees)

    private void Start()
    {
        // Store the initial rotation of the lever
        initialRotation = transform.rotation;

        // Optionally, get the XRGrabInteractable if not set in inspector
        if (grabInteractable == null)
            grabInteractable = GetComponent<XRGrabInteractable>();

       
    }

    private void OnGrabStarted(XRBaseInteractor interactor)
    {
        // Store the initial Z rotation when the lever is grabbed
        lastRotationZ = transform.rotation.eulerAngles.z;
    }

    private void OnGrabExited(XRBaseInteractor interactor)
    {
        // Optionally, reset the lever when released (this can be adjusted based on your needs)
        transform.rotation = initialRotation;
    }

    private void Update()
    {
        if (grabInteractable.isSelected)
        {
            // Calculate the difference in rotation along the Z-axis
            float deltaRotationZ = transform.rotation.eulerAngles.z - lastRotationZ;

            // Update the current rotation, but only allow movement along the Z-axis
            currentRotationZ = transform.rotation.eulerAngles.z;

            // Constrain the rotation to the range of 0 to 90 degrees
            currentRotationZ = Mathf.Clamp(currentRotationZ, minRotationZ, maxRotationZ);

            // Apply the constrained rotation
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, currentRotationZ);

            // Update the last rotation for the next frame
            lastRotationZ = transform.rotation.eulerAngles.z;
        }
    }
    
    
    
    
    
    
    
    
    
    
    
}
