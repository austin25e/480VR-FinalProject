using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(0, 3, 0);
    public float openSpeed = 2f;

    private Vector3 closedPosition;
    private Vector3 targetPosition;
    private bool isOpening = false;

    void Start()
    {
        closedPosition = transform.position;
        targetPosition = closedPosition;
    }

    public void OpenDoor()
    {
        targetPosition = closedPosition + openOffset;
        isOpening = true;
    }

    void Update()
    {
        if (isOpening)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, openSpeed * Time.deltaTime);
        }
    }
}
