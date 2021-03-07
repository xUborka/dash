using UnityEngine;

namespace Assets.Scripts
{
    public class BetterJumping : MonoBehaviour
    {
        private Rigidbody2D rb;
        public float fallMultiplier = 2.5f;
        public float lowJumpMultiplier = 2f;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        bool TouchJump()
        {
            if (Input.touchCount > 0)
            {
                // Touch touch = Input.GetTouch(0);
                foreach (Touch touch in Input.touches)
                {
                    if (touch.position.x < Screen.width / 2f)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        void FixedUpdate()
        {
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
            }
            else if (rb.velocity.y > 0 && (!Input.GetButton("Jump") && !TouchJump()))
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
            }
        }
    }
}
