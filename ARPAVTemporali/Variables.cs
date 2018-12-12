using System;
using System.Collections.Generic;
using Xamarin.Forms.GoogleMaps;
using ARPAVTemporali.Models;
using Xamarin.Forms;
using ARPAVTemporali.Localization;

namespace ARPAVTemporali
{
    public class Variables
    {

        public Variables()
        {

        }

        // latest db version. used to check if an update is necessary
        public static string db_version = "2018-05-03";

        public static string HTMLBaseStyle =
            "<style>" +
                "*{font-family:helvetica, sans-serif;}" +
                "body{background-color:#ffffff;color:#000000}" +
                "a:link{color: #007267;}" +
            "</style>"; //default style for webviews
        public static int MaxNotificationNumber = 20; //numero massimo di notifiche da visualizzare
        public static string DatabaseName = "db.sqlite"; // "db.development.sqlite";
        //public static string ApplicationServerURL = "http://radaralert.arpa.veneto.it";
        public static string ApplicationServerURL = "http://192.168.1.111:3000";
        public static Position InitialPosition = new Position(45.4095409, 11.8765508); // Padova = 45.4095409, 11.8765508
        public static Bounds BoundingBoxMosaico = new Bounds(new Position(44.0, 9.6), new Position(47.2, 15.0));
        public static Bounds BoundingBoxFulmini = new Bounds(new Position(44.5, 10.0), new Position(47.0, 14.0));
        public static double InitialMapZoom = 7d;
        public static Dictionary<string, string> MapTypes
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    {AppResources.Variables_MapTypes_Standard,"Street"},
                    {AppResources.Variables_MapTypes_Rilievo, "Terrain"},
                    {AppResources.Variables_MapTypes_Satellite, "Satellite"},
                    {AppResources.Variables_MapTypes_Ibrida, "Hybrid"},
                };
            }
        }
        public static Dictionary<string, string> Sounds
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    { AppResources.Variables_Sounds_tuono, "thunder1.mp3"},
                    { "default", ""},
                };
            }
        }

        public static Dictionary<string, int> AnimationSpeeds
        {
            get
            {
                return new Dictionary<string, int>()
                {
                    {AppResources.Variables_AnimationSpeeds_lenta, 3},
                    {AppResources.Variables_AnimationSpeeds_media, 2},
                    {AppResources.Variables_AnimationSpeeds_rapida, 1},
                };
            }
        }
        public static Dictionary<string, int> AnimationDurations
        {
            get
            {
                return new Dictionary<string, int>()
                {
                    {"1 " + AppResources.Variables_ora, 60}, //1 ora
                    {"2 " + AppResources.Variables_ore, 120}, //2 ore
                    {"3 " + AppResources.Variables_ore, 180}, //3 ore
                };
            }
        }

        public static List<double> Ranges = new List<double> { 10, 20, 30, 40, 50 }; //raggio
        //public static List<int> Intervals = new List<int> { 1, 30, 60, 120 }; //intervallo tra le notifiche
        public static Dictionary<string, int> Intervals
        {
            get
            {
                return new Dictionary<string, int>()
                {
                    {"10 " + AppResources.Variables_minuti, 10},
                    {"30 " + AppResources.Variables_minuti, 30},
                    {"1 " + AppResources.Variables_ora, 60},
                    {"2 " + AppResources.Variables_ore, 120},
                };
            }
        }

        public static Dictionary<string, int> Overlays
        {
            get
            {
                return new Dictionary<string, int>()
                {
                    {AppResources.Variables_Overlay_Radar, 0},
                    {AppResources.Variables_Overlay_Fulmini, 1},
                    {AppResources.Variables_Overlay_Ibrida, 2},
                };
            }
        }

        public static Dictionary<string, int> Intensities
        {
            get
            {
                return new Dictionary<string, int>()
                {
                    {AppResources.Variables_Intensita_Intenso, 0},
                    {AppResources.Variables_Intensita_MoltoIntenso,1},
                };
            }
        }
    }
}
