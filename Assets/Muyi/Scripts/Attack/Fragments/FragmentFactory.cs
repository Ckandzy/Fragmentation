using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FragmentFactory {
    public static IFragment GetFragment(FragmentType type, int id)
    {
        switch (type)
        {
            case FragmentType.Guard:return GetGuardFrag(id);
        }
        throw new System.Exception("no but try get it");
    } 

    private static IFragment GetGuardFrag(int id)
    {
        switch (id)
        {
            case 1: return new CriterionFrag();
            case 2: return new ShockFrag();
            case 3: return new TrialFrag();
        }
        throw new System.Exception("no but try get it");
    }
}

public static class FragmentSpriteUtil
{
    public static string getFragmentSpriteName(IFragment _frag)
    {
        switch (_frag.GetFragType())
        {
            case FragmentType.Guard: return  GetGuardFragSpriteName(_frag.ID);

        }
        throw new System.Exception("no but try get it");
    }

    private static string GetGuardFragSpriteName(int id)
    {
        switch (id)
        {
            case 1: return "CriterionFragSprite";// return new CriterionFrag();
            case 2: return "ShockFragSprite";// return new ShockFrag();
            case 3: return "TrialFragSprite";// return new TrialFrag();
        }
        throw new System.Exception("no but try get it");
    }
}
