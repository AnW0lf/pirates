using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowBase : MonoBehaviour, IInterfaceWindow
{
    [SerializeField] protected GameObject back;
    [SerializeField] protected GameObject rays;
    public bool Opened { get; protected set; }

    public virtual void Open(object[] args) {
        Opened = true;
        if (back != null && !back.activeInHierarchy) back.SetActive(Opened);
        if (rays != null && !rays.activeInHierarchy) rays.SetActive(Opened);
    }

    public virtual void Close() {
        Opened = false;
        if (back != null && back.activeInHierarchy) back.SetActive(Opened);
        if (rays != null && rays.activeInHierarchy) rays.SetActive(Opened);
    }
}
