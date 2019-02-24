using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed = 10;
    float deletetime = 0;

    public Vector3 right;

    private void Update()
    {
        transform.position += speed * right * Time.deltaTime;
        speed += 2 * Time.deltaTime;
        if((deletetime += Time.deltaTime) >= 2)
        {
            Destroy(gameObject);
        }
    }
}

