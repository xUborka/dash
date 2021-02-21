using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D Controller;
    private float _horizontalMove = 0f;
    public float RunSpeed = 50;
    public Animator Animator;
    private bool _jump;
    private bool _crouch;
    private bool _dash;
    private bool _enabled = false;

    public void SetMovement(bool val)
    {
        _enabled = val;
    }

    public void KillPlayer(){
        Animator.SetBool("PlayerJumping", false);
        Animator.SetBool("PlayerDashing", false);
        Animator.SetBool("PlayerDied", true);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 10.0f);
    }

    private void Update()
    {
        if (!_enabled)
        {
            return;
        }
        _horizontalMove = RunSpeed;
        Animator.SetFloat("PlayerSpeed", Mathf.Abs(_horizontalMove));
        if (Input.GetButtonDown("Jump"))
        {
            _jump = true;
            Animator.SetBool("PlayerJumping", true);
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
                    print("Jump Pressed!");
                    _jump = true;
                    Animator.SetBool("PlayerJumping", true);
                }
                if (touch.phase == TouchPhase.Began && touch.position.x > Screen.width / 2f)
                {
                    _dash = true;
                }
            }
        }

        Controller.Move(_horizontalMove * Time.fixedDeltaTime, _crouch, _jump, _dash);
        _jump = false;
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
