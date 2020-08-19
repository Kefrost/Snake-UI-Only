using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SocialPlatforms.Impl;

public class HighScoreManager : MonoBehaviour
{
    public static Scores highScores;

    public static HighScoreManager Instance;

    private void Awake()
    {
        Instance = this;

        if (highScores == null)
        {
            highScores = new Scores();
        }

        var json = PlayerPrefs.GetString("HighScore", "{\"scores\":[0,0,0,0,0,0,0,0,0,0]}");

        Debug.Log(json);

        
            highScores = JsonUtility.FromJson<Scores>(json);

            highScores.scores = highScores.scores.OrderByDescending(x => x).ToList();
        
    }

    public void SetHighScore(int score)
    {
        if (!highScores.scores.Any())
        {
            highScores.scores.Insert(0, score);
        }
        else if (score > highScores.scores[highScores.scores.Count - 1])
        {
            highScores.scores.Insert(0, score);
        }

        if (highScores.scores.Count > 10)
        {
            highScores.scores.RemoveAt(highScores.scores.Count - 1);
        }

        highScores.scores = highScores.scores.OrderByDescending(x => x).ToList();

        var json = JsonUtility.ToJson(highScores);

        PlayerPrefs.SetString("HighScore", json);
        PlayerPrefs.Save();
    }

    [Serializable]
    public class Scores
    {
        public List<int> scores;

        public Scores()
        {
            this.scores = new List<int>();
        }
        public Scores(List<int> score)
        {
            this.scores = new List<int>();
            this.scores = score;
        }
    }
}
