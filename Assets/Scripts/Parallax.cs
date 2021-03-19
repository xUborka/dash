using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Camera Cam;
    public Transform Subject;
    private float my_start_position;
    private Vector2 camera_start_position;
    private Vector2 Travel => (Vector2)Cam.transform.position - camera_start_position;
    private float DistFromSubject => transform.position.z - Subject.position.z;
    private float ClippingPlane => Cam.transform.position.z + (DistFromSubject > 0 ? Cam.farClipPlane : Cam.nearClipPlane);
    private float ParallaxFactor => Mathf.Abs(DistFromSubject) / ClippingPlane;

    private float renderer_x_bounds;

    private void Start()
    {
        my_start_position = transform.position.x;
        camera_start_position = Cam.transform.position;
        renderer_x_bounds = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float temp = Cam.transform.position.x * (1-ParallaxFactor);
        float movement = Cam.transform.position.x * ParallaxFactor;
        Vector3 next_pos = new Vector3(my_start_position+movement, transform.position.y, transform.position.z);
        if (temp > my_start_position + renderer_x_bounds)
        {
            my_start_position += renderer_x_bounds;
        }
        transform.position = next_pos;
    }
}
