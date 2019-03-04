using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXControllerM : MonoBehaviour
{
    private static VFXControllerM _instance;
    public static VFXControllerM Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<VFXControllerM>();
            return _instance;
        }
    }
    public GameObject VFXLighting;
  
    private IEnumerator ShutDown(float time, System.Action call)
    {
        yield return new WaitForSeconds(time);
        call ();
    }


    #region 闪电链
    public void  MakeLighting(Transform _start, Transform end, float _time)
    {
        VFXLighting.GetComponent<LineRenderer>().enabled = true;
        VFXLighting.GetComponent<UVChainLightning>().start = _start;
        VFXLighting.GetComponent<UVChainLightning>().target = end;
        StartCoroutine(ShowDownLighting(_time));
    }

    private IEnumerator ShowDownLighting(float time)
    {
        yield return new WaitForSeconds(time);
        VFXLighting.GetComponent<LineRenderer>().enabled = false;
    }
    #endregion
}
