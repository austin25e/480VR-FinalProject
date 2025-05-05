using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class wheatCounter : MonoBehaviour
{
    //public GameObject winText; --> not used anymore
    public XRSocketInteractor socket1;
    public XRSocketInteractor socket2;
    public XRSocketInteractor socket3;
    public TMButtonScript nextButton;

    private void OnTriggerEnter(Collider other)
    {
        if (socket1.hasSelection && socket2.hasSelection && socket3.hasSelection)
        {
            ChatManager.Instance.AdvanceObjective();
            nextButton.UnlockButton(true);
            //Invoke("HideWinText", 8f);
        }
    }
}
