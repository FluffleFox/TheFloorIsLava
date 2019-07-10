 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Menu : MonoBehaviour
{
    public InputField[] Keys;
    public GameObject Player;
    GameObject Last;
    CreateGame Creator;

    private void Awake()
    {
        Creator = GameObject.Find("Creator").GetComponent<CreateGame>();
    }


    public void Play1()
    {
        transform.rotation *= Quaternion.Euler(0, 180, 0);
    }

    public void Play2()
    {
        SceneManager.LoadScene(1);
    }

    public void Back()
    {
        transform.rotation *= Quaternion.Euler(0, 180, 0);
    }

    public void Next()
    {
        Creator.Models.Add(Last);
        Destroy(Last);
        Last = (GameObject)Instantiate(Player, new Vector3(-2.65f, 3.095183f, -4.088077f), Quaternion.Euler(new Vector3(-152.095f, -0.4190063f, 90.10799f)));
        Last.transform.localScale = Vector3.one * 90.0f;

        for (int i = 0; i < 6; i++)
        {
            Keys[i].text = null;
        }
    }
}
