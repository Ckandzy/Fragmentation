using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public enum NPCType
    {
        QuestNPC,
        DrugSellNPC
    }

	/// <summary>
	/// 当前脚本的名称：INPC
    /// the interface of NPC
    /// 当前的交互需要调用--由player去调用，激活
	/// </summary>
	public interface INPC {
        /// <summary>
        /// when the player enter the collider -- use it
        /// </summary>
        void PlayerClose(); // when the player enter the collider -- use it

        /// <summary>
        /// // when the player out the collider
        /// </summary>
        void PlayerExit(); // when the player out the collider -- use it

        /// <summary>
        /// when the player enter the collider and interaction (交互)
        /// </summary>
        void Active(); // Interaction(交互) for UI or other 

        /// <summary>
        /// // such as when the talk has next page, the next interaction(交互)
        /// </summary>
        void Next(); // when the talk has next page

        /// <summary>
        /// // close the active and exit -- use it
        /// </summary>
        void unActive(); // close the active and exit

        /// <summary>
        /// // get the NPC type -- NPCType
        /// </summary>
        /// <returns></returns>
        NPCType getNPCType(); // get the NPC type
    }

    /// <summary>
    ///  该类是由NPC检测，用户的靠近，和离开 即：PlayerClose（）， PlayerExit
    ///  但交互还是需要NPC来使用
    /// </summary>
    public abstract class ACNPC : MonoBehaviour, INPC
    {
        protected bool isClosePlayer = false;
        protected virtual void Update()
        {
            checkPlayer();
        }


        /// <summary>
        /// // the Physics Area to check Player -- rect
        /// </summary>
        /// <returns></returns>
        protected virtual Collider2D checkPlayer()
        {
            Vector2 offset = transform.position;
            Vector2 size = new Vector2(-1, +1);

            // the step of Physics check and ignore slef's collider
            GetComponent<BoxCollider2D>().enabled = false;
            Collider2D collider = Physics2D.OverlapArea(offset - size, offset + size);
            GetComponent<BoxCollider2D>().enabled = true;
            Debug.DrawLine(offset - size, offset + size);

            if (collider == null)
            {
                if (isClosePlayer == true) PlayerExit();
                return null;
            }

            if (collider.transform.tag == "Player")
            {
                if (isClosePlayer == false) PlayerClose();
            }

            return collider;
        }

        public abstract void Active();

        public abstract NPCType getNPCType();

        public abstract void Next();

        public abstract void unActive();

        public virtual void PlayerClose()
        {
            isClosePlayer = true;
        }

        public virtual void PlayerExit()
        {
            isClosePlayer = false;
        }
    }