using UnityEngine;

public class OreCollector : MonoBehaviour
{
    public string acceptedOreTag;
    public Transform returnPoint; 
    public FactoryGameController gameController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(acceptedOreTag))
        {
            Debug.Log("Accepted: " + other.tag);
            gameController?.ReportOre(other.tag);
            Destroy(other.gameObject);
        }
        else
        {
            Debug.Log("Rejected: " + other.tag);
            Rigidbody rb = other.attachedRigidbody;
            if (rb != null && returnPoint != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.MovePosition(returnPoint.position);
            }
        }
    }
}
