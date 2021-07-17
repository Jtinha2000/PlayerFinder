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
using UnityEngine;
using System.Collections;
using ClassLibrary3.Models;

namespace ClassLibrary3.Commands
{
    class GpsCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "Gps";

        public string Help => Main.Instance.Translate("Gps_Help");

        public string Syntax => "/Gps <Name>";

        public List<string> Aliases => new List<string> { "Rastrear", "Gps" };

        public List<string> Permissions => new List<string> { "FindPlayer.Commands.Gps" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length < 1)
            {
                UnturnedChat.Say(caller, Main.Instance.Translate("Gps_LenghtError"), UnityEngine.Color.red);
                return;
            }
            UnturnedPlayer PlayerSource = UnturnedPlayer.FromName(caller.DisplayName);
            int CatchHelper = 0;
            try
            {
                CooldawnList CooldawnIdentifier = Main.Instance.CooldawnList.First(x => x.SteamIdentifier == PlayerSource.CSteamID);
            }
            catch (Exception)
            {
                CatchHelper = 1;
            }
            if (CatchHelper == 0)
            {
                CooldawnList CooldawnIdentifier = Main.Instance.CooldawnList.First(x => x.SteamIdentifier == PlayerSource.CSteamID);
                UnturnedChat.Say(caller, Main.Instance.Translate("Gps_Cooldown", CooldawnIdentifier.Cooldown), UnityEngine.Color.red);
                return;
            }
            int po = 0;
            UnturnedPlayer PlayerTarget = UnturnedPlayer.FromName(command[0]);
            for (int timer = 0; Main.Instance.CooldawnTargetList.Count > timer; timer++)
            {
                if (Main.Instance.CooldawnTargetList[timer].SteamIdentifier == PlayerTarget.CSteamID)
                {
                    po = 1;
                }
            }
            try
            {
                if (po == 0)
                {
                    KeyValuePair<string, Steamworks.CSteamID> PlayerTargetPoint = Main.Instance.PlayerList.First(x => x.Value == PlayerTarget.CSteamID);
                    UnturnedPlayer PlayerTargetDefinitly = UnturnedPlayer.FromCSteamID(PlayerTargetPoint.Value);
                    var Positionbtwx = PlayerSource.Position.x - PlayerTargetDefinitly.Position.x;
                    var Positionbtwy = PlayerSource.Position.y - PlayerTargetDefinitly.Position.y;
                    var Positionbtyz = PlayerSource.Position.z - PlayerTargetDefinitly.Position.z;
                    var Positionbtw = Positionbtwy + Positionbtyz + Positionbtwx;
                    UnturnedChat.Say(PlayerSource, Main.Instance.Translate("Gps_WhileMessage", Positionbtw.ToString("F0")), UnityEngine.Color.red);
                    Main.Instance.truable.Add(new GpsTemplate(PlayerSource.CSteamID, true));
                    if (Main.Instance.Configuration.Instance.MessageTarget == true)
                    {
                        UnturnedChat.Say(PlayerTarget, Main.Instance.Translate("Gps_Target_Warn"), UnityEngine.Color.blue);
                    }
                    UnturnedChat.Say(PlayerSource, Main.Instance.Translate("Gps_Sucess", UnityEngine.Color.blue));
                    Main.Instance.CooldawnTargetList.Add(new CooldawnList(Main.Instance.Configuration.Instance.GpsPerPlayerCooldown, PlayerTargetDefinitly.CSteamID));
                    CooldawnList CourotineParametersTarget = Main.Instance.CooldawnTargetList.First(x => x.SteamIdentifier == PlayerTargetDefinitly.CSteamID);
                    Main.Instance.CooldawnStartTarget(CourotineParametersTarget.Cooldown, () =>
                    {
                        Main.Instance.CooldawnTargetList.Remove(CourotineParametersTarget);
                    }, CourotineParametersTarget.SteamIdentifier);
                    Main.Instance.CooldawnList.Add(new CooldawnList(Main.Instance.Configuration.Instance.GpsCommandCooldown, PlayerSource.CSteamID));
                    CooldawnList CourotineParameters = Main.Instance.CooldawnList.First(x => x.SteamIdentifier == PlayerSource.CSteamID);
                    Main.Instance.CooldawnStart(CourotineParameters.Cooldown, () =>
                    {
                        Main.Instance.CooldawnList.Remove(CourotineParameters);
                    }, CourotineParameters.SteamIdentifier);
                    if (PlayerTarget.IsInVehicle)
                    {
                        Main.Instance.CooldawnStartGps(Main.Instance.Configuration.Instance.GpsVehicleRepeatTimes, Main.Instance.Configuration.Instance.GpsVehicleRepeatCooldowns, PlayerSource, true, Positionbtw, PlayerTarget, PlayerTargetDefinitly);
                    }
                    else
                    {
                        Main.Instance.CooldawnStartGps(Main.Instance.Configuration.Instance.GpsRepeatTimes, Main.Instance.Configuration.Instance.GpsRepeatCooldowns, PlayerSource, true, Positionbtw, PlayerTarget, PlayerTargetDefinitly);
                    }
                }
                else
                {
                    int a = Main.Instance.CooldawnTargetList.FindIndex(x => x.SteamIdentifier == PlayerTarget.CSteamID);
                    UnturnedChat.Say(caller, Main.Instance.Translate("Gps_PlayerPerCooldown", Main.Instance.CooldawnTargetList[a].Cooldown, UnityEngine.Color.red));
                }
            }
            catch (Exception)
            {
                UnturnedChat.Say(caller, Main.Instance.Translate("Gps_PlayerNotFind", UnityEngine.Color.red));
            }
        }
    }
}
