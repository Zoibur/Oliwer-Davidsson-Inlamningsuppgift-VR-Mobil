using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class LeverPull : MonoBehaviour
{
    public Transform leverBase; // Pivot point
    public float torqueMultiplier = 50f;

    private XRGrabInteractable grab;
    private Rigidbody rb;
    private Transform interactorTransform;
  

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grab = GetComponent<XRGrabInteractable>();

        grab.trackRotation = false; // Important: disable default rotation tracking
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        interactorTransform = args.interactorObject.transform;
        
        
        
    }

    void OnRelease(SelectExitEventArgs args)
    {
        interactorTransform = null;
        rb.angularVelocity = Vector3.zero; // Optional: stop spinning
       
    }

    void FixedUpdate()
    {
        if (interactorTransform != null)
        {
            Vector3 localDirection = leverBase.InverseTransformPoint(interactorTransform.position) - leverBase.InverseTransformPoint(transform.position);
            float targetAngle = Mathf.Atan2(localDirection.y, localDirection.x) * Mathf.Rad2Deg;

            // Get current local Z rotation
            float currentZ = transform.localEulerAngles.z;
            if (currentZ > 180f) currentZ -= 360f;

            // Calculate angle difference
            float angleDiff = Mathf.DeltaAngle(currentZ, targetAngle);
            float torque = angleDiff * torqueMultiplier;

            // Apply torque around Z
            rb.AddRelativeTorque(new Vector3(0f, 0f, torque));
        }
    }
}