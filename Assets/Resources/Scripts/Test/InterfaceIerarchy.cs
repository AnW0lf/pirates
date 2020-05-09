using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceIerarchy : MonoBehaviour
{
    public bool Done { get; protected set; }
    public Action onDone = null;

    protected int counter = 0;
    [SerializeField] protected List<WindowBase> windows;

    void Start()
    {
        counter = 0;
        Done = false;
        EventManager.Subscribe("LevelUp", StartWindowsQueue);
    }

    private void StartWindowsQueue(object[] arg0)
    {
        counter = 0;
        Done = false;
        if (windows.Count > counter)
            windows[counter].Open(arg0);
        else Done = true;
    }

    public void Next()
    {
        if (windows.Count > ++counter)
            windows[counter].Open(new object[0]);
        else
        {
            Done = true;
            LeanTween.delayedCall(0.5f, () =>
            {
                onDone?.Invoke();
                onDone = null;
            });
        }
    }
}
