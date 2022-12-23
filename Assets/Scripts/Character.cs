using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Character : MonoBehaviour
{
    public FloatingJoystick dynamicJoystick;

    public float speed = 2f;
    public float turnSpeed = 2f;
    Transform levels;

    //Case Controller
    public int trailerMaxFloorX = 4;
    public int trailerMaxFloorZ = 4;
    public int trailerMaxSize = 64;

    public List<Transform> wheels;
    Transform spawnPoint;

    public GameObject particleSmoke;

    public void Start()
    {
        Debug.Log(PlayerPrefs.GetInt("Level"));
        levels = GameObject.Find("Levels").transform;
        levels.GetChild(PlayerPrefs.GetInt("Level") >= 0 ? PlayerPrefs.GetInt("Level") : 0).gameObject.SetActive(true);
        dynamicJoystick = GameObject.Find("Floating Joystick").GetComponent<FloatingJoystick>();
        SetUpdates();
        spawnPoint = levels.GetChild(PlayerPrefs.GetInt("Level")).GetChild(2);
        transform.position = spawnPoint.position;
    }

    private void SetUpdates()
    {
        trailerMaxSize = PlayerPrefs.GetInt("MaxSize") > trailerMaxSize
            ? PlayerPrefs.GetInt("MaxSize")
            : trailerMaxSize;

        speed = PlayerPrefs.GetFloat("Speed") > speed ? PlayerPrefs.GetFloat("Speed") : speed;

        transform.GetChild(0).localScale = PlayerPrefs.GetFloat("Scale") > transform.GetChild(0).localScale.y
            ? new Vector3(transform.GetChild(0).localScale.x,
                PlayerPrefs.GetFloat("Scale"), transform.GetChild(0).localScale.z)
            : transform.GetChild(0).localScale;
    }

    // Update is called once per frame
    private bool movetoWay = false;

    void Update()
    {
        if (Input.GetMouseButton(0) && !movetoWay)
        {
            JoystickController();
        }
        else
        {
            particleSmoke.SetActive(false);
        }
    }


    void JoystickController()
    {
        float horizontal = dynamicJoystick.Horizontal;
        float vertical = dynamicJoystick.Vertical;
        if (dynamicJoystick.transform.GetChild(0).gameObject.activeSelf)
        {
            for (int i = 0; i < wheels.Count; i++)
            {
                wheels[i].transform.Rotate(-3f, 0, 0);
            }

            Vector3 addedPos = new Vector3(horizontal * speed * Time.deltaTime, 0, vertical * speed * Time.deltaTime);
            transform.position += addedPos;


            Vector3 direction = Vector3.forward * vertical + Vector3.right * horizontal;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),
                turnSpeed * Time.deltaTime);
            particleSmoke.SetActive(true);
        }
    }

    public void MoveToWay(Transform way)
    {
        movetoWay = true;
        levels.GetChild(PlayerPrefs.GetInt("Level") + 1).gameObject.SetActive(true);
        transform.DOMove(new Vector3(way.GetChild(0).transform.position.x, transform.position.y,
            way.GetChild(0).transform.position.z), 3f).OnComplete(() =>
        {
            transform.DOMove(new Vector3(way.GetChild(way.childCount - 1).transform.position.x, transform.position.y,
                way.GetChild(way.childCount - 1).transform.position.z + 5f), 2f).OnComplete(() =>
            {
                movetoWay = false;
                PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") >= 0 ? PlayerPrefs.GetInt("Level") + 1 : 0);
                levels.GetChild(PlayerPrefs.GetInt("Level") - 1).gameObject.SetActive(false);
            });
        });
    }


    public void UpdateMaxSize(int update)
    {
        trailerMaxSize += update;
        PlayerPrefs.SetInt("MaxSize", trailerMaxSize);
    }

    public void UpdateMaxSpeed(float update)
    {
        speed += update;
        PlayerPrefs.SetFloat("Speed", speed);
    }


    public void UpdateScale(float update)
    {
        transform.GetChild(0).localScale = new Vector3(transform.GetChild(0).localScale.x,
            transform.GetChild(0).localScale.y + update, transform.GetChild(0).localScale.z);
        PlayerPrefs.SetFloat("Scale", transform.GetChild(0).localScale.y);
    }
}