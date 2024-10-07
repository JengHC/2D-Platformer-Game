using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 Velocity = new Vector2(10, 0);

    private void FixedUpdate()
    {
        transform.Translate(Velocity * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Terrain")
        {
            Destroy(gameObject);
        }
    }
}
