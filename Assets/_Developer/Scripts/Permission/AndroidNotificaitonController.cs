using System;
using UnityEngine;
using UnityEngine.Android;

#if UNITY_ANDROID
using Unity.Notifications.Android;
using UnityEngine.Android;
#endif

namespace Game.Modules.LocalNotifcation
{
    public class AndroidNotificaitonController : MonoBehaviour
    {

        public static AndroidNotificaitonController instance;

        private void Awake()
        {
            instance = this;
        }
        private string channelName;

        public void RequestAutorization()
        {
            if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
            {
                Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
            }
            Debug.Log("Notificaiton Authorization is done");
        }
#if UNITY_ANDROID

        //public void RegisterNotificaitonChannel(string channelId = "defaut_channel", string channelName = "Default Channel",
        //    string description = "Genric Notificaitons")
        //{
        //    this.channelName = channelId;
        //    var channel = new AndroidNotificationChannel()
        //    {
        //        Id = channelId,
        //        Name = channelName,
        //        Importance = Importance.High,
        //        Description = description
        //    };
        //    AndroidNotificationCenter.RegisterNotificationChannel(channel);
        //    Debug.Log(channelId + " :Notificaiton Channel Registered: " + channelName);
        //}

        //public void ScheduleNotificaiton(LocalNotificationData data)
        //{
        //    DateTime dateToFire = DateTime.Now;
        //    var notification = new AndroidNotification();
        //    notification.Title = data.title;
        //    notification.Text = data.body;
        //    notification.ShowTimestamp = true;
        //    notification.Style = NotificationStyle.BigTextStyle;
        //    notification.LargeIcon = data.largeIcon;
        //    notification.SmallIcon = data.smallIcon;
        //    notification.FireTime = dateToFire.AddHours(data.hours).AddMinutes(data.min).AddSeconds(data.sec);
        //    notification.IntentData = "{\"title\": \"Notification Cli\", \"data\": \"\"}";
        //    int notificaitonID = AndroidNotificationCenter.SendNotification(notification, channelName);
        //    Debug.Log("Notification Sceduled for: " + notification.FireTime);

        //    //Saving notificaitonID so, it can be canceled or resceduled whenever user launch app
        //    PlayerPrefs.SetInt(data.notificationIDKey, notificaitonID);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="data"></param>
        ///// <param name="repeatDayCycle"></param>
        //public void ScheduleDailyNotification(LocalNotificationData data, int repeatDayCycle)
        //{
        //    DateTime dateToFire = DateTime.Now;
        //    var notification = new AndroidNotification();
        //    notification.Title = data.title;
        //    notification.Text = data.body;
        //    notification.ShowTimestamp = true;
        //    notification.Style = NotificationStyle.BigTextStyle;
        //    notification.LargeIcon = data.largeIcon;
        //    notification.SmallIcon = data.smallIcon;
        //    notification.RepeatInterval = TimeSpan.FromDays(repeatDayCycle); // Repeat every "repeatDayCycle" days
        //    notification.FireTime = GetNextTriggerTime(1, data.hours, data.min, data.sec);

        //    int notificaitonID = AndroidNotificationCenter.SendNotification(notification, channelName);
        //    Debug.Log("Notification Sceduled for day: " + notification.FireTime + " and repeat interval will be: " + notification.RepeatInterval);

        //    //Saving notificaitonID so, it can be canceled or resceduled whenever user launch app
        //    PlayerPrefs.SetInt(data.notificationIDKey, notificaitonID);
        //}

        ///// <summary>
        ///// Sending notificaton on every specific day in week.
        ///// </summary>
        ///// <param name="data"></param>
        ///// <param name="repeatDayCycle"></param>
        ///// <param name="dayOfWeek"></param>
        //public void ScheduleWeeklyNotification(LocalNotificationData data, int repeatDayCycle, DayOfWeek dayOfWeek)
        //{
        //    DateTime dateToFire = DateTime.Now;
        //    var notification = new AndroidNotification();

        //    notification.Title = data.title;
        //    notification.Text = data.body;
        //    notification.ShowTimestamp = true;
        //    notification.Style = NotificationStyle.BigTextStyle;
        //    notification.LargeIcon = data.largeIcon;
        //    notification.SmallIcon = data.smallIcon;
        //    notification.RepeatInterval = TimeSpan.FromDays(repeatDayCycle); // Repeat every "repeatDayCycle" days
        //    notification.FireTime = GetNextDay(dayOfWeek, data.hours, data.min, data.sec);

        //    int notificaitonID = AndroidNotificationCenter.SendNotification(notification, channelName);
        //    Debug.Log("Notification Sceduled for week: " + notification.FireTime + " and repeat interval will be: " + notification.RepeatInterval);

        //    //Saving notificaitonID so, it can be canceled or resceduled whenever user launch app
        //    PlayerPrefs.SetInt(data.notificationIDKey, notificaitonID);
        //}

        ///// <summary>
        ///// Return time after slectiong specific day in week with specific time of day.
        ///// </summary>
        ///// <param name="dayOfWeek"></param>
        ///// <param name="hrs"></param>
        ///// <param name="min"></param>
        ///// <param name="sec"></param>
        ///// <returns></returns>
        //private DateTime GetNextDay(DayOfWeek dayOfWeek, int hrs = 0, int min = 0, int sec = 0)
        //{
        //    DateTime today = DateTime.Now.Date;
        //    int daysUntilDay = ((int)dayOfWeek - (int)today.DayOfWeek + 7) % 7;

        //    DateTime nextDay = today.AddDays(daysUntilDay).AddHours(hrs).AddMinutes(min).AddSeconds(sec);

        //    if (nextDay <= DateTime.Now)
        //    {
        //        nextDay = nextDay.AddDays(7);
        //    }

        //    return nextDay;
        //}

        ///// <summary>
        ///// Return next targeted time if the specified time has already passed today, set it for tomorrow.
        ///// </summary>
        ///// <param name="hrs"></param>
        ///// <param name="min"></param>
        ///// <param name="sec"></param>
        ///// <returns></returns>
        //private DateTime GetNextTriggerTime(int days, int hrs = 0, int min = 0, int sec = 0)
        //{
        //    DateTime nextTriggerTime = DateTime.Now.Date; // Start from today
        //    nextTriggerTime = nextTriggerTime.AddHours(hrs).AddMinutes(min).AddSeconds(sec);
        //    nextTriggerTime = nextTriggerTime.AddDays(days);

        //    /*
        //    if (nextTriggerTime <= DateTime.Now)
        //    {
        //        nextTriggerTime = nextTriggerTime.AddDays(days);
        //    }
        //    */
        //    return nextTriggerTime;
        //}
#endif
    }
}