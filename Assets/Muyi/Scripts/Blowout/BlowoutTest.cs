using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowoutTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        BlowoutSMB<GameObject>.Init(GetComponent<Animator>(), this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
