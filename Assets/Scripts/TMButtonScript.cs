using UnityEngine;

public class TMButtonScript : MonoBehaviour
{
    public int buttonNumber;
    public bool isUnlocked = false;
    public bool isSelected = false;
    public AudioSource buttonPressSFX;
    public Material defaultMaterial;
    public Material selectedMaterial;
    public Material unactivatedMaterial;

    private Renderer buttonRenderer;
    private static TMButtonScript currentlySelectedButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonRenderer = GetComponent<Renderer>();
        buttonRenderer.material = defaultMaterial;

        if (!isUnlocked)
        {
            buttonRenderer.material = unactivatedMaterial;
        }
    }

    public void PressButton()
    {
        if (!isUnlocked || isSelected)
        {
            return;
        }

        // Deactivate all other buttons
        if (currentlySelectedButton != null && currentlySelectedButton != this)
        {
            Debug.Log("DEACTIVATE ALL OTHER BUTTONS");
            currentlySelectedButton.isSelected = false;
            currentlySelectedButton.buttonRenderer.material = currentlySelectedButton.defaultMaterial;
        }

        // Select this button
        Debug.Log("SELECT THIS BUTTON");
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
