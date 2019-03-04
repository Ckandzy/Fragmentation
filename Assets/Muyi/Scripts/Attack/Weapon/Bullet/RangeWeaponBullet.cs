using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponBullet : IBullet {

    //public GameObject BlowoutObj;

    public void OnHit(TakeDamager takeDamagerBase, TakeDamageable damageable)
    {
        //GameObject vfx = Instantiate(Resources.Load<GameObject>("VFX/Blowout/PistolBulletHit"));
        //vfx.transform.position = transform.position;
        //vfx.GetComponent<Animation>().Play();
        //Destroy(this.gameObject);
        //gameObject.SetActive(false);
       // GameObject obj = MonoBehaviour.Instantiate(BlowoutObj);
        //obj.transform.position = transform.position;
    }


}


