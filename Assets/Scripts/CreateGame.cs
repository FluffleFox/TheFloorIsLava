using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateGame : MonoBehaviour
{
    public GameObject Prefab;
    public List<GameObject> Models;
    public List<Movement> Set;
    public float Radius;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            for(int i=0; i<Models.Count; i++)
            {
                GameObject GO = (GameObject)Instantiate(Prefab, new Vector3(Radius * Mathf.Sin(((360.0f / Models.Count) * i) * Mathf.Deg2Rad), 5.0f, Radius * Mathf.Cos(((360.0f / Models.Count) * i) * Mathf.Deg2Rad)), Quaternion.identity);
                for (int j = 0; j < 6; j++)
                { GO.GetComponent<Movement>().Arrows[j] = Set[i].Arrows[j]; }
            }
        }
    }
}
