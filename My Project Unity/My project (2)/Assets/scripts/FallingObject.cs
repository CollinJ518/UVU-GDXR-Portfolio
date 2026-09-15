using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public float fallSpeed = 5f;
    public float destroyYPosition = -10f; // Below this, object is destroyed (off-screen)

    void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        if (transform.position.y < destroyYPosition)
        {
            Destroy(gameObject);
        }
    }
}
