using UnityEngine;
using System.Collections.Generic;


public class EggGoal : MonoBehaviour
{
    public EggGoalManager goalManager; 

    private HashSet<GameObject> snappedEggs = new HashSet<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Egg")) return;
        if (snappedEggs.Contains(other.gameObject)) return;

        Debug.Log("Egg entered goal!");

        SnapAndLockEgg(other.gameObject);
        snappedEggs.Add(other.gameObject);

        goalManager?.ReportEggPlaced();
    }

    private void SnapAndLockEgg(GameObject egg)
    {
        egg.transform.position = transform.position;
        egg.transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x, 
            transform.rotation.eulerAngles.y,
            transform.rotation.eulerAngles.z
        );

        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab = egg.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grab != null)
        {
            if (grab.isSelected && grab.interactorsSelecting.Count > 0)
            {
                var interactor = grab.interactorsSelecting[0];
                grab.interactionManager.SelectExit(interactor, grab);
            }

            grab.enabled = false;
        }

        Rigidbody rb = egg.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }
}
