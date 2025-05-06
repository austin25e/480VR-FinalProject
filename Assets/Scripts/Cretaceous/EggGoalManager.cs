using UnityEngine;

public class EggGoalManager : MonoBehaviour
{
    public int targetEggs = 3;
    public TMButtonScript nextButton;

    private int totalEggsPlaced = 0;
    private bool hasWon = false;

    public void ReportEggPlaced()
    {
        totalEggsPlaced++;
        ChatManager.Instance.AdvanceObjective();
        ChatManager.Instance.NoSpeakAdvanceObjective();
        if (!hasWon && totalEggsPlaced >= targetEggs)
        {
            hasWon = true;
            Debug.Log("Goal!");
            nextButton.UnlockButton(true);
        }
    }

    public void ReportEggFailed()
    {
        if (!hasWon)
        {
            Debug.Log("Lost!");
            ChatManager.Instance.NoSpeakAdvanceObjective();
        }
    }
}
