using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActivator : MonoBehaviour
{
    public IslandCtrl[] islands;

    private void Start()
    {
        foreach (IslandCtrl island in islands)
            island.Level = Island.Instance().Level;
    }
}
