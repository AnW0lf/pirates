using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppsFlyerStartUp : MonoBehaviour
{
    [SerializeField]
    private string appsFlyerDevKey = "YOUR_APPSFLYER_DEV_KEY", IosAppId = "YOUR_APP_ID_HERE", AndroidPackageName = "YOUR_ANDROID_PACKAGE_NAME_HERE";

    void Start()
    {
        /* Mandatory - set your AppsFlyer’s Developer key. */
        AppsFlyer.setAppsFlyerKey(appsFlyerDevKey);
        /* For detailed logging */
        /* AppsFlyer.setIsDebug (true); */
#if UNITY_IOS
  /* Mandatory - set your apple app ID
   NOTE: You should enter the number only and not the "ID" prefix */
  AppsFlyer.setAppID (IosAppId);
  AppsFlyer.trackAppLaunch ();
#elif UNITY_ANDROID
        /* Mandatory - set your Android package name */
        AppsFlyer.setAppID(AndroidPackageName);
        /* For getting the conversion data in Android, you need to add the "AppsFlyerTrackerCallbacks" listener.*/
        AppsFlyer.init(appsFlyerDevKey, "AppsFlyerTrackerCallbacks");
#endif
    }

}
