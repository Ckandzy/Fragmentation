﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Gamekit2D
{
    [RequireComponent(typeof(CharacterController2D))]
    [RequireComponent(typeof(Animator))]
    public class PlayerCharacter : MonoBehaviour
    {
        static protected PlayerCharacter s_PlayerInstance;
        static public PlayerCharacter PlayerInstance { get { return s_PlayerInstance; } }

        public InventoryController InventoryController
        {
            get { return m_InventoryController; }
        }

        public SpriteRenderer spriteRenderer;
        public TakeDamageable damageable;
        public int CanJumpCount = 1;
        public Transform cameraFollowTarget;

        public float maxSpeed = 10f;
        public float groundAcceleration = 100f;
        public float groundDeceleration = 100f;
        [Range(0f, 1f)] public float pushingSpeedProportion;

        [Range(0f, 1f)] public float airborneAccelProportion;
        [Range(0f, 1f)] public float airborneDecelProportion;
        public float gravity = 50f;
        public float jumpSpeed = 20f;
        public float jumpAbortSpeedReduction = 100f;
        public int maxJumpTimes = 2;
        public int remainingJumpTimes;

        [Range(k_MinHurtJumpAngle, k_MaxHurtJumpAngle)]
        public float hurtJumpAngle = 45f;
        public float hurtJumpSpeed = 5f;
        /// <summary>
        /// 两次闪烁间隔时间
        /// </summary>
        public float flickeringDuration = 0.1f;

        public float meleeAttackDashSpeed = 5f;
        public bool dashWhileAirborne = false;

        public RandomAudioPlayer footstepAudioPlayer;
        public RandomAudioPlayer landingAudioPlayer;
        public RandomAudioPlayer hurtAudioPlayer;
        public RandomAudioPlayer meleeAttackAudioPlayer;
        public RandomAudioPlayer rangedAttackAudioPlayer;

        /// <summary>
        /// 每秒射击次数
        /// </summary>
        public float shotsPerSecond = 1f;
        /// <summary>
        /// 子弹速度
        /// </summary>
        public float bulletSpeed = 5f;
        public float holdingGunTimeoutDuration = 10f;
        public bool rightBulletSpawnPointAnimated = true;

        public float cameraHorizontalFacingOffset;
        public float cameraHorizontalSpeedOffset;
        public float cameraVerticalInputOffset;
        public float maxHorizontalDeltaDampTime;
        public float maxVerticalDeltaDampTime;
        /// <summary>
        /// 根据操作轴改变相机的偏移延迟(长按下镜头下移，长按上镜头上移)
        /// </summary>
        [Tooltip("要造成相机垂直偏移的Input持续时间")]
        public float verticalCameraOffsetDelay;

        public bool spriteOriginallyFacesLeft;

        /*protected*/public CharacterController2D m_CharacterController2D;
        protected Animator m_Animator;
        protected CapsuleCollider2D m_Capsule;
        protected BoxCollider2D m_Box;
        protected Transform m_Transform;
        protected Vector2 m_MoveVector;
        protected List<Pushable> m_CurrentPushables = new List<Pushable>(4);
        protected Pushable m_CurrentPushable;
        protected Climbable m_CurrentClimbable;
        /// <summary>
        /// 受伤跳跃正切值
        /// </summary>
        protected float m_TanHurtJumpAngle;
        protected WaitForSeconds m_FlickeringWait;
        protected Coroutine m_FlickerCoroutine;
        /*protected*/public Transform m_CurrentBulletSpawnPoint;
        /// <summary>
        /// 射击间隔时间
        /// </summary>
        protected float m_ShotSpawnGap;
        protected WaitForSeconds m_ShotSpawnWait;
        protected Coroutine m_ShootingCoroutine;
        protected float m_NextShotTime;
        protected bool m_IsFiring;
        protected float m_ShotTimer;
        protected float m_HoldingGunTimeRemaining;
        protected TileBase m_CurrentSurface;
        protected float m_CamFollowHorizontalSpeed;
        protected float m_CamFollowVerticalSpeed;
        protected float m_VerticalCameraOffsetTimer;
        protected InventoryController m_InventoryController;

        protected Checkpoint m_LastCheckpoint = null;
        protected Vector2 m_StartingPosition = Vector2.zero;
        protected bool m_StartingFacingLeft = false;

        protected bool m_InPause = false;

        protected readonly int m_HashHorizontalSpeedPara = Animator.StringToHash("HorizontalSpeed");
        protected readonly int m_HashVerticalSpeedPara = Animator.StringToHash("VerticalSpeed");
        protected readonly int m_HashGroundedPara = Animator.StringToHash("Grounded");
        protected readonly int m_HashCrouchingPara = Animator.StringToHash("Crouching");
        protected readonly int m_HashPushingPara = Animator.StringToHash("Pushing");
        protected readonly int m_HashTimeoutPara = Animator.StringToHash("Timeout");
        protected readonly int m_HashRespawnPara = Animator.StringToHash("Respawn");
        protected readonly int m_HashDeadPara = Animator.StringToHash("Dead");
        protected readonly int m_HashHurtPara = Animator.StringToHash("Hurt");
        protected readonly int m_HashForcedRespawnPara = Animator.StringToHash("ForcedRespawn");
        protected readonly int m_HashMeleeAttackPara = Animator.StringToHash("MeleeAttack");
        protected readonly int m_HashRangeAttackPara = Animator.StringToHash("RangeAttack");
        protected readonly int m_HashHoldingGunPara = Animator.StringToHash("HoldingGun");
        protected readonly int m_HashClimbingPara = Animator.StringToHash("Climbing");

        protected const float k_MinHurtJumpAngle = 0.001f;
        protected const float k_MaxHurtJumpAngle = 89.999f;
        protected const float k_GroundedStickingVelocityMultiplier = 3f;    // This is to help the character stick to(粘连在) vertically moving platforms.

        //used in non alloc version of physic function
        protected ContactPoint2D[] m_ContactsBuffer = new ContactPoint2D[16];

        // MonoBehaviour Messages - called by Unity internally.
        void Awake()
        {
            s_PlayerInstance = this;

            m_CharacterController2D = GetComponent<CharacterController2D>();
            m_Animator = GetComponent<Animator>();
            m_Capsule = GetComponent<CapsuleCollider2D>();
            m_Box = GetComponent<BoxCollider2D>();
            m_Transform = transform;
            m_InventoryController = GetComponent<InventoryController>();
        }

        void Start()
        {
            hurtJumpAngle = Mathf.Clamp(hurtJumpAngle, k_MinHurtJumpAngle, k_MaxHurtJumpAngle);
            m_TanHurtJumpAngle = Mathf.Tan(Mathf.Deg2Rad * hurtJumpAngle);
            m_FlickeringWait = new WaitForSeconds(flickeringDuration);

            //meleeDamager.DisableDamage();

            m_ShotSpawnGap = 1f / shotsPerSecond;
            m_NextShotTime = Time.time;
            m_ShotSpawnWait = new WaitForSeconds(m_ShotSpawnGap);

            //计算相机横纵轴跟随速度
            if (!Mathf.Approximately(maxHorizontalDeltaDampTime, 0f))
            {
                float maxHorizontalDelta = maxSpeed * cameraHorizontalSpeedOffset + cameraHorizontalFacingOffset;
                m_CamFollowHorizontalSpeed = maxHorizontalDelta / maxHorizontalDeltaDampTime;
            }

            if (!Mathf.Approximately(maxVerticalDeltaDampTime, 0f))
            {
                float maxVerticalDelta = cameraVerticalInputOffset;
                m_CamFollowVerticalSpeed = maxVerticalDelta / maxVerticalDeltaDampTime;
            }

            SceneLinkedSMB<PlayerCharacter>.Initialise(m_Animator, this);

            m_StartingPosition = transform.position;
            m_StartingFacingLeft = GetFacing() < 0.0f;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            Pushable pushable = other.GetComponent<Pushable>();
            if (pushable != null)
            {
                m_CurrentPushables.Add(pushable);
            }
            Climbable climbable = other.GetComponent<Climbable>();
            if (climbable != null)
            {
                m_CurrentClimbable = climbable;
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            Pushable pushable = other.GetComponent<Pushable>();
            if (pushable != null)
            {
                if (m_CurrentPushables.Contains(pushable))
                    m_CurrentPushables.Remove(pushable);
            }
            Climbable climbable = other.GetComponent<Climbable>();
            if (climbable != null)
            {
                m_CurrentClimbable = null;
            }
        }

        void Update()
        {
            
        }

        void FixedUpdate()
        { 
            m_CharacterController2D.Move(m_MoveVector * Time.deltaTime);
            m_Animator.SetFloat(m_HashHorizontalSpeedPara, m_MoveVector.x);
            m_Animator.SetFloat(m_HashVerticalSpeedPara, m_MoveVector.y);
            //UpdateBulletSpawnPointPositions();
            UpdateCameraFollowTargetPosition();
        }

        public void Unpause()
        {
            //if the timescale is already > 0, we 
            if (Time.timeScale > 0)
                return;

            StartCoroutine(UnpauseCoroutine());
        }

        protected IEnumerator UnpauseCoroutine()
        {
            Time.timeScale = 1;
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("UIMenus");
            PlayerInput.Instance.GainControl();
            //we have to wait for a fixed update so the pause button state change, otherwise we can get in case were the update
            //of this script happen BEFORE the input is updated, leading to setting the game in pause once again
            yield return new WaitForFixedUpdate();
            yield return new WaitForEndOfFrame();
            m_InPause = false;
        }
       
        
        protected void UpdateCameraFollowTargetPosition()
        {
            float newLocalPosX;
            float newLocalPosY = 0f;

            float desiredLocalPosX = (spriteOriginallyFacesLeft ^ spriteRenderer.flipX ? -1f : 1f) * cameraHorizontalFacingOffset;
            desiredLocalPosX += m_MoveVector.x * cameraHorizontalSpeedOffset;
            if (Mathf.Approximately(m_CamFollowHorizontalSpeed, 0f))
                newLocalPosX = desiredLocalPosX;
            else
                newLocalPosX = Mathf.Lerp(cameraFollowTarget.localPosition.x, desiredLocalPosX, m_CamFollowHorizontalSpeed * Time.deltaTime);

            bool moveVertically = false;
            if (!Mathf.Approximately(PlayerInput.Instance.Vertical.Value, 0f))
            {
                m_VerticalCameraOffsetTimer += Time.deltaTime;

                if (m_VerticalCameraOffsetTimer >= verticalCameraOffsetDelay)
                    moveVertically = true;
            }
            else
            {
                moveVertically = true;
                m_VerticalCameraOffsetTimer = 0f;
            }

            if (moveVertically)
            {
                //这里根据摇杆轴的纵轴参数调整相机垂直角度的偏移，表现为：长按上或者下会使相机朝上或者下偏移，长按时间取决于verticalCameraOffsetDelay
                float desiredLocalPosY = PlayerInput.Instance.Vertical.Value * cameraVerticalInputOffset;
                if (Mathf.Approximately(m_CamFollowVerticalSpeed, 0f))
                    newLocalPosY = desiredLocalPosY;
                else
                    newLocalPosY = Mathf.MoveTowards(cameraFollowTarget.localPosition.y, desiredLocalPosY, m_CamFollowVerticalSpeed * Time.deltaTime);
            }

            cameraFollowTarget.localPosition = new Vector2(newLocalPosX, newLocalPosY);
        }

        /// <summary>
        /// 无敌闪烁
        /// </summary>
        /// <returns></returns>
        protected IEnumerator Flicker()
        {
            float timer = 0f;

            while (timer < damageable.invulnerabilityDuration)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
                yield return m_FlickeringWait;
                timer += flickeringDuration;
            }

            spriteRenderer.enabled = true;
        }

        #region Public functions - called mostly by StateMachineBehaviours in the character's Animator Controller but also by Events.
        #region 移动参数设置
        public void SetMoveVector(Vector2 newMoveVector)
        {
            m_MoveVector = newMoveVector;
        }

        public void SetHorizontalMovement(float newHorizontalMovement)
        {
            m_MoveVector.x = newHorizontalMovement;
        }

        public void SetVerticalMovement(float newVerticalMovement)
        {
            m_MoveVector.y = newVerticalMovement;
        }

        /// <summary>
        /// Increment 增量
        /// </summary>
        /// <param name="additionalMovement"></param>
        public void IncrementMovement(Vector2 additionalMovement)
        {
            m_MoveVector += additionalMovement;
        }

        public void IncrementHorizontalMovement(float additionalHorizontalMovement)
        {
            m_MoveVector.x += additionalHorizontalMovement;
        }

        public void IncrementVerticalMovement(float additionalVerticalMovement)
        {
            m_MoveVector.y += additionalVerticalMovement;
        }

        public void GroundedVerticalMovement()
        {
            m_MoveVector.y -= gravity * Time.deltaTime;

            if (m_MoveVector.y < -gravity * Time.deltaTime * k_GroundedStickingVelocityMultiplier)
            {
                m_MoveVector.y = -gravity * Time.deltaTime * k_GroundedStickingVelocityMultiplier;
            }
        }

        public void GroundedHorizontalMovement(bool useInput, float speedScale = 1f)
        {
            float desiredSpeed = useInput ? PlayerInput.Instance.Horizontal.Value * maxSpeed * speedScale : 0f;
            //acceleration 加速度
            float acceleration = useInput && PlayerInput.Instance.Horizontal.ReceivingInput ? groundAcceleration : groundDeceleration;
            m_MoveVector.x = Mathf.MoveTowards(m_MoveVector.x, desiredSpeed, acceleration * Time.deltaTime);
        }

        public Vector2 GetMoveVector()
        {
            return m_MoveVector;
        }
        #endregion

        public bool IsFalling()
        {
            return m_MoveVector.y < 0f && !m_Animator.GetBool(m_HashGroundedPara);
        }

        #region 角色朝向设置
        public void UpdateFacing()
        {
            bool faceLeft = PlayerInput.Instance.Horizontal.Value < 0f;
            bool faceRight = PlayerInput.Instance.Horizontal.Value > 0f;

            if (faceLeft)
            {
                //spriteRenderer.flipX = !spriteOriginallyFacesLeft;
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (faceRight)
            {
                //spriteRenderer.flipX = spriteOriginallyFacesLeft;
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

        public void UpdateFacing(bool faceLeft)
        {
            if (faceLeft)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

        /// <summary>
        /// 获取角色sprite朝向
        /// </summary>
        /// <returns>-1f朝左，1f朝右</returns>
        public float GetFacing()
        {
            return spriteRenderer.flipX != spriteOriginallyFacesLeft ? -1f : 1f;
        }
        #endregion      

        /// <summary>
        /// Crouching 蹲伏
        /// </summary>
        public void CheckForCrouching()
        {
            m_Animator.SetBool(m_HashCrouchingPara, PlayerInput.Instance.Vertical.Value < 0f);
        }

        public bool CheckForGrounded()
        {
            bool wasGrounded = m_Animator.GetBool(m_HashGroundedPara);
            bool grounded = m_CharacterController2D.IsGrounded;
            if (grounded)
            {
                FindCurrentSurface();

                if (!wasGrounded && m_MoveVector.y < -1.0f)
                {//only play the landing sound if falling "fast" enough (avoid small bump playing the landing sound)
                    landingAudioPlayer.PlayRandomSound(m_CurrentSurface);
                }
            }
            else
                m_CurrentSurface = null;
            m_Animator.SetBool(m_HashGroundedPara, grounded);

            return grounded;
        }

        public void FindCurrentSurface()
        {
            Collider2D groundCollider = m_CharacterController2D.GroundColliders[0];

            if (groundCollider == null)
                groundCollider = m_CharacterController2D.GroundColliders[1];

            if (groundCollider == null)
                return;

            TileBase b = PhysicsHelper.FindTileForOverride(groundCollider, transform.position, Vector2.down);
            if (b != null)
            {
                m_CurrentSurface = b;
            }
        }

        #region 推动对象
        /*
         *推动物体逻辑概要：
         * 在状态机中所有状态类调用CheckForPushing()检测是否符合条件，如果是
         * 设置m_CurrentPushable字段为当前推动的物体
         * 判断可推动对象 相对角色的左右方向 及 键盘或者控制器的左右输入方向 是否要处理推动（如箱子在左，角色在右，此时输入为向右移动则不需要推动）
         * 执行m_CharacterController2D.Teleport(moveToPosition);  
         * 注：CheckForPushing() 只判断角色是否符合推动状态条件，并设置Pushing flag，等待状态机的切换，及保持推动过程中角色和可推动对象的相对位置恒定
         * 不做相关移动操作，而具体的移动逻辑为：
         * Pushing状态中调用m_MonoBehaviour.GroundedHorizontalMovement(true, m_MonoBehaviour.pushingSpeedProportion); 设置m_MoveVector（缓慢）
         * Pushing状态中调用MovePushable()，m_CurrentPushable.Move(m_MoveVector * Time.deltaTime); ***此处是以m_MoveVector为箱子移动速度
         */
        /// <summary>
        /// 判断是否处于推动状态
        /// </summary>
        public void CheckForPushing()
        {
            bool pushableOnCorrectSide = false;
            Pushable previousPushable = m_CurrentPushable;

            m_CurrentPushable = null;

            if (m_CurrentPushables.Count > 0)
            {
                bool movingRight = PlayerInput.Instance.Horizontal.Value > float.Epsilon;
                bool movingLeft = PlayerInput.Instance.Horizontal.Value < -float.Epsilon;

                for (int i = 0; i < m_CurrentPushables.Count; i++)
                {
                    float pushablePosX = m_CurrentPushables[i].pushablePosition.position.x;
                    float playerPosX = m_Transform.position.x;
                    if (pushablePosX < playerPosX && movingLeft || pushablePosX > playerPosX && movingRight)
                    {
                        pushableOnCorrectSide = true;
                        m_CurrentPushable = m_CurrentPushables[i];
                        break;
                    }
                }

                if (pushableOnCorrectSide)
                {
                    Vector2 moveToPosition = movingRight ? m_CurrentPushable.playerPushingRightPosition.position : m_CurrentPushable.playerPushingLeftPosition.position;
                    moveToPosition.y = m_CharacterController2D.Rigidbody2D.position.y;
                    //m_CharacterController2D.transform.position == m_CharacterController2D.Rigidbody2D.position
                    //2018.11.28 Hotkang 注： 此处的逻辑是：保持推动时角色和推动物体的相对位置不变（由pushable组件下的left和right position确定）
                    //如果不调用该函数，会出现碰撞抖动：角色一直向前推动，但可推动对象已经不可向前移动（碰撞体接触），此时有可能推动对象会嵌入其他碰撞体
                    m_CharacterController2D.Teleport(moveToPosition);
                }
            }

            if(previousPushable != null && m_CurrentPushable != previousPushable)
            {//we changed pushable (or don't have one anymore), stop the old one sound
                previousPushable.EndPushing();
            }

            m_Animator.SetBool(m_HashPushingPara, pushableOnCorrectSide);
        }

        public void MovePushable()
        {
            //we don't push ungrounded pushable, avoid pushing floating pushable or falling pushable.
            if (m_CurrentPushable && m_CurrentPushable.Grounded)
                m_CurrentPushable.Move(m_MoveVector * Time.deltaTime);
        }

        public void StartPushing()
        {
            if (m_CurrentPushable)
                m_CurrentPushable.StartPushing();
        }

        public void StopPushing()
        {
            if(m_CurrentPushable)
                m_CurrentPushable.EndPushing();
        }

        public void UpdateJump()
        {
            if (!PlayerInput.Instance.Jump.Held && m_MoveVector.y > 0.0f)
            {
                m_MoveVector.y -= jumpAbortSpeedReduction * Time.deltaTime;
            }
        }
        #endregion

        #region 攀爬楼梯

        public void CheckForClimbing()
        {
            if (m_CurrentClimbable != null && PlayerInput.Instance.Vertical.Value > float.Epsilon)
            {
                m_Animator.SetBool(m_HashClimbingPara, true);
                StartClimb();
            }
            else
            {
                m_Animator.SetBool(m_HashClimbingPara, false);
                StopClimb();
            }

            //Debug.Log((m_CurrentClimbable != null).ToString() + " , " + (PlayerInput.Instance.Vertical.Value > float.Epsilon).ToString());
        }

        public void StartClimb()
        {
            m_CharacterController2D.transform.position = new Vector3(
                m_CurrentClimbable.transform.position.x, 
                m_CharacterController2D.transform.position.y,
                m_CharacterController2D.transform.position.z
                );
        }

        public void StopClimb()
        {
            m_Animator.speed = 1f;
        }

        public void LadderVerticalMovement()
        {
            float desiredSpeed = PlayerInput.Instance.Vertical.Value * /*maxSpeed*/1f;
            float acceleration;

            if (PlayerInput.Instance.Vertical.ReceivingInput)
                acceleration = groundAcceleration * airborneAccelProportion;
            else
                acceleration = groundDeceleration * airborneDecelProportion;

            m_MoveVector.y = Mathf.MoveTowards(m_MoveVector.y, desiredSpeed, acceleration * Time.deltaTime);
            //m_Animator.speed = 1f * m_MoveVector.y / /*maxSpeed*/ 1f;
        }

        #endregion

        #region 空中移动
        public void AirborneHorizontalMovement()
        {
            float desiredSpeed = PlayerInput.Instance.Horizontal.Value * maxSpeed;
            float acceleration;
           
            if (PlayerInput.Instance.Horizontal.ReceivingInput)
                acceleration = groundAcceleration * airborneAccelProportion;
            else
                acceleration = groundDeceleration * airborneDecelProportion;

            m_MoveVector.x = Mathf.MoveTowards(m_MoveVector.x, desiredSpeed, acceleration * Time.deltaTime);
        }

        public void AirborneVerticalMovement()
        {
            if (Mathf.Approximately(m_MoveVector.y, 0f) || m_CharacterController2D.IsCeilinged && m_MoveVector.y > 0f)
            {
                m_MoveVector.y = 0f;
            }
            m_MoveVector.y -= gravity * Time.deltaTime;
        }
        #endregion

        /// <summary>
        /// 跳跃按键检测
        /// </summary>
        /// <returns></returns>
        public bool CheckForJumpInput()
        {
            return PlayerInput.Instance.Jump.Down;
        }

        /// <summary>
        /// 平台穿越按键检测（下蹲+跳跃键）
        /// </summary>
        /// <returns></returns>
        public bool CheckForFallInput()
        {
            return PlayerInput.Instance.Vertical.Value < -float.Epsilon && PlayerInput.Instance.Jump.Down;
        }

        public bool MakePlatformFallthrough()
        {
            int colliderCount = 0;
            int fallthroughColliderCount = 0;
        
            for (int i = 0; i < m_CharacterController2D.GroundColliders.Length; i++)
            {
                Collider2D col = m_CharacterController2D.GroundColliders[i];
                if(col == null)
                    continue;

                colliderCount++;

                if (PhysicsHelper.ColliderHasPlatformEffector (col))
                    fallthroughColliderCount++;
            }

            if (fallthroughColliderCount == colliderCount)
            {
                for (int i = 0; i < m_CharacterController2D.GroundColliders.Length; i++)
                {
                    Collider2D col = m_CharacterController2D.GroundColliders[i];
                    if (col == null)
                        continue;

                    PlatformEffector2D effector;
                    PhysicsHelper.TryGetPlatformEffector (col, out effector);
                    FallthroughReseter reseter = effector.gameObject.AddComponent<FallthroughReseter>();
                    reseter.StartFall(effector);
                    //set invincible for half a second when falling through a platform, as it will make the player "standup"
                    StartCoroutine(FallThroughtInvincibility());
                }
            }

            return fallthroughColliderCount == colliderCount;
        }

        IEnumerator FallThroughtInvincibility()
        {
            damageable.EnableInvulnerability(true);
            yield return new WaitForSeconds(0.5f);
            damageable.DisableInvulnerability();
        }

        public void EnableInvulnerability()
        {
            damageable.EnableInvulnerability();
        }

        public void DisableInvulnerability()
        {
            damageable.DisableInvulnerability();
        }

        public Vector2 GetHurtDirection()
        {
            Vector2 damageDirection = damageable.GetDamageDirection();

            if (damageDirection.y < 0f)
                return new Vector2(Mathf.Sign(damageDirection.x), 0f);

            float y = Mathf.Abs(damageDirection.x) * m_TanHurtJumpAngle;

            return new Vector2(damageDirection.x, y).normalized;
        }

        public void OnHurt(TakeDamager damager, TakeDamageable damageable)
        {
            //if the player don't have control, we shouldn't be able to be hurt as this wouldn't be fair
            if (!PlayerInput.Instance.HaveControl)
                return;

            UpdateFacing(damageable.GetDamageDirection().x > 0f);
            damageable.EnableInvulnerability();

            m_Animator.SetTrigger(m_HashHurtPara);

            //we only force respawn if health > 0, otherwise both forceRespawn & Death trigger are set in the animator, messing with each other.
            if(damageable.CurrentHealth > 0 && damager.forceRespawn)
                m_Animator.SetTrigger(m_HashForcedRespawnPara);

            m_Animator.SetBool(m_HashGroundedPara, false);
            //hurtAudioPlayer.PlayRandomSound();

            //if the health is < 0, mean die callback will take care of respawn
            if(damager.forceRespawn && damageable.CurrentHealth > 0)
            {
                //StartCoroutine(DieRespawnCoroutine(false, true));
            }
        }
        /*
        public void OnDie()
        {
            m_Animator.SetTrigger(m_HashDeadPara);

            StartCoroutine(DieRespawnCoroutine(true, false));
        }
        */
        /*
        IEnumerator DieRespawnCoroutine(bool resetHealth, bool useCheckPoint)
        {
            PlayerInput.Instance.ReleaseControl(true);
            yield return new WaitForSeconds(1.0f); //wait one second before respawing
            yield return StartCoroutine(ScreenFader.FadeSceneOut(useCheckPoint ? ScreenFader.FadeType.Black : ScreenFader.FadeType.GameOver));
            if(!useCheckPoint)
                yield return new WaitForSeconds (2f);
            Respawn(resetHealth, useCheckPoint);
            yield return new WaitForEndOfFrame();
            yield return StartCoroutine(ScreenFader.FadeSceneIn());
            PlayerInput.Instance.GainControl();
        }
        */
        public void StartFlickering()
        {
            m_FlickerCoroutine = StartCoroutine(Flicker());
        }

        public void StopFlickering()
        {
            StopCoroutine(m_FlickerCoroutine);
            spriteRenderer.enabled = true;
        }

        public bool CheckForMeleeAttackInput()
        {
            return PlayerInput.Instance.MeleeAttack.Down;
        }

        public bool CheckForRangeAttackInput()
        {
            return PlayerInput.Instance.RangedAttack.Down;
        }
        /*
        public void MeleeAttack()
        {
            m_Animator.SetTrigger(m_HashMeleeAttackPara);
        }

        public void RangeAttack()
        {
            m_Animator.SetTrigger(m_HashRangeAttackPara);
        }

        public void EnableMeleeAttack()
        {
            //meleeDamager.EnableDamage();
            meleeDamager.disableDamageAfterHit = true;
            meleeAttackAudioPlayer.PlayRandomSound();
        }

        public void DisableMeleeAttack()
        {
            meleeDamager.DisableDamage();
        }
        */
        public void TeleportToColliderBottom()
        {
            if (m_Capsule != null)
            {
                Vector2 colliderBottom = m_CharacterController2D.Rigidbody2D.position + m_Capsule.offset + Vector2.down * m_Capsule.size.y * 0.5f;
                m_CharacterController2D.Teleport(colliderBottom);
            }
            else if (m_Box != null)
            {
                Vector2 colliderBottom = m_CharacterController2D.Rigidbody2D.position + m_Box.offset + Vector2.down * m_Box.size.y * 0.5f;
                m_CharacterController2D.Teleport(colliderBottom);
            }
        }
        #endregion
        public void PlayFootstep()
        {
            footstepAudioPlayer.PlayRandomSound(m_CurrentSurface);
            var footstepPosition = transform.position;
            footstepPosition.z -= 1;
            VFXController.Instance.Trigger("DustPuff", footstepPosition, 0, false, null, m_CurrentSurface);
        }
        /*
        public void Respawn(bool resetHealth, bool useCheckpoint)
        {
            if (resetHealth)
                damageable.SetHealth(damageable.startingHealth);

            //we reset the hurt trigger, as we don't want the player to go back to hurt animation once respawned
            m_Animator.ResetTrigger(m_HashHurtPara);
            if (m_FlickerCoroutine != null)
            {//we stop flcikering for the same reason
                StopFlickering();
            }

            m_Animator.SetTrigger(m_HashRespawnPara);

            if (useCheckpoint && m_LastCheckpoint != null)
            {
                UpdateFacing(m_LastCheckpoint.respawnFacingLeft);
                GameObjectTeleporter.Teleport(gameObject, m_LastCheckpoint.transform.position);
            }
            else
            {
                UpdateFacing(m_StartingFacingLeft);
                GameObjectTeleporter.Teleport(gameObject, m_StartingPosition);
            }
        }
        */
        public void SetChekpoint(Checkpoint checkpoint)
        {
            m_LastCheckpoint = checkpoint;
        }

        //This is called by the inventory controller on key grab, so it can update the Key UI.
        public void KeyInventoryEvent()
        {
            if (KeyUI.Instance != null) KeyUI.Instance.ChangeKeyUI(m_InventoryController);
        }
    }
}
