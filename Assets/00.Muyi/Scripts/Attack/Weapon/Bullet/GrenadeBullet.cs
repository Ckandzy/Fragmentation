using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 手榴弹
/// </summary>
public class GrenadeBullet : MonoBehaviour {
    public float Power = 10;//发射的速度
    public float Angle = 45;//发射的角度
    public float Gravity = -10;//这个代表重力加速度
    public bool IsOne = false;


    private Vector3 MoveSpeed;//初速度向量
    private Vector3 GritySpeed = Vector3.zero;//重力的速度向量，t时为0
    private float dTime;//已经过去的时间

    private Vector3 currentAngle;
   
    void Start()
    {
        // 初始速度
        MoveSpeed = Quaternion.Euler(new Vector3(0, 0, Angle)) * Vector3.right * Power;
        currentAngle = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GritySpeed.y = Gravity * (dTime += Time.fixedDeltaTime); // v = at
        //位移模拟轨迹
        transform.position += (MoveSpeed + GritySpeed) * Time.fixedDeltaTime;
        // 求旋转度数 v.y / v.x -- 用弧度转角
        // ------
        // |   
        // |
        currentAngle.z = Mathf.Atan((MoveSpeed.y + GritySpeed.y) / MoveSpeed.x) * Mathf.Rad2Deg;
        //currentAngle.z = Mathf.Tan((MoveSpeed.y + GritySpeed.y) / MoveSpeed.x);
        transform.eulerAngles = currentAngle;
    }
}


