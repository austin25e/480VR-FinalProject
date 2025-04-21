using UnityEngine;

public class NailController : MonoBehaviour
{
    [Header("Tuning")]
    public float hitDepth = 0.1f;
    public float maxDepth = 0.6f;

    Vector3 startPos;
    Vector3 sinkDir;       // ← local‐axis direction to drive the nail
    float   currentDepth;
    bool    isDone;
    GameManager gm;

    void Start()
    {
        startPos     = transform.position;
        currentDepth = 0f;
        isDone       = false;

        // determine the axis the nail should move along.
        // if your nail’s “tip” points out its local +Z, use transform.forward.
        // if it points out local +Y, use transform.up. Here’s an example:
        sinkDir = -transform.up;    // ← nail sinks along its negative local‑up

        gm = GameManager.Instance;
        if (gm == null)
            Debug.LogError("NailController: no GameManager in scene!");
    }

    void OnCollisionEnter(Collision col)
    {
        if (isDone) return;
        if (!col.collider.CompareTag("Hammer")) return;

        currentDepth = Mathf.Min(currentDepth + hitDepth, maxDepth);
        transform.position = startPos + sinkDir * currentDepth;

        if (Mathf.Approximately(currentDepth, maxDepth))
        {
            isDone = true;
            gm?.RegisterHammeredNail();

            // disable further hits
            GetComponent<Collider>().enabled = false;
            this.enabled = false;
        }
    }
}
