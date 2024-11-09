using UnityEngine;

public class MoveLeftAndLoop : MonoBehaviour
{
    public float speed = 2.0f; 
    public float screenRightEdge = 9.0f; 
    public float screenLeftEdge = -9.0f; 

    void Update()
    {
        
        transform.position += Vector3.left * speed * Time.deltaTime;

        
        if (transform.position.x < screenLeftEdge)
        {
            transform.position = new Vector3(screenRightEdge, transform.position.y, transform.position.z);
        }
    }
}
