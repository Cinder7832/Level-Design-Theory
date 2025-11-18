using UnityEngine;

public class PipeMovement : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 1f;

    void Start()
    {
        
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1f);
        transform.position = Vector3.Lerp(pointA.position, pointB.position, t);
    }
}
