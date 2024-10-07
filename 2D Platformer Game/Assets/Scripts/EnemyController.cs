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
        // ���� �տ� ��ֹ��� �ְų�, ���������� ������ ���
        if (FrontCollider.IsTouching(TerrainCollider) || !FrontBottomCollider.IsTouching(TerrainCollider))
        {
            
            vx = -vx;
            // �� ĳ������ ��������Ʈ�� �¿�� ��������, �̵� ������ �ð������� �ݿ�
            transform.localScale = new Vector2(-transform.localScale.x, 1);
        }
    }

    private void FixedUpdate()
    { 
      // vx �������� ���� �̵���Ű��
      // Time.fixedDeltaTime�� ���� ������ ���� ������� ������ �ӵ��� �̵��ϵ��� ����
        transform.Translate(vx * Time.fixedDeltaTime);
    }
}
