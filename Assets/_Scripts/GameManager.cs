using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private enum State
    {
        UPGRADE,
        INGAME,
        WIN,
        LOSE
    }

    private State state = State.INGAME;

    [Space(1)]
    [Header("UI")]
    public GameObject MenuPanel;
    public GameObject InGame;
    public GameObject WinPanel;
    public GameObject LosePanel;
    public Text MenuLevelText;
    public Text MoneyText;
    public Text NextLevelText;
    public Text CurrentLevelText;
    public Text MenuMoneyText;
    public Text ScoreText;
    public Text HealthUpgradeText;
    public Text SpeedUpgradeText;
    public Text BoxUpgradeText;
    public GameObject header;
    public GameObject levelTextInMenu;
    public Button UpgradeEnergyButton;
    public Button UpgradeSpeedButton;
    public Button UpgradeBoxButton;
    public Sprite grayUpgradeImage;
    public Sprite greenUpgradeImage;

    [Space(1)]
    [Header("Level Settings")]
    public int Level;
    public int fakelevel;
    public GameObject Confettis;

    [Space(1)]
    [Header("Game Settings")]
    public bool DeleteSave;
    public float currentmoney;
    public bool isGameContinue;

    [Space(1)]
    [Header("Upgrade Settings")]
    public float energyDecreaseValue;
    public float speedDecreaseValue;
    public float boxDecreaseValue;
    public List<float> energyMoney;
    public List<float> speedMoney;
    public List<float> boxMoney;
    public int upgradeEnergyLevel = 1;
    public int upgradeSpeedLevel = 1;
    public int upgradeBoxLevel = 1;
    public bool didReachMaxLevelEnergy;
    public bool didReachMaxLevelSpeed;
    public bool didReachMaxLevelBox;

    [SerializeField] EnergyBar energyBar;
    [SerializeField] PlayerController playerController;
    [SerializeField] CollectorController collectorController;

    private void Awake()
    {
        if (instance == null) instance = this;

        SetState(State.INGAME);

        Level = PlayerPrefs.GetInt("Level");
        if (Level == 0)
        {
            Level = 1;
        }
        fakelevel = PlayerPrefs.GetInt("FakeLevel");
        if (fakelevel == 0)
        {
            fakelevel = 1;
        }


    }

    private void SetState(State newState)
    {
        state = newState;
        MenuPanel.gameObject.SetActive(state == State.UPGRADE);
        InGame.SetActive(state == State.INGAME);
        WinPanel.SetActive(state == State.WIN);
        LosePanel.SetActive(state == State.LOSE);
    }

    void Start()
    {

        currentmoney = PlayerPrefs.GetFloat("Money", currentmoney);
        MenuMoneyText.text = "$" + PlayerPrefs.GetFloat("Money").ToString("F1");
        MoneyText.text = PlayerPrefs.GetFloat("Money").ToString("F1");

        MenuLevelText.text = "Level " + fakelevel.ToString();
        CurrentLevelText.text = fakelevel.ToString();
        int nxtlvl = fakelevel;
        nxtlvl++;
        NextLevelText.text = nxtlvl.ToString();

        CheckUpgradeButtons();

    }

    public void CheckUpgradeButtons()
    {
        if (PlayerPrefs.GetInt("UpgradeEnergyLevel") == 0)
            upgradeEnergyLevel = PlayerPrefs.GetInt("UpgradeEnergyLevel", 1);
        else
            upgradeEnergyLevel = PlayerPrefs.GetInt("UpgradeEnergyLevel");

        if (PlayerPrefs.GetInt("UpgradeSpeedLevel") == 0)
            upgradeSpeedLevel = PlayerPrefs.GetInt("UpgradeSpeedLevel", 1);
        else
            upgradeSpeedLevel = PlayerPrefs.GetInt("UpgradeSpeedLevel");

        if (PlayerPrefs.GetInt("UpgradeBoxLevel") == 0)
            upgradeSpeedLevel = PlayerPrefs.GetInt("UpgradeBoxLevel", 1);
        else
            upgradeSpeedLevel = PlayerPrefs.GetInt("UpgradeBoxLevel");

        PlayerPrefs.SetInt("UpgradeEnergyLevel", upgradeEnergyLevel);
        PlayerPrefs.SetInt("UpgradeSpeedLevel", upgradeSpeedLevel);
        PlayerPrefs.SetInt("UpgradeBoxLevel", upgradeBoxLevel);

        if (upgradeEnergyLevel < energyMoney.Count)
            HealthUpgradeText.text = "Level " + PlayerPrefs.GetInt("NextLevelEnergy", 1).ToString() + "\nEnergy " + "\n $" + energyMoney[upgradeEnergyLevel - 1];
        else
        {
            HealthUpgradeText.text = "Max Energy";
            UpgradeEnergyButton.GetComponent<Image>().sprite = grayUpgradeImage;
        }


        if (upgradeSpeedLevel < speedMoney.Count)
            SpeedUpgradeText.text = "Level " + PlayerPrefs.GetInt("NextLevelSpeed", 1).ToString() + "\nSpeed " + "\n $" + speedMoney[upgradeSpeedLevel - 1];
        else
        {
            SpeedUpgradeText.text = "Max Speed";
            UpgradeSpeedButton.GetComponent<Image>().sprite = grayUpgradeImage;
        }

        if (upgradeBoxLevel < boxMoney.Count)
            BoxUpgradeText.text = "Level " + PlayerPrefs.GetInt("NextLevelBox", 1).ToString() + "\nBox " + "\n $" + boxMoney[upgradeBoxLevel - 1];
        else
        {
            BoxUpgradeText.text = "Max Box";
            UpgradeBoxButton.GetComponent<Image>().sprite = grayUpgradeImage;
        }

        //Make buttons gray if the player can't afford them
        if (PlayerPrefs.GetFloat("Money") < energyMoney[upgradeSpeedLevel - 1])
        {
            UpgradeEnergyButton.GetComponent<Image>().sprite = grayUpgradeImage;
        }
        else
        {
            UpgradeEnergyButton.GetComponent<Image>().sprite = greenUpgradeImage;
        }

        if (PlayerPrefs.GetFloat("Money") < speedMoney[upgradeSpeedLevel - 1])
        {
            UpgradeSpeedButton.GetComponent<Image>().sprite = grayUpgradeImage;
        }
        else
        {
            UpgradeSpeedButton.GetComponent<Image>().sprite = greenUpgradeImage;
        }

        if (PlayerPrefs.GetFloat("Money") < boxMoney[upgradeBoxLevel - 1])
        {
            UpgradeBoxButton.GetComponent<Image>().sprite = grayUpgradeImage;
        }
        else
        {
            UpgradeBoxButton.GetComponent<Image>().sprite = greenUpgradeImage;
        }

        PlayerPrefs.Save();
    }


    public void UpdateMoneyText()
    {
        MoneyText.text = currentmoney.ToString("F1");
        MenuMoneyText.text = "$" + currentmoney.ToString("F1");
    }

    public void TapToStart()
    {
        SetState(State.INGAME);
        isGameContinue = true;
        header.SetActive(false);
        levelTextInMenu.SetActive(false);


    }

    public bool IsGameWon()
    {
        return state == State.WIN;
    }

    public bool IsGameEnd()
    {
        return state == State.LOSE || state == State.WIN;
    }

    public void Lose()
    {
        if (state != State.WIN && state != State.LOSE) //!levelCompleted && !isLose)
        {
            MenuMoneyText.enabled = true;

            SetState(State.LOSE);
            isGameContinue = false;

        }
    }

    public void Win()
    {
        print("Win");
        if (!IsGameEnd())
        {

            Level++;

            if (Level >= 5) // If we reached max level on build settings, then reset our level to start for a loop
            {
                Level = 1;
            }
            fakelevel++;

            MoneyEarned(); //Earned Money

            PlayerPrefs.SetInt("FakeLevel", fakelevel);
            PlayerPrefs.SetInt("Level", Level);

            print("level" + Level);

            PlayerPrefs.SetFloat("Money", currentmoney);


            PlayerPrefs.Save();
            NextLevelButton();
        }
    }

    public void MoneyEarned() // Call this if we earn money
    {

        currentmoney += 10;
        PlayerPrefs.SetFloat("Money", currentmoney);
        PlayerPrefs.Save();
        UpdateMoneyText();

    }

    public void NextLevelButton()
    {
        print(Level);
        SceneManager.LoadScene(Level);

        MenuLevelText.text = "Level " + PlayerPrefs.GetInt("Level").ToString();
    }

    public void ResetLevelButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void UpgradeEnergy()// Upgrade Player Energy
    {


        currentmoney = PlayerPrefs.GetFloat("Money");
        didReachMaxLevelEnergy = false;


        if (upgradeEnergyLevel < energyMoney.Count)
        {
            if (currentmoney >= energyMoney[upgradeEnergyLevel - 1] && energyMoney.Count > 0)
            {

                energyBar.UpgradeEnergy(energyDecreaseValue);


                currentmoney -= (int)energyMoney[upgradeEnergyLevel - 1];

                int nextlevel = upgradeEnergyLevel + 1;
                PlayerPrefs.SetInt("NextLevelEnergy", nextlevel);
                PlayerPrefs.SetInt("UpgradeEnergyLevel", upgradeEnergyLevel);
                HealthUpgradeText.text = "Level " + PlayerPrefs.GetInt("NextLevelEnergy").ToString() + "\nEnergy " + "\n $" + energyMoney[upgradeEnergyLevel];
                upgradeEnergyLevel += 1;


                PlayerPrefs.SetFloat("Money", currentmoney);

                MenuMoneyText.text = "$" + PlayerPrefs.GetFloat("Money").ToString("F1");
                MoneyText.text = PlayerPrefs.GetFloat("Money").ToString("F1");
                PlayerPrefs.Save();
            }
        }

        CheckUpgradeButtonSprite();


    }


    public void UpgradeSpeed()// Upgrade Player Speed
    {


        currentmoney = PlayerPrefs.GetFloat("Money");

        didReachMaxLevelSpeed = false;

        if (upgradeSpeedLevel < speedMoney.Count)
        {
            if (currentmoney >= speedMoney[upgradeSpeedLevel - 1] && speedMoney.Count > 0)
            {


                playerController.UpgradeSpeed(speedDecreaseValue);

                currentmoney -= (int)speedMoney[upgradeSpeedLevel - 1];
                int nextLevel = upgradeSpeedLevel + 1;

                PlayerPrefs.SetInt("NextLevelSpeed", nextLevel);
                PlayerPrefs.SetInt("UpgradeSpeedLevel", upgradeSpeedLevel);
                SpeedUpgradeText.text = "Level " + PlayerPrefs.GetInt("NextLevelSpeed").ToString() + "\nSpeed " + "\n $" + speedMoney[upgradeSpeedLevel];
                upgradeSpeedLevel += 1;


                PlayerPrefs.SetFloat("Money", currentmoney);
                MenuMoneyText.text = "$" + PlayerPrefs.GetFloat("Money").ToString("F1");
                MoneyText.text = PlayerPrefs.GetFloat("Money").ToString("F1");

                PlayerPrefs.Save();


            }
        }

        CheckUpgradeButtonSprite();

    }

    public void UpgradeBox()// Upgrade Box
    {


        currentmoney = PlayerPrefs.GetFloat("Money");

        didReachMaxLevelSpeed = false;

        if (upgradeBoxLevel < boxMoney.Count)
        {
            if (currentmoney >= boxMoney[upgradeBoxLevel - 1] && boxMoney.Count > 0)
            {

                collectorController.UpgradeBox(boxDecreaseValue);

                currentmoney -= (int)boxMoney[upgradeBoxLevel - 1];
                int nextLevel = upgradeBoxLevel + 1;

                PlayerPrefs.SetInt("NextLevelBox", nextLevel);
                PlayerPrefs.SetInt("UpgradeBoxLevel", upgradeBoxLevel);
                BoxUpgradeText.text = "Level " + PlayerPrefs.GetInt("NextLevelBox").ToString() + "\nBox " + "\n $" + boxMoney[upgradeBoxLevel];
                upgradeBoxLevel += 1;


                PlayerPrefs.SetFloat("Money", currentmoney);
                MenuMoneyText.text = "$" + PlayerPrefs.GetFloat("Money").ToString("F1");
                MoneyText.text = PlayerPrefs.GetFloat("Money").ToString("F1");

                PlayerPrefs.Save();


            }
        }

        CheckUpgradeButtonSprite();

    }


    private void CheckUpgradeButtonSprite()
    {
        if (upgradeEnergyLevel >= energyMoney.Count)
        {
            HealthUpgradeText.text = "Max Energy";
            didReachMaxLevelEnergy = true;
        }

        if (upgradeSpeedLevel >= speedMoney.Count)
        {
            SpeedUpgradeText.text = "Max Speed";
            didReachMaxLevelSpeed = true;
        }


        if (upgradeBoxLevel >= boxMoney.Count)
        {
            BoxUpgradeText.text = "Max Box";
            didReachMaxLevelBox = true;
        }


        if (didReachMaxLevelEnergy)
        {
            UpgradeEnergyButton.GetComponent<Image>().sprite = grayUpgradeImage;
        }
        else if (currentmoney >= energyMoney[upgradeEnergyLevel - 1] && energyMoney.Count > 0)
        {
            UpgradeEnergyButton.GetComponent<Image>().sprite = greenUpgradeImage;
        }
        else if (currentmoney < energyMoney[upgradeEnergyLevel - 1] && energyMoney.Count > 0)
        {
            UpgradeEnergyButton.GetComponent<Image>().sprite = grayUpgradeImage;
        }


        if (didReachMaxLevelSpeed)
        {
            UpgradeSpeedButton.GetComponent<Image>().sprite = grayUpgradeImage;
        }
        else if (currentmoney >= speedMoney[upgradeSpeedLevel - 1] && speedMoney.Count > 0)
        {
            UpgradeSpeedButton.GetComponent<Image>().sprite = greenUpgradeImage;
        }
        else if (currentmoney < speedMoney[upgradeSpeedLevel - 1] && speedMoney.Count > 0)
        {
            UpgradeSpeedButton.GetComponent<Image>().sprite = grayUpgradeImage;
        }

        if (didReachMaxLevelBox)
        {
            UpgradeBoxButton.GetComponent<Image>().sprite = grayUpgradeImage;
        }
        else if (currentmoney >= boxMoney[upgradeBoxLevel - 1] && boxMoney.Count > 0)
        {
            UpgradeBoxButton.GetComponent<Image>().sprite = greenUpgradeImage;
        }
        else if (currentmoney < boxMoney[upgradeBoxLevel - 1] && boxMoney.Count > 0)
        {
            UpgradeBoxButton.GetComponent<Image>().sprite = grayUpgradeImage;
        }
    }

}
