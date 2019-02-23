using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

public class PlotController : MonoBehaviour
{
    static PlotController s_Instance;
    public static PlotController Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;

            s_Instance = FindObjectOfType<PlotController>();

            if (s_Instance != null)
                return s_Instance;

            Create();

            return s_Instance;
        }
        set { s_Instance = value; }
    }

    [SerializeField]
    public List<MonoBehaviour> PlotList;
    protected Queue<MonoBehaviour> PlotQueue;
    static void Create()
    {
        GameObject plotController = new GameObject("PlotController");
        s_Instance = plotController.AddComponent<PlotController>();
    }

    private IEnumerator Start()
    {
        PlotQueue = new Queue<MonoBehaviour>(PlotList);
        //PlayerInput.Instance.ReleaseControl(true);
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine((PlotQueue.Dequeue() as IPlot).DoPlot());
        yield return StartCoroutine((PlotQueue.Dequeue() as IPlot).DoPlot());
    }
}