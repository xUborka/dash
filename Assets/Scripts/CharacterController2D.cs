using UnityEngine;
using UnityEngine.Events;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CharacterController2D : MonoBehaviour
{
    public ParticleSystem Dust;
    [Header("Movement")]
    [SerializeField] private float _mJumpForce = 400f;                          // Amount of force added when the player jumps.
    [Range(0, 1)] [SerializeField] private float _mCrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)] [SerializeField] private float _mMovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask _mWhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform _mGroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform _mCeilingCheck;                          // A position marking where to check for ceilings
    [SerializeField] private Collider2D _mCrouchDisableCollider;                // A collider that will be disabled when crouching

    private const float KGroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool _mGrounded;            // Whether or not the player is grounded.
    private const float KCeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    private Rigidbody2D _mRigidbody2D;
    private bool _mFacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 _mVelocity = Vector3.zero;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    public BoolEvent OnDashEvent;
    private bool _mWasCrouching;
    private bool _mWasDashing;

    private float _tempY = 1f;

    private void Awake()
    {
        _mRigidbody2D = GetComponent<Rigidbody2D>();

        OnLandEvent = OnLandEvent ?? new UnityEvent();

        OnCrouchEvent = OnCrouchEvent ?? new BoolEvent();

        OnDashEvent = OnDashEvent ?? new BoolEvent();
    }

    private void FixedUpdate()
    {
        var wasGrounded = _mGrounded;
        _mGrounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_mGroundCheck.position, KGroundedRadius, _mWhatIsGround);
        foreach (var t in colliders)
        {
            if (t.gameObject != gameObject)
            {
                _mGrounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }

    public void Move(float move, bool crouch, bool jump, bool dash)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(_mCeilingCheck.position, KCeilingRadius, _mWhatIsGround))
            {
                crouch = true;
            }
        }

        //only control the player if grounded or airControl is turned on
        if (_mGrounded || m_AirControl)
        {

            // If crouching
            if (crouch)
            {
                if (!_mWasCrouching)
                {
                    _mWasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }

                // Reduce the speed by the crouchSpeed multiplier
                move *= _mCrouchSpeed;

                // Disable one of the colliders when crouching
                if (_mCrouchDisableCollider != null)
                    _mCrouchDisableCollider.enabled = false;
            }
            else
            {
                // Enable the collider when not crouching
                if (_mCrouchDisableCollider != null)
                    _mCrouchDisableCollider.enabled = true;

                if (_mWasCrouching)
                {
                    _mWasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            if (dash)
            {
                if (!_mWasDashing)
                {
                    _mWasDashing = true;
                    OnDashEvent.Invoke(true);
                }
                move *= 2.0f;
                _tempY = 1.2f;

            }
            else
            {
                if (_mWasDashing)
                {
                    _mWasDashing = false;
                    OnDashEvent.Invoke(false);
                }
            }

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, _tempY * _mRigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            _mRigidbody2D.velocity = Vector3.SmoothDamp(_mRigidbody2D.velocity, targetVelocity, ref _mVelocity, _mMovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !_mFacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && _mFacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }

        // If the player should jump...
        if (_mGrounded && jump)
        {
            Jump(new Vector2(0f, _mJumpForce));
        }
    }

    private void Jump(Vector2 force)
    {
        Dust.Play();
        // Add a vertical force to the player.
        _mGrounded = false;
        _mRigidbody2D.AddForce(force);
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        _mFacingRight = !_mFacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void CreateDust()
    {
        Dust.Play();
    }
}