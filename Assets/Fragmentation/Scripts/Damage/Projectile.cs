using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Damager))]
public class Projectile : MonoBehaviour
{
    [System.Serializable]
    public class ProjectData
    {
        public Vector3 lauchPos;
        public Vector2 gravity;
        public float launchSpeed;
        public Vector2 direction;
        public bool Track;
        public Transform Target;
        /// <summary>
        /// 灵敏度, 导弹追踪强度
        /// </summary>
        public float trackSensitivity;
    }
    #region Trajectory Mode
    [Header("Trajectory")]
    public ProjectData projectData;

    public Vector2 moveVector;
    #endregion

    protected Rigidbody2D m_Rigidbody2D;

    [HideInInspector]
    public ProjectileObject projectilePoolObject;
    [HideInInspector]
    public Camera mainCamera;

    public void ReturnToPool()
    {
        projectilePoolObject.ReturnToPool();
    }
    private void OnEnable()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        moveVector = projectData.launchSpeed * projectData.direction.normalized;
        transform.rotation = Quaternion.FromToRotation(transform.right, projectData.direction);
        Debug.Log(moveVector.normalized + " , " + transform.right);
    }

    void FixedUpdate()
    {
        m_Rigidbody2D.MovePosition(m_Rigidbody2D.position + moveVector * Time.deltaTime);
        if (projectData.Track)
        {
            Vector3 dir = Vector3.RotateTowards(
                moveVector.normalized, 
                (projectData.Target.position - transform.position).normalized, 
                projectData.trackSensitivity, 
                0f
             );

            Debug.DrawRay(transform.position, moveVector, Color.red);
            moveVector = projectData.launchSpeed * dir;
            Debug.DrawRay(transform.position, transform.right, Color.blue);

            //Unity源代码:
            //public Vector3 right { get { return rotation * Vector3.right; } set { rotation = Quaternion.FromToRotation(Vector3.right, value); } }
            
            //重点[错误写法] :transform.rotation = Quaternion.FromToRotation(transform.right, dir);
            //正确写法[1] :transform.right = dir;
            //正确写法[2] :
            transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
        }
    }

    public void OnHitDamageable(Damager origin, Damageable damageable)
    {
        //FindSurface(origin.LastHit);
    }

    public void OnHitNonDamageable(Damager origin)
    {
        //FindSurface(origin.LastHit);
    }
}
