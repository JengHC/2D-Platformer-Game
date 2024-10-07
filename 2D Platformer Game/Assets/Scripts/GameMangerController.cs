using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class GameMangerController : MonoBehaviour
{
    public GameObject CinemachineInstance;
    public PlayerController Player;
    public ObjectPool BulletPool;

    [SerializeField]
    private GameObject popupCanvas;


    public LifeDisplayer LifeDisplayerInstance;

    int life = 3;

    private static GameMangerController instance;
    public static GameMangerController Instance
    {
        get
        {
            return instance;
        }
    }

    public TMP_Text TimeLimitLabel;
    
    public float TimeLimit = 30;

    private bool isCleared;
    public bool IsCleared
    {
        get 
        { 
            return isCleared; 
        }
    }

    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        life = 3;
        LifeDisplayerInstance.SetLives(life);
    }

    // Update is called once per frame
    void Update()
    {
        TimeLimit -= Time.deltaTime;
        TimeLimitLabel.text = "Time Left: " + ((int)TimeLimit);

        // 시간이 다 되면 게임 오버
        // +(추가해볼것) 목숨이 하나 깎이고, 시간이 다시 30초 늘어난다.
        if (TimeLimit <= 0)
        {
            GameOver();
        }
    }

    public void AddTime(float time)
    {
        TimeLimit += time;
    }

    // Player가 Die 상태일 때, CinemachineCamera 작동 멈춤 
    public void Die()
    {
        CinemachineInstance.SetActive(false);
        life--;
        LifeDisplayerInstance.SetLives(life);

        Invoke("Restart", 2);
    }

    void Restart()
    {
        if (life > 0)
        {
            CinemachineInstance.SetActive(true);
            Player.Restart();
        }
        
        else 
        {
            GameOver();
        }
    }

    void GameOver()
    {
        isCleared = false;
        popupCanvas.SetActive(true);
        Debug.Log("Game Over");
    }

    public void GameClear()
    {
        isCleared = true;
        popupCanvas.SetActive(true);
    }

}
