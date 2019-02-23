using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class DirectorManager : MonoBehaviour/*, IDataPersister*/, IPlot
{
    public PlayableDirector director;

    public bool HasDone { get; set; }

    private void Awake()
    {
        HasDone = false;
    }

    public IEnumerator DoPlot()
    {
        TriggerDirecotr();
        while (!HasDone)
        {
            yield return null;
        }
        yield break;
    }

    public void TriggerDirecotr()
    {
        Invoke("FinishInvoke", (float)director.duration);
    }

    void FinishInvoke()
    {
        HasDone = true;
    }
}