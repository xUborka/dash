using UnityEngine;

public class GravityFlipScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Physics2D.gravity = new Vector3(0f, 9.81f, 0f);
            collider.gameObject.transform.localScale = new Vector3(1, -1, 1);
        }
    }
}
