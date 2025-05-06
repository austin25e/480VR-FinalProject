using UnityEngine;
using System.Collections.Generic;

public class EggGoal : MonoBehaviour
{
    private int eggCount = 0;
    public int targetEggs;

    public TMButtonScript nextButton;

    private HashSet<GameObject> countedEggs = new HashSet<GameObject>();
    private bool hasWon = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Egg"))
        {
            if (!countedEggs.Contains(other.gameObject))
            {
                Debug.Log("Enter!");
                countedEggs.Add(other.gameObject);
                eggCount++;

                ChatManager.Instance.AdvanceObjective();
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
                Debug.Log("Exit!");
                countedEggs.Remove(other.gameObject);
                eggCount--;

                CheckWin();
            }
        }
    }

    private void CheckWin()
    {
        if (eggCount >= targetEggs)
        {
            if (!hasWon)
            {
                Debug.Log("You won!");
                hasWon = true;
                nextButton.UnlockButton(true);
            }
        }
        else
        {
            if (hasWon)
            {
                Debug.Log("Lost win state due to egg removal.");
                nextButton.UnlockButton(false);
                hasWon = false;
            }

            ChatManager.Instance.NoSpeakAdvanceObjective();
        }
    }
}
