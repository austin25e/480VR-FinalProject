using UnityEngine;

public class AreaBoxScript : MonoBehaviour
{
    public Transform vrPlayer;  // Assign your XR Origin (not the camera)

    // Update is called once per frame
    void Update()
    {
        transform.position = vrPlayer.position;
    }
}
