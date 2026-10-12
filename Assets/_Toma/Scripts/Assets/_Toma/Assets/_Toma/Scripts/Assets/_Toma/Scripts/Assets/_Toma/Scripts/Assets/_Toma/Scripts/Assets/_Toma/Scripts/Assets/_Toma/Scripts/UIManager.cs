using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Buttons - Assign in Inspector")]
    public Joystick joystick; // Use Unity Joystick pack or UI joystick
    public Button jumpButton;
    public Button duckButton;
    public Button catchButton;

    PlayerController player;

    void Start(){
        player = FindObjectOfType<PlayerController>();
        
        if(jumpButton) jumpButton.onClick.AddListener(()=>{ if(player) player.DoJump(); });
        if(duckButton) duckButton.onClick.AddListener(()=>{ if(player) player.DoDuck(); });
        if(catchButton) catchButton.onClick.AddListener(()=>{ if(player) player.DoCatch(); });
    }

    void Update(){
        // Update joystick helper for PlayerController
        if(joystick!=null){
            MobileJoystick.Horizontal = joystick.Horizontal;
        } else {
            // Fallback keyboard for Unity Editor
            if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) MobileJoystick.Horizontal = -1;
            else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) MobileJoystick.Horizontal = 1;
            else MobileJoystick.Horizontal = 0;
        }

        // Keyboard shortcuts for testing
        if(Input.GetKeyDown(KeyCode.Space) && player) player.DoJump();
        if(Input.GetKeyDown(KeyCode.S) && player) player.DoDuck();
        if(Input.GetKeyDown(KeyCode.F) && player) player.DoCatch();
    }
}
