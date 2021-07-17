using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Provider;
using SDG.Framework;
using Rocket.Core;
using SDG.Unturned;
using Rocket.API.Collections;
using Rocket.Unturned.Events;

namespace ClassLibrary3
{
    public class Config : IRocketPluginConfiguration 
    {
        public string Div { get; set; }
        public float GpsRepeatCooldowns { get; set; }
        public float GpsVehicleRepeatCooldowns { get; set; }
        public int GpsRepeatTimes { get; set; }
        public int GpsVehicleRepeatTimes { get; set; }
        public float FindPerPlayerCooldown { get; set; }
        public float GpsPerPlayerCooldown { get; set; }
        public bool MessageTarget { get; set; }
        public string MarkText { get; set; }
        public bool HasPermissionBypass { get; set; }
        public string PermissionBypass { get; set; }
        public string FindPermission { get; set; }
        public string GpsPermission { get; set; }
        public float FindCommandCooldown { get; set; }
        public float GpsCommandCooldown { get; set; }
        public bool AdmIsTarget { get; set; }
        public int GpsProximtyToCancel { get; set; }
        public bool HasProximityCancelGps { get; set; }
        public void LoadDefaults()
        {
            Div = "GlobalConfig";
            MessageTarget = true;
            AdmIsTarget = true;
            HasPermissionBypass = true;
            MarkText = "Bounty Contract";
            PermissionBypass = "FindPlayers.Bypass";
            Div = "FindCommandConfig";
            FindCommandCooldown = 10;
            FindPerPlayerCooldown = 20;
            FindPermission = "FindPlayer.Commands.Find";
            Div = "GpsCommandConfig";
            GpsCommandCooldown = 10;
            GpsPerPlayerCooldown = 20;
            GpsRepeatCooldowns = 30;
            GpsRepeatTimes = 10;
            GpsVehicleRepeatCooldowns = 30;
            GpsVehicleRepeatTimes = 10;
            GpsPermission = "FindPlayer.Commands.Gps";
            HasProximityCancelGps = true;
            GpsProximtyToCancel = 30;
        }
    }
}
