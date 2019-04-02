using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowsManager : MonoBehaviour
{
    public WindowController[] mcs;

    public void SetWindows(List<PierManager> piers)
    {
        for (int i = 0; i < piers.Count && i < mcs.Length; i++)
            mcs[i].GenerateMenu(piers[i]);
    }
}
