using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saver : MonoBehaviour
{
    private Island island;

    private void OnApplicationFocus(bool focus) { Save(); }

    private void OnApplicationPause(bool pause) { Save(); }

    private void OnApplicationQuit() { Save(); }

    private void Save() { Island.Instance().Save(); }
}
