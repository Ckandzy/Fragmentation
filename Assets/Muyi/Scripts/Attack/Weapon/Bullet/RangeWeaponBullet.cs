using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponBullet : IBullet {
 
    public void OnHit(TakeDamager takeDamagerBase, TakeDamageable damageable)
    {
        if(GetComponent<Animator>())
            GetComponent<Animator>().SetTrigger("OnHit");
        StartCoroutine(DestroyDelay(0));
    }
}


