using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public Text scoreText;
    public Canvas highScoreCanvas;
    public List<Text> texts;
    void Start()
    {
        scoreText.text = $"Your score: {GameManager.appleCount}";
        HighScoreManager.Instance.SetHighScore(GameManager.appleCount);
        for (int i = 0; i < texts.Count; i++)
        {
            texts[i].text = HighScoreManager.highScores.scores[i].ToString();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void HighScores()
    {
        highScoreCanvas.gameObject.SetActive(true);
    }

    public void BackToEndScreen()
    {
        highScoreCanvas.gameObject.SetActive(false);

    }
}
