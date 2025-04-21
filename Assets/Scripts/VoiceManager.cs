using UnityEngine;
using TMPro;
using Meta.WitAi.Requests;
using Oculus.Voice;
using Meta.Voice;
using UnityEngine.SceneManagement;

public class ChatManager : MonoBehaviour
{
    [SerializeField] private AppVoiceExperience voiceSDK;
    [SerializeField] private TextMeshPro chatLabel;

    void Awake()
    {
        if (voiceSDK == null) voiceSDK = FindFirstObjectByType<AppVoiceExperience>();
        if (chatLabel == null)
        {
            Debug.LogError("Chat label is not assigned in the inspector.");
        }
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
        // Kick off the conversation
        voiceSDK.Activate();
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

        int activeSceneNum = SceneManager.GetActiveScene().buildIndex;

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
                            reply = "You are currently in the time machine room!";
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
                            reply = "Your objective is to find the 3 dinsaur eggs hidden in this time period and bring them back to the shelves. You can find them by each dinosaur!";
                            break;

                        case "location":
                            reply = "You are currently in the Cretaceous period, near the T-Rex!";
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
                            reply = "Your objective is to steer this vessel to the new world! Make sure you watch out for any trouble along the way!";
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

        // Immediately listen again for a natural back‑and‑forth
        voiceSDK.Activate();
    }

}
