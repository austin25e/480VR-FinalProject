using UnityEngine;

public class EggGoal : MonoBehaviour
{
    private int eggCount = 0;
    public int targetEggs = 3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Egg"))
        {
            eggCount++;
            if (eggCount >= targetEggs)
            {
                Debug.Log("You won!");
            }
        }
    }
}
