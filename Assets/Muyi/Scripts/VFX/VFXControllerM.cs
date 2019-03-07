using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;
using UnityEngine.UI;
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
    public GameObject ForceField; // 力场
    public GameObject Fire;
    public Image ScreenImage;

    private IEnumerator ShutDown(float time, System.Action call)
    {
        yield return new WaitForSeconds(time);
        call ();
    }

    private void Awake()
    {
        ScreenImage = GameObject.Find("UICanvas").transform.Find("ScreenImage").GetComponent<Image>();
    }

    private void Start()
    {
        m_FirePool = FireVFXPool.GetObjectPool(Fire);
    }

    #region 铁腕强权的秒杀效果
    public void ScreenFlicker(float duraingTime)
    {
        StartCoroutine(Flicker(duraingTime));
    }

    public void OverFlicker()
    {
        StopCoroutine("Flicker");
        StartCoroutine(Over(1));
    }
    private IEnumerator Flicker(float duraingTime)
    {
        float timer = 0;
        while (timer <= duraingTime)
        {
            timer += 0.1f;
            ScreenImage.color = new Color(0.77f, 0.17f, 0.09f, 0.7f * (timer / duraingTime));
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator Over(float duraingTime)
    {
        float timer = 0;
        while (timer <= duraingTime)
        {
            timer += 0.1f;
            ScreenImage.color = new Color(0.77f, 0.17f, 0.09f, 0.8f * (1 - timer / duraingTime));
            yield return new WaitForSeconds(0.1f);
        }
    }
    #endregion

    #region 火焰
    private FireVFXPool m_FirePool;
    public void MakeFire(Transform _start, float _time)
    {
        FireVFXObject obj = m_FirePool.Pop();
        obj.instance.transform.SetParent(_start);
        obj.instance.transform.localPosition = Vector2.zero;
        StartCoroutine(ShutDown(_time,
            () => { obj.ReturnToPool(); }
            ));
    }

    #endregion

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

public class FireVFXPool : ObjectPool<FireVFXPool, FireVFXObject, Fire>
{
    static protected Dictionary<GameObject, FireVFXPool> s_PoolInstances = new Dictionary<GameObject, FireVFXPool>();

    private void Awake()
    {
        //This allow to make Pool manually added in the scene still automatically findable & usable
        if (prefab != null && !s_PoolInstances.ContainsKey(prefab))
            s_PoolInstances.Add(prefab, this);
    }

    private void OnDestroy()
    {
        s_PoolInstances.Remove(prefab);
    }

    //initialPoolCount is only used when the objectpool don't exist
    static public FireVFXPool GetObjectPool(GameObject prefab, int initialPoolCount = 10)
    {
        FireVFXPool objPool = null;
        if (!s_PoolInstances.TryGetValue(prefab, out objPool))
        {
            GameObject obj = new GameObject(prefab.name + "_Pool");
            objPool = obj.AddComponent<FireVFXPool>();
            objPool.prefab = prefab;
            objPool.initialPoolCount = initialPoolCount;

            s_PoolInstances[prefab] = objPool;
        }

        return objPool;
    }
}

public class FireVFXObject : PoolObject<FireVFXPool, FireVFXObject, Fire>
{
    public Transform transform;
    //public Rigidbody2D rigidbody2D;
    public SpriteRenderer spriteRenderer;
    //public Bullet bullet;
    public Fire fire;
    protected override void SetReferences()
    {
        transform = instance.transform;
        //rigidbody2D = instance.GetComponent<Rigidbody2D>();
        spriteRenderer = instance.GetComponent<SpriteRenderer>();
        fire = instance.GetComponent<Fire>();
        fire.PoolObject = this;
    }

    public override void WakeUp()
    {
        instance.SetActive(true);
    }

    public override void Sleep()
    {
        instance.SetActive(false);
    }
}
