using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TMButtonScript : MonoBehaviour
{
    [SerializeField] private InteractionLayerMask hoverLayer; // Layer for hovering (e.g., "UI")
    [SerializeField] private InteractionLayerMask selectLayer; // Layer for selection (e.g., "Default")

    public int buttonNumber;
    public bool isUnlocked = false;
    public bool isSelected = false;
    public AudioSource buttonPressSFX;
    public Material defaultMaterial;
    public Material selectedMaterial;
    public Material unactivatedMaterial;

    public Renderer buttonRenderer;
    private static TMButtonScript currentlySelectedButton;
    private XRSimpleInteractable _interactable;
    private Vector3 origButtonPos = new Vector3(0, 3.286142e-05f, 0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // buttonRenderer = GetComponent<Renderer>();
        _interactable = GetComponent<XRSimpleInteractable>();
        buttonRenderer.material = defaultMaterial;
        // Debug.Log("BUTTON RENDERER: " + buttonRenderer.name);

        // Set up dual interaction
        _interactable.hoverEntered.AddListener(OnHoverEnter);

        if (!isUnlocked)
        {
            buttonRenderer.material = unactivatedMaterial;
        }
    }

    public void UnlockButton(bool toggle)
    {
        isUnlocked = toggle;

        if (toggle)
        {
            buttonRenderer.material = defaultMaterial;
        }
        else
        {
            buttonRenderer.material = unactivatedMaterial;
        }
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        // Only apply hover effects if not physically pressed
        if (args.interactorObject.interactionLayers == hoverLayer)
        {
            PressButton();
        }
    }

    public void PressButton()
    {
        Debug.Log("Press Button Start!");
        if (!isUnlocked || isSelected)
        {
            return;
        }

        // Deactivate all other buttons
        if (currentlySelectedButton != null && currentlySelectedButton != this)
        {
            // Debug.Log("DEACTIVATE ALL OTHER BUTTONS");
            currentlySelectedButton.isSelected = false;
            currentlySelectedButton.buttonRenderer.material = currentlySelectedButton.defaultMaterial;
            currentlySelectedButton.buttonRenderer.transform.localPosition = origButtonPos;
        }

        // Select this button
        Debug.Log("SELECT THIS BUTTON");
        buttonRenderer.transform.localPosition = new Vector3(0, -0.46f, -0.056f);
        isSelected = true;
        buttonRenderer.material = selectedMaterial;
        currentlySelectedButton = this;
        buttonPressSFX.Play();
    }

    public static TMButtonScript GetSelectedButton()
    {
        return currentlySelectedButton;
    }
}
