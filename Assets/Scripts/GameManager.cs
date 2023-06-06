using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager gameManagerInstance;
    public static GameManager Instance
    {
        get
        {
            if (gameManagerInstance == null)
                gameManagerInstance = FindObjectOfType<GameManager>();

            return gameManagerInstance;
        }
    }

    public const int MaxLives = 5;
    public int InitialMoney;
    public GameObject hightMoneyTextFieldObject;

    public int Level;
    public GameObject VictoryText;
    public GameObject GameOverText;

    public int InitialTurretPrice;
    public int InitialRocketPrice;
    public int TurretPriceAddition;
    public int RocketPriceAddition;

    private int turretPrice;
    private int rocketPrice;

    public static int Lives;
    private int money;
    private HealthDrawerScript healthDrawer;
    private MoneyDrawer moneyDrawer;
    private FileManager fileManager;

    private int remainingEnemies;

    private Dictionary<string, string> saveData;

    // Use this for initialization
    void Start ()
    {
        money = InitialMoney;

        turretPrice = InitialTurretPrice;
        rocketPrice = InitialRocketPrice;

        healthDrawer = GetComponent<HealthDrawerScript>();
        moneyDrawer = GetComponent<MoneyDrawer>();
        fileManager = GetComponent<FileManager>();

        moneyDrawer.Draw(InitialMoney);

        remainingEnemies = GetComponent<EnemySpawner>().Waves.Sum(w => w.Amount);
        saveData = fileManager.loadSaveData();

        hightMoneyTextFieldObject.GetComponent<Text>().text = saveData[Level.ToString()];
    }

    public void EnemyEscaped(GameObject enemy)
    {
        Lives--;
        CameraShaker.Instance.Shake();
        healthDrawer.Draw(Lives);

        if (Lives <= 0)
        {
            GameOver();
        }

        remainingEnemies--;
        if(remainingEnemies == 0) Victory();
    }

    public void EnemyKilled(GameObject enemy)
    {
        remainingEnemies--;
        if(remainingEnemies == 0) Victory();
    }

    public int GetMoney()
    {
        return money;
    }

    public void AddMoney(int value)
    {
        money += value;
        moneyDrawer.Draw(money);
    }

    public void TurretBuilt(GameObject turret)
    {
        if (turret.CompareTag("turretTower"))
        {
            money -= turretPrice;
            turretPrice += TurretPriceAddition;
        }
        else
        {
            money -= rocketPrice;
            rocketPrice += RocketPriceAddition;
        }

        moneyDrawer.Draw(money);
    }

    public void CoinCollected(GameObject coin)
    {
        money += CoinScript.Value;
        moneyDrawer.Draw(money);
    }

    public bool EnoughMoneyForTurret(string tag)
    {
        if(tag == "turretTower")
            return money >= turretPrice;

        return money >= rocketPrice;
    }

    public int MoneyForTurret(string tag)
    {
        return tag == "turretTower" ? turretPrice : rocketPrice;
    }

    public void Victory()
    {
        saveHighestMoneyToFile();
        VictoryText.SetActive(true);
        Invoke("NextLevel", 5.0f);
    }
    
    public void GameOver()
    {
        GameOverText.SetActive(true);
        Invoke("BackToMainMenu", 5.0f);
    }

    public void NextLevel()
    {
        if (Level <= 2)
        {
            SceneManager.LoadScene("Level_0" + (Level + 1));
        }
        else
        {
            SceneManager.LoadScene("Menu_screen");
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu_screen");
    }

    private void saveHighestMoneyToFile()
    {
        double saveMoney = double.Parse(saveData[Level.ToString()]);

        saveData[Level.ToString()] = money > saveMoney ? money.ToString() : saveMoney.ToString();

        fileManager.saveSaveData(saveData);
    }
}
