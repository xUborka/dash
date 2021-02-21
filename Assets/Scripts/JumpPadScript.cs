using UnityEngine;

public class JumpPadScript : MonoBehaviour
{
    [Header("Base")]
    [SerializeField] private float _force = 900f;

    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _animator.SetTrigger("Launch");
            collision.gameObject.GetComponent<Rigidbody2D>
                ().AddForce(new Vector2(0f, _force));
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        //if (collision.gameObject.CompareTag("Player"))
        //{
        //    _animator.ResetTrigger("Launch");
        //}
    }
}
