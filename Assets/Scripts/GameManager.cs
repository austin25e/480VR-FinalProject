using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshPro congratsText; // assign your “Congrats!” text here

    [HideInInspector]
    public int nailsHammered = 0;
    public int targetNails = 6;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (congratsText == null)
            Debug.LogError("GameManager: congratsText not set!");
        else
            congratsText.gameObject.SetActive(false);
    }

    public void RegisterHammeredNail()
    {
        nailsHammered++;
        if (nailsHammered >= targetNails)
            congratsText.gameObject.SetActive(true);
    }
}
