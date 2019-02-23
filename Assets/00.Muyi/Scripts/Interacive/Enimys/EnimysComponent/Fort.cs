using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fort : MonoBehaviour {
    public GameObject bullet;

    float waitTime = 4;
    float nowWaitTime = 0;

    public void Update()
    {
        if(nowWaitTime >= waitTime)
        {
            nowWaitTime = 0;
            GameObject bullet1 = Instantiate(bullet);
            bullet1.transform.position = transform.position;
            bullet1.GetComponent<RangeWeaponBullet>().Set(3, 10, Vector2.right, BuffFactory.GetBuff(2));
        }

        nowWaitTime += Time.deltaTime;
    }
}


