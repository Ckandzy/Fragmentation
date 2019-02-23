using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackBullet : MonoBehaviour {
    public float StartSpeed = 10;
    public float Gravity = -10;
    public float Angle = 45;//发射的角度

    public Vector3 MoveSpeed = Vector2.zero;
    public Vector3 GravitySpeed = Vector2.zero;

    public string FindTagGameObj;

    private void Start()
    {
        Debug.Log(Quaternion.Euler(new Vector3(0, 0, Angle)));
        MoveSpeed = Quaternion.Euler(new Vector3(0, 0, Angle)) * Vector2.right * StartSpeed;
    }

    private void Update()
    {
        FindPath();
    }

    private Transform FindTagTransform(string _s)
    {
        return GameObject.FindGameObjectWithTag(_s).transform;
    }

    float timer = 0;
    float fallDownTime = 2;
    float riseUpTime = 5;
    private void FindPath()
    {
        
        if((timer += Time.deltaTime) <= fallDownTime)
        {
            //GravitySpeed.y = Gravity * timer; // v = at
            // 下降阶段
        }
        else if (timer > riseUpTime)
        {
            // 追踪阶段
            return;
        }
        else
        {
            if(Gravity < 0) Gravity = -1 * Gravity;
            MoveSpeed.x -= Time.deltaTime * 1;
            //GravitySpeed.y = Gravity * timer;// * (timer += Time.deltaTime); // v = at
            // 上升阶段
        }

        // timer += Time.deltaTime;
        GravitySpeed.y = Gravity; // * timer;
        transform.position += (GravitySpeed + MoveSpeed) * Time.fixedDeltaTime;
        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan((MoveSpeed.y + GravitySpeed.y) / MoveSpeed.x) * Mathf.Rad2Deg);
    }
}


