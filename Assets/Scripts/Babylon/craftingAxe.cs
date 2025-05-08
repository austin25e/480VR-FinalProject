using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Collections;
using System.Net.Sockets;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.InputSystem;

public class craftingAxe : MonoBehaviour
{
    //public GameObject instructionText; --> not used anymore
    //public GameObject instructionText1; --> not used anymore
    public GameObject spawn_item;
    public GameObject spawn_location;
    public XRSocketInteractor socket1;
    public XRSocketInteractor socket2;
    public AudioSource craftSFX;
    private int StepCounter = 0; // 

    private void OnTriggerEnter(Collider other)
    {
        if (socket1.hasSelection && socket2.hasSelection)
        {
            // instructionText.SetActive(true);
            if (StepCounter < 2)
            {
                ChatManager.Instance.AdvanceObjective();
            }
            if (StepCounter > 0 && other.CompareTag("rock"))
            {
                // instructionText.SetActive(false);
                IXRSelectInteractable interactable1 = socket1.GetOldestInteractableSelected();
                IXRSelectInteractable interactable2 = socket2.GetOldestInteractableSelected();

                GameObject item1 = interactable1.transform.gameObject;
                GameObject item2 = interactable2.transform.gameObject;

                craftSFX.Play();

                Destroy(item1);
                Destroy(item2);

                Vector3 spawnPosition = spawn_location.transform.position;
                Quaternion spawnRotation = spawn_location.transform.rotation;
                spawnPosition.y += 1.0f;
                Instantiate(spawn_item, spawnPosition, spawnRotation);
                socket1.socketActive = false;
                socket2.socketActive = false;
                // instructionText1.SetActive(true);
            }

            StepCounter++;
        }




    }
}
