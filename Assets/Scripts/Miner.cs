using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = Unity.Mathematics.Random;


//withRigidBody
// public class Miner : MonoBehaviour
// {
//     private Character character;
//
//
//     public GameObject cube;
//     float speed;
//     [SerializeField] private Transform baseMine;
//
//     private void Start()
//     {
//         character = transform.root.GetComponent<Character>();
//         speed = character.speed;
//     }
//
//
//     // private void OnTriggerEnter(Collider other)
//     // {
//     //     if (other.transform.root.CompareTag("Mine"))
//     //     {
//     //         // other.gameObject.SetActive(false);
//     //         // Transform minePart = Instantiate(cube, other.transform.position, Quaternion.identity).transform;
//     //         // minePart.name = other.GetComponent<MeshRenderer>().material.name;
//     //         // minePart.SetParent(baseMine);
//     //
//     //
//     //         other.transform.SetParent(baseMine);
//     //         Vector3 randomPos = (UnityEngine.Random.insideUnitSphere * 5f + other.transform.position);
//     //         StartCoroutine(SlowSpeed());
//     //         StartCoroutine(MoveToFloor(other.transform, randomPos));
//     //     }
//     // }
//
//     private void OnCollisionEnter(Collision other)
//     {
//         Debug.Log("in heree");
//         if (other.transform.root.CompareTag("Mine"))
//         {
//             // other.gameObject.SetActive(false);
//             // Transform minePart = Instantiate(cube, other.transform.position, Quaternion.identity).transform;
//             // minePart.name = other.GetComponent<MeshRenderer>().material.name;
//             // minePart.SetParent(baseMine);
//
//
//             other.transform.SetParent(baseMine);
//             Vector3 randomPos = (UnityEngine.Random.insideUnitSphere * 5f + other.transform.position);
//             StartCoroutine(SlowSpeed());
//             StartCoroutine(MoveToFloor(other.transform, randomPos));
//         }
//     }
//
//     IEnumerator SlowSpeed()
//     {
//         character.speed = speed * 0.8f;
//         yield return new WaitForSeconds(0.2f);
//         character.speed = speed;
//     }
//
//     // IEnumerator MoveToFloor(Transform minePart,Transform parentPos)
//     // {
//     //     
//     //     Vector3 randomPos = (UnityEngine.Random.insideUnitSphere * 5f + parentPos.position);
//     //
//     //     float i = 0f;
//     //     while (i <= 1)
//     //     {
//     //         i += Time.deltaTime * 0.4f;
//     //         minePart.position = Vector3.Lerp(minePart.position,
//     //             new Vector3(randomPos.x, -6f, randomPos.z), i);
//     //
//     //         if (Vector3.Distance(minePart.position,
//     //                 new Vector3(randomPos.x, -6f, randomPos.z)) <= 0.2f)
//     //         {
//     //             break;
//     //         }
//     //
//     //         yield return null;
//     //     }
//     // }
//
//     IEnumerator MoveToFloor(Transform minePart, Vector3 parentPos)
//     {
//         //  Vector3 randomPos = (UnityEngine.Random.insideUnitSphere * 5f + parentPos);
//
//         // float i = 0f;
//         // while (i <= 1)
//         // {
//         //     i += Time.deltaTime * 0.4f;
//         //     minePart.position = Vector3.Lerp(minePart.position,
//         //         new Vector3(randomPos.x, -6f, randomPos.z), i);
//         //
//         //     if (Vector3.Distance(minePart.position,
//         //             new Vector3(randomPos.x, -6f, randomPos.z)) <= 0.2f)
//         //     {
//         //         break;
//         //     }
//         //
//         //     yield return null;
//         // 
//
//         // minePart.DOMove(new Vector3(parentPos.x, minePart.position.y, parentPos.z), 1f).SetEase(Ease.InSine);
//         minePart.AddComponent<Rigidbody>();
//         minePart.GetComponent<Rigidbody>().mass = 10f;
//         yield return null;
//     }
// }


// withoutrigidbody
public class Miner : MonoBehaviour
{
    private Character character;


    public TextMeshProUGUI moneyText;
    private RaycastHit hit;
    float speed;
    [SerializeField] private Transform baseMine;

    private void Start()
    {
        moneyText = GameObject.Find("MoneyText").GetComponent<TextMeshProUGUI>();
        character = transform.root.GetComponent<Character>();
        speed = character.speed;
        baseMine = GameObject.Find("BaseMine").transform;
    }

    private Vector3 firstPos;
    private Vector3 secondPos;

    float speedX
    {
        get { return Vector3.Distance(firstPos, secondPos) / Time.deltaTime; }
    }

    private void Update()
    {
        firstPos = transform.position;
    }

    private void LateUpdate()
    {
        secondPos = transform.position;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.parent.CompareTag("Mine") || other.transform.parent.CompareTag("Next"))
        {
            //other.gameObject.SetActive(false);
            other.transform.SetParent(baseMine);
            StartCoroutine(SlowSpeed());
            StartCoroutine(MoveToFloor(other.transform, other.transform.position));
        }

        if (other.transform.parent.name == "Moneys")
        {
            int money = int.Parse(moneyText.text) + int.Parse(other.transform.name);
            moneyText.text = (money).ToString();
            PlayerPrefs.SetInt("Money", int.Parse(moneyText.text));
            other.transform.gameObject.SetActive(false);
        }
    }

    private Vector3 myPos;
    private Vector3 colPos;

    private void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.transform.root.name == "BaseMine" &&
            character.trailerMaxSize <= transform.root.GetChild(2).childCount)
        {
            myPos = transform.position;
            myPos.y = 0f;

            colPos = collisionInfo.transform.position;
            colPos.y = 0f;

            collisionInfo.transform.position += (colPos - myPos).normalized * character.speed * Time.deltaTime;
        }
    }


    IEnumerator SlowSpeed()
    {
        character.speed = speed * 0.8f;
        yield return new WaitForSeconds(0.2f);
        character.speed = speed;
    }

    IEnumerator MoveToFloor(Transform minePart, Vector3 parentPos)
    {
        Vector3 randomPos = (UnityEngine.Random.insideUnitSphere * 5f + parentPos);

        minePart.DOMove(new Vector3(randomPos.x, minePart.position.y, randomPos.z), 1f).SetEase(Ease.OutQuint);
        minePart.DORotate(
            new Vector3(UnityEngine.Random.Range(0, 180), UnityEngine.Random.Range(0, 180),
                UnityEngine.Random.Range(0, 180)), 1f).SetEase(Ease.InOutBounce);
        yield return null;
    }
}