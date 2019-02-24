using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Test : MonoBehaviour
{
    Rigidbody2D rig;
    float Speep = 20;
    bool isGround = true;
    public GameObject bullet;
    Vector2 dir = Vector2.right;

    float attacktime = 0.3f;
    float d_time = 0;
    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        if(x != 0 || y != 0) dir = new Vector2(x,y);
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rig.velocity = new Vector2(rig.velocity.x, 10);
            isGround = false;
        }
        if (Input.GetKeyDown(KeyCode.J) && d_time > attacktime)
        {
            d_time = 0;
            GameObject obj = Instantiate(bullet);
            obj.transform.position = transform.position;
            obj.GetComponent<Bullet>().right = dir;
            if (x < 0) obj.GetComponent<SpriteRenderer>().flipX = true ;
        }
        if(d_time < attacktime)
             d_time += Time.deltaTime;
        Move(x);
    }

    private void Move(float x)
    {
        rig.velocity = new Vector2(x * 5, rig.velocity.y);
    }

    void Log(string _s)
    {
        Debug.Log(_s);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Platform")
        {
            isGround = true;
        }
    }
}

