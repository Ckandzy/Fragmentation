using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

public class RangeWaponAttackAudio : RandomAudioPlayer
{
    [System.Serializable]
    public struct WeaponClip
    {
        public WeaponName weaponName;
        public AudioClip clips;

    }

    public WeaponClip[] WeaponClips;
    Dictionary<WeaponName, AudioClip> WeaponClipDic;

    private void Awake()
    {
        m_Source = GetComponent<AudioSource>();
        WeaponClipDic = new Dictionary<WeaponName, AudioClip>();

        for (int i = 0; i < WeaponClips.Length; i++)
        {
            if(!WeaponClipDic.ContainsKey(WeaponClips[i].weaponName))
                WeaponClipDic.Add(WeaponClips[i].weaponName, WeaponClips[i].clips);
        }
    }

    public void PlayRandomSound(WeaponName weaponName)
    {
        AudioClip source = null;
        if(WeaponClipDic.TryGetValue(weaponName, out source))
        {
            if (randomizePitch)
                m_Source.pitch = Random.Range(1.0f - pitchRange, 1.0f + pitchRange);
            m_Source.PlayOneShot(source);
        }
    }
}
