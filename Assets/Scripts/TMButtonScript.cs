using UnityEngine;

public class TMButtonScript : MonoBehaviour
{
    public int buttonNumber;
    public bool isUnlocked = false;
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
        if (!isUnlocked)
        {
            return;
        }

        // Deactivate all other buttons
        if (currentlySelectedButton != null && currentlySelectedButton != this)
        {
            Debug.Log("DEACTIVATE ALL OTHER BUTTONS");
            currentlySelectedButton.buttonRenderer.material = currentlySelectedButton.defaultMaterial;
        }

        // Select this button
        Debug.Log("SELECT THIS BUTTON");
        buttonRenderer.material = selectedMaterial;
        currentlySelectedButton = this;
    }

    public static TMButtonScript GetSelectedButton()
    {
        return currentlySelectedButton;
    }
}
