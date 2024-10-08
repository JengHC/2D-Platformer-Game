using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public PlayerController Player;
    public GameObject Cinemachine;
    
    private void Start()
    {
        GameMangerController.Instance.Player = Player;
        GameMangerController.Instance.CinemachineInstance = Cinemachine;
    }
}
