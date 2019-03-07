using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

public class RangeWaponAttackAudio : RandomAudioPlayer
{
    [System.Serializable]
    public struct WeaponClip
    {
        public WeaponName tile;
        public AudioClip clips;

    }

    public WeaponClip[] WeaponClips;
    Dictionary<WeaponName, AudioClip> WeaponClipDic;

    private void Awake()
    {
        m_Source = GetComponent<AudioSource>();
        WeaponClipDic = new Dictionary<WeaponName, AudioClip>();

        //for (int i = 0; i < WeaponClipDic.Length; ++i)
        {
            //if (overrides[i].tile == null)
            //   continue;

            // m_LookupOverride[overrides[i].tile] = overrides[i].clips;
            // }
        }

        //public void PlayRandomSound(WeaponName weaponName)
        {
            // AudioClip[] source = clips;


        }
    }
}
