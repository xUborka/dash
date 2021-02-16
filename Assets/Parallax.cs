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
    // Start is called before the first frame update
    private void Start()
    {
        _startPosition = Cam.transform.position;
        _startZ = transform.position.z;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector2 newPos = _startPosition + Travel * ParallaxFactor;
        transform.position = new Vector3(newPos.x, newPos.y, _startZ);
    }
}
