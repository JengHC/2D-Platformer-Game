using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultPopup : MonoBehaviour
{
    [SerializeField]
    private TMP_Text resultTitle;
    [SerializeField]
    private TMP_Text scroreLabel;
    [SerializeField]
    GameObject highScoreObject;
    [SerializeField]
    GameObject highscorePopup;


    private void OnEnable()
    {
        Time.timeScale = 0;

        if(GameMangerController.Instance.IsCleared)
        {
            resultTitle.text = "Cleard";
            scroreLabel.text = GameMangerController.Instance.TimeLimit.ToString("#.##");
            SaveHighScore();
        }
        else
        {
            resultTitle.text = "GameOver";
            scroreLabel.text = "";
            highScoreObject.SetActive(false);
        }

    }

    private void OnDisable()
    {
        
    }

    void SaveHighScore()
    {
        float highscore = PlayerPrefs.GetFloat("highscore", 0);

        if(GameMangerController.Instance.TimeLimit>highscore)
        {
            highScoreObject.SetActive(true);
            PlayerPrefs.SetFloat("highscore", GameMangerController.Instance.TimeLimit);
            PlayerPrefs.Save(); //하드에 제대로 저장하는거
        }
        else
        {
            highScoreObject.SetActive(false);
        }
    }
    public void TryAgainPressed()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene");
    }

    public void QuitPressed()
    {
        Time.timeScale = 1;
        Application.Quit();
    }

    public void ShowHighScorePressed()
    {
        highscorePopup.SetActive(true);
    }
}
