using UnityEngine;

public class PlayerMovementAnimator : MonoBehaviour
{
    private Animator Animator;
    private bool _jump;
    private bool _crouch;
    private bool _dash;

    public void Awake(){
        Animator = GetComponent<Animator>();
    }

    public void KillPlayer(){
        Animator.SetBool("PlayerJumping", false);
        Animator.SetBool("PlayerDashing", false);
        Animator.SetBool("PlayerDied", true);
    }

    public void SetPlayerSpeed(float velocity){
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
