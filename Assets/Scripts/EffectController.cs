using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public GameObject particleMoney;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.root.CompareTag("Car") && transform.childCount >= 0)
        {
            particleMoney.SetActive(true);
        }
        else
        {
            particleMoney.SetActive(false);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.root.CompareTag("Car"))
        {
            particleMoney.SetActive(false);
        }
    }
}