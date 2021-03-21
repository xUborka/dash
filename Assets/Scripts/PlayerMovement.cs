using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D Controller;
    public Animator Animator;
    private bool _jump;
    private bool _crouch;
    private bool _dash;
    public bool _enabled;

    public void Awake(){
        _enabled = false;
    }

    public void SetMovement(bool val)
    {
        _enabled = val;
    }

    public void KillPlayer(){
        Animator.SetBool("PlayerJumping", false);
        Animator.SetBool("PlayerDashing", false);
        Animator.SetBool("PlayerDied", true);
    }

    private void Update()
    {
        if (!_enabled)
        {
            return;
        }
        if (Input.GetButtonDown("Jump"))
        {
            _jump = true;
        }
        if (Input.GetButtonDown("Crouch"))
        {
            _crouch = true;

        }
        else if (Input.GetButtonUp("Crouch"))
        {
            _crouch = false;
        }

        if (Input.GetButtonDown("Dash"))
        {
            _dash = true;
        }
        else if (Input.GetButtonUp("Dash"))
        {
            _dash = false;
        }

        // Touch
        if (Input.touchCount > 0)
        {
            // Touch touch = Input.GetTouch(0);
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began && touch.position.x < Screen.width / 2f)
                {
                    _jump = true;
                }
                if (touch.phase == TouchPhase.Began && touch.position.x > Screen.width / 2f)
                {
                    _dash = true;
                }
            }
        }

        Controller.Move(_crouch, _jump, _dash);
        _jump = false;
    }

    public void SetPlayerMovementAnimation(float velocity){
        Animator.SetFloat("PlayerSpeed", Mathf.Abs(velocity));
    }

    public void OnJumping(){
        Animator.SetBool("PlayerJumping", true);
    }

    public void OnLanding()
    {
        Animator.SetBool("PlayerJumping", false);
    }

    public void OnCrouching(bool isCrouching)
    {
        Animator.SetBool("PlayerCrouching", isCrouching);
    }

    public void OnDashing(bool isDashing)
    {
        _dash = isDashing;
        Animator.SetBool("PlayerDashing", isDashing);
    }
}
