using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Camera Cam;
    public Transform Subject;
    private Vector2 _startPosition;
    private float _startZ;
    private Vector2 Travel => (Vector2)Cam.transform.position - _startPosition;
    private float DistFromSubject => transform.position.z - Subject.position.z;
    private float ClippingPlane => Cam.transform.position.z + (DistFromSubject > 0 ? Cam.farClipPlane : Cam.nearClipPlane);
    private float ParallaxFactor => Mathf.Abs(DistFromSubject) / ClippingPlane;

    // Test
    private float length, startpos;

    private void Start()
    {
        _startPosition = Cam.transform.position;
        _startZ = transform.position.z;
        
        
        // Test
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void LateUpdate()
    {
        Vector2 newPos = (Vector2)transform.position - Travel * (1-ParallaxFactor);
        transform.position = new Vector3(newPos.x, newPos.y, _startZ);
        _startPosition = (Vector2)Cam.transform.position;
        
        
        // Test
        print(Cam.transform.position.x.ToString() + ' ' + newPos.x.ToString() + ' ' + length.ToString());
        if (Cam.transform.position.x > newPos.x + length) transform.position = new Vector3(newPos.x+length*2, newPos.y, _startZ);
    }
}
