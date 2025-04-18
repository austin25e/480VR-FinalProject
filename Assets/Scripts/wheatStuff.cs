using UnityEngine;

public class wheatStuff : MonoBehaviour
{
    public GameObject objectToDestory;
    public GameObject spawnItem;
    public GameObject axe;
    private int hitCount;

    public void onContact(Collider item)
    {
        if (item.CompareTag("Axe") & hitCount < 3)
        {
            hitCount++;
        } else if (item.CompareTag("Axe") & hitCount == 3)
        {
            hitCount -= 3;
            Destroy(objectToDestory);
        }
    }
}
