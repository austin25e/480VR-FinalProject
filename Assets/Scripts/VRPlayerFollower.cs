using UnityEngine;

public class VRPlayerFollower : MonoBehaviour
{
    [Header("Player Reference")]
    public Transform vrPlayer;  // Assign your XR Origin (not the camera)

    [Header("Position Settings")]
    public Vector3 relativeOffset = new Vector3(0.5f, 2f, -1.5f); // (right, up, back)
    public float positionSmoothness = 5f;
    public float rotationSmoothness = 3f;

    private Vector3 targetPosition;

    // Update is called once per frame
    void Update()
    {
        if (vrPlayer == null) return;

        // Calculate position in world space (ignoring player rotation)
        targetPosition = vrPlayer.position 
                       + vrPlayer.TransformDirection(relativeOffset);

        // Smooth position movement
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            positionSmoothness * Time.deltaTime
        );

        // Face toward player (optional)
        Vector3 lookDirection = vrPlayer.position - transform.position;
        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSmoothness * Time.deltaTime
            );
        }
    }
}
