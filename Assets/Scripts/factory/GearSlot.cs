using UnityEngine;

public class GearSlot : MonoBehaviour
{
    public string correctGearID; // Expected gear's ID
    public float snapRange = 0.2f; // How close the gear must get to snap
    private bool isFilled = false;

    private void OnTriggerStay(Collider other)
    {
        if (isFilled) return;

        Gear gear = other.GetComponent<Gear>();
        if (gear != null)
        {
            // Check if the correct gear and close enough
            float distance = Vector3.Distance(transform.position, gear.transform.position);

            if (gear.gearID == correctGearID && distance <= snapRange)
            {
                gear.SnapToPosition(transform);
                isFilled = true;
                Debug.Log("Correct gear placed!");
            }
        }
    }
}
