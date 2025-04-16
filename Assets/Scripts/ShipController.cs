using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShipController : MonoBehaviour
{
    public Transform wheel;                        // wood_wheel transform
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;    // XR Grab Interactable on wheel
    public Transform xrRig;                        // XR Origin (to move with the ship)

    public float moveSpeed = 5f;
    public float turnSpeed = 50f;

    private bool isGrabbed = false;
    private float lastWheelY;
    private Vector3 wheelStartPos;
    private Vector3 lastShipPosition;

    void Start()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }

        wheelStartPos = wheel.position;
        lastShipPosition = transform.position;
    }

    void Update()
    {
        // Always lock the wheel's position in case of drifting
        wheel.position = wheelStartPos;

        if (isGrabbed)
        {
            // Move the ship forward
            Vector3 move = transform.forward * moveSpeed * Time.deltaTime;
            transform.Translate(move, Space.World);

            // Move XR rig with ship
            Vector3 shipDelta = transform.position - lastShipPosition;
            if (xrRig != null) xrRig.position += shipDelta;
            lastShipPosition = transform.position;

            // Calculate rotation delta
            float currentWheelY = wheel.localEulerAngles.y;
            float turnInput = Mathf.DeltaAngle(lastWheelY, currentWheelY);
            transform.Rotate(Vector3.up, turnInput * turnSpeed * Time.deltaTime / 90f);
            lastWheelY = currentWheelY;
        }
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        lastWheelY = wheel.localEulerAngles.y;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
            grabInteractable.selectExited.RemoveListener(OnRelease);
        }
    }
}
