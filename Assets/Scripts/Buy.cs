using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Buy : MonoBehaviour
{
    [SerializeField] private GameObject incrementalsPanel;
    private Tween signTween;


    //Button Elements


    private void OnTriggerEnter(Collider other)
    {
        signTween = transform.GetChild(1).DOScale(new Vector3(0.1f, 0.2f, 0.1f), 1.75f)
            .OnKill(() => { transform.GetChild(1).DOScale(UnityEngine.Vector3.zero, 0.6f); })
            .OnComplete((() =>
            {
                incrementalsPanel.SetActive(true);
                StartCoroutine(PanelAnim());
            }));
    }


    private void OnTriggerExit(Collider other)
    {
        signTween.Kill();

        for (int i = 0; i < 3; i++)
        {
            incrementalsPanel.transform.GetChild(i).GetComponent<RectTransform>().DOScale(Vector3.zero, 0.5f);
        }

        count = 0;
        incrementalsPanel.SetActive(false);
    }


    private int count = 0;

    IEnumerator PanelAnim()
    {
        incrementalsPanel.transform.GetChild(count).GetComponent<RectTransform>().DOScale(Vector3.one, 0.5f);
        count++;
        yield return new WaitForSeconds(0.3f);
        if (count < incrementalsPanel.transform.childCount)
        {
            StartCoroutine(PanelAnim());
        }
    }

    public void BuyNewCar()
    {
        PlayerPrefs.SetInt("MaxSize", 0);
        PlayerPrefs.SetFloat("Speed", 0);
        PlayerPrefs.SetFloat("Scale", 0);
    }
}