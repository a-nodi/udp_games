using UnityEngine;

public class MoveLeftAndLoop : MonoBehaviour
{
    public float speed = 2.0f; 
    public float screenRightEdge = 9.0f; 
    public float screenLeftEdge = -9.0f; 

    void Start()
    {
        
        transform.position = new Vector3(17.6, 0, transform.position.z);
        transform.localScale = new Vector3(0.75f, 0.6f, transform.localScale.z);
    }

    void Update()
    {
        
        transform.position += Vector3.left * speed * Time.deltaTime;

        
        if (transform.position.x < screenLeftEdge)
        {
            transform.position = new Vector3(screenRightEdge, transform.position.y, transform.position.z);
        }
    }
}
