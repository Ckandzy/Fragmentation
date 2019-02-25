using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

public class ABulletsPool : ObjectPool<ABulletsPool, APoolBullet>
{
    static protected Dictionary<GameObject, ABulletsPool> s_PoolInstances = new Dictionary<GameObject, ABulletsPool>();

    private void Awake()
    {
        //This allow to make Pool manually added in the scene still automatically findable & usable
        if (prefab != null && !s_PoolInstances.ContainsKey(prefab))
            s_PoolInstances.Add(prefab, this);
    }

    private void Start()
    {
        
    }

    public void Restart(Transform _parent)
    {
        DestroyAll(_parent);
        for (int i = 0; i < initialPoolCount; i++)
        {
            APoolBullet newPoolObject = CreateNewPoolObject(_parent);
            pool.Add(newPoolObject);
        }
    }

    protected void DestroyAll(Transform _parent)
    {
        for (int i = 0; i < _parent.childCount; i++)
        {
            Destroy(_parent.GetChild(i).gameObject);
        }
        pool = new List<APoolBullet>();
    }

    private void OnDestroy()
    {
        s_PoolInstances.Remove(prefab);
    }

    private new APoolBullet CreateNewPoolObject() { return null; }

    public APoolBullet CreateNewPoolObject(Transform parent)
    {
        APoolBullet newPoolObject = new APoolBullet();
        newPoolObject.instance = Instantiate(prefab);
        newPoolObject.instance.transform.SetParent(parent);
        newPoolObject.inPool = true;
        newPoolObject.SetReferences(this as ABulletsPool);
        newPoolObject.Sleep();
        return newPoolObject;
    }

    //initialPoolCount is only used when the objectpool don't exist
    static public ABulletsPool GetObjectPool(GameObject prefab, int initialPoolCount = 10)
    {
        ABulletsPool objPool = null;
        if (!s_PoolInstances.TryGetValue(prefab, out objPool))
        {
            GameObject obj = new GameObject(prefab.name + "_Pool");
            objPool = obj.AddComponent<ABulletsPool>();
            objPool.prefab = prefab;
            objPool.initialPoolCount = initialPoolCount;

            s_PoolInstances[prefab] = objPool;
        }

        return objPool;
    }
}

public class APoolBullet : PoolObject<ABulletsPool, APoolBullet>
{
    public Transform transform;
    public Rigidbody2D rigidbody2D;
    public SpriteRenderer spriteRenderer;
    public IBullet bullet;

    protected override void SetReferences()
    {
        transform = instance.transform;
        rigidbody2D = instance.GetComponent<Rigidbody2D>();
        spriteRenderer = instance.GetComponent<SpriteRenderer>();
        bullet = instance.GetComponent<IBullet>();
        bullet.bulletPoolObject = this;
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
