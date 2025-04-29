using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public Vector3 beltDirection = Vector3.forward;
    public float speed = 1f;

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null && !rb.isKinematic)
        {
            rb.linearVelocity = beltDirection.normalized * speed;
        }
    }
}
