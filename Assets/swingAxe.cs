using UnityEngine;
using UnityEngine.InputSystem;

public class swingAxe : MonoBehaviour
{
    public Animator ani;
    public string animationName;

    public void playAnimation()
    {
        ani.Play(animationName);
    }
}
