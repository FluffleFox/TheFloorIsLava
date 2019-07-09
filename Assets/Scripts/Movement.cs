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
    /// </summary>

    bool[] Taped = new bool[4];
    float[] Times = new float[4];
    float[] Dashes = new float[4];
    float Dash;

    public float MovementSpeed = 1.0f;
    public float DashSpeed = 1.0f;
    public float DashDruation = 1.0f;
    public float JumpForce = 20.0f;

    //Vector3 Translation;
    float dx;
    float dy;
    float dz;
    public float SlowDown = 1;
    Rigidbody RB;

    public bool Ground = false;
    public float GravityScale = 10;
    public LayerMask Mask;
   // Animator Anim;

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
            // Translation += new Vector3(0, 0, 1+Dashes[0]) * MovementSpeed;
            dz = (1 + Dashes[0]) * MovementSpeed;
        }
        else if (Input.GetKey(Arrows[1]))
        {
            //Translation += new Vector3(0, 0, -1 - Dashes[1]) * MovementSpeed;
            dz = (-1 - Dashes[1]) * MovementSpeed;
        }
        else
        {
            if (Mathf.Abs(dz) > 1f)
            { dz -= Mathf.Sign(dz) * SlowDown * Time.deltaTime; }
            else { dz = 0; }
        }


        if (Input.GetKey(Arrows[2]))
        {
            // Translation += new Vector3(-1 - Dashes[2], 0, 0) * MovementSpeed;
            dx = (-1 - Dashes[2]) * MovementSpeed;
        }
        else if (Input.GetKey(Arrows[3]))
        {
            //Translation += new Vector3(1 + Dashes[3], 0, 0) * MovementSpeed;
            dx = (1 + Dashes[3]) * MovementSpeed;
        }
        //else { dx = 0; }
        else
        {
            if (Mathf.Abs(dx) > 1f)
            { dx -= Mathf.Sign(dx) * SlowDown * Time.deltaTime; }
            else { dx = 0; }
        }

        if (Ground)
        {
            //RB.AddForce(new Vector3(0.0f, 1.0f, 0.0f)*JumpForce, ForceMode.Impulse);
            dy = 0;
            if(Input.GetKey(Arrows[4]))
            {
                dy = JumpForce;
               // Anim.SetBool("Jump", true);
            }
        }
        else { dy -= GravityScale * Time.deltaTime; }

        for (int i=0; i<4; i++)
        {
            if (Input.GetKeyDown(Arrows[i]))
            {
                Dash = 0;
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
                if (Dashes[i] >= Dash) { Dash = Dashes[i]; }
            }
            //Anim.SetFloat("Dash", Dash);
            Times[i] -= Time.deltaTime;
            if(Times[i]<0.0f) { Taped[i] = false; }
            if(Dashes[i]>=0.0f) { Dashes[i] -= (Time.deltaTime*DashSpeed*(1.0f/DashDruation)); }
            else { Dashes[i] = 0.0f; }
        }

        

        //transform.Translate(Translation * Time.deltaTime, Space.World);
        //RB.AddForce(Translation, ForceMode.Force);

        RB.velocity = new Vector3(dx,dy,dz);
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
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Ground")
        {
            Ground = false;
            transform.parent = collision.transform.parent;
        }
    }
}
