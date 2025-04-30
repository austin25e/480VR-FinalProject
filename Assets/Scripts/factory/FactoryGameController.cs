using UnityEngine;
using TMPro;

public class FactoryGameController : MonoBehaviour
{
    public int requiredOreCount = 3;
    private int oreCount = 0;
    private int coalCount = 0;

    public GameObject door; 
    public TextMeshPro winText; 
    public int Gears;

    private int correctGearPlacements = 0;
    private bool gearGameUnlocked = false;

    void Start()
    {
        if (winText != null)
            winText.gameObject.SetActive(false);
    }

    public void ReportOre(string oreTag)
    {
        if (gearGameUnlocked) return;

        if (oreTag == "Ore") oreCount++;
        else if (oreTag == "Coal") coalCount++;

        if (oreCount >= requiredOreCount && coalCount >= requiredOreCount)
        {
            UnlockGearGame();
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

        if (correctGearPlacements >= Gears && winText != null)
        {
            winText.gameObject.SetActive(true);
        }
    }
}
