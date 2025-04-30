using UnityEngine;

public class GearSlot : MonoBehaviour
{
    public string correctGearID; 
    public float snapRange = 0.2f; 
    private bool isFilled = false;

    public FactoryGameController gameController;


    private void OnTriggerStay(Collider other)
    {
        if (isFilled) return;

        Gear gear = other.GetComponent<Gear>();
        if (gear != null)
        {
            float distance = Vector3.Distance(transform.position, gear.transform.position);

            if (gear.gearID == correctGearID && distance <= snapRange)
            {
                gear.SnapToPosition(transform);
                gameController?.ReportCorrectGearPlacement();
                isFilled = true;
                Debug.Log("Correct gear placed!");
            }
        }
    }
}
