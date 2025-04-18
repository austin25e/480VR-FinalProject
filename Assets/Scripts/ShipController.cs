using UnityEngine;


public class ShipController : MonoBehaviour
{
    [Header("References")]
    public Transform wheelTransform;       // Your wheel’s Transform (pivoted correctly)
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable wheelGrab;   // The XRGrabInteractable on that wheel
    public Transform environmentToMove;    // The Island/world you want to shift
    public Transform centerPoint;          // The fixed pivot (boat or XR Rig)

    [Header("Settings")]
    public float moveSpeed       = 5f;     // “Forward” speed in units/sec
    public float turnSensitivity = 1f;     // Scale for wheel Δ‑angle → world rotation

    bool  isSteering    = false;
    float lastWheelRotX = 0f;             // Track wheel’s X (or Z) axis rotation

    void Awake()
    {
        wheelGrab.selectEntered.AddListener(_ => BeginSteering());
        wheelGrab.selectExited  .AddListener(_ => EndSteering());
    }

    void BeginSteering()
    {
        isSteering    = true;
        lastWheelRotX = wheelTransform.localEulerAngles.x;  // or .z if your wheel turns on Z
    }

    void EndSteering()
    {
        isSteering = false;
    }

    void Update()
    {
        if (!isSteering) return;

        // 1) How much the wheel turned this frame
        float currentX = wheelTransform.localEulerAngles.x;
        float deltaX   = Mathf.DeltaAngle(lastWheelRotX, currentX);
        lastWheelRotX  = currentX;

        // 2) Compute new position by rotating the offset vector,
        //    but DO NOT change environmentToMove.rotation
        Vector3 pivot  = centerPoint.position;
        Vector3 offset = environmentToMove.position - pivot;
        Quaternion rot = Quaternion.AngleAxis(-deltaX * turnSensitivity, Vector3.up);
        Vector3 newPos = pivot + (rot * offset);

        // 3) Slide backward “forward” motion
        newPos += -centerPoint.forward * moveSpeed * Time.deltaTime;

        // 4) Apply ONLY position
        environmentToMove.position = newPos;
    }
}
