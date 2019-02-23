﻿using UnityEngine;

namespace Gamekit2D
{
    public class PlayerInput : InputComponent, IDataPersister
    {
        public static PlayerInput Instance
        {
            get { return s_Instance; }
        }

        protected static PlayerInput s_Instance;
    
    
        public bool HaveControl { get { return m_HaveControl; } }

        public InputButton Pause = new InputButton(KeyCode.Escape, XboxControllerButtons.Menu);
        public InputButton Interact = new InputButton(KeyCode.E, XboxControllerButtons.Y);
        public InputButton MeleeAttack = new InputButton(KeyCode.K, XboxControllerButtons.X);
        public InputButton RangedAttack = new InputButton(KeyCode.O, XboxControllerButtons.B);
        public InputButton Jump = new InputButton(KeyCode.Space, XboxControllerButtons.A);
        public InputAxis Horizontal = new InputAxis(KeyCode.D, KeyCode.A, XboxControllerAxes.LeftstickHorizontal);
        public InputAxis Vertical = new InputAxis(KeyCode.W, KeyCode.S, XboxControllerAxes.LeftstickVertical);
        public InputButton Attack = new InputButton(KeyCode.J, XboxControllerButtons.A);
        public InputButton Weapon1 = new InputButton(KeyCode.Alpha1, XboxControllerButtons.A);
        public InputButton Weapon2 = new InputButton(KeyCode.Alpha2, XboxControllerButtons.A);
        public InputButton Weapon3 = new InputButton(KeyCode.Alpha3, XboxControllerButtons.A);
        public InputButton Weapon4 = new InputButton(KeyCode.Alpha4, XboxControllerButtons.A);
        public InputButton Tab = new InputButton(KeyCode.Tab, XboxControllerButtons.X);
        public InputButton Exit = new InputButton(KeyCode.Escape, XboxControllerButtons.X);
        [HideInInspector]
        public DataSettings dataSettings;

        protected bool m_HaveControl = true;

        protected bool m_DebugMenuIsOpen = false;

        void Awake ()
        {
            if (s_Instance == null)
                s_Instance = this;
            else
                throw new UnityException("There cannot be more than one PlayerInput script.  The instances are " + s_Instance.name + " and " + name + ".");
        }

        void OnEnable()
        {
            if (s_Instance == null)
                s_Instance = this;
            else if(s_Instance != this)
                throw new UnityException("There cannot be more than one PlayerInput script.  The instances are " + s_Instance.name + " and " + name + ".");
        
            PersistentDataManager.RegisterPersister(this);

            //Application.targetFrameRate = 60;
        }

        void OnDisable()
        {
            PersistentDataManager.UnregisterPersister(this);

            s_Instance = null;
        }

        /// <summary>
        /// Call by Update() in InputComponent.
        /// </summary>
        /// <param name="fixedUpdateHappened"></param>
        protected override void GetInputs(bool fixedUpdateHappened)
        {
            //Debug.Log(fixedUpdateHappened);
            Pause.Get(fixedUpdateHappened, inputType);
            Interact.Get(fixedUpdateHappened, inputType);
            MeleeAttack.Get(fixedUpdateHappened, inputType);
            RangedAttack.Get(fixedUpdateHappened, inputType);
            Jump.Get(fixedUpdateHappened, inputType);
            Horizontal.Get(inputType);
            Vertical.Get(inputType);

            Attack.Get(fixedUpdateHappened, inputType);
            Weapon1.Get(fixedUpdateHappened, inputType);
            Weapon2.Get(fixedUpdateHappened, inputType);
            Weapon3.Get(fixedUpdateHappened, inputType);
            Weapon4.Get(fixedUpdateHappened, inputType);
            Tab.Get(fixedUpdateHappened, inputType);
            Exit.Get(fixedUpdateHappened, inputType);

            if (Input.GetKeyDown(KeyCode.F12))
            {
                m_DebugMenuIsOpen = !m_DebugMenuIsOpen;
            }
        }

        public override void GainControl()
        {
            m_HaveControl = true;

            GainControl(Pause);
            GainControl(Interact);
            GainControl(MeleeAttack);
            GainControl(RangedAttack);
            GainControl(Jump);
            GainControl(Horizontal);
            GainControl(Vertical);

            GainControl(Attack);
            GainControl(Weapon1);
            GainControl(Weapon2);
            GainControl(Weapon3);
            GainControl(Weapon4);
            GainControl(Tab);
            GainControl(Exit);
        }

        public override void ReleaseControl(bool resetValues = true)
        {
            m_HaveControl = false;

            ReleaseControl(Pause, resetValues);
            ReleaseControl(Interact, resetValues);
            ReleaseControl(MeleeAttack, resetValues);
            ReleaseControl(RangedAttack, resetValues);
            ReleaseControl(Jump, resetValues);
            ReleaseControl(Horizontal, resetValues);
            ReleaseControl(Vertical, resetValues);

            ReleaseControl(Attack, resetValues);
            ReleaseControl(Weapon1, resetValues);
            ReleaseControl(Weapon2, resetValues);
            ReleaseControl(Weapon3, resetValues);
            ReleaseControl(Weapon4, resetValues);
            ReleaseControl(Tab, resetValues);
            ReleaseControl(Exit, resetValues);
        }

        public void DisableMeleeAttacking()
        {
            MeleeAttack.Disable();
        }

        public void EnableMeleeAttacking()
        {
            MeleeAttack.Enable();
        }

        public void DisableRangedAttacking()
        {
            RangedAttack.Disable();
        }

        public void EnableRangedAttacking()
        {
            RangedAttack.Enable();
        }

        public DataSettings GetDataSettings()
        {
            return dataSettings;
        }

        public void SetDataSettings(string dataTag, DataSettings.PersistenceType persistenceType)
        {
            dataSettings.dataTag = dataTag;
            dataSettings.persistenceType = persistenceType;
        }

        public Data SaveData()
        {
            return new Data<bool, bool>(MeleeAttack.Enabled, RangedAttack.Enabled);
        }

        public void LoadData(Data data)
        {
            Data<bool, bool> playerInputData = (Data<bool, bool>)data;

            if (playerInputData.value0)
                MeleeAttack.Enable();
            else
                MeleeAttack.Disable();

            if (playerInputData.value1)
                RangedAttack.Enable();
            else
                RangedAttack.Disable();
        }

        void OnGUI()
        {
            if (m_DebugMenuIsOpen)
            {
                const float height = 100;

                GUILayout.BeginArea(new Rect(30, Screen.height - height, 200, height));

                GUILayout.BeginVertical("box");
                GUILayout.Label("Press F12 to close");

                bool meleeAttackEnabled = GUILayout.Toggle(MeleeAttack.Enabled, "Enable Melee Attack");
                bool rangeAttackEnabled = GUILayout.Toggle(RangedAttack.Enabled, "Enable Range Attack");

                if (meleeAttackEnabled != MeleeAttack.Enabled)
                {
                    if (meleeAttackEnabled)
                        MeleeAttack.Enable();
                    else
                        MeleeAttack.Disable();
                }

                if (rangeAttackEnabled != RangedAttack.Enabled)
                {
                    if (rangeAttackEnabled)
                        RangedAttack.Enable();
                    else
                        RangedAttack.Disable();
                }
                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
        }
    }
}
