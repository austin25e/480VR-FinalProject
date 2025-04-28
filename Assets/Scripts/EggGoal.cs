using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EggGoal : MonoBehaviour
{
    private int eggCount = 0;
    public int targetEggs;

    public GameObject winCanvas;
    public TMButtonScript nextButton;

    private HashSet<GameObject> countedEggs = new HashSet<GameObject>();

    private void Start()
    {
        if (winCanvas != null)
        {
            winCanvas.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Egg"))
        {
            if (!countedEggs.Contains(other.gameObject))
            {
                countedEggs.Add(other.gameObject);
                eggCount++;
                CheckWin();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Egg"))
        {
            if (countedEggs.Contains(other.gameObject))
            {
                countedEggs.Remove(other.gameObject);
                eggCount--;

                // Optional: hide the win canvas if an egg leaves
                if (winCanvas != null)
                {
                    winCanvas.SetActive(false);
                }
            }
        }
    }

    private void CheckWin()
    {
        if (eggCount >= targetEggs)
        {
            Debug.Log("You won!");
            if (winCanvas != null)
            {
                winCanvas.SetActive(true);
                nextButton.UnlockButton(true);
            }
        }
    }
}
