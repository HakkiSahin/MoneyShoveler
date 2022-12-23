using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI yourMoneyText;
    [SerializeField] private TextMeshProUGUI typeText;
    public IncrementelUpdateType IncrementelUpdateType;
    private Button btn;

    private GameObject car;

    // Start is called before the first frame update
    private void Start()
    {
        car = GameObject.FindWithTag("Car");
        btn = GetComponent<Button>();
        btn?.onClick.AddListener(() => BuyIncrementel());
        StartingCostLoad();
    }

    private void StartingCostLoad()
    {
        moneyText.text = PlayerPrefs.GetInt(typeText.text + "MONEY") > 0
            ? PlayerPrefs.GetInt(typeText.text + "MONEY").ToString()
            : "500";

        levelText.text = PlayerPrefs.GetInt(typeText.text + "LEVEL") > 0
            ? "LEVEL " + PlayerPrefs.GetInt(typeText.text + "LEVEL")
            : "LEVEL 1";
    }

    private void BuyIncrementel()
    {
        if (int.Parse(yourMoneyText.text) > int.Parse(moneyText.text))
        {
            yourMoneyText.text = (int.Parse(yourMoneyText.text) - int.Parse(moneyText.text)).ToString();
            PlayerPrefs.SetInt("Money", int.Parse(yourMoneyText.text));


            string[] levelArray = levelText.text.Split(' ');
            levelText.text = "LEVEL " + (int.Parse(levelArray[1]) + 1);
            PlayerPrefs.SetInt(typeText.text + "LEVEL", int.Parse(levelArray[1]) + 1);
            moneyText.text = Mathf.Round(int.Parse(moneyText.text) + int.Parse(moneyText.text) * 0.3f).ToString();
            PlayerPrefs.SetInt(typeText.text + "MONEY", int.Parse(moneyText.text));
            AddIncremante(IncrementelUpdateType);
        }

        ControlLevelCar();
    }


    void ControlLevelCar()
    {
        if (PlayerPrefs.GetInt("SIZELEVEL") / (PlayerPrefs.GetInt("Car") + 1) >= 5 &&
            PlayerPrefs.GetInt("CAPACITYLEVEL") / (PlayerPrefs.GetInt("Car") + 1) >= 5)
        {
            PlayerPrefs.SetInt("Car", (PlayerPrefs.GetInt("Car") + 1));
            LevelController._instance.GetActiveCar((PlayerPrefs.GetInt("Car")));
            transform.parent.parent.gameObject.SetActive(false);
        }
    }

    void AddIncremante(IncrementelUpdateType type)
    {
        switch (type)
        {
            case IncrementelUpdateType.Capacity:
                car.GetComponent<Character>().UpdateMaxSize(12);
                break;
            case IncrementelUpdateType.Size:
                car.GetComponent<Character>().UpdateScale(0.1f);
                break;
            case IncrementelUpdateType.Speed:
                car.GetComponent<Character>().UpdateMaxSpeed(0.5f);
                break;
        }
    }
}

public enum IncrementelUpdateType
{
    Speed,
    Size,
    Capacity
}