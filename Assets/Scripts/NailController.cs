using UnityEngine;

public class NailController : MonoBehaviour
{
    public float hitDepth = 0.1f;
    public float maxDepth = 0.6f;

    Vector3 startPos;
    Vector3 sinkDir;      
    float   currentDepth;
    bool    isDone;
    GameManager gm;

    void Start()
    {
        startPos     = transform.position;
        currentDepth = 0f;
        isDone       = false;

        sinkDir = -transform.up;    

        gm = GameManager.Instance;
       
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
