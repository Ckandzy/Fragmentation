using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

public class Audio : RandomAudioPlayer
{
    public void PlayRandomSound(int index)
    {
        AudioClip[] source = clips;

        int choice = Random.Range(0, source.Length);

        if (randomizePitch)
            m_Source.pitch = Random.Range(1.0f - pitchRange, 1.0f + pitchRange);
      
        m_Source.PlayOneShot(source[index]);
    }
}
