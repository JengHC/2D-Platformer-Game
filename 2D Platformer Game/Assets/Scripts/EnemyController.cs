using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float Speed=2;
    public CompositeCollider2D TerrainCollider;
    public Collider2D FrontBottomCollider;
    public Collider2D FrontCollider;

    Vector2 vx;


    void Start()
    {
        vx = Vector2.right * Speed;
    }


    void Update()
    {
        // 적의 앞에 장애물이 있거나, 낭떠러지에 도달한 경우
        if (FrontCollider.IsTouching(TerrainCollider) || !FrontBottomCollider.IsTouching(TerrainCollider))
        {
            
            vx = -vx;
            // 적 캐릭터의 스프라이트를 좌우로 반전시켜, 이동 방향을 시각적으로 반영
            transform.localScale = new Vector2(-transform.localScale.x, 1);
        }
    }

    private void FixedUpdate()
    { 
      // vx 방향으로 적을 이동시키기
      // Time.fixedDeltaTime을 곱해 프레임 수에 상관없이 일정한 속도로 이동하도록 만듦
        transform.Translate(vx * Time.fixedDeltaTime);
    }
}
