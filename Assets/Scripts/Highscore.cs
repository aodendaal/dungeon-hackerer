using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highscore : MonoBehaviour {

    private class Score
    {
        public string Name { get; set; }
        public int Amount { get; set; }

        public Score(string scoreString)
        {
            Name = scoreString.Split(':')[0];
            Amount = int.Parse(scoreString.Split(':')[1]);
        }
    }

    [SerializeField]
    private Text highscoresText;

    private string highscoreKey = "Highscores";

	// Use this for initialization
	void Start () {
        PlayerPrefs.DeleteKey(highscoreKey);
        if (!PlayerPrefs.HasKey(highscoreKey))
        {
            PlayerPrefs.SetString(highscoreKey, "Andre:1|Marc:0");
        }

        ShowHighscores();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetHighScore(string name, int score)
    {
    }

    public void ShowHighscores()
    {
        var scoresString = PlayerPrefs.GetString(highscoreKey);
        var scores = scoresString.Split('|');

        highscoresText.text = string.Empty;
        var count = 1;
        foreach (var score in scores)
        {
            var name = score.Split(':')[0];
            var amount = score.Split(':')[1];
            highscoresText.text += string.Format("{0}. {1}\t\t\t\t\t{2}\n", count++, name, amount);
        }
    }
}