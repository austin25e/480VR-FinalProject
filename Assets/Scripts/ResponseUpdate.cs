using UnityEngine;
using UnityEngine.Events;
using Oculus.Voice;          // Use the proper namespace for the SDK
using Meta.WitAi.CallbackHandlers;             // Use the proper namespace for the SDK
using TMPro;

public class ResponseUpdate : MonoBehaviour
{
    public TextMeshPro responseText; // Assign your UI Text element her
    [SerializeField] private AppVoiceExperience appVoiceExperience; // Reference to the AppVoiceExperience component

    public void UpdateResponse(string[] values)
    {
        if (responseText == null)
        {
            Debug.LogError("Response Text is not assigned.");
            return;
        }

        if (values.Length > 0 && values[0] == "greeting") // Check if the array has at least one element
        {
            // If the intent is "greeting", display "Hello"
            responseText.text = "Hello_";
        }
        else
        {
            // Assign a default or other value
            responseText.text = "Hello";
            appVoiceExperience.Deactivate(); // Deactivate the voice experience
        }
    }
}
