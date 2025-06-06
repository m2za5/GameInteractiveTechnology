using UnityEngine;

public class StimulusMover : MonoBehaviour
{
    public float radius = 200f;
    public float speed = 1f;
    private float time = 0f;
    private Vector3 origin;

    void Start()
    {
        origin = transform.position;
    }

    void Update()
    {
        time += Time.deltaTime * speed;
        Vector3 offset = new Vector3(Mathf.Cos(time), Mathf.Sin(time), 0) * radius;
        transform.position = origin + offset;
    }
}