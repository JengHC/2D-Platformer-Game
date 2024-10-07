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

        //------캐릭터 왼쪽,오른쪽 벽 확인------
        onWall = LeftWallCollider.IsTouching(TerrainCollider) || RightWallCollider.IsTouching(TerrainCollider);

        if (onWall && !grounded && vx != 0)
        {
            // 벽에 붙어 있을 때 중력 효과 약화 (미끄러짐)
            vy = Mathf.Max(vy, -WallSlideSpeed);

            if (Input.GetButtonDown("Jump"))
            {
                wallJumping = true;     // 벽 점프 중
                vy = JumpSpeed;         // 점프시 세로 속도 설정

                // 벽에서 반대 방향으로 점프
                if (LeftWallCollider.IsTouching(TerrainCollider))
                {
                    vx = WallJumpSpeed; // 오른쪽으로 점프
                }
                else if (RightWallCollider.IsTouching(TerrainCollider))
                {
                    vx = -WallJumpSpeed; // 왼쪽으로 점프
                }
            }
        }
        // 벽 점프 후 일정 시간 동안 벽에 다시 붙지 않도록 설정
        if (wallJumping)
        {
            // 일정 시간 후 다시 벽에 붙을 수 있게끔 설정
            Invoke("StopWallJump", 0.3f);
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
        if (BottomCollider.IsTouching(TerrainCollider)) // 지금 바닥에 붙어있었습니다.
        {
            //-----벽타기-----
            grounded = true;
            wallJumping = false;
            //-----벽타기-----

            if (!grounded)          // 지금은 땅에 붙었는데, 아까는 안붙어있었음
            {
                if (vx == 0)           // 가로방향으로 멈춘 상태에서 착지
                {                 
                    GetComponent<Animator>().SetTrigger("Idle");
                }
                else                // 가로방향으로 이동하면서 착지
                {
                    GetComponent<Animator>().SetTrigger("Run");
                }
            }
            else
            {                       // 땅에 계속 붙어있었음.
                if (vx != preVx)
                {
                    if (vx == 0)    // 멈춰있음
                    {
                        GetComponent<Animator>().SetTrigger("Idle");
                    }
                    else            // 달리기 시작
                    {
                        GetComponent<Animator>().SetTrigger("Run");
                    }
                }
            }
        }
        else
        {
            //----- 벽타기 추가
            grounded = false;
            //------
            if (grounded)            // 지금은 땅에 안붙어있지만, 아까는 붙어있었음.
            {
                GetComponent<Animator>().SetTrigger("Jump");
            }
            if (vy < 0)
            {
                // vy값은 jumpspeed 값이니까, jump스피드가 -로 내려가면 Fall 애니메이션 동작
                GetComponent<Animator>().SetTrigger("Fall");
            }
        }

        //위에 grounded는 현재와 아까 상태를 비교하는 것

        // 점프 구현 (땅에 붙어 있을 때 점프)
        grounded = BottomCollider.IsTouching(TerrainCollider); // 

        // 윗 키 누르면 점프 속도 업
        if (Input.GetButtonDown("Jump") && grounded)
        {
            vy = JumpSpeed;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                vy = BoostedJumpSpeed;
            }
        }
        preVx = vx;             // 이전 속도

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

//if (vx > 0) // 오른쪽으로 이동
//{
//    // 오른쪽을 바라보게 하기 위해 x 스케일을 1로 설정
//    transform.localScale = new Vector2(1, 1);
//}
//else if (vx < 0) // 왼쪽으로 이동
//{
//    // 왼쪽을 바라보게 하기 위해 x 스케일을 -1로 설정
//    transform.localScale = new Vector2(-1, 1);
//}