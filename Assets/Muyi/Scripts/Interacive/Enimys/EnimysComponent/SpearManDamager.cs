using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearManDamager : TakeDamager
{
    private void Update()
    {
        
    }

    // 改为射线检测, 需要获取人物当前的状态，扇形检测
   

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<DamageAble>())
        {
            //collision.transform.GetComponent<DamageAble>().WasDamage(2, BuffFactory.GetBuff(2));
        }
    }
}


