using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class leaderBoard : MonoBehaviour
{
    public TMP_Text text;
    private void Start()
    {
        Debug.Log(PlayerPrefs.GetInt("ScoreCount").ToString());
        UpdateBoard();
    }
    public void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.D))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Deleted All Keys");
        }
    }
    public void UpdateBoard()
    {
        int count = 0;
        if (PlayerPrefs.HasKey("ScoreCount"))
        {
            count = PlayerPrefs.GetInt("ScoreCount");
        }
        List<PlayerScore> scores = new List<PlayerScore>();
        string message = "Liderlik Tablosu";
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                string name = PlayerPrefs.GetString($"PlayerName{i}");
                int score = PlayerPrefs.GetInt(name);
                scores.Add(new PlayerScore(name, score));
                Debug.Log($"Added {name} - {score}");

                scores = scores.OrderByDescending(x => x.score).ToList();
            }

            int kacinci = 0;
            foreach (PlayerScore score in scores)
            {
                kacinci++;
                message += "\n";
                message += $"{kacinci}. {score.name}           {score.score}";
            }
        }
        text.text = message;
    }

    public static void addSocre(string name , int score)
    {
        int count = 0;
        if (PlayerPrefs.HasKey("ScoreCount"))
        {
            count = PlayerPrefs.GetInt("ScoreCount");
        }
        if (PlayerPrefs.HasKey(name))
        {
            if (PlayerPrefs.GetInt(name) < score)
            {
                PlayerPrefs.SetInt(name, score);
            }
        }
        else
        {
            PlayerPrefs.SetInt(name, score);
            PlayerPrefs.SetInt("ScoreCount", count + 1);
            PlayerPrefs.SetString($"PlayerName{count}", name);
        }
    }
}


public class PlayerScore
{
    public string name;
    public int score;
    public PlayerScore(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}