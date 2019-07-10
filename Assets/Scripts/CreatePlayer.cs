using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatePlayer : MonoBehaviour
{
    Movement CurrentSet;
    public InputField[] Keys;
    CreateGame Creator;

    private void Awake()
    {
        Creator = GameObject.Find("Creator").GetComponent<CreateGame>();
        CurrentSet = new Movement();
        CurrentSet.MovementSpeed = 10;
        CurrentSet.DashSpeed = 30;
        CurrentSet.DashDruation = 0.5f;
        CurrentSet.JumpForce = 15;
        CurrentSet.SlowDown = 60;
        CurrentSet.Acceleration = 60;
        CurrentSet.GravityScale = 30;
        CurrentSet.Mask = LayerMask.NameToLayer("Ground");
        CurrentSet.CharacterMask = LayerMask.NameToLayer("Character");
    }

    public void Next()
    {
        for (int i = 0; i < 6; i++)
        {
            CurrentSet.Arrows[i] = Keys[i].text;
        }
        Creator.Set.Add(CurrentSet);
    }

}
