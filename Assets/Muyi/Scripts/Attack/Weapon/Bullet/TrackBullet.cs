using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackBullet : MonoBehaviour {
    public float StartSpeed = 10;
    public float Gravity = -10;
    public float Angle = 45;//发射的角度

    protected Vector3 MoveSpeed = Vector2.zero;
    protected Vector3 GravitySpeed = Vector2.zero;

    public string FindGameObjTag = "Player";

    Transform target;
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag(FindGameObjTag).transform;
        //Debug.Log(target);
        MoveSpeed = Quaternion.Euler(new Vector3(0, 0, Angle)) * Vector2.right * StartSpeed;
    }

    private void Update()
    {
        NonTargetFindPath();
    }

    public void HasTargetFindPath()
    {

        transform.position += (target.position - transform.position).normalized * MoveSpeed.x * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan(MoveSpeed.y / MoveSpeed.x) * Mathf.Rad2Deg);
    }

    float timer = 0;
    float fallDownTime = 2;
    float riseUpTime = 4;
    public void NonTargetFindPath()
    {
        //if(MoveSpeed.)
        
        if ((timer += Time.deltaTime) <= fallDownTime)
        {
            // 下降阶段
        }
        else if (timer > riseUpTime)
        {
            // 追踪阶段
            if (target != null)
            {
                Debug.Log("进入追踪阶段");
                HasTargetFindPath();
                return;
            }
            else
            {
                timer = 0;
            }
        }
        else
        {
            if (Gravity < 0) Gravity = -1 * Gravity;
            //MoveSpeed.x -= Time.deltaTime * 1;
            //GravitySpeed.y = Gravity * timer;// * (timer += Time.deltaTime); // v = at
            // 上升阶段
        }

        GravitySpeed.y = Gravity * timer * Time.deltaTime;
        MoveSpeed += GravitySpeed;
        transform.position += MoveSpeed * Time.fixedDeltaTime;
        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan(MoveSpeed.y / MoveSpeed.x) * Mathf.Rad2Deg);
        /*
        // timer += Time.deltaTime;
        GravitySpeed.y = Gravity; // * timer;
        transform.position += (GravitySpeed + MoveSpeed) * Time.fixedDeltaTime;
        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan((MoveSpeed.y + GravitySpeed.y) / MoveSpeed.x) * Mathf.Rad2Deg);
        */
    }
}


