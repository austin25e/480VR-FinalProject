using UnityEngine;

public class wheatStuff : MonoBehaviour
{
    public string targetTag = "Axe";
    public GameObject droppedWheat;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Destroy(gameObject);
            Vector3 spawnPosition = gameObject.transform.position;
            Quaternion spawnRotation = gameObject.transform.rotation;

            spawnPosition.y += 1.0f;
            Instantiate(droppedWheat, spawnPosition, spawnRotation);

        }
    }
}
