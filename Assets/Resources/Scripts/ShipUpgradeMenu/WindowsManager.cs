using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowsManager : MonoBehaviour
{
    public WindowController[] mcs;

    private SnapScrolling ss;

    private void Awake()
    {
        ss = mcs[0].transform.GetComponentInParent<SnapScrolling>();
    }

    public void SetWindows(List<ShipCtrl> piers)
    {
        for (int i = 0; i < piers.Count && i < mcs.Length; i++)
            mcs[i].GenerateMenu(piers[i]);
    }

    public void FocusWindow(int id)
    {
        ss.SetPan(id);
    }
}
