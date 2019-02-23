using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuyiFrame;

public class FragmentItem : MItem {
    public IFragment ItemFragment;
    public Sprite Sprite;

    public FragmentType GetFragmentType()
    {
        return ItemFragment.GetFragType();
    }

    public string Des()
    {
        return ItemFragment.Des();
    }
}


