using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Ugandan Middle Player")]
    public float moveSpeed = 6f;
    public float leftLimit = -2f;
    public float rightLimit = 2f;
    public float jumpForce = 7f;
    
    [Header("Actions")]
    public bool isJumping = false;
    public bool isDucking = false;
    public bool isCatching = false;
    
    Rigidbody rb;
    Animator anim;
    CapsuleCollider col;
    float originalColHeight;
    Vector3 startPos;

    void Start(){
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
        startPos = transform.position;
        if(col) originalColHeight = col.height;
    }

    void Update(){
        HandleMovement();
        
        // Check ground
        if(Physics.Raycast(transform.position, Vector3.down, 1.1f))
            isJumping = false;
    }

    void HandleMovement(){
        float h = 0f;
        // Mobile joystick + keyboard
        h = Input.GetAxis("Horizontal");
        if(Application.isMobilePlatform) h = MobileJoystick.Horizontal;

        Vector3 pos = transform.position;
        pos.x += h * moveSpeed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, leftLimit, rightLimit);
        transform.position = pos;

        if(anim){
            anim.SetFloat("Speed", Mathf.Abs(h));
            if(h!=0) transform.localScale = new Vector3(Mathf.Sign(h)*Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    public void DoJump(){
        if(isJumping || isDucking) return;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isJumping = true;
        if(anim) anim.SetTrigger("Jump");
    }

    public void DoDuck(){
        if(isJumping) return;
        if(isDucking) { StandUp(); return; }
        isDucking = true;
        if(col){ col.height = originalColHeight * 0.5f; col.center = new Vector3(col.center.x, col.center.y*0.5f, col.center.z); }
        if(anim) anim.SetBool("Duck", true);
        Invoke(nameof(StandUp), 0.8f);
    }

    void StandUp(){
        isDucking = false;
        if(col){ col.height = originalColHeight; col.center = new Vector3(col.center.x, originalColHeight/2f, col.center.z); }
        if(anim) anim.SetBool("Duck", false);
    }

    public void DoCatch(){
        if(isJumping || isDucking) return;
        isCatching = true;
        if(anim) anim.SetTrigger("Catch");
        Invoke(nameof(EndCatch), 0.5f);
    }

    void EndCatch(){ isCatching = false; }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Ball")){
            Ball ball = other.GetComponent<Ball>();
            if(ball==null) return;

            if(isCatching && ball.isCatchable){
                if(AudioManager.Instance) AudioManager.Instance.Play("catch");
                if(AudioManager.Instance) AudioManager.Instance.Play("cheer");
                if(GameManager.Instance) GameManager.Instance.PlayerCaught();
                ball.Caught();
            } else if(!isDucking){
                if(AudioManager.Instance) AudioManager.Instance.Play("hit");
                if(GameManager.Instance) GameManager.Instance.PlayerHit();
                ball.Hit();
            } else {
                if(GameManager.Instance) GameManager.Instance.PlayerDodged();
                ball.Hit();
            }
        }
    }
}

// Simple mobile joystick helper
public static class MobileJoystick{
    public static float Horizontal = 0f;
}
