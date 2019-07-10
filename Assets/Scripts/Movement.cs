using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public string[] Arrows;
    /// <summary>
    /// 1 Right
    /// 2 Left
    /// 3 Down
    /// 4 Up
    /// 5 Jump
    /// 6 Block
    /// </summary>

    bool[] Taped = new bool[4];
    float[] Times = new float[4];
    float[] Dashes = new float[4];
    float Dash;

    public float MovementSpeed = 1.0f;
    public float DashSpeed = 1.0f;
    public float DashDruation = 1.0f;
    public float JumpForce = 20.0f;

    public float Acceleration = 1;
    float dx;
    float dy;
    float dz;
    public float SlowDown = 1;
    Rigidbody RB;

    public bool Ground = false;
    public float GravityScale = 10;
    public LayerMask Mask;
    public LayerMask CharacterMask;
    // Animator Anim;

    GameObject Target;
    bool Block;

    private void Start()
    {
        RB = GetComponent<Rigidbody>();
        float Distance = float.PositiveInfinity;
        Vector3 Record = transform.position;
        if(Physics.OverlapSphere(transform.position, 100, Mask).Length >= 1)
        {
            foreach(Collider k in Physics.OverlapSphere(transform.position, 100, Mask))
            {
                if(Vector3.Distance(transform.position, k.transform.position) < Distance && k.transform.position.y<=0.1f)
                {
                    Distance = Vector3.Distance(transform.position, k.transform.position);
                    Record = k.transform.position;
                }
            }
        }
        transform.position = new Vector3(Record.x, Record.y + 2, Record.z);
       // Anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKey(Arrows[0]))
        {
            if (dz < MovementSpeed)
            {
               dz += Acceleration * Time.deltaTime;
            }
            else
            {
                dz = MovementSpeed;
            }
            dz += Dashes[0];
        }
        else if (Input.GetKey(Arrows[1]))
        {
            if (dz > -MovementSpeed)
            {
                dz -= Acceleration * Time.deltaTime;
            }
            else
            {
                dz = -MovementSpeed;
            }
            dz -= Dashes[1];
        }
        else
        {
            if (Mathf.Abs(dz) > 1.0f)
            { dz -= Mathf.Sign(dz) * SlowDown * Time.deltaTime; }
            else { dz = 0; }
        }

        if (Input.GetKey(Arrows[2]))
        {
            if (dx > -MovementSpeed)
            {
                dx -= Acceleration * Time.deltaTime;
            }
            else
            {
                dx = -MovementSpeed;
            }
            dx -= Dashes[2];
        }
        else if (Input.GetKey(Arrows[3]))
        {
            if (dx < MovementSpeed)
            {
                dx += Acceleration * Time.deltaTime;
            }
            else
            {
                dx = MovementSpeed;
            }
            dx += Dashes[3];
        }
        else
        {
            if (Mathf.Abs(dx) > 1f)
            { dx -= Mathf.Sign(dx) * SlowDown * Time.deltaTime; }
            else { dx = 0; }
        }


        //skakanie
        if (Ground)
        {
            dy = 0;
            if(Input.GetKey(Arrows[4]))
            {
                dy = JumpForce;
               // Anim.SetBool("Jump", true);
            }
        }
        else { dy -= GravityScale * Time.deltaTime; }


        //Dashe
        Dash = 0;
        for (int i=0; i<4; i++)
        {
            if (Input.GetKeyDown(Arrows[i]))
            {
                if (Taped[i])
                {
                    Dashes[i] = DashSpeed;
                    Taped[i] = false;
                    Times[i] = 0.0f;
                }
                else
                {
                    Taped[i] = true;
                    Times[i] = 0.3f;
                }
            }
            if (Dashes[i] > Dash) { Dash = Dashes[i]; }
            //Anim.SetFloat("Dash", Dash);
            Times[i] -= Time.deltaTime;
            if(Times[i]<0.0f) { Taped[i] = false; }
            if(Dashes[i]>0.0f) { Dashes[i] -= (Time.deltaTime*DashSpeed*(1.0f/DashDruation)); }
            else { Dashes[i] = 0.0f; }
        }

        //Poruszanie się
        if (Block) { RB.velocity = new Vector3(dx, dy, dz)*0.1f; }
        else { RB.velocity = new Vector3(dx, dy, dz); }

        if (SlowDown < 60) { SlowDown += SlowDown * Time.deltaTime; } 
        else { SlowDown = 60; }


        //Walka
        RaycastHit Info;
        if (Physics.SphereCast(transform.position + new Vector3(0.0f, 0.85f, 0.0f), 0.85f, new Vector3(dx, dy, dz), out Info, 1.0f, CharacterMask))
        {
            Target = Info.collider.gameObject;
        }

        if (Input.GetKey(Arrows[5])) { Block = true; }
        else { Block = false; }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Ground")
        {
            Ground = true;
            transform.parent = collision.gameObject.transform;
            //Anim.SetBool("Jump", false);
        }
        else if (collision.collider.tag == "Finish")
        {
            Destroy(this.gameObject);
        }
        else if (collision.collider.tag == "Player")
        {
            if (collision.collider.gameObject == Target)
            {
                collision.collider.gameObject.SendMessage("Hit", new Vector3(dx, dy, dz) * (1+Dash/DashSpeed));
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Ground")
        {
            Ground = false;
            transform.parent = collision.transform.parent;
        }
    }

    private void Hit(Vector3 attack)
    {
        Debug.Log("Hit");
        if (Block)
        {
            dx += attack.x*0.1f;
            dz += attack.z*0.1f;
        }
        else
        {
            dx += attack.x;
            dz += attack.z;
            SlowDown *= 0.85f;
        }
    }
}
