using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IBullet : MonoBehaviour {
    public float LiveTime;
    public float Speed;
    public Vector2 DirVec;
    public bool NotInVisibleDestroy = false;

    bool inVisible = true;

    float timer = 0;
    private void Update()
    {
        // 间隔时间
        timer += Time.deltaTime;
        // 毁灭
        if (!inVisible) Destroy();
        if (timer > LiveTime) Destroy();
        // 移动
        transform.position += (Vector3)DirVec * Speed * Time.deltaTime;
        Speed += Time.deltaTime * 30;
    }

    protected void LookAt(Transform enemy)
    {
        Vector3 dir = enemy.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void Set(float livetime, float speed, Vector2 dir, IBuff buff = null)
    {
        LiveTime = livetime;
        Speed = speed;
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
}


