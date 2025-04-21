using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class wheatCounter : MonoBehaviour
{
    public GameObject winText;
    public XRSocketInteractor socket1;
    public XRSocketInteractor socket2;
    public XRSocketInteractor socket3;

    private void OnTriggerEnter(Collider other)
    {
        if (socket1.hasSelection && socket2.hasSelection && socket3.hasSelection)
        {
            winText.SetActive(true);
            Invoke("HideWinText", 8f);
        }
    }
}
