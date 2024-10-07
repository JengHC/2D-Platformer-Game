using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum State
    {
        Playing,
        Dead
    }
    public float Speed = 10;
    public float JumpSpeed = 12;
    public float BoostedJumpSpeed = 14; // ������ Ű ���� ������ ��, ���� ����

    public Collider2D BottomCollider;
    public CompositeCollider2D TerrainCollider;

    public GameObject BulletPrefab;

    //---------�� ����,�����̴�-----------
    public float WallSlideSpeed = 2;
    public float WallJumpSpeed = 10;
    public Collider2D LeftWallCollider;
    public Collider2D RightWallCollider;
    //------------------------------------

    // ��� ����
    float vx = 0;
    bool grounded;
    float preVx = 0;
    bool onWall;
    bool wallJumping;

    Vector2 originalPosition;
    State state;


    void Start()
    {
        originalPosition = transform.position;
        state = State.Playing;
    }

    public void Restart()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        GetComponent<BoxCollider2D>().enabled = true;

        transform.eulerAngles = Vector3.zero;
        transform.position = originalPosition;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;  // �������� �������� �ӵ� �ʱ�ȭ
        state = State.Playing;
    }

    void Update()
    {
        if (state != State.Playing) return;

        vx = Input.GetAxisRaw("Horizontal") * Speed;
        float vy = GetComponent<Rigidbody2D>().linearVelocityY;

        //------ĳ���� ����,������ �� Ȯ��------
        onWall = LeftWallCollider.IsTouching(TerrainCollider) || RightWallCollider.IsTouching(TerrainCollider);

        if (onWall && !grounded && vx != 0)
        {
            // ���� �پ� ���� �� �߷� ȿ�� ��ȭ (�̲�����)
            vy = Mathf.Max(vy, -WallSlideSpeed);

            if (Input.GetButtonDown("Jump"))
            {
                wallJumping = true;     // �� ���� ��
                vy = JumpSpeed;         // ������ ���� �ӵ� ����

                // ������ �ݴ� �������� ����
                if (LeftWallCollider.IsTouching(TerrainCollider))
                {
                    vx = WallJumpSpeed; // ���������� ����
                }
                else if (RightWallCollider.IsTouching(TerrainCollider))
                {
                    vx = -WallJumpSpeed; // �������� ����
                }
            }
        }
        // �� ���� �� ���� �ð� ���� ���� �ٽ� ���� �ʵ��� ����
        if (wallJumping)
        {
            // ���� �ð� �� �ٽ� ���� ���� �� �ְԲ� ����
            Invoke("StopWallJump", 0.3f);
        }
        //-------���κ� ��Ÿ�� �߰�

        if (vx < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        if (vx > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        // �ִϸ��̼� ó��
        if (BottomCollider.IsTouching(TerrainCollider)) // ���� �ٴڿ� �پ��־����ϴ�.
        {
            //-----��Ÿ��-----
            grounded = true;
            wallJumping = false;
            //-----��Ÿ��-----

            if (!grounded)          // ������ ���� �پ��µ�, �Ʊ�� �Ⱥپ��־���
            {
                if (vx == 0)           // ���ι������� ���� ���¿��� ����
                {                 
                    GetComponent<Animator>().SetTrigger("Idle");
                }
                else                // ���ι������� �̵��ϸ鼭 ����
                {
                    GetComponent<Animator>().SetTrigger("Run");
                }
            }
            else
            {                       // ���� ��� �پ��־���.
                if (vx != preVx)
                {
                    if (vx == 0)    // ��������
                    {
                        GetComponent<Animator>().SetTrigger("Idle");
                    }
                    else            // �޸��� ����
                    {
                        GetComponent<Animator>().SetTrigger("Run");
                    }
                }
            }
        }
        else
        {
            //----- ��Ÿ�� �߰�
            grounded = false;
            //------
            if (grounded)            // ������ ���� �Ⱥپ�������, �Ʊ�� �پ��־���.
            {
                GetComponent<Animator>().SetTrigger("Jump");
            }
            if (vy < 0)
            {
                // vy���� jumpspeed ���̴ϱ�, jump���ǵ尡 -�� �������� Fall �ִϸ��̼� ����
                GetComponent<Animator>().SetTrigger("Fall");
            }
        }

        //���� grounded�� ����� �Ʊ� ���¸� ���ϴ� ��

        // ���� ���� (���� �پ� ���� �� ����)
        grounded = BottomCollider.IsTouching(TerrainCollider); // 

        // �� Ű ������ ���� �ӵ� ��
        if (Input.GetButtonDown("Jump") && grounded)
        {
            vy = JumpSpeed;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                vy = BoostedJumpSpeed;
            }
        }
        preVx = vx;             // ���� �ӵ�

        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(vx, vy);

        if(Input.GetButtonDown("Fire1"))
        {
            Vector2 bulletV = new Vector2(10, 0);

            if(GetComponent<SpriteRenderer>().flipX)
            {
                bulletV.x = -bulletV.x;
            }
            GameObject bullet = Instantiate(BulletPrefab);
            bullet.transform.position = transform.position;
            bullet.GetComponent<Bullet>().Velocity = bulletV;
        }
    }

    void StopWallJump()
    {
        wallJumping = false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Die();
        }
    }
    void Die()
    {
        state = State.Dead;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        GetComponent<Rigidbody2D>().angularVelocity = 720;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        GetComponent<BoxCollider2D>().enabled = false;

        GameMangerController.Instance.Die();
    }
}

//if (vx > 0) // ���������� �̵�
//{
//    // �������� �ٶ󺸰� �ϱ� ���� x �������� 1�� ����
//    transform.localScale = new Vector2(1, 1);
//}
//else if (vx < 0) // �������� �̵�
//{
//    // ������ �ٶ󺸰� �ϱ� ���� x �������� -1�� ����
//    transform.localScale = new Vector2(-1, 1);
//}