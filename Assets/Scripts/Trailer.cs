using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class Trailer : MonoBehaviour
{
    private Vector3 direction = Vector3.forward;
    [SerializeField] Transform topPoint;
    private Transform trailer;
    private Character character;
    private bool[,,] trailerCase;
    private int Ysize = 0;

    public GameObject cube;
    public MinesScriptable mines;


    private void Start()
    {
        character = transform.root.GetComponent<Character>();
        Ysize = character.trailerMaxSize / (character.trailerMaxFloorX * character.trailerMaxFloorZ) + 1;

        trailer = transform.root.GetChild(2);
        trailerCase = new bool[character.trailerMaxFloorX, character.trailerMaxFloorZ, Ysize];
        EmptyTheMatris();
    }

    public void EmptyTheMatris()
    {
        for (int y = 0; y < Ysize; y++)
        {
            for (int x = 0; x < character.trailerMaxFloorX; x++)
            {
                for (int z = 0; z < character.trailerMaxFloorZ; z++)
                {
                    trailerCase[x, z, y] = false;
                }
            }
        }

        count = 0;
    }


    public List<GameObject> minesParts = new List<GameObject>();

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.transform.root.name == "BaseMine")
    //     {
    //         if (character.trailerMaxSize > count)
    //         {
    //             count++;
    //             Transform minePart = Instantiate(cube, transform.parent.GetChild(1).position, Quaternion.identity)
    //                 .transform;
    //             minePart.name = other.transform.GetComponent<MeshRenderer>().material.name;
    //             Destroy(other.gameObject);
    //
    //             for (int i = 0; i < mines.minesList.Count; i++)
    //             {
    //                 if (minePart.name.Contains(mines.minesList[i].mineName))
    //                 {
    //                     minePart.GetComponent<MeshRenderer>().material = mines.minesList[i].mineMaterial;
    //                     minesParts.Add(minePart.gameObject);
    //                     break;
    //                 }
    //             }
    //
    //             // StartCoroutine(MoveToTrailer(minePart));
    //         }
    //         else
    //         {
    //             Vector3 randomPos = (UnityEngine.Random.insideUnitSphere * 5f + other.transform.position);
    //             other.transform.DOMove(new Vector3(randomPos.x, other.transform.position.y, randomPos.z), 0.2f);
    //         }
    //     }
    // }


    /// <summary>
    ///  One to one
    /// </summary>
    /// <param name="minePart"></param>
    /// <returns></returns>
    // IEnumerator MoveToTrailer(Transform minePart)
    // {
    //     //yield return new WaitForSeconds(0.6f);
    //     for (int y = 0; y < Ysize; y++)
    //     {
    //         for (int x = 0; x < character.trailerMaxFloorX; x++)
    //         {
    //             for (int z = 0; z < character.trailerMaxFloorZ; z++)
    //             {
    //                 
    //                 if (!trailerCase[x, z, y])
    //                 {
    //                     
    //                     
    //                     trailerCase[x, z, y] = true;
    //                     minePart.position = transform.root.GetChild(1).position;
    //                     minePart.SetParent(trailer);
    //                     
    //                   
    //                     
    //                     minePart.DOLocalJump(topPoint.localPosition, 8f,
    //                         1, 0.05f).OnComplete(() =>
    //                     {
    //                         if (minesParts.Count > 1)
    //                         {
    //                             minesParts.RemoveAt(0);
    //                             StartCoroutine(MoveToTrailer(minesParts[0].transform));
    //                         }
    //                         else
    //                         {
    //                             isWork = false;
    //                         }
    //                         
    //                         
    //                         minePart.DOLocalJump(trailer.GetChild(0).localPosition +
    //                                              new Vector3(minePart.localScale.x * (x + 1),
    //                                                  minePart.localScale.y * (y + 1), -minePart.localScale.z * (z + 1)),
    //                             3f,
    //                             1, 0.2f).OnComplete(() => { });
    //                     });
    //                     minePart.localRotation = trailer.GetChild(0).localRotation;
    //                     yield break;
    //                 }
    //             }
    //         }
    //     }
    //
    //     yield return null;
    // }
    private int count = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.name == "BaseMine")
        {
            if (character.trailerMaxSize > count)
            {
                count++;
                Transform minePart = Instantiate(cube, transform.parent.GetChild(1).position, Quaternion.identity)
                    .transform;
                minePart.name = other.transform.GetComponent<MeshRenderer>().material.name;
                Destroy(other.gameObject);

                for (int i = 0; i < mines.minesList.Count; i++)
                {
                    if (minePart.name.Contains(mines.minesList[i].mineName))
                    {
                        minePart.GetComponent<MeshRenderer>().material = mines.minesList[i].mineMaterial;
                        minesParts.Add(minePart.gameObject);
                        break;
                    }
                }

                StartCoroutine(MoveToTrailer(minePart));
            }
        }

       
    }

    /// <summary>
    /// Topluca
    /// </summary>
    /// <param name="minePart"></param>
    /// <returns></returns>
    IEnumerator MoveToTrailer(Transform minePart)
    {
        //yield return new WaitForSeconds(0.6f);
        for (int y = 0; y < Ysize; y++)
        {
            for (int x = 0; x < character.trailerMaxFloorX; x++)
            {
                for (int z = 0; z < character.trailerMaxFloorZ; z++)
                {
                    if (!trailerCase[x, z, y])
                    {
                        trailerCase[x, z, y] = true;
                        minePart.position = transform.root.GetChild(1).position;
                        minePart.SetParent(trailer);
                        minePart.DOLocalJump(topPoint.localPosition, 8f,
                            1, 0.05f).OnComplete(() =>
                        {
                            minePart.DOLocalJump(trailer.GetChild(0).localPosition +
                                                 new Vector3(minePart.localScale.x * (x + 1),
                                                     minePart.localScale.y * (y + 1),
                                                     -minePart.localScale.z * (z + 1)),
                                3f,
                                1, 0.2f).OnComplete(() => { });
                        });
                        minePart.localRotation = trailer.GetChild(0).localRotation;
                        yield break;
                    }
                }
            }
        }

        yield return null;
    }


    private bool isWork;

    private RaycastHit hit;

    private void Update()
    {
        // Debug.DrawRay(transform.root.position, transform.root.forward * 15f, Color.blue);
        // if (Physics.Raycast(transform.root.position, transform.root.forward, out hit, 15f, LayerMask))
        // {
        //     hit.transform.position += hit.transform.position / 3;
        // }
    }
}