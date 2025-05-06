using UnityEngine;
using TMPro;
using Oculus.Voice;
using Meta.Voice;
using Meta.WitAi.TTS.Utilities;
using Meta.WitAi.Requests;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[RequireComponent(typeof(AppVoiceExperience), typeof(TTSSpeaker))]
public class ChatManager : MonoBehaviour
{
    public static ChatManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private AppVoiceExperience voiceSDK;
    [SerializeField] private TextMeshPro chatLabel;
    [SerializeField] private TTSSpeaker ttsSpeaker;

    // Hardcoded dialogue objectives per scene
    private Dictionary<int, List<string>> _dialogueLookup;
    private int _activeSceneNum;
    private int _currentObjective;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Auto-find references
        if (voiceSDK == null) voiceSDK = FindFirstObjectByType<AppVoiceExperience>();
        if (chatLabel == null) Debug.LogError("Chat label not assigned");
        if (ttsSpeaker == null) ttsSpeaker = FindFirstObjectByType<TTSSpeaker>();

        // Locate the TTSSpeaker instance in the scene
        if (ttsSpeaker == null)
            ttsSpeaker = FindFirstObjectByType<TTSSpeaker>();

        // Make sure it has an AudioSource
        if (ttsSpeaker != null && ttsSpeaker.GetComponent<AudioSource>() == null)
        {
            Debug.LogWarning("AudioSource missing on TTSSpeaker—adding one at runtime.");
            var src = ttsSpeaker.gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
        }
        // Determine active scene index
        _activeSceneNum = SceneManager.GetActiveScene().buildIndex;

        // Initialize hardcoded dialogue objectives and corresponding replies
        _dialogueLookup = new Dictionary<int, List<string>>
        {
            { 0, new List<string>
                {
                    // Starting ship objectives
                    "Hello there traveler! I am your tour guide. You can interact with me with your voice! You can move around using the left joystick and look around by moving your head.",
                    "Walk up and press on the first time period and then pull the lever."
                }
            },
            { 1, new List<string>
                {
                    // Cretaceous objectives
                    "Welcome to the Cretaceous Period! Your objective is to find the 3 dinosaur eggs hidden in this time period and bring them back to the shelves",
                    "Find the 3 dinosaur eggs hidden in this time period and bring them back to the shelves",
                    "2 eggs left to find!", "Find the eggs and bring them back to the shelves",
                    "1 egg left to find!", "Find the eggs and bring them back to the shelves",
                    "Congratulations! You have found all of the dinosaur eggs! Now head back to the time machine and select the next time period.",
                }
            },
            { 2, new List<string>
                {
                    // Babylon objectives
                    "Welcome to Mesopotamia! Your objective is on the other side of the bridge or you can explore the area and find pedestals to learn more about this time period.",
                    "Your Objective is to find a stone and a stick and place them on the stump. The items will be somewhere on the ground to your left!",
                    "Find a stone and a stick near the trees and place them on the stump.",
                    "Find another stone and hit it against the objects on the stump",
                    "Take the axe that you made and use it to cut the wheat and then place it in the pot! You'll need to collect 3 pieces to advance.",
                    "Congratulations, you have completed your task! To continue, head back to the ship."
                }
            },
            { 3, new List<string>
                {
                    // Boat objectives
                    "Welcome to the Age of Exploration! Your objective is to repair the boat and then sail us to the new world. There are 6 nails to hammer in. You can find the hammer on the ground near you and the nails sticking out of the boat.",
                    "5 nails left to hammer in!", "4 nails left to hammer in!", "3 nails left to hammer in!", "2 nails left to hammer in!",
                    "1 nail left to hammer in!", "Congratulations! You have repaired the boat! Now head to the wheel and steer us to the new world.",
                    "To steer the boat, grab the wheel and turn it left or right.",
                    "Congratulations! You landed! To continue, head back to the time machine and select the next time period."
                }
            },
            { 4, new List<string>
                {
                    // Industrial Revolution objectives
                    "Welcome to the Industrial Revolution! Walk into the factory to begin! You can also walk to the pedistal to learn more about the first major part of the industrial revolution.",
                    "Your objective is to place the ore on the right conveyor belt and coal on the left conveyor belt.",
                    "The door is now open! Proceed through the door to the next room to continue.",
                    "Your next objective is to grab and place the 4 gears in the correct slots on the machine. The gears are located in front of you. To learn about the second major part of the industrial revolution, walk to the pedistal.",
                    "Your objective is to place the 4 gears on the machine.", "Congratulations! You have fixed the machine! Now return to the time machine and continue to the modern era."
                }
            },
            { 5, new List<string>
                {
                    // Modern day objectives
                    "Welcome to the Modern Era! There are no objectives in this time period. Feel free to walk around and explore! If at any point you want to return to a previous time period, just walk to the time machine and select the time period you want to go to.",
                    "There are no objectives for this time period. If at any point you want to return to a previous time period, just walk to the time machine and select the time period you want to go to."
                }
            },

        };
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        voiceSDK.VoiceEvents.OnFullTranscription.AddListener(HandleUserSpoke);
        voiceSDK.VoiceEvents.OnComplete.AddListener(HandleBotResponse);
        
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        voiceSDK.VoiceEvents.OnFullTranscription.RemoveListener(HandleUserSpoke);
        voiceSDK.VoiceEvents.OnComplete.RemoveListener(HandleBotResponse);
        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _activeSceneNum = scene.buildIndex;
        _currentObjective = 0;

        // Re-find per-scene objects if needed:
        voiceSDK = FindFirstObjectByType<AppVoiceExperience>();
        ttsSpeaker = FindFirstObjectByType<TTSSpeaker>();

        if (ttsSpeaker == null)
        {
            Debug.LogWarning("TTSSpeaker not found in the new scene!");
            return;
        }

        // Ensure TTSSpeaker has an AudioSource
        if (ttsSpeaker.GetComponent<AudioSource>() == null)
        {
            Debug.LogWarning("AudioSource missing on TTSSpeaker—adding one at runtime.");
            var src = ttsSpeaker.gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
        }

        // Speak the intro or first objective for the new scene:
        Debug.Log($"Scene {scene.name} loaded. Current objective: {_currentObjective}");
        if (_activeSceneNum != 0)
        {
            SpeakObjective();
        }
        
    }

    void Start()
    {
        // Speak the initial objective
        //_currentObjective = 0;
        //_activeSceneNum = SceneManager.GetActiveScene().buildIndex;
        SpeakObjective();
    }

    void Update()
    {
        if (!voiceSDK.Active && !ttsSpeaker.IsSpeaking)
        {
            voiceSDK.Activate();
        }
    }
    //Speak current objective for this scene.
    private void SpeakObjective()
    {
        if (!_dialogueLookup.TryGetValue(_activeSceneNum, out var list) || list.Count == 0) return;
        _currentObjective = Mathf.Clamp(_currentObjective, 0, list.Count - 1);
        string text = list[_currentObjective];
        chatLabel.text = text;

        ttsSpeaker.Speak(text);
        ttsSpeaker.Events.OnPlaybackStart.AddListener((s, c) => voiceSDK.Deactivate());
        ttsSpeaker.Events.OnPlaybackComplete.AddListener((s, c) => voiceSDK.Activate());
        ttsSpeaker.Events.OnPlaybackComplete.AddListener((s, c) => chatLabel.text = " ");
        
    }

    private void HandleUserSpoke(string userText)
    {
        voiceSDK.Activate(userText);
    }

    private void HandleBotResponse(VoiceServiceRequest req)
    {
        if (req.State != VoiceRequestState.Successful) return;

        var intents = req.Results.ResponseData["intents"];
        if (intents.Count == 0)
        {
            // Fallback reply
            return;
        }

        string intent = intents[0]["name"].Value.ToLowerInvariant();

        // If user asks for current objective, repeat it
        if (intent == "objective")
        {
            SpeakObjective();
            return;
        }

        // Static intent responses per scene
        string reply = string.Empty;
        switch (_activeSceneNum)
        {
            case 0:
                switch (intent)
                {
                    case "greeting": reply = "Hello there, I am your tour guide!"; break;
                    case "location": reply = "You are currently in the time machine!"; break;
                    case "farewell": reply = "Safe travels!"; break;
                }
                break;
            case 1:
                switch (intent)
                {
                    case "greeting": reply = "Hello there, I am still here!"; break;
                    case "location": reply = "You are currently in the Cretaceous period!"; break;
                    case "farewell": reply = "Leaving so soon? We've only just begun!"; break;
                        //case "confirm": /* Figuring out still? */ break;
                        //case "denial": reply = "That's okay! You can always come back later!"; break;
                }
                break;
            case 2:
                switch (intent)
                {
                    case "greeting": reply = "Hello again!"; break;
                    case "location": reply = "You are currently in Mesopotamia, near the Tigris and Euphrates rivers!"; break;
                    case "farewell": reply = "You can't leave yet! You haven't finished the tour!"; break;
                }
                break;
            case 3:
                switch (intent)
                {
                    case "greeting": reply = "Hello young sailor!"; break;
                    case "location": reply = "You are currently in the Atlantic Ocean, sailing towards the new world!"; break;
                    case "farewell": reply = "Mutiny! You'll have to walk the plank if you leave now!"; break;
                }
                break;
            case 4:
                switch (intent)
                {
                    case "greeting": reply = "Hello there, I am still here!"; break;
                    case "location": reply = "You are currently in the Industrial Revolution!"; break;
                    case "farewell": reply = "You can't escape the factory! Mu ha ha ha!"; break;
                }
                break;
            default:
                reply = "Sorry, there seems to be a problem with this timeline..."; break;
        }

        if (string.IsNullOrEmpty(reply)) return;
        chatLabel.text = reply;

        ttsSpeaker.Speak(reply);
        ttsSpeaker.Events.OnPlaybackStart.AddListener((s, c) => voiceSDK.Deactivate());
        ttsSpeaker.Events.OnPlaybackComplete.AddListener((s, c) => voiceSDK.Activate());
        ttsSpeaker.Events.OnPlaybackComplete.AddListener((s, c) => chatLabel.text = " ");
    }

    //Advance the objective counter and announce next.
    public void AdvanceObjective()
    {
        _currentObjective++;
        SpeakObjective();
    }

    //Advance the objective counter without announcing next.
    public void NoSpeakAdvanceObjective()
    {
        _currentObjective++;
    }
}
