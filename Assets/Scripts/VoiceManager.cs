using UnityEngine;
using TMPro;
using Meta.WitAi.Requests;
using Oculus.Voice;
using Meta.Voice;
using UnityEngine.SceneManagement;
using Meta.WitAi.TTS.Utilities;

public class ChatManager : MonoBehaviour
{
    [SerializeField] private AppVoiceExperience voiceSDK;
    [SerializeField] private TextMeshPro chatLabel;
    [SerializeField] private TTSSpeaker ttsSpeaker;
    private int activeSceneNum;

    void Awake()
    {
        activeSceneNum = SceneManager.GetActiveScene().buildIndex;
        if (voiceSDK == null) voiceSDK = FindFirstObjectByType<AppVoiceExperience>();
        if (chatLabel == null)
        {
            Debug.LogError("Chat label is not assigned in the inspector.");
        }
        if (ttsSpeaker == null)
            ttsSpeaker = FindFirstObjectByType<TTSSpeaker>();

        if (ttsSpeaker == null)
            Debug.LogError("TTSSpeaker component not found in scene. Please add TTSSpeaker.prefab.");
    }

    void OnEnable()
    {
        voiceSDK.VoiceEvents.OnFullTranscription.AddListener(HandleUserSpoke);
        voiceSDK.VoiceEvents.OnComplete.AddListener(HandleBotResponded);
    }

    void OnDisable()
    {
        voiceSDK.VoiceEvents.OnFullTranscription.RemoveListener(HandleUserSpoke);
        voiceSDK.VoiceEvents.OnComplete.RemoveListener(HandleBotResponded);
    }

    void Start()
    {
        string introText = " ";

        switch (activeSceneNum)
        {
            case 0:
                introText = "Hello there traveler! I am your tour guide. You can interact with me with your voice! Try saying 'Hello'";
                Debug.Log("Scene 0 loaded: Announcer will spawn here.");
                break;
            case 1:
                introText = "Welcome to the Cretaceous Period! Your objective here is to collect 3 dinosaur eggs and return them to the shelves. I'll be following right behind you if you have any questions!";
                Debug.Log("Scene 1 loaded: Announcer will spawn here.");
                break;
            case 2:
                introText = "Welcome to Mesopotamia, head over to the bridge to start your task! Don't forget I'll be following right behind you if you have any questions!";
                Debug.Log("Scene 2 loaded: Announcer will spawn here.");
                break;
            case 3:
                introText = "Welcome to the Age of Exploration! Your objective is to repair the boat and then sail us to the new world! As always ill be behind you if you need me!";
                Debug.Log("Scene 3 loaded: Announcer will spawn here.");
                break;
            case 4:
                introText = "";
                Debug.Log("Scene 4 loaded: Announcer will spawn here.");
                break;
            default:
                introText = "There seems to be a problem with this timeline... I don't know where we are!";
                Debug.Log("Unknown scene loaded: No specific action for announcer.");
                break;
        }

        chatLabel.text = $"{introText}";

        if (ttsSpeaker != null && !string.IsNullOrEmpty(introText))
        {
            ttsSpeaker.Speak(introText);
            ttsSpeaker.Events.OnPlaybackStart.AddListener((speaker, clipData) =>
            {
                // Deactivate the voice SDK while TTS is playing
                voiceSDK.Deactivate();
            });

            ttsSpeaker.Events.OnPlaybackComplete.AddListener((speaker, clipData) =>
            {
                // Reactivate the voice SDK after TTS has finished playing
                voiceSDK.Activate();
            });
        }
    }

    // 1️⃣ User spoke → show it, then send to Wit for intent
    private void HandleUserSpoke(string userText)
    {
        //chatLabel.text = $"You: {userText}";
        voiceSDK.Activate(userText);
    }

    


    // 2️⃣ Wit.ai responded → map to your custom reply, then display & listen again
    private void HandleBotResponded(VoiceServiceRequest req) //will be implmenting a tracking system to advance the diagloue as the player moves through the level.
    {
        if (req.State != VoiceRequestState.Successful) return;

        // Grab the intents array
        var results = req.Results.ResponseData;
        var intents = results["intents"];
        string reply = " ";

        if (intents.Count > 0)
        {
            // Pull the top intent’s name
            string topIntent = intents[0]["name"].Value.ToLowerInvariant();

            switch (activeSceneNum)
            {
                case 0:
                    switch (topIntent)
                    {
                        case "greeting":
                            reply = "Hello there, I am your tour guide!";
                            break;

                        case "objective":
                            reply = "Walk up and select the first time period and then pull the lever";
                            break;

                        case "location":
                            reply = "You are currently in the time machine!";
                            break;

                        case "farewell":
                            reply = "Safe travels!";
                            break;

                        default:
                            reply = "Sorry, I didn't catch that.";
                            break;
                    }
                    break;
                case 1:
                    switch (topIntent)
                    {
                        case "greeting":
                            reply = "Hello there, I am still here!";
                            break;

                        case "objective":
                            reply = "Your objective is to find the 3 dinosaur eggs hidden in this time period and bring them back to the shelves. You can find them by each dinosaur!";
                            break;

                        case "location":
                            reply = "You are currently in the Cretaceous period!";
                            break;

                        case "farewell":
                            reply = "Leaving so soon? We've only just begun!";
                            break;

                        default:
                            reply = "Sorry, I didn't catch that.";
                            break;
                    }
                    break;
                case 2:
                    switch (topIntent)
                    {
                        case "greeting":
                            reply = "Hello again!";
                            break;

                        case "objective":
                            reply = "Your objective is to go to the bridge and follow the instructions over there. You can find the bridge crossing the river!";
                            break;

                        case "location":
                            reply = "You are currently in Mesopotamia, near the Tigris and Euphrates rivers!";
                            break;

                        case "farewell":
                            reply = "You can't leave yet! You haven't finished the tour!";
                            break;

                        default:
                            reply = "Sorry, I didn't catch that.";
                            break;
                    }
                    break;
                case 3:
                    switch (topIntent)
                    {
                        case "greeting":
                            reply = "Hello young sailor!";
                            break;

                        case "objective":
                            reply = "Your objective is to repair the boat and then sail us to the new world!";
                            break;

                        case "location":
                            reply = "You are currently in the Atlantic Ocean, sailing towards the new world!";
                            break;

                        case "farewell":
                            reply = "Mutiny! You'll have to walk the plank if you leave now!";
                            break;

                        default:
                            reply = "Sorry, I didn't catch that.";
                            break;
                    }
                    break;
                default:
                    reply = "Sorry, there seems to be a problem with this timeline...";
                    break;
            }

        }
        else
        {
            // No intent was confident enough
            //reply = "I'm not sure what you meant. Could you rephrase?";
            //sometimes triggers with just noise so left it blank as the defaults work fine
        }

        // Display your custom bot text
        chatLabel.text = $"{reply}";

        if (ttsSpeaker != null && !string.IsNullOrEmpty(reply))
        {
            ttsSpeaker.Speak(reply);
            ttsSpeaker.Events.OnPlaybackStart.AddListener((speaker, clipData) =>
            {
                // Deactivate the voice SDK while TTS is playing
                voiceSDK.Deactivate();
            });

            ttsSpeaker.Events.OnPlaybackComplete.AddListener((speaker, clipData) =>
            {
                // Reactivate the voice SDK after TTS has finished playing
                voiceSDK.Activate();
            });
        }
    }

}
