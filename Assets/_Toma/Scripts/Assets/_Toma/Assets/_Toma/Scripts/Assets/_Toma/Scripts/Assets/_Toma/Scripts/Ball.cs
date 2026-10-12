using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Kaveera Ball")]
    public bool isCatchable = false;
    public float lifeTime = 5f;
    
    Rigidbody rb;
    bool hasHit = false;

    void Awake(){
        rb = GetComponent<Rigidbody>();
        if(rb==null) rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    public void Launch(Vector3 dir, float speed){
        rb.velocity = dir.normalized * speed;
        Invoke(nameof(MakeCatchable), 0.3f);
        Destroy(gameObject, lifeTime);
        // Spin for kaveera ball effect
        rb.angularVelocity = new Vector3(Random.Range(-10,10), Random.Range(-10,10), Random.Range(-10,10));
    }

    void MakeCatchable(){ isCatchable = true; }

    public void Caught(){
        var throwers = FindObjectsOfType<ThrowerAI>();
        if(throwers.Length>0) throwers[Random.Range(0, throwers.Length)].Stun(3f);
        Destroy(gameObject);
    }

    public void Hit(){
        if(hasHit) return;
        hasHit = true;
        Destroy(gameObject, 0.1f);
    }

    void OnCollisionEnter(Collision c){
        if(c.gameObject.CompareTag("Ground")){
            isCatchable = true;
            rb.velocity *= 0.5f;
        }
    }
}
