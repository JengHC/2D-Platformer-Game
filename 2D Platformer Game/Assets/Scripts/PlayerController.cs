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
    public float BoostedJumpSpeed = 14; // 윗방향 키 같이 눌렀을 때, 점프 증가

    public Collider2D BottomCollider;
    public CompositeCollider2D TerrainCollider;

    public GameObject BulletPrefab;

    //---------벽 점프,슬라이더-----------
    public float WallSlideSpeed = 2;
    public float WallJumpSpeed = 10;
    public Collider2D LeftWallCollider;
    public Collider2D RightWallCollider;
    //------------------------------------

    // 멤버 변수
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
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;  // 떨어지고 있을때의 속도 초기화
        state = State.Playing;
    }

    void Update()
    {
        if (state != State.Playing) return;

        vx = Input.GetAxisRaw("Horizontal") * Speed;
        float vy = GetComponent<Rigidbody2D>().linearVelocityY;

        // 땅에 닿아있는지 체크
        grounded = BottomCollider.IsTouching(TerrainCollider);

        //------캐릭터 왼쪽,오른쪽 벽 확인------
        onWall = LeftWallCollider.IsTouching(TerrainCollider) || RightWallCollider.IsTouching(TerrainCollider);

        if (onWall && !grounded && vx != 0)
        {
            // 벽에 붙어 있을 때 중력 효과 약화 (미끄러짐)
            vy = Mathf.Max(vy, -WallSlideSpeed);

            if (Input.GetButtonDown("Jump"))
            {
                wallJumping = true;     // 벽 점프 중
                vy = JumpSpeed;         // 점프 시 세로 속도 설정

                // 벽에서 반대 방향으로 점프
                if (LeftWallCollider.IsTouching(TerrainCollider))
                {
                    vx = WallJumpSpeed; // 오른쪽으로 점프
                }
                else if (RightWallCollider.IsTouching(TerrainCollider))
                {
                    vx = -WallJumpSpeed; // 왼쪽으로 점프
                }

                Invoke("StopWallJump", 0.3f); // 일정 시간 후 벽 점프 종료
            }
        }
        //-------윗부분 벽타기 추가

        if (vx < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        if (vx > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }



        // 애니메이션 처리
        if (grounded)  // 땅에 닿아있을 때
        {
            if (!wallJumping)
            {
                GetComponent<Animator>().ResetTrigger("Fall");
                GetComponent<Animator>().ResetTrigger("Jump");

                if (vx == 0)
                {
                    GetComponent<Animator>().SetTrigger("Idle");
                }
                else
                {
                    GetComponent<Animator>().SetTrigger("Run");
                }
            }
        }
        else  // 공중에 있을 때
        {
            if (vy > 0)  // 위로 올라가는 중이면 Jump 애니메이션
            {
                GetComponent<Animator>().SetTrigger("Jump");
                GetComponent<Animator>().ResetTrigger("Fall");
            }
            else if (vy < 0)  // 아래로 내려가는 중이면 Fall 애니메이션
            {
                GetComponent<Animator>().SetTrigger("Fall");
                GetComponent<Animator>().ResetTrigger("Jump");
            }
        }

        // 윗 키 누르면 점프 속도 업
        if (Input.GetButtonDown("Jump") && grounded)
        {
            vy = JumpSpeed;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                vy = BoostedJumpSpeed;
            }
        }

        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(vx, vy);

        // 총알 발사
        if (Input.GetButtonDown("Fire1"))
        {
            // 총알 방향
            Vector2 bulletV = new Vector2(10, 0);

            // 방향 변경
            if (GetComponent<SpriteRenderer>().flipX)
            {
                bulletV.x = -bulletV.x;
            }

            GameObject bullet = GameMangerController.Instance.BulletPool.GetObject();
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
