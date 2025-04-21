using UnityEngine;
using UnityEngine.UI;  // Required for Text component
using TMPro; // Required for TextMeshPro component

public class TextDisplay : MonoBehaviour
{
    public float displayTime; // Time in seconds to display the text
    public TextMeshPro canvasText;
   void Start ()
   {
      if (canvasText == null)
      {
         Debug.LogError("canvasText1 is not assigned in the inspector.");
         return;
      }

      if (displayTime <= 0)
      {
         Debug.LogError("displayTime must be greater than 0");
         return;
      }

      if (!canvasText.enabled)
      {
         canvasText.enabled = true; // Enable the text if it's not already enabled
         Invoke("DisableText", displayTime); //invoke after n seconds
      } else {
        Invoke("DisableText", displayTime);//invoke after n seconds
      }
      
   }
   void DisableText()
   {
      canvasText.enabled = false;
   } 
}