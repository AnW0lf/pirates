using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

public class Notifications : MonoBehaviour
{
    public string[] levelUpTitle, levelUpDescription;
    public string dailyTitle, dailyDescription;

#if UNITY_ANDROID
    private string dailyNotificationChannelID = "daily", dailyNotificationChannelName = "Daily Notifications";
    private string levelUpNotificationChannelID = "levelup", levelUpNotificationChannelName = "Level Up Notifications";
    private string smallIcon = "icon_0", largeIcon = "icon_1";
    private enum Delay { Day, Hour, Minutes, Seconds};
#endif

    void Start()
    {
#if UNITY_IOS
                UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert | UnityEngine.iOS.NotificationType.Badge | UnityEngine.iOS.NotificationType.Sound);
#elif UNITY_ANDROID
        CreateNotificationChannel(dailyNotificationChannelID, dailyNotificationChannelName, Importance.High, "Channel for daily notifications");
        CreateNotificationChannel(levelUpNotificationChannelID, levelUpNotificationChannelName, Importance.High, "Channel for level up notifications");
#endif
    }

    private void OnApplicationPause(bool pause)
    {
#if UNITY_IOS

                UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
                UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();

                if (pause)
                {
                    DateTime levelUpDateToNotify = DateTime.Now.AddHours(3);
                    DateTime dailyDateToNotify = DateTime.Now.AddDays(1);
            
                    // LEVEL UP

                    UnityEngine.iOS.LocalNotification levelUpNotif = new UnityEngine.iOS.LocalNotification();
                    levelUpNotif.fireDate = levelUpDateToNotify;

                    int randomDescr = (int)UnityEngine.Random.Range(0f, levelUpTitle.Length);

                    levelUpNotif.alertTitle = levelUpTitle[randomDescr];
                    levelUpNotif.alertBody = levelUpDescription[randomDescr];

                    levelUpNotif.repeatInterval = UnityEngine.iOS.CalendarUnit.Day;

                    UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(levelUpNotif);


                    // DAILY BONUS

                    UnityEngine.iOS.LocalNotification dailyNotif = new UnityEngine.iOS.LocalNotification();
                    dailyNotif.fireDate = dailyDateToNotify;
                    dailyNotif.alertTitle = dailyTitle;
                    dailyNotif.alertBody = dailyDescription;

                    dailyNotif.repeatInterval = UnityEngine.iOS.CalendarUnit.Day;

                    UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(dailyNotif);
                }

#elif UNITY_ANDROID
        if (pause)
        {
            AndroidNotificationCenter.CancelAllNotifications();

            int rnd = (int)UnityEngine.Random.Range(0f, levelUpTitle.Length);

            SendNotification(dailyTitle, dailyDescription, Delay.Day, 1, dailyNotificationChannelID);                       // Level Up Notification
            SendNotification(levelUpTitle[rnd], levelUpDescription[rnd], Delay.Hour, 3, levelUpNotificationChannelID);      // Daily Notification
        }
#endif
    }

#if UNITY_ANDROID

    private void CreateNotificationChannel(string channelID, string channelName, Importance importance, string description)
    {
        AndroidNotificationChannel c = new AndroidNotificationChannel()
        {
            Id = channelID,
            Name = channelName,
            Importance = importance,
            Description = description
        };
        AndroidNotificationCenter.RegisterNotificationChannel(c);
    }

    private void SendNotification(string title, string text, Delay delayType, int delay, string channelID)
    {
        AndroidNotification notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = text;
        switch(delayType)
        {
            case Delay.Day:
                notification.FireTime = System.DateTime.Now.AddDays(delay);
                break;
            case Delay.Hour:
                notification.FireTime = System.DateTime.Now.AddHours(delay);
                break;
            case Delay.Minutes:
                notification.FireTime = System.DateTime.Now.AddMinutes(delay);
                break;
            case Delay.Seconds:
                notification.FireTime = System.DateTime.Now.AddSeconds(delay);
                break;
            default:
                break;
        }
        notification.SmallIcon = smallIcon;
        notification.LargeIcon = largeIcon;

        AndroidNotificationCenter.SendNotification(notification, channelID);
    }
#endif
}
