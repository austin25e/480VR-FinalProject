using UnityEngine;
using TMPro;

public class FactoryGameController : MonoBehaviour
{
    public int requiredOreCount = 3;
    private int oreCount = 0;
    private int coalCount = 0;

    public GameObject door; 
    //public TextMeshPro winText; --> No longer needed, handled in ChatManager
    public int Gears;

    private int correctGearPlacements = 0;
    private bool gearGameUnlocked = false;

    public TMButtonScript nextButton;

    void Start()
    {
        //if (winText != null)
        //    winText.gameObject.SetActive(false); --> No longer needed, handled in ChatManager
    }

    public void ReportOre(string oreTag)
    {
        if (gearGameUnlocked) return;

        if (oreTag == "Ore") oreCount++;
        else if (oreTag == "Coal") coalCount++;

        if (oreCount >= requiredOreCount && coalCount >= requiredOreCount)
        {
            UnlockGearGame();
            ChatManager.Instance.AdvanceObjective();
        }
    }

    void UnlockGearGame()
    {
        gearGameUnlocked = true;

        if (door != null)
        {
            DoorController doorCtrl = door.GetComponent<DoorController>();
            if (doorCtrl != null)
                doorCtrl.OpenDoor();
        }

    }

    public void ReportCorrectGearPlacement()
    {
        if (!gearGameUnlocked) return;

        correctGearPlacements++;

        if (correctGearPlacements >= Gears) // && winText != null // --> No longer needed, handled in ChatManager
        {
            ChatManager.Instance.AdvanceObjective();
            //winText.gameObject.SetActive(true); --> No longer needed, handled in ChatManager
            nextButton.UnlockButton(true);
        }
    }
}
