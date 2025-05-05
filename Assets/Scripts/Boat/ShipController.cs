using UnityEngine;


public class ShipController : MonoBehaviour
{
    public Transform wheelTransform;        
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable wheelGrab;    
    public Transform environmentToMove;     
    public Transform centerPoint;          

    [Header("Settings")]
    public float moveSpeed       = 5f;      
    public float turnSensitivity = 1f;      

    [Header("Finish Condition")]
    public float finishDistance = 1f;       

    //public GameObject congratsUI; --> Not used anymore    

    bool  isSteering    = false;
    bool  finished      = false;
    float lastWheelRotX = 0f;       
    public TMButtonScript nextButton;    
    public AudioSource wheelSteerSFX;  

    void Awake()
    {
        wheelGrab.selectEntered.AddListener(_ => BeginSteering());
        wheelGrab.selectExited .AddListener(_ => EndSteering());

        //if (congratsUI != null) congratsUI.SetActive(false);
    }

    void BeginSteering()
    {
        if (finished) return;              
        isSteering    = true;
        wheelSteerSFX.Play();
        lastWheelRotX = wheelTransform.localEulerAngles.x;
    }

    void EndSteering()
    {
        isSteering = false;
        wheelSteerSFX.Stop();
    }

    void Update()
    {
        if (!isSteering || finished) return;

        float currentX = wheelTransform.localEulerAngles.x;
        float deltaX   = Mathf.DeltaAngle(lastWheelRotX, currentX);
        lastWheelRotX  = currentX;

        Vector3 pivot  = centerPoint.position;
        Vector3 offset = environmentToMove.position - pivot;

        Quaternion rot = Quaternion.AngleAxis(-deltaX * turnSensitivity, Vector3.up);
        Vector3 newPos = pivot + (rot * offset);

        newPos += -centerPoint.forward * moveSpeed * Time.deltaTime;

        environmentToMove.position = newPos;

        if (Vector3.Distance(environmentToMove.position, centerPoint.position) <= finishDistance)
        {
            finished = true;
            isSteering = false;
            nextButton.UnlockButton(true);
            //if (congratsUI != null)
            //    congratsUI.SetActive(true);
            ChatManager.Instance.AdvanceObjective();
        }
    }
}

