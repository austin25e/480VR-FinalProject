using UnityEngine;
using Meta.WitAi.TTS.Utilities;  // Namespace for TTSSpeaker

public class AreaSpeechTrigger : MonoBehaviour
{
    [Tooltip("Assign the TTSSpeaker that will play the speech.")]
    [SerializeField] private TTSSpeaker ttsSpeaker;

    [Tooltip("Key or full text to speak when the player enters.")]
    [TextArea] public string speechText = "You have entered a mysterious zone.";

    [Tooltip("Player tag to detect entering the zone.")]
    public string playerTag = "Player";

    private bool _hasSpoken = false;

    void Awake()
    {
        // Auto-find TTSSpeaker if not assigned
        if (ttsSpeaker == null)
        {
            ttsSpeaker = FindFirstObjectByType<TTSSpeaker>();
            if (ttsSpeaker == null)
            {
                Debug.LogError("AreaSpeechTrigger: No TTSSpeaker found in scene.");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Only trigger once and only for the player
        if (!_hasSpoken && other.CompareTag(playerTag))
        {
            if (!string.IsNullOrEmpty(speechText))
            {
                ttsSpeaker.Speak(speechText);      // Invoke TTS playback :contentReference[oaicite:1]{index=1}
                _hasSpoken = true;                // Prevent repeat triggers
            }
        }
    }

    // Optional: reset when player leaves to allow re-triggering
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            _hasSpoken = false;
        }
    }
}