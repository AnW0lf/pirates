using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notifications : MonoBehaviour
{
    public string levelUpTitle, levelUpDescription;
    public string dailyTitle, dailyDescription;

    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_IOS
                UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert | UnityEngine.iOS.NotificationType.Badge | UnityEngine.iOS.NotificationType.Sound);
        #endif
    }

    private void OnApplicationPause(bool pause)
    {
        #if UNITY_IOS

                UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
                UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();

                if (pause)
                {
                    DateTime levelUpDateToNotify = DateTime.Now.AddHours(2);
                    DateTime dailyDateToNotify = DateTime.Now.AddDays(1);
            
                    // LEVEL UP

                    UnityEngine.iOS.LocalNotification levelUpNotif = new UnityEngine.iOS.LocalNotification();
                    levelUpNotif.fireDate = levelUpDateToNotify;
                    levelUpNotif.alertTitle = levelUpTitle;
                    levelUpNotif.alertBody = levelUpDescription;

                    levelUpNotif.repeatInterval = UnityEngine.iOS.CalendarUnit.Hour;

                    UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(levelUpNotif);


                    // DAILY BONUS

                    UnityEngine.iOS.LocalNotification dailyNotif = new UnityEngine.iOS.LocalNotification();
                    dailyNotif.fireDate = dailyDateToNotify;
                    dailyNotif.alertTitle = dailyTitle;
                    dailyNotif.alertBody = dailyDescription;

                    dailyNotif.repeatInterval = UnityEngine.iOS.CalendarUnit.Day;

                    UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(dailyNotif);
                }

        #endif
    }
}
