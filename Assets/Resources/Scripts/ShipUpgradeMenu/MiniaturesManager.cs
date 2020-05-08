using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniaturesManager : MonoBehaviour
{
    public MiniatureContoller[] mcs;

    private void OnEnable()
    {
        EventManager.Subscribe("SnapScrolling", FocusMiniature);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe("SnapScrolling", FocusMiniature);
    }

    private void FocusMiniature(object[] arg0)
    {
        if (arg0.Length > 0)
        {
            FocusMiniature((int)arg0[0]);
        }
    }

    public void SetMiniatures(List<PierManager> piers)
    {
        for (int i = 0; i < piers.Count && i < mcs.Length; i++)
            mcs[i].SetInfo(piers[i]);
    }

    public void FocusMiniature(int id)
    {
        mcs[id].FocusThis();
    }
}
