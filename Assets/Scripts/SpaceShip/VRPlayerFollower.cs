using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class VRPlayerFollower : MonoBehaviour
{
    [Header("Player Reference")]
    public Transform vrPlayer;  // Assign your XR Origin (not the camera)
    public Transform playerAreaBox;

    [Header("Position Settings")]
    public Vector3 relativeOffset = new Vector3(0.5f, 2f, -1.5f); // (right, up, back)
    public float positionSmoothness = 5f;
    public float rotationSmoothness = 3f;
    public bool followEnabled = true;
    private bool npcGrabbed = false;

    private Vector3 targetPosition;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Rigidbody rigidBody;

    void Start()
    {
        int activeSceneNum = SceneManager.GetActiveScene().buildIndex;
        // Debug.Log("Active Scene is " + activeSceneNum);
        if (activeSceneNum == 5)
        {
            grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            rigidBody = GetComponent<Rigidbody>();

            if (grabInteractable != null && rigidBody != null)
            {
                grabInteractable.selectEntered.AddListener(OnGrab);
                // grabInteractable.selectExited.AddListener(OnRelease);
            }
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // Debug.Log("Grabbing");
        rigidBody.useGravity = true;
        npcGrabbed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (vrPlayer == null) return;
        if (playerAreaBox == null) return;
        if (followEnabled == false) return;
        if (npcGrabbed == true) return;

        //TODO: Use the PlayerAreaBox to calculate the NPC's follow position as back right of the box instead of the actual player.
        // Calculate position in world space (ignoring player rotation)
        targetPosition = playerAreaBox.position 
                       + playerAreaBox.TransformDirection(relativeOffset);

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
            Quaternion targetRotation = Quaternion.LookRotation(-lookDirection) * Quaternion.Euler(30, 0, 0);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSmoothness * Time.deltaTime
            );
        }

        // // Calculate position in world space (ignoring player rotation)
        // targetPosition = vrPlayer.position 
        //                + vrPlayer.TransformDirection(relativeOffset);

        // // Smooth position movement
        // transform.position = Vector3.Lerp(
        //     transform.position,
        //     targetPosition,
        //     positionSmoothness * Time.deltaTime
        // );

        // // Face toward player (optional)
        // Vector3 lookDirection = vrPlayer.position - transform.position;
        // if (lookDirection != Vector3.zero)
        // {
        //     Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        //     transform.rotation = Quaternion.Slerp(
        //         transform.rotation,
        //         targetRotation,
        //         rotationSmoothness * Time.deltaTime
        //     );
        // }
    }
}
