using UnityEngine;
using UnityEngine.SceneManagement;

public class TMLeverScript : MonoBehaviour
{
    [Header("Lever Rotation Settings")]
    public float rotationAngle = 45f; // How far the lever rotates
    public float rotationSpeed = 90f; // Degrees per second
    public Transform leverPivot; // The part that rotates (assign in inspector)

    [Header("Activation Settings")]
    public float activationDelay = 0.3f; // Time after rotation starts to activate

    private bool isActivated = false;
    private Quaternion startRotation;
    private Quaternion endRotation;
    private float rotationProgress = 0f;

    void Start()
    {
        startRotation = leverPivot.localRotation;
        endRotation = startRotation * Quaternion.Euler(rotationAngle, 0, 0);
    }

    public void ActivateLever()
    {
        if (!isActivated)
        {
            isActivated = true;
            rotationProgress = 0f;
            Invoke("ActivateButton", activationDelay);
        }
    }

    void Update()
    {
        if (isActivated && rotationProgress < 1f)
        {
            rotationProgress += rotationSpeed * Time.deltaTime / rotationAngle;
            leverPivot.localRotation = Quaternion.Slerp(
                startRotation, 
                endRotation, 
                Mathf.Clamp01(rotationProgress)
            );

            // ResetLever();
        }
        else if (isActivated && leverPivot.localRotation == endRotation)
        {
            ResetLever();
        }
        // else if (!isActivated && rotationProgress > 0f)
        // {
            
        // }
    }

    private void ActivateButton()
    {
        TMButtonScript selectedButton = TMButtonScript.GetSelectedButton();
        
        if (selectedButton != null)
        {
            Debug.Log($"Activated button {selectedButton.buttonNumber}");
            // Add your button-specific actions here
            SceneManager.LoadScene(selectedButton.buttonNumber);
        }
        else
        {
            Debug.Log("Lever activated but no button selected!");
        }
    }

    // Call this to reset the lever (optional)
    public void ResetLever()
    {
        isActivated = false;
        leverPivot.localRotation = startRotation;
    }


    /*public float pullThreshold = 0.2f;
    public Transform lever;
    public float returnSpeed = 5f;

    private Vector3 initialPosition;
    private bool isGrabbed = false;
    private bool wasActivated = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPosition = lever.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrabbed)
        {
            // Simple pull detection
            float pullDistance = initialPosition.y - lever.localPosition.y;
            
            // Activate if pulled past threshold
            if (pullDistance >= pullThreshold && !wasActivated)
            {
                ActivateSelectedButton();
                wasActivated = true;
            }
            else if (pullDistance < pullThreshold * 0.8f)
            {
                wasActivated = false;
            }
        }
        else if (lever.localPosition != initialPosition)
        {
            // Return to original position when not grabbed
            lever.localPosition = Vector3.Lerp(
                lever.localPosition,
                initialPosition,
                Time.deltaTime * returnSpeed
            );
        }
    }

     // Call this when the lever is grabbed
    public void GrabLever()
    {
        Debug.Log("GRAB LEVER");
        isGrabbed = true;
        wasActivated = false;
    }

    // Call this when the lever is released
    public void ReleaseLever()
    {
        Debug.Log("RELEASE LEVER");
        isGrabbed = false;
    }

    private void ActivateSelectedButton()
    {
        TMButtonScript selectedButton = TMButtonScript.GetSelectedButton();

        if (selectedButton != null)
        {
            Debug.Log($"Activating button {selectedButton.buttonNumber}");

        }
        else
        {
            Debug.Log("No button selected!");
        }

        // Release the lever after activation
        
    }*/
}
