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
    public GameObject VFXLighting; // 闪电链
    public GameObject XLine; // 激光
    public GameObject ForceField;

    private IEnumerator ShutDown(float time, System.Action call)
    {
        yield return new WaitForSeconds(time);
        call ();
    }

    #region 力场
    public void MakeForceField(Transform _start, float _time)
    {
        ForceField.SetActive(true);
        ForceField.transform.SetParent(_start);
        ForceField.transform.localPosition = new Vector2(0,0);
        StartCoroutine(ShutDown(_time,
            () => {
                ForceField.SetActive(false); ForceField.transform.SetParent(transform); }));
    }
    #endregion

    #region 雷电激光
    public void MakeLaser(Vector2 _start, Vector2 end, float _time)
    {
        XLine.GetComponent<LineRenderer>().enabled = true;
        XLine.GetComponent<LineRenderer>().SetPosition(0, _start);
        XLine.GetComponent<LineRenderer>().SetPosition(1, end);
        StartCoroutine(ShutDown(_time, ()=> XLine.GetComponent<LineRenderer>().enabled = false));
    }
    #endregion

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
