using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TestTMLeverScript : MonoBehaviour
{
    [Header("Joint Settings")]
    public float springDamper = 2f;
    public float springForce = 10f;
    public Vector2 angleLimits = new Vector2(0, 60);

    [Header("Activation")]
    public float activationThreshold = 55f; // Degrees
    public float returnDelay = 0.5f; // Seconds before return
    public AudioSource pullSound;

    private HingeJoint _hinge;
    private XRGrabInteractable _grabInteractable;
    private float _originalSpringTarget;
    private bool _hasActivated;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _hinge = GetComponent<HingeJoint>();
        _grabInteractable = GetComponent<XRGrabInteractable>();

        ConfigureJoint();

        _grabInteractable.selectEntered.AddListener(OnGrab);
        _grabInteractable.selectExited.AddListener(OnRelease);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_hasActivated && _hinge.angle >= activationThreshold)
        {
            Debug.Log("Lever can activate, but needs an timeline selected!");
            pullSound.Play();
            if (TMButtonScript.GetSelectedButton() != null)
            {
                Debug.Log("Activate Lever!");
                ActivateLever();
            }
            
        }
    }

    private void ConfigureJoint()
    {
        _hinge.useSpring = true;
        _hinge.limits = new JointLimits {
            min = angleLimits.x,
            max = angleLimits.y,
            bounciness = 0,
            contactDistance = 0
        };

        _originalSpringTarget = angleLimits.x;
        _hinge.spring = new JointSpring {
            spring = springForce,
            damper = springDamper,
            targetPosition = _originalSpringTarget
        };
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        Debug.Log("Grabbing Lever!");
        // Disable spring while grabbed
        // _hinge.useSpring = false;
        _hasActivated = false;
        
        // // Play sound if available
        // if (_audioSource && pullSound)
        //     _audioSource.PlayOneShot(pullSound);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        // Re-enable spring with delay
        // Invoke(nameof(EnableSpring), returnDelay);
        Debug.Log("Release Lever!");
        JointSpring spring = _hinge.spring;
        spring.targetPosition = _originalSpringTarget;
        _hinge.spring = spring;
    }

    private void ActivateLever()
    {
        _hasActivated = true;

        TMButtonScript selectedButton = TMButtonScript.GetSelectedButton();

        Debug.Log($"Activated button {selectedButton.buttonNumber}");
        SceneManager.LoadScene(selectedButton.buttonNumber);
    }
}
