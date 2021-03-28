using UnityEngine;
using UnityEngine.Events;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class CharacterController2D : MonoBehaviour
{
    public ParticleSystem Dust;

    [Header("Components")]
    private Rigidbody2D _rb;

    [Header("Movement")]
    private PlayerMovementInputHandler playerInputHandler;
    private PlayerMovementAnimator playerMovementAnimator;
    [SerializeField] private float _jumpForce = 15f;                            // Amount of force added when the player jumps.
    [SerializeField] private LayerMask _whatIsGround;                           // A mask determining what is ground to the character
    [SerializeField] private Transform _groundCheck;                            // A position marking where to check if the player is grounded.
    [SerializeField] private float RunSpeed = 50;

    [Header("Jump")]
    [SerializeField] private float _hangTime = 0.5f;
    [SerializeField] private float _jumpBufferLength = 0.25f;
    private float _hangTimeCounter;
    private float _jumpBufferCounter;

    [Header("Dash")]
    [SerializeField] private float _startDashTime = 0.15f;
    [SerializeField] private float _dashSpeed = 50;
    private Vector2 velocity_before_dash = new Vector2(0.0f, 0.0f);
    private float _dashTime;

    private bool _grounded = true;            // Whether or not the player is grounded.
    private const float KGroundedRadius = .2f; // Radius of the overlap circle to determine if grounded

    [Header("Events")]
    public UnityEvent OnLandEvent;
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }
    public BoolEvent OnCrouchEvent;
    public BoolEvent OnDashEvent;

    [Header("Audio")]
    AudioSource audioSource;
    public AudioClip jumpAudio;
    public AudioClip landAudio;
    public AudioClip deathAudio;
    public AudioClip dashAudio;

    [Header("Post-Processing")]
    private Volume post_processing_volume;


    private bool _wasCrouching = false;
    private bool _wasDashing = false;

    private bool CanJump => _jumpBufferCounter > 0.0f && _hangTimeCounter > 0.0f;

    private float move;
    private bool jump;
    private bool crouch;
    private bool dash;

    private void Start()
    {
        _dashTime = _startDashTime;
        move = 0.0f;
        jump = false;
        crouch = false;
        dash = false;
    }

    public void die(){
        audioSource.PlayOneShot(deathAudio);
        playerMovementAnimator.KillPlayer();
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        post_processing_volume = GameObject.Find("GlobalPostProcessing").GetComponent<Volume>();
        audioSource = GetComponent<AudioSource>();
        playerInputHandler = GetComponent<PlayerMovementInputHandler>();
        playerMovementAnimator = GetComponent<PlayerMovementAnimator>();

        OnLandEvent = OnLandEvent ?? new UnityEvent();
        OnCrouchEvent = OnCrouchEvent ?? new BoolEvent();
        OnDashEvent = OnDashEvent ?? new BoolEvent();
    }

    private void Update()
    {
        var wasGrounded = _grounded;
        _grounded = false;

        var colliders = Physics2D.OverlapCircleAll(_groundCheck.position, KGroundedRadius, _whatIsGround);
        foreach (var t in colliders)
        {
            if (t.gameObject != gameObject)
            {
                _grounded = true;
                if (!wasGrounded)
                {
                    OnLandEvent.Invoke();
                audioSource.PlayOneShot(landAudio, 1f);

                }
            }
        }
    }

    public void Move(bool _jump, bool _dash)
    {
        move = RunSpeed;
        jump = jump || _jump;
        dash = dash || _dash;
    }

    public void FixedUpdate(){
        if (playerInputHandler._enabled){
            move = RunSpeed * Time.fixedDeltaTime;
        } else {
            move = 0f;
        }
        print("Move: " + move.ToString());
        playerMovementAnimator.SetPlayerSpeed(move);

        //only control the player if grounded or airControl is turned on
        if (_grounded)
        {
            _hangTimeCounter = _hangTime;
            _rb.velocity = new Vector2(move * 10f, _rb.velocity.y);
        }

        Dash();

        _hangTimeCounter -= Time.fixedDeltaTime;

        if (jump)
        {
            _jumpBufferCounter = _jumpBufferLength;
        }
        else
        {
            _jumpBufferCounter -= Time.fixedDeltaTime;
        }

        if (CanJump)
        {
            audioSource.PlayOneShot(jumpAudio);
            jump = false;
            Jump(new Vector2(0f, _jumpForce));
            _hangTimeCounter = _jumpBufferCounter = 0;
            playerMovementAnimator.OnJumping();
        }
    }

    private void setChromaticAberration(float value){
        ChromaticAberration ca;
        post_processing_volume.profile.TryGet<ChromaticAberration>(out ca);
        ca.intensity.value = value;
    }

    private void Dash()
    {
        if (dash)
        {
            if (!_wasDashing)
            {
                velocity_before_dash = _rb.velocity;
                _wasDashing = true;
                OnDashEvent.Invoke(true);
                audioSource.PlayOneShot(dashAudio, 0.5f);
                setChromaticAberration(1.0f);

            }

            if (_dashTime <= 0.0f)
            {
                _rb.velocity = new Vector2(velocity_before_dash.x, _rb.velocity.y);
                dash = false;
                OnDashEvent.Invoke(false);
                setChromaticAberration(0.0f);
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
        // dash = false;
    }

    private void Jump(Vector2 force)
    {
        CreateDust();
        // Add a vertical force to the player.
        _grounded = false; 
        _rb.velocity = new Vector2(_rb.velocity.x, 0.0f); // Remove current y velocity
        _rb.velocity += Vector2.up * _jumpForce;
    }

    private void CreateDust()
    {
        Dust.Play();
    }
}
