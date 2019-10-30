using GameAnalyticsSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GA_Initialize : MonoBehaviour
{

#if UNITY_IPHONE
    void Start()
    {
        GameAnalytics.Initialize();
    }
#endif

}
