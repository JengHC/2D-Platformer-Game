using TMPro;
using UnityEngine;

public class HighscorePopup : MonoBehaviour
{
    public TMP_Text ScoreLabel;

    private void OnEnable()
    {
        string[] scores = PlayerPrefs.GetString("HighScores", "").Split(",");
        string result = "";

        for(int i=0; i<scores.Length; i++)
        {
            result += (i+1)+". " + scores[i]+ "\n";
        }
        ScoreLabel.text = result;
    }

    public void CloasePressed()
    {
        gameObject.SetActive(false);
    }
}
