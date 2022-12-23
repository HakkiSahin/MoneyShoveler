using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController _instance;
    private GameObject[] mines;
    public Transform levelWay;
    public Character car;
    private Transform baseMine;
    private bool isWork;
    private Transform activeLevel;
    public Transform levels;
    public List<GameObject> cars;


    private void Awake()
    {
        _instance = this;

        PlayerPrefs.SetInt("Car", PlayerPrefs.GetInt("Car") >= 0 ? PlayerPrefs.GetInt("Car") : 0);

        GetActiveCar(PlayerPrefs.GetInt("Car"));
    }


    public void GetActiveCar(int activeCarIndex)
    {
        Vector3 newPos = Vector3.zero;
        GameObject obj = GameObject.FindWithTag("Car");
        if (obj != null)
        {
            newPos = obj.transform.position;
            Destroy(obj);
        }

        Instantiate(cars[activeCarIndex], newPos,
            quaternion.identity);
        LevelStart();
    }

    void LevelStart()
    {
        isWork = false;
        car = GameObject.FindWithTag("Car").GetComponent<Character>();

        mines = GameObject.FindGameObjectsWithTag("Mine");
        baseMine = GameObject.Find("BaseMine").transform;
        count = 0;
        activeLevel = levels.GetChild(PlayerPrefs.GetInt("Level") >= 0 ? PlayerPrefs.GetInt("Level") : 0);
        levelWay = activeLevel.GetChild(3);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < mines.Length; i++)
        {
            if (mines[i].transform.childCount > 0)
            {
                return;
            }
        }


        activeLevel.GetChild(4).gameObject.SetActive(true);

        if (activeLevel.GetChild(4).childCount <= 5 && !isWork)
        {
            StartCoroutine(WayCreate());
        }
    }

    private int count = 0;

    IEnumerator WayCreate()
    {
        isWork = true;
        levelWay.GetChild(count).DOLocalMoveY(0, 0.3f);

        count++;

        yield return new WaitForSeconds(0.05f);
        if (count < levelWay.childCount)
        {
            StartCoroutine(WayCreate());
        }
        else
        {
            car.MoveToWay(levelWay);
        }
    }
}