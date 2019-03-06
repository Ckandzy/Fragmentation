using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IBullet : MonoBehaviour {
    public float LiveTime;
    public float Speed;
    public Vector2 DirVec;
    public bool NotInVisibleDestroy = false;
    public APoolBullet bulletPoolObject;
    public float AddSpeed = 30;
    public float timer = 0;
    private void Update()
    {
        // 间隔时间
        timer += Time.deltaTime;
        // 毁灭
        if (!IsVisible() || timer >= LiveTime) StartCoroutine(DestroyDelay(0));
        // 移动
        transform.position += (Vector3)DirVec.normalized * (Speed + AddSpeed * timer * Time.deltaTime * AddSpeed) * Time.deltaTime;
    }

    protected void LookAt(Transform enemy)
    {
        Vector3 dir = enemy.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void Set(float livetime, float speed, Vector2 dir, List<IBuff> buff = null)
    {
        LiveTime = livetime;
        Speed = speed;
        DirVec = dir;
        //GetComponent<SpriteRenderer>().flipX = (dir.x == -1);
        if (dir.x != 0)
            transform.localScale = new Vector2(dir.x / Mathf.Abs(dir.x), transform.localScale.y);
        timer = 0;
    }

    public bool IsVisible()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(this.transform.position);
        if (pos.x < 0 || pos.x > 1 || pos.y < 0 || pos.y > 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    protected IEnumerator DestroyDelay(float time)
    {
        yield return new WaitForSeconds(time);
        bulletPoolObject.ReturnToPool();
    }
}


