using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 Velocity = new Vector2(10, 0);

    private void Update()
    {
        if(!GetComponent<SpriteRenderer>().isVisible)
        {
            // ÃÑ¾Ë »ç¶óÁü
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(Velocity * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Terrain")
        {
            gameObject.SetActive(false);
        }
        else if(collision.gameObject.tag=="Enemy")
        {
            Debug.Log("Enemy Shoot");

            gameObject.SetActive(false);
            collision.GetComponent<EnemyController>().Hit(1);
        }
    }
}
