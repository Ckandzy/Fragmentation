using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponBullet : TakeDamager {
    public float LiveTime;
    public float Speed;
    public Vector2 DirVec;
   // public IBuff Buff;
    bool inVisible = true;

    float time = 0;
    private void Update()
    {
        // 间隔时间
        time += Time.deltaTime;
        // 毁灭
        if (!inVisible) Destroy();
        if (time > LiveTime) Destroy();
        // 移动
        transform.position += (Vector3)DirVec * Speed * Time.deltaTime;
        Speed += Time.deltaTime * 30;
    }

    public void Set(float livetime, float speed, Vector2 dir, IBuff buff = null)
    {
        LiveTime = livetime;
        Speed = speed;
        Buff = buff;
        DirVec = dir;
        GetComponent<SpriteRenderer>().flipX = (dir.x == -1);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void OnBecameVisible()
    {
        inVisible = true;
    }

    public void OnBecameInvisible()
    {
        inVisible = false;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<TakeDamageable>())
        {
            Debug.Log(collision.transform);
            collision.transform.GetComponent<TakeDamageable>().TakeDamage(this, Buff);
            Destroy(gameObject);
        }
    }

    public int takeDamageNum = 50;
}


