using UnityEngine;
using TMPro;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Nail Game")]
    public TextMeshPro congratsText;    
    public int targetNails = 6;

    [Header("Ship Unlock")]
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable wheelGrab;    
    public MonoBehaviour       shipController; 

    private int  nailsHammered = 0;
    private bool nailGameDone  = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        if (congratsText != null) 
            congratsText.gameObject.SetActive(false);

        if (wheelGrab != null) 
            wheelGrab.enabled = false;
        if (shipController != null) 
            shipController.enabled = false;
    }

    public void RegisterHammeredNail()
    {
        if (nailGameDone) return;

        nailsHammered++;
        if (nailsHammered >= targetNails)
        {
            nailGameDone = true;

            if (congratsText != null)
                congratsText.gameObject.SetActive(true);

            if (wheelGrab != null)
                wheelGrab.enabled = true;

            if (shipController != null)
                shipController.enabled = true;
        }
    }
}
