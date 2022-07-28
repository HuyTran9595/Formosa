using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS
using Unity.Notifications.iOS;
using System;
#endif
#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif
public class NotoficationsManager : MonoBehaviour
{
#if UNITY_ANDROID
    private static bool primarySetup = false;
    private static AndroidNotificationChannel primaryChannel;
    private static void SetupChannel()
    {
        if (primarySetup == false)
        {

            primaryChannel.Id = "channel_0";
            primaryChannel.Name = "Primary Channel";
            primaryChannel.Importance = Importance.High;
            primaryChannel.Description = "Generic notifications";
            AndroidNotificationCenter.RegisterNotificationChannel(primaryChannel);
            primarySetup = true;
        }
    }

#endif
#if UNITY_IOS
    private static iOSNotificationTimeIntervalTrigger timeTrigger;
    private static void SetupTrigger(int hours, int minutes, int seconds)
    {

        timeTrigger.TimeInterval = new System.TimeSpan(hours, minutes, seconds);
        timeTrigger.Repeats = false;
    }
#endif
    public static void SendNotofication(string name, string text, float secondsToSend)
    {
#if UNITY_ANDROID
        {
            SetupChannel();
            var notification = new AndroidNotification();
            notification.Title = name;
            notification.Text = text;
            notification.FireTime = System.DateTime.Now.AddSeconds(secondsToSend);

            AndroidNotificationCenter.SendNotification(notification, primaryChannel.Id);
        }
#endif

#if UNITY_IOS
        {
            SetupTrigger(0, 0, (int)secondsToSend);

            iOSNotification notification = new iOSNotification();
            notification.Identifier = "_notification_01";
            notification.Title = name;
            notification.Body = text;
            notification.Subtitle = "Test notofication";
            notification.ShowInForeground = true;
            notification.ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound);
            notification.CategoryIdentifier = "category_a";
            notification.ThreadIdentifier = "thread1";

            iOSNotificationCenter.ScheduleNotification(notification);
        }
#endif
    }
}
