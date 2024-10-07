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
            string[] scoreArray = savedScoreString.Split(',');  // 저장된 점수를 배열로 분리
            List<string> scoreList = new List<string>(scoreArray);

            for(int i=0; i<scoreList.Count; i++)                // 적당한 위치에 새 점수 넣기
            {
                float savedScore = float.Parse(scoreList[i]);
                if(savedScore < score)                          // 저장된 점수가 더 낮으면
                {
                    scoreList.Insert(i, currentScoreString);    // 낮은 점수는 뒤로 배치
                    break;
                }
            }

            if(scoreArray.Length == scoreList.Count) // 위치를 찾지 못하면 마지막으로 저장
            {
                scoreList.Add(currentScoreString);
            }

            if(scoreList.Count>10)  // 기록이 10개 이상이면 마지막 기록 삭제
            {
                scoreList.RemoveAt(10);
            }

            string result = string.Join(",", scoreList); // 리시트를 한 개의 스트링으로 합치기
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
