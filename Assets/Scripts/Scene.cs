using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{
    public int NumberOfObject = 5; //Ilość obiektów zawsze zdatnych do pływania
    public float FlowSpeed = 3;
    public GameObject[] ObiectList;
    List<GameObject> ActiveObject = new List<GameObject>();
    float Highest;
    float DistanceBetween;

    void Awake()
    {
        FlowSpeed *= -1.0f;
        for (int i = 0; i < NumberOfObject; i++)
        {
            float dx = Random.Range(-30.0f, 0.0f);
            GameObject GO;
            if (dx < -15.0f)
            {
                GO = (GameObject)Instantiate(ObiectList[Random.Range(0, ObiectList.Length)], new Vector3(dx, 0.0f, Random.Range(-30.0f - dx, 30.0f+dx)), Quaternion.Euler(Random.Range(-90.0f, 90.0f), Random.Range(-90.0f, 90.0f), Random.Range(-90.0f, 90.0f)));
            }
            else
            {
                GO = (GameObject)Instantiate(ObiectList[Random.Range(0, ObiectList.Length)], new Vector3(dx, 0.0f, Random.Range(dx, -dx)), Quaternion.Euler(Random.Range(-90.0f, 90.0f), Random.Range(-90.0f, 90.0f), Random.Range(-90.0f, 90.0f)));
            }
            ActiveObject.Add(GO);
            GO.GetComponent<FlowObject>().Translation = (GO.transform.position.normalized);
            GO.transform.parent = this.transform;
        }

        DistanceBetween = 18.0f / (NumberOfObject*1.1213f);
        for(int i=1; i<Mathf.CeilToInt(1.1213f*NumberOfObject)+1; i++)
        {
            GameObject GO = (GameObject)Instantiate(ObiectList[Random.Range(0, ObiectList.Length)], new Vector3(-2.5f, i*DistanceBetween, Random.Range(-2.0f, 2.0f)), Quaternion.Euler(Random.Range(-90.0f, 90.0f), Random.Range(-90.0f, 90.0f), Random.Range(-90.0f, 90.0f)));
            ActiveObject.Add(GO);
            GO.GetComponent<FlowObject>().Translation = new Vector3(0.0f, FlowSpeed, 0.0f);
            GO.transform.parent = this.transform;
        }
    }

    void Update()
    {
        float tmpHight = 0;
        foreach (GameObject k in ActiveObject)
        {
            if (Mathf.Abs(k.transform.position.x) + Mathf.Abs(k.transform.position.z) > 30f)
            {
                k.GetComponent<FlowObject>().Translation = new Vector3(0, FlowSpeed, 0);
                if (k.transform.position.y < -2.0f && Highest<(18.0f-DistanceBetween))
                {
                    k.transform.position = new Vector3(-2.5f, 18.0f, Random.Range(-2.0f,2.0f));
                    k.transform.rotation = Quaternion.Euler(Random.Range(-90.0f, 90.0f), Random.Range(-90.0f, 90.0f), Random.Range(-90.0f, 90.0f));
                    Highest = 18.0f;
                }
            }

            if (k.transform.position.y > 0.0f && k.transform.position.y < 0.05f*Mathf.Abs(FlowSpeed))
            {
                k.transform.position = new Vector3(-2.5f, 0.0f, k.transform.position.z);
                float dx = k.transform.position.z / 4.0f;
                k.GetComponent<FlowObject>().Translation = new Vector3(-1.0f + Mathf.Abs(dx), 0.0f, dx).normalized * Mathf.Abs(FlowSpeed);
            }
            if (k.transform.position.y > tmpHight) { tmpHight = k.transform.position.y; }
        }
        Highest = tmpHight;
    }
}
