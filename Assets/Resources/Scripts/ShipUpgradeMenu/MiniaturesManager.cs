using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniaturesManager : MonoBehaviour
{
    public MiniatureContoller[] mcs;

    public void SetMiniatures(List<PierManager> piers)
    {
        for (int i = 0; i < piers.Count && i < mcs.Length; i++)
            mcs[i].SetInfo(piers[i]);
    }
}
