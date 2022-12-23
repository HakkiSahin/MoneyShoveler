using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class Sell : MonoBehaviour
{
    private bool sell = false;

    private Transform trailer;

    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] MinesScriptable mines;

    public GameObject moneyObj;
    public Transform spawnPoint;
    public Transform jumpPoint;
    public Transform stackArea;

    private Vector3 trans;

    private void Start()
    {
        moneyText.text = PlayerPrefs.GetInt("Money").ToString();
        trans = stackArea.GetChild(0).transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.root.GetComponent<Character>())
        {
            trailer = other.transform.root.GetChild(2);
            if (trailer.childCount <= 1)
            {
                sell = false;
            }
            else if (trailer.childCount > 1 && !sell)
            {
                sell = true;
                GetMoveMoney();
            }
        }
    }

    void GetMoveMoney()
    {
        StartCoroutine(GetMoney());
    }


    IEnumerator GetMoney()
    {
        Transform trans = trailer.GetChild(trailer.childCount - 1);
        trans.SetParent(null);

        trans.DOJump(transform.GetChild(0).position, 2f, 1, 0.2f).OnComplete(() =>
        {
            AddMoneyByMineType(trans.name);
            Destroy(trans.gameObject);
        });


        yield return new WaitForSeconds(0.04f);
        if (trailer.childCount > 1)
            StartCoroutine(GetMoney());
        else
        {
            sell = false;
            trailer.root.GetChild(3).GetComponent<Trailer>().EmptyTheMatris();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Character>())
        {
            sell = false;
        }
    }


    void AddMoneyByMineType(string mineType)
    {
        for (int i = 0; i < mines.minesList.Count; i++)
        {
            if (mineType.Contains(mines.minesList[i].mineName))
            {
                // moneyText.text = (int.Parse(moneyText.text) + mines.minesList[i].mineValue).ToString();
                // PlayerPrefs.SetInt("Money", int.Parse(moneyText.text));
                MakeMoney(mines.minesList[i].mineValue);
                break;
            }
        }
    }

    private float x = 0;
    private float y = 0;
    private float z = 0;

    void MakeMoney(float moneyValue)
    {
        GameObject money = Instantiate(moneyObj, spawnPoint.position, Quaternion.identity);
        money.transform.eulerAngles = new Vector3(90, 0, -90);
        money.name = moneyValue.ToString();


        money.transform.DOMove(jumpPoint.position, 0.4f).OnComplete(() =>
        {
            for (int i = 1; i < stackArea.childCount; i++)
            {
                if (!stackArea.GetChild(i).gameObject.activeSelf)
                {
                    money.transform.DOJump(
                        stackArea.GetChild(i).transform.position, 2f, 1, 0.3f).OnComplete(() =>
                    {
                        stackArea.GetChild(i).gameObject.SetActive(true);
                        stackArea.GetChild(i).name = moneyValue.ToString();
                        Destroy(money);
                    });

                    return;
                }
            }


            if ((stackArea.childCount - 1) % 5 == 0)
            {
                money.transform.DOJump(
                    trans, 2f, 1, 0.3f);
                trans.x -= money.transform.localScale.x * 0.44f;
                trans.z = stackArea.GetChild(0).transform.position.z;
            }

            if ((stackArea.childCount - 1) % 30 == 0)
            {
                trans.y += money.transform.localScale.y * 0.15f;
                trans.z = stackArea.GetChild(0).transform.position.z;
                trans.x = stackArea.GetChild(0).transform.position.x;
            }

            money.transform.DOJump(
                trans, 2f, 1, 0.3f);

            trans.z -= money.transform.localScale.z / 1.3f;

            money.transform.SetParent(stackArea);
        });
    }
}