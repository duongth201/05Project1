using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;
    public static ScoreManager Instance => instance;

    public List<GameObject> TextFieldScoreOne;
    public List<GameObject> TextFieldScoreTwo;
    public List<GameObject> TextFieldScoreThree;
    public GameObject TextFieldScoreLevel;

    private Dictionary<string, List<int>> highScores;
    private string filePath;
    private int level;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        level = 1;
        TextFieldScoreLevel.GetComponent<Text>().text = "Level " + level;

        highScores = new Dictionary<string, List<int>>();
        filePath = Application.persistentDataPath + "/highscores.txt";
        Debug.Log(filePath);

        LoadHighScores();
    }

    public void LoadScore()
    {
        highScores = new Dictionary<string, List<int>>();
        filePath = Application.persistentDataPath + "/highscores.txt";

        LoadHighScores();

        List<int> scores = GetHighScores("Level1");

        TextFieldScoreOne.ForEach(x => x.GetComponent<Text>().text = scores[0].ToString());
        TextFieldScoreTwo.ForEach(x => x.GetComponent<Text>().text = scores[1].ToString());
        TextFieldScoreThree.ForEach(x => x.GetComponent<Text>().text = scores[2].ToString());
    }

    public void NextLevelScore()
    {
        if (level < 3) level++;
        highScores = new Dictionary<string, List<int>>();
        filePath = Application.persistentDataPath + "/highscores.txt";

        LoadHighScores();

        List<int> scores = GetHighScores("Level" + level);

        TextFieldScoreLevel.GetComponent<Text>().text = "Level " + level;
        TextFieldScoreOne.ForEach(x => x.GetComponent<Text>().text = scores[0].ToString());
        TextFieldScoreTwo.ForEach(x => x.GetComponent<Text>().text = scores[1].ToString());
        TextFieldScoreThree.ForEach(x => x.GetComponent<Text>().text = scores[2].ToString());
    }

    public void BackNextLevelScore()
    {
        if (level > 1) level--;
        highScores = new Dictionary<string, List<int>>();
        filePath = Application.persistentDataPath + "/highscores.txt";

        LoadHighScores();

        List<int> scores = GetHighScores("Level" + level);

        TextFieldScoreLevel.GetComponent<Text>().text = "Level " + level;
        TextFieldScoreOne.ForEach(x => x.GetComponent<Text>().text = scores[0].ToString());
        TextFieldScoreTwo.ForEach(x => x.GetComponent<Text>().text = scores[1].ToString());
        TextFieldScoreThree.ForEach(x => x.GetComponent<Text>().text = scores[2].ToString());
    }

    private void LoadHighScores()
    {
       try
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    string[] data = line.Split(':');
                    string levelKey = data[0];
                    List<int> scores = data[1].Split(',').Select(int.Parse).ToList();
                    highScores.Add(levelKey, scores);
                }
            }
            else
            {
                for (int i = 1; i <= 3; i++)
                {
                    string levelKey = "Level" + i.ToString();
                    highScores.Add(levelKey, new List<int>() { 0, 0, 0 });

                    SaveHighScores();
                }
            }
        } catch (IOException ex)
        {
            Debug.Log($"{Time.frameCount}. exception: {ex.Message}");
        }
    }

    private void SaveHighScores()
    {
        List<string> lines = new List<string>();

        foreach (var kvp in highScores)
        {
            string levelKey = kvp.Key;
            List<int> scores = kvp.Value;
            string scoresString = string.Join(",", scores.Select(x => x.ToString()).ToArray());
            string line = levelKey + ":" + scoresString;
            lines.Add(line);
        }

        try
        {
            File.WriteAllLines(filePath, lines.ToArray());
        } catch (IOException ex)
        {
            Debug.Log($"{Time.frameCount}. exception: {ex.Message}");
        }
    }

    public void SaveHighScore(string levelName, int score)
    {
        if (highScores.ContainsKey(levelName))
        {
            List<int> scores = highScores[levelName];
            scores.Add(score);
            scores.Sort((a, b) => b.CompareTo(a));
            scores = scores.Take(3).ToList();
            highScores[levelName] = scores;

            SaveHighScores();
        }
        else
        {
            Debug.LogError("Level " + levelName + " không tồn tại trong danh sách số điểm cao nhất!");
        }
    }

    public List<int> GetHighScores(string levelName)
    {
        if (highScores.ContainsKey(levelName))
        {
            return highScores[levelName];
        }
        else
        {
            Debug.LogError("Level " + levelName + " không tồn tại trong danh sách số điểm cao nhất!");
            return new List<int>();
        }
    }
}
