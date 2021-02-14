using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Camera cam;
    public Transform subject;
    Vector2 startPosition;
    float startZ;
    Vector2 travel => (Vector2)cam.transform.position - startPosition;
    float distFromSubject => transform.position.z - subject.position.z;
    float clippingPlane => (cam.transform.position.z + (distFromSubject > 0 ? cam.farClipPlane : cam.nearClipPlane));
    float parallaxFactor => Mathf.Abs(distFromSubject) / clippingPlane;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = cam.transform.position;
        startZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPos = startPosition + travel * parallaxFactor;
        transform.position = new Vector3(newPos.x, newPos.y, startZ);
    }
}
