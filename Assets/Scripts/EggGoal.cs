using UnityEngine;
using UnityEngine.UI; 

public class EggGoal : MonoBehaviour
{
    private int eggCount = 0;
    public int targetEggs = 1;

    public GameObject winCanvas; 

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
            eggCount++;

            if (eggCount >= targetEggs)
            {
                Debug.Log("You won!");
                if (winCanvas != null)
                {
                    winCanvas.SetActive(true); 
                }
            }
        }
    }
}
