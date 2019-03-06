using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FragmentFactory {

    public static IFragment GetFragment(FragmentName _name)
    {
        switch (_name)
        {
            case FragmentName.CriterionFrag: return new CriterionFrag();
            case FragmentName.ShockFrag:return new ShockFrag();
            case FragmentName.TrialFrag: return new TrialFrag();
            case FragmentName.Arrogant: return new ArrogantFrag();
            case FragmentName.Disorder: return new DisorderFrag();
            case FragmentName.Riot: return new RiotFrag();
            case FragmentName.WindFrag: return new WindFrag();
            case FragmentName.FireFrag: return new FireFrag();
        }
        throw new System.Exception("no but try get it");
    }

    public static IFragment GetFragment(FragmentType type, int id)
    {
        switch (type)
        {
            case FragmentType.Guard:return GetGuardFrag(id);
            case FragmentType.Demon:return GetDemonFrag(id);
        }
        throw new System.Exception("no but try get it");
    }

    private static IFragment GetDemonFrag(int id)
    {
        switch (id)
        { 
            case 1: return new ArrogantFrag();
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
            case 4: return new ArrogantFrag();
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
