using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static  class Extentions
{
    public static void AddListInt(this List<int> list, List<int> _list)
    {
        foreach (int i in _list)
        {
            if (!list.Contains(i)) list.Add(i);
        }
    }

    public static void AddListBuff(this List<IBuff> list, List<int> addList)
    {
        foreach (IBuff i in list)
        {
            if (!list.Contains(i)) list.Add(i);
        }
    }
}
