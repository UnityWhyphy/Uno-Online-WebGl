//using System;
//using System.Collections;
//using UnityEngine;

//#if UNITY_IOS
//using Unity.Notifications.iOS;
//#endif

//namespace Game.Modules.LocalNotifcation
//{
//    public class IosNotificaitonController : MonoBehaviour
//    {
//        public static IosNotificaitonController instance;

//        private void Awake()
//        {
//            instance = this;
//        }

//#if UNITY_IOS
//        public IEnumerator RequestAuthorization(/*Action onCompleted*/)
//        {
//            Logger.Print($"======>> RequestAuthorization Called <<========");
//            using var req = new AuthorizationRequest(
//                AuthorizationOption.Alert | AuthorizationOption.Badge,
//                true);

//            while (!req.IsFinished)
//            {
//                yield return null;
//            }

//            //onCompleted?.Invoke();
//        }


//        //        public void ScheduleNotificaiton(LocalNotificationData data, bool isRepeat, string identifier, string categoryIdentifier = "default_category"
//        //            , string threadIdentifier = "thread1")
//        //        {
//        //            var timeTrigger = new iOSNotificationTimeIntervalTrigger()
//        //            {
//        //                TimeInterval = new TimeSpan(data.days, data.hours, data.min, data.sec),
//        //                Repeats = isRepeat
//        //            };

//        //            var notification = new iOSNotification()
//        //            {
//        //                Identifier = identifier,
//        //                Title = data.title,
//        //                Body = data.body,
//        //                Subtitle = data.subtitle,
//        //                ShowInForeground = true,
//        //                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
//        //                CategoryIdentifier = categoryIdentifier,
//        //                ThreadIdentifier = threadIdentifier,
//        //                Trigger = timeTrigger
//        //            };
//        //            notification.ShowInForeground = true;
//        //            iOSNotificationCenter.ScheduleNotification(notification);
//        //            Debug.Log("Notification Sceduled for: " + notification.Trigger.ToJson());

//        //            //Saving notificaitonID so, it can be canceled or resceduled whenever user launch app
//        //            PlayerPrefs.SetString(data.notificationIDKey, notification.Identifier);
//        //        }

//        //        /// <summary>
//        //        /// 
//        //        /// </summary>
//        //        /// <param name="data"></param>
//        //        /// <param name="repeatDayCycle"></param>
//        //        public void ScheduleDailyNotification(LocalNotificationData data, string identifier, string categoryIdentifier = "default_category"
//        //            , string threadIdentifier = "thread1")
//        //        {
//        //            var timeTrigger = new iOSNotificationTimeIntervalTrigger()
//        //            {
//        //                TimeInterval = new TimeSpan(GetNextTriggerTime(1, data.hours).Ticks - DateTime.Now.Ticks),//16 represent 4pm
//        //                Repeats = true
//        //            };

//        //            Debug.Log(" time: " + DateTime.Now.AddMinutes(data.min));

//        //            var notification = new iOSNotification()
//        //            {
//        //                Identifier = identifier,
//        //                Title = data.title,
//        //                Body = data.body,
//        //                Subtitle = data.subtitle,
//        //                ShowInForeground = true,
//        //                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
//        //                CategoryIdentifier = categoryIdentifier,
//        //                ThreadIdentifier = threadIdentifier,
//        //                Trigger = timeTrigger
//        //            };

//        //            iOSNotificationCenter.ScheduleNotification(notification);
//        //            Debug.Log("Notification Sceduled for daily: " + notification.Trigger.ToJson());

//        //            //Saving notificaitonID so, it can be canceled or resceduled whenever user launch app
//        //            PlayerPrefs.SetString(data.notificationIDKey, notification.Identifier);
//        //        }

//        //        /// <summary>
//        //        /// Sending notificaton on every specific day in week.
//        //        /// </summary>
//        //        /// <param name="data"></param>
//        //        /// <param name="repeatDayCycle"></param>
//        //        /// <param name="dayOfWeek"></param>
//        //        public void ScheduleWeeklyNotification(LocalNotificationData data, DayOfWeek dayOfWeek, string identifier, string categoryIdentifier = "default_category"
//        //            , string threadIdentifier = "thread1")
//        //        {
//        //            var timeTrigger = new iOSNotificationTimeIntervalTrigger()
//        //            {
//        //                TimeInterval = new TimeSpan(GetNextDay(dayOfWeek, 22).Ticks - DateTime.Now.Ticks), // 22 represent 10pm
//        //                Repeats = true,
//        //            };

//        //            var notification = new iOSNotification()
//        //            {
//        //                Identifier = identifier,
//        //                Title = data.title,
//        //                Body = data.body,
//        //                Subtitle = data.subtitle,
//        //                ShowInForeground = true,
//        //                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
//        //                CategoryIdentifier = categoryIdentifier,
//        //                ThreadIdentifier = threadIdentifier,
//        //                Trigger = timeTrigger
//        //            };

//        //            iOSNotificationCenter.ScheduleNotification(notification);
//        //            Debug.Log("Notification Sceduled for daily: " + notification.Trigger.ToJson());

//        //            //Saving notificaitonID so, it can be canceled or resceduled whenever user launch app
//        //            PlayerPrefs.SetString(data.notificationIDKey, notification.Identifier);
//        //        }

//        //        /// <summary>
//        //        /// Return time after slectiong specific day in week with specific time of day.
//        //        /// </summary>
//        //        /// <param name="dayOfWeek"></param>
//        //        /// <param name="hrs"></param>
//        //        /// <param name="min"></param>
//        //        /// <param name="sec"></param>
//        //        /// <returns></returns>
//        //        private DateTime GetNextDay(DayOfWeek dayOfWeek, int hrs = 0, int min = 0, int sec = 0)
//        //        {
//        //            DateTime today = DateTime.Now.Date;
//        //            int daysUntilDay = ((int)dayOfWeek - (int)today.DayOfWeek + 7) % 7;

//        //            DateTime nextDay = today.AddDays(daysUntilDay).AddHours(hrs).AddMinutes(min).AddSeconds(sec);

//        //            if (nextDay <= DateTime.Now)
//        //            {
//        //                nextDay = nextDay.AddDays(7);
//        //            }

//        //            return nextDay;
//        //        }

//        //        /// <summary>
//        //        /// Return next targeted time if the specified time has already passed today, set it for tomorrow.
//        //        /// </summary>
//        //        /// <param name="hrs"></param>
//        //        /// <param name="min"></param>
//        //        /// <param name="sec"></param>
//        //        /// <returns></returns>
//        //        private DateTime GetNextTriggerTime(int days, int hrs = 0, int min = 0, int sec = 0)
//        //        {
//        //            DateTime nextTriggerTime = DateTime.Now.Date; // Start from today
//        //            nextTriggerTime = nextTriggerTime.AddHours(hrs).AddMinutes(min).AddSeconds(sec);
//        //            nextTriggerTime = nextTriggerTime.AddDays(days);
//        //            /*
//        //            if (nextTriggerTime <= DateTime.Now)
//        //            {
//        //                nextTriggerTime = nextTriggerTime.AddDays(days);
//        //            }
//        //            */
//        //            return nextTriggerTime;
//        //        }
//#endif
//        //}
//    }
//}