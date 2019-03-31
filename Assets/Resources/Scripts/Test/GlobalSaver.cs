using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSaver : MonoBehaviour
{
    private Global global;

    private void Awake() { global = Global.Instance; }

    private void OnApplicationFocus(bool focus) { Save(); }

    private void OnApplicationPause(bool pause) { Save(); }

    private void OnApplicationQuit() { Save(); }

    private void Save()
    {
        global.Save();
    }
}
