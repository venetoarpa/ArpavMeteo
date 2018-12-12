using System;
using System.Collections.Generic;

namespace ARPAVTemporali
{
    public class Events
    {
        
        public static string CheckSetting = "CheckSettings";
        public static string UpdateSettings = "UpdateSettings";
        public static string LanguageSelected = "LanguageSelected";
        public static string MapTypeValueChangedLabel = "MapTypeValueChanged";
        public static string ActiveOverlaysValueChangedLabel = "ActiveOverlaysValueChanged";
        public static string OpacityValueChangedLabel = "OpacityValueChanged";
        public static string AnimationSpeedValueChangedLabel = "AnimationSpeedValueChanged";
        public static string AnimationDurationValueChangedLabel = "AnimationDurationValueChanged";
        public static string ComuneValueChanged = "ComuneValueChanged";
        public static string AddComuneLabel = "AddComune";
        public static string MapPlayingStatusChangedLabel = "MapPlayingStatusChanged";
        public static string NewNotificationReceived = "NewNotificationReceived";
        public static string NotificationUpdated = "NotificationUpdated";
        public static string NotificationDeleted = "NotificationDeleted";
        public static string CheckNotificationCount = "CheckNotificationCount";
        public static string NotificationIntervalValueChanged = "NotificationIntervalValueChanged";
        public static string NotificationSoundValueChanged = "NotificationSoundValueChanged";
        public static string NotificationEnabledValueChanged = "NotificationEnabledValueChanged";
        public static string NotificationDNDEnabledValueChanged = "NotificationDNDEnabledValueChanged";
        public static string NotificationDNDFromValueChanged = "NotificationDNDFromValueChanged";
        public static string NotificationDNDToValueChanged = "NotificationDNDToValueChanged";
        public static string ToggleLayerMenu = "ToggleLayerMenu";
        public static string ShareMap = "ShareMap";
        public static string OnSleep = "OnSleep";
        //remote calls event
        public static string FetchOverlaysDone = "FetchOverlaysDone";
    }
}
