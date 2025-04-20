using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpdateTimeSign : MonoBehaviour
{
    public TextMeshPro periodNameText;
    public TextMeshPro yearsText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int activeSceneNum = SceneManager.GetActiveScene().buildIndex;

        switch (activeSceneNum)
        {
            case 0:
                periodNameText.text = "Area 0 Space";
                yearsText.text = "???";
                break;
            case 1:
                periodNameText.text = "Area 1 Cretaceous";
                yearsText.text = "70 million years ago";
                break;
            case 2:
                periodNameText.text = "Area 2 Mesopotamia";
                yearsText.text = "10,000 BCE";
                break;
            case 3:
                periodNameText.text = "Area 3 Atlantic Exploration";
                yearsText.text = "1501-1600";
                break;
            case 4:
                periodNameText.text = "Area 4 Industrial Revolution";
                yearsText.text = "1801-1900";
                break;
            case 5:
                periodNameText.text = "Area 5 Modern Day";
                yearsText.text = "2025";
                break;
            default:
                periodNameText.text = "-";
                yearsText.text = "-";
                break;
        }
    }
}
