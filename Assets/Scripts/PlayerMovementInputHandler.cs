using UnityEngine;

public class PlayerMovementInputHandler : MonoBehaviour
{
    public CharacterController2D Controller;
    private bool _jump;
    private bool _crouch;
    private bool _dash;
    public bool _enabled;

    public void Awake(){
        _enabled = false;
        _crouch = false;
    }

    public void SetMovement(bool val)
    {
        _enabled = val;
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
        // if (Input.GetButtonDown("Crouch"))
        // {
        //     _crouch = true;

        // }
        // else if (Input.GetButtonUp("Crouch"))
        // {
        //     _crouch = false;
        // }

        if (Input.GetButtonDown("Dash"))
        {
            _dash = true;
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
        _dash = false;
    }
}
