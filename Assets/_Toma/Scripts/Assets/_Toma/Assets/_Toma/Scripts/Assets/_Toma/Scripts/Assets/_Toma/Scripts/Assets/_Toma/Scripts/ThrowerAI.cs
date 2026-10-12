using UnityEngine;

public class ThrowerAI : MonoBehaviour
{
    [Header("Village Thrower")]
    public GameObject ballPrefab;
    public Transform throwPoint;
    public bool isLeftThrower = true;
    
    float nextThrowTime;
    bool isStunned = false;
    float stunEndTime;
    Transform middlePlayer;
    Animator anim;

    void Start(){
        middlePlayer = GameObject.FindGameObjectWithTag("Player")?.transform;
        anim = GetComponent<Animator>();
        nextThrowTime = Time.time + Random.Range(1f, 3f);
    }

    void Update(){
        if(isStunned){
            if(Time.time > stunEndTime) isStunned = false;
            else return;
        }

        if(GameManager.Instance!=null && GameManager.Instance.isGameOver) return;
        if(Time.time >= nextThrowTime) Throw();
    }

    void Throw(){
        if(middlePlayer==null || ballPrefab==null || throwPoint==null) return;

        GameObject ball = Instantiate(ballPrefab, throwPoint.position, Quaternion.identity);
        
        // Ugandan straight throw - aim directly at middle player
        Vector3 target = middlePlayer.position + Vector3.up * 0.5f;
        // Add small prediction for moving player
        Rigidbody playerRb = middlePlayer.GetComponent<Rigidbody>();
        if(playerRb!=null) target += playerRb.velocity * 0.3f;

        Vector3 dir = (target - throwPoint.position).normalized;
        
        // Small random spread for difficulty
        dir += new Vector3(Random.Range(-0.1f,0.1f), Random.Range(-0.1f,0.1f), 0);
        
        float speed = GameManager.Instance!=null ? GameManager.Instance.GetThrowSpeed() : 8f;

        if(ball.TryGetComponent<Ball>(out Ball b)) b.Launch(dir, speed);
        else if(ball.TryGetComponent<Rigidbody>(out Rigidbody rb)) rb.velocity = dir * speed;

        if(anim) anim.SetTrigger("Throw");
        if(AudioManager.Instance) AudioManager.Instance.Play("toma");

        nextThrowTime = Time.time + Random.Range(1.5f, 3.5f);
    }

    public void Stun(float duration){
        isStunned = true;
        stunEndTime = Time.time + duration;
        if(anim) anim.SetTrigger("Stunned");
    }
}
