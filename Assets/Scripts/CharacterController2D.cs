using UnityEngine;
using UnityEngine.Events;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CharacterController2D : MonoBehaviour
{
    public ParticleSystem Dust;

    [Header("Components")]
    private Rigidbody2D _rb;

    [Header("Movement")]
    [SerializeField] private float _jumpForce = 20f;                          // Amount of force added when the player jumps.
    [Range(0, 1)] [SerializeField] private float _crouchSpeed = .4f;           // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)] [SerializeField] private float _movementSmoothing = .0f;  // How much to smooth out the movement
    [SerializeField] private LayerMask _whatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform _groundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform _ceilingCheck;                          // A position marking where to check for ceilings
    [SerializeField] private Collider2D _crouchDisableCollider;                // A collider that will be disabled when crouching


    [Header("Jump")]
    [SerializeField] private float _hangTime = 0.5f;
    [SerializeField] private float _jumpBufferLength = 0.1f;

    [Header("Dash")]

    [SerializeField] private float _startDashTime = 0.15f;
    [SerializeField] private float _dashSpeed = 100;
    private Vector2 velocity_before_dash = new Vector2(0.0f, 0.0f);

    private float _dashTime;

    private float _hangTimeCounter;
    private float _jumpBufferCounter;

    private const float KGroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool _grounded;            // Whether or not the player is grounded.
    private const float KCeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up

    private Vector3 _velocity = Vector3.zero;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    public BoolEvent OnDashEvent;
    private bool _wasCrouching = false;
    private bool _wasDashing = false;

    private bool CanJump => _jumpBufferCounter > 0.0f && _hangTimeCounter > 0.0f;

    private void Start()
    {
        _dashTime = _startDashTime;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        OnLandEvent = OnLandEvent ?? new UnityEvent();

        OnCrouchEvent = OnCrouchEvent ?? new BoolEvent();

        OnDashEvent = OnDashEvent ?? new BoolEvent();
    }

    private void Update()
    {
        var wasGrounded = _grounded;
        _grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        var colliders = Physics2D.OverlapCircleAll(_groundCheck.position, KGroundedRadius, _whatIsGround);
        foreach (var t in colliders)
        {
            if (t.gameObject != gameObject)
            {
                _grounded = true;
                if (!wasGrounded)
                {
                    OnLandEvent.Invoke();
                }
            }
        }
    }

    public void Move(float move, bool crouch, bool jump, bool dash)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(_ceilingCheck.position, KCeilingRadius, _whatIsGround))
            {
                crouch = true;
            }
        }

        //only control the player if grounded or airControl is turned on
        if (_grounded)
        {
            _hangTimeCounter = _hangTime;
            // If crouching
            if (crouch)
            {
                if (!_wasCrouching)
                {
                    _wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }

                // Reduce the speed by the crouchSpeed multiplier
                move *= _crouchSpeed;

                // Disable one of the colliders when crouching
                if (_crouchDisableCollider != null)
                    _crouchDisableCollider.enabled = false;
            }
            else
            {
                // Enable the collider when not crouching
                if (_crouchDisableCollider != null)
                    _crouchDisableCollider.enabled = true;

                if (_wasCrouching)
                {
                    _wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, _rb.velocity.y);
            // And then smoothing it out and applying it to the character
            _rb.velocity = Vector3.SmoothDamp(_rb.velocity, targetVelocity, ref _velocity, _movementSmoothing);
        }

        Dash(dash);

        _hangTimeCounter -= Time.fixedDeltaTime;

        if (jump)
        {
            _jumpBufferCounter = _jumpBufferLength;
        }
        else
        {
            _jumpBufferCounter -= Time.fixedDeltaTime;
        }

        // If the player should jump...
        if (CanJump)
        {
            Jump(new Vector2(0f, _jumpForce));
            _hangTimeCounter = _jumpBufferCounter = 0;
        }
    }

    private void Dash(bool dash)
    {
        if (dash)
        {
            if (!_wasDashing)
            {
                velocity_before_dash = _rb.velocity;
                _wasDashing = true;
                OnDashEvent.Invoke(true);
            }

            if (_dashTime <= 0.0f)
            {
                _rb.velocity = new Vector2(velocity_before_dash.x, _rb.velocity.y);
                dash = false;
                OnDashEvent.Invoke(false);
            }
            else
            {
                _dashTime -= Time.fixedDeltaTime;
                _rb.velocity = Vector2.right * _dashSpeed;
            }
        }
        else
        {
            if (_grounded){
                _wasDashing = false;
                _dashTime = _startDashTime;
            }
            if (_wasDashing)
            {
                _rb.velocity = new Vector2(velocity_before_dash.x, _rb.velocity.y);
                OnDashEvent.Invoke(false);
            }
        }
    }

    private void Jump(Vector2 force)
    {
        CreateDust();
        // Add a vertical force to the player.
        _grounded = false;
        _rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void CreateDust()
    {
        Dust.Play();
    }
}
