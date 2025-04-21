using UnityEngine;

public class NailController : MonoBehaviour
{
    public float hitDepth = 0.1f;    // how far the nail sinks per hit
    public float maxDepth = 0.6f;    // depth at which the nail is "done"
    private float currentDepth = 0f;
    private bool isDone = false;
    private GameManager gm;

    void Start()
    {
        gm = GameManager.Instance;
        if (gm == null)
            Debug.LogError("NailController: GameManager.Instance is null! Did you add the GameManager in your scene?");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isDone) return;
        if (!collision.collider.CompareTag("Hammer")) return;

        // drive the nail down
        float step = Mathf.Min(hitDepth, maxDepth - currentDepth);
        transform.position += Vector3.down * step;
        currentDepth += step;

        // once fully in, register with the manager
        if (currentDepth >= maxDepth)
        {
            isDone = true;
            gm?.RegisterHammeredNail();
        }
    }
}
