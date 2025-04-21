using UnityEngine;


public class ShipController : MonoBehaviour
{
    [Header("References")]
    public Transform wheelTransform;        // your wheel’s Transform
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable wheelGrab;    // XRGrabInteractable on the wheel
    public Transform environmentToMove;     // the “island” you push around
    public Transform centerPoint;           // the fixed boat/XR Rig

    [Header("Settings")]
    public float moveSpeed       = 5f;      // forward speed (units/sec)
    public float turnSensitivity = 1f;      // wheel Δ‑angle → world rotation multiplier

    [Header("Finish Condition")]
    public float finishDistance = 1f;       // how close the island must get to count as “hit”
    public GameObject congratsUI;          // your “Congrats!” panel (initially inactive)

    bool  isSteering    = false;
    bool  finished      = false;
    float lastWheelRotX = 0f;             // track wheel’s rotation along its turning axis

    void Awake()
    {
        // hook up grab events
        wheelGrab.selectEntered.AddListener(_ => BeginSteering());
        wheelGrab.selectExited .AddListener(_ => EndSteering());

        // ensure the UI is hidden at start
        if (congratsUI != null) congratsUI.SetActive(false);
    }

    void BeginSteering()
    {
        if (finished) return;              // do nothing if already done
        isSteering    = true;
        lastWheelRotX = wheelTransform.localEulerAngles.x; // or .z if your wheel uses Z
    }

    void EndSteering()
    {
        isSteering = false;
    }

    void Update()
    {
        // 1) bail out if not actively steering or already finished
        if (!isSteering || finished) return;

        // 2) compute wheel Δ‑angle
        float currentX = wheelTransform.localEulerAngles.x;
        float deltaX   = Mathf.DeltaAngle(lastWheelRotX, currentX);
        lastWheelRotX  = currentX;

        // 3) rebuild island position:
        Vector3 pivot  = centerPoint.position;
        Vector3 offset = environmentToMove.position - pivot;

        // 3a) apply a tiny rotation around the boat’s Y-axis
        Quaternion rot = Quaternion.AngleAxis(-deltaX * turnSensitivity, Vector3.up);
        Vector3 newPos = pivot + (rot * offset);

        // 3b) then slide it "backward" to simulate forward motion
        newPos += -centerPoint.forward * moveSpeed * Time.deltaTime;

        environmentToMove.position = newPos;

        // 4) check finish condition
        if (Vector3.Distance(environmentToMove.position, centerPoint.position) <= finishDistance)
        {
            finished = true;
            isSteering = false;
            if (congratsUI != null)
                congratsUI.SetActive(true);
        }
    }
}

