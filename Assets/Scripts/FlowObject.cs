using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowObject : MonoBehaviour
{
    public Vector3 Translation;
    void Update()
    {
        transform.Translate(Translation * Time.deltaTime,Space.World);
    }
}
