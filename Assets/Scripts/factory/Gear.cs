using UnityEngine;


public class Gear : MonoBehaviour
{
    public string gearID; // Unique identifier
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Rigidbody rb;

    private void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
    }

    public void SnapToPosition(Transform target)
    {

        // Snap to target
        transform.position = target.position;
        transform.rotation = Quaternion.Euler(
            target.rotation.eulerAngles.x + 90,
            target.rotation.eulerAngles.y,
            target.rotation.eulerAngles.z
        );
        // Lock gear (disable physics and grabbing)
        grabInteractable.enabled = false;
        rb.isKinematic = true;
        rb.useGravity = false;
    }
}
