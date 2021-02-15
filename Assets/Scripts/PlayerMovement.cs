using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D Controller;
    private float _horizontalMove = 0f;
    public float RunSpeed = 40f;
    public Animator Animator;
    private bool _jump;
    private bool _crouch;
    private bool _dash;

    private void Update()
    {
        // horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        _horizontalMove = RunSpeed;
        Animator.SetFloat("PlayerSpeed", Mathf.Abs(_horizontalMove));
        if (Input.GetButtonDown("Jump")){
            _jump = true;
            Animator.SetBool("PlayerJumping", true);
        }
        if (Input.GetButtonDown("Crouch")){
            _crouch = true;
            
        } else if(Input.GetButtonUp("Crouch")){
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
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Stationary && touch.position.x < Screen.width/2f && Input.touchCount < 2) {
                _jump = true;
                Animator.SetBool("PlayerJumping", true);

            }
        }
    }

    public void OnLanding(){
        Animator.SetBool("PlayerJumping", false);
    }

    public void OnCrouching(bool isCrouching){
        Animator.SetBool("PlayerCrouching", isCrouching);
    }

    public void OnDashing(bool isDashing)
    {
        Animator.SetBool("PlayerDashing", isDashing);
    }

    private void FixedUpdate(){
        Controller.Move(_horizontalMove * Time.fixedDeltaTime, _crouch, _jump, _dash);
        _jump = false;
    }
}
