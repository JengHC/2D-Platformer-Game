using NUnit.Framework;
using System.Collections.Generic;
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
        float score = GameMangerController.Instance.TimeLimit;

        float highscore = PlayerPrefs.GetFloat("highscore", 0);

        if(score>highscore)
        {
            highScoreObject.SetActive(true);
            PlayerPrefs.SetFloat("highscore", GameMangerController.Instance.TimeLimit);
            PlayerPrefs.Save(); 
        }
        else
        {
            highScoreObject.SetActive(false);
        }

        string currentScoreString = score.ToString("#.###");
        string savedScoreString = PlayerPrefs.GetString("HighScores", "");


        if(savedScoreString=="")
        {
            PlayerPrefs.SetString("HighScores",currentScoreString);

        }
        else
        {
            string[] scoreArray = savedScoreString.Split(',');  // ����� ������ �迭�� �и�
            List<string> scoreList = new List<string>(scoreArray);

            for(int i=0; i<scoreList.Count; i++)                // ������ ��ġ�� �� ���� �ֱ�
            {
                float savedScore = float.Parse(scoreList[i]);
                if(savedScore < score)                          // ����� ������ �� ������
                {
                    scoreList.Insert(i, currentScoreString);    // ���� ������ �ڷ� ��ġ
                    break;
                }
            }

            if(scoreArray.Length == scoreList.Count) // ��ġ�� ã�� ���ϸ� ���������� ����
            {
                scoreList.Add(currentScoreString);
            }

            if(scoreList.Count>10)  // ����� 10�� �̻��̸� ������ ��� ����
            {
                scoreList.RemoveAt(10);
            }

            string result = string.Join(",", scoreList); // ����Ʈ�� �� ���� ��Ʈ������ ��ġ��
            Debug.Log(result);
            PlayerPrefs.SetString("HighScores", result);
        }

        PlayerPrefs.Save();

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
