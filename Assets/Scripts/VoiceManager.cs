using UnityEngine;
using UnityEngine.Events;
using Oculus.Voice;          // Use the proper namespace for the SDK
using Meta.WitAi.CallbackHandlers;             // Use the proper namespace for the SDK
using TMPro;                // For TextMeshPro

public class VoiceActivate : MonoBehaviour
{
    [SerializeField] private AppVoiceExperience appVoiceExperience; // Reference to the AppVoiceExperience component
    //[SerializeField] private WitResponseMatcher witResponseMatcher; // Reference to the WitResponseMatcher component
    void Start()
    {
        appVoiceExperience = GetComponent<AppVoiceExperience>();
        appVoiceExperience.Activate(); // Activate the voice experience
    }

}