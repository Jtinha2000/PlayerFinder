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
using System.Collections;
using UnityEngine;
using ClassLibrary3.Models;

namespace ClassLibrary3
{//(x => x.Key == PlayerSource.CSteamID);
    public class Main : RocketPlugin<Config>
    {
        public List<GpsTemplate> truable { get; set; }
        public static Main Instance { get; set; }
        public Dictionary<string, Steamworks.CSteamID> PlayerList { get; set; }
        public List<CooldawnList> CooldawnList { get; set; }
        public List<CooldawnList> CooldawnTargetList { get; set; }
        protected override void Load()  
        {
            truable = new List<GpsTemplate>();
            CooldawnTargetList = new List<CooldawnList>();
            CooldawnList = new List<CooldawnList>();
            Instance = this;
            PlayerList = new Dictionary<string, Steamworks.CSteamID>();
            U.Events.OnPlayerConnected += Events_OnPlayerConnected;
            U.Events.OnPlayerDisconnected += Events_OnPlayerDisconnected;
        }

        private void Events_OnPlayerDisconnected(UnturnedPlayer player)
        {
            if (PlayerList.ContainsValue(player.CSteamID))
            {
                PlayerList.Remove(player.DisplayName);
            }
        }

        private void Events_OnPlayerConnected(UnturnedPlayer player)
        {
            if (Main.Instance.Configuration.Instance.AdmIsTarget == true && player.IsAdmin)
            {
                PlayerList.Add(player.DisplayName, player.CSteamID);
                return;
            }
            else if (player.HasPermission(Main.Instance.Configuration.Instance.PermissionBypass) && Main.Instance.Configuration.Instance.HasPermissionBypass == true)
            {
                return;
            }
            else
            {
                PlayerList.Add(player.DisplayName, player.CSteamID);
            }
        }
        public void CooldawnStartGps(int times, float time, UnturnedPlayer PlayerSource, bool aw, Single Positionbtw, UnturnedPlayer PlayerTarget, UnturnedPlayer PlayerTargetDefinitly)
        {
            StartCoroutine(WaitGps(times, time, PlayerSource, aw, Positionbtw, PlayerTarget, PlayerTargetDefinitly));
        }
        private IEnumerator WaitGps(int times, float time, UnturnedPlayer PlayerSource, bool aw, Single Positionbtw, UnturnedPlayer PlayerTarget, UnturnedPlayer PlayerTargetDefinitly)
        {
            int a  = truable.FindIndex(x => x.SteamId == PlayerSource.CSteamID);
            for (int timer = 0; timer < times; timer++)
            {
                var Positionbtwx = PlayerSource.Position.x - PlayerTargetDefinitly.Position.x;
                var Positionbtwy = PlayerSource.Position.y - PlayerTargetDefinitly.Position.y;
                var Positionbtyz = PlayerSource.Position.z - PlayerTargetDefinitly.Position.z;
                Positionbtw = Positionbtwy + Positionbtyz + Positionbtwx;
                if (truable[a].a == false)
                {
                    timer = times;
                }
                for(int tim = 0; tim < Main.Instance.Configuration.Instance.GpsRepeatCooldowns; tim++)
                {
                    if (Main.Instance.Configuration.Instance.GpsProximtyToCancel == Positionbtw && Main.Instance.Configuration.Instance.HasProximityCancelGps == true)
                    {
                        timer = times;
                    }
                    yield return new WaitForSeconds(1);
                }
                UnturnedChat.Say(PlayerSource, Main.Instance.Translate("Gps_WhileMessage", Positionbtw.ToString("F0")), UnityEngine.Color.red);
                if (Main.Instance.Configuration.Instance.MessageTarget == true)
                {
                    UnturnedChat.Say(PlayerTarget, Main.Instance.Translate("Gps_Target_Warn"), UnityEngine.Color.blue);
                }
            }
            truable.Remove(truable[a]);
            
        }
        public void CooldawnStartTarget(float time, System.Action action, Steamworks.CSteamID a)
        {
            StartCoroutine(WaitTarget(time, action, a));
        }
        private IEnumerator WaitTarget(float time, System.Action action, Steamworks.CSteamID User)
        {
            for (int timer = 0; timer < time; timer++)
            {
                int a = CooldawnTargetList.FindIndex(x => x.SteamIdentifier == User);
                CooldawnTargetList[a].Cooldown = CooldawnTargetList[a].Cooldown - 1;
                yield return new WaitForSeconds(1);
            }
            action();
        }
        public void CooldawnStart(float time, System.Action action, Steamworks.CSteamID a)
        {
            StartCoroutine(Wait(time, action, a));
        }
        private IEnumerator Wait(float time, System.Action action, Steamworks.CSteamID User)
        {
            for (int timer = 0; timer < time; timer++)
            {
                int a = CooldawnList.FindIndex(x => x.SteamIdentifier == User);
                CooldawnList[a].Cooldown = CooldawnList[a].Cooldown - 1;
                yield return new WaitForSeconds(1);
            }
            action();
        }
        protected override void Unload()
        {
            U.Events.OnPlayerConnected -= Events_OnPlayerConnected;
            U.Events.OnPlayerDisconnected -= Events_OnPlayerDisconnected;
        }
        public override TranslationList DefaultTranslations => new TranslationList
        {
            {"Find_LenghtError", "Incorrect command format, try use /find <playername>" },
            {"Find_PlayerNotFind", "Player was not find" },
            {"Find_Help", "A command who find the player who you specify" },
            {"Find_Cooldown", "The FindCommand is in cooldown, please wait {0} seconds" },
            {"Find_Sucess", "The bounty is marked in your map" },
            {"Target_Warn", "You're being hunted" },
            {"Find_PlayerPerCooldown", "The selected player is on personal cooldown, please wait {0}" },
            {"Gps_WhileMessage", "The bounty is {0}M away of you!" },
            {"Gps_LenghtError", "Incorrect command format, try use /find <playername>" },
            {"Gps_PlayerNotFind", "Player was not find" },
            {"Gps_Help", "A command who find the player who you specify" },
            {"Gps_Cooldown", "The FindCommand is in cooldown, please wait {0} seconds" },
            {"Gps_Sucess", "The bounty is marked in your map" },
            {"Gps_Target_Warn", "You're being hunted" },
            {"Gps_Cancel_Help", "Help" },
            {"Gps_Cancel_Lenght", "Lenght Error" },
            {"Gps_PlayerPerCooldown", "The selected player is on personal cooldown, please wait {0}" },
        };

        public static object Instace { get; internal set; }
    }
}
