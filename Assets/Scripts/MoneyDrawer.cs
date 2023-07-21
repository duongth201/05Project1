using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MoneyDrawer : MonoBehaviour
{
    public GameObject TextFieldObject;
    public GameObject TextFieldMoney;
    private Text text;
    private string filePath;
    private Dictionary<string, List<int>> highScores;

    public void Draw(int money)
    {
        text.text = money.ToString();
    }

    public void SetMoneyHight(string level)
    {
        highScores = new Dictionary<string, List<int>>();
        filePath = Application.persistentDataPath + "/highscores.txt";

        LoadHighScores();

        TextFieldMoney.GetComponent<Text>().text = highScores["Level" + level][0].ToString();
    }

	// Use this for initialization
	void OnEnable ()
    {
        text = TextFieldObject.GetComponent<Text>();
	}

    private void LoadHighScores()
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
    }

}
