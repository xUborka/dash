using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D Controller;
    float horizontalMove = 0f;
    public float runSpeed = 40f;
    public Animator animator;
    bool jump = false;
    bool crouch = false;

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("PlayerSpeed", Mathf.Abs(horizontalMove));
        if (Input.GetButtonDown("Jump")){
            jump = true;
            animator.SetBool("PlayerJumping", true);
        }
        if (Input.GetButtonDown("Crouch")){
            crouch = true;
            
        } else if(Input.GetButtonUp("Crouch")){
            crouch = false;
        }

        // Touch
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Stationary && touch.position.x < Screen.width/2 && Input.touchCount < 2) {
                jump = true;
                animator.SetBool("PlayerJumping", true);

            }
        }
    }

    public void OnLanding(){
        animator.SetBool("PlayerJumping", false);
    }

    public void OnCrouching(bool isCrouching){
        animator.SetBool("PlayerCrouching", isCrouching);
    }

    void FixedUpdate(){
        Controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
}
