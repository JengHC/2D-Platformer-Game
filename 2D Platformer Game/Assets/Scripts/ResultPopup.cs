using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultPopup : MonoBehaviour
{
    [SerializeField]
    private TMP_Text resultTitle;
    [SerializeField]
    private TMP_Text scroreLabel;

    private void OnEnable()
    {
        if(GameMangerController.Instance.IsCleared)
        {
            resultTitle.text = "Cleard";
            scroreLabel.text = GameMangerController.Instance.TimeLimit.ToString("#.##");
        }
        else
        {
            resultTitle.text = "GameOver";
            scroreLabel.text = "";
        }

    }

    private void OnDisable()
    {
        
    }


    public void TryAgainPressed()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitPressed()
    {
        Application.Quit();
    }
}
