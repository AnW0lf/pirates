using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saver : MonoBehaviour
{
    private Island island;

    private void Awake() { island = Island.Instance(); }

    private void OnApplicationFocus(bool focus) { Save(); }

    private void OnApplicationPause(bool pause) { Save(); }

    private void OnApplicationQuit() { Save(); }

    private void Save()
    {
        island.Save();
    }
}
