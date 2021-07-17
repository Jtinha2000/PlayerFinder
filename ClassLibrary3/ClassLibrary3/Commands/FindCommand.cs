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
    class FindCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "Find";

        public string Help => Main.Instance.Translate("Find_Help");

        public string Syntax => "/Find <Name>";

        public List<string> Aliases => new List<string> {"Find", "Achar", "Caçar", "Follow", "Descobrir", "Localizar", "Localização"};

        public List<string> Permissions => new List<string> { "FindPlayer.Commands.Find" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length < 1)
            {
                UnturnedChat.Say(caller, Main.Instance.Translate("Find_LenghtError"), UnityEngine.Color.red);
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
                UnturnedChat.Say(caller, Main.Instance.Translate("Find_Cooldown", CooldawnIdentifier.Cooldown), UnityEngine.Color.red);
                return;
            }
            int po = 0;
            UnturnedPlayer PlayerTarget = UnturnedPlayer.FromName(command[0]);
            for (int timer = 0;  Main.Instance.CooldawnTargetList.Count > timer; timer++)
            {
                if (Main.Instance.CooldawnTargetList[timer].SteamIdentifier == PlayerTarget.CSteamID)
                {
                    po = 1;
                }
            }
            try
            {
                if(po == 0)
                {
                    KeyValuePair<string, Steamworks.CSteamID> PlayerTargetPoint = Main.Instance.PlayerList.First(x => x.Value == PlayerTarget.CSteamID);
                    UnturnedPlayer PlayerTargetDefinitly = UnturnedPlayer.FromCSteamID(PlayerTargetPoint.Value);
                    PlayerSource.Player.quests.replicateSetMarker(true, PlayerTargetDefinitly.Position, Main.Instance.Configuration.Instance.MarkText);
                    if (Main.Instance.Configuration.Instance.MessageTarget == true)
                    {
                        UnturnedChat.Say(PlayerTarget, Main.Instance.Translate("Target_Warn"), UnityEngine.Color.blue);
                    }
                    UnturnedChat.Say(PlayerSource, Main.Instance.Translate("Find_Sucess", UnityEngine.Color.blue));
                    Main.Instance.CooldawnTargetList.Add(new CooldawnList(Main.Instance.Configuration.Instance.FindPerPlayerCooldown, PlayerTargetDefinitly.CSteamID));
                    CooldawnList CourotineParametersTarget = Main.Instance.CooldawnTargetList.First(x => x.SteamIdentifier == PlayerTargetDefinitly.CSteamID);
                    Main.Instance.CooldawnStartTarget(CourotineParametersTarget.Cooldown, () =>
                    {
                        Main.Instance.CooldawnTargetList.Remove(CourotineParametersTarget);
                    }, CourotineParametersTarget.SteamIdentifier);
                    Main.Instance.CooldawnList.Add(new CooldawnList(Main.Instance.Configuration.Instance.FindCommandCooldown, PlayerSource.CSteamID));
                    CooldawnList CourotineParameters = Main.Instance.CooldawnList.First(x => x.SteamIdentifier == PlayerSource.CSteamID);
                    Main.Instance.CooldawnStart(CourotineParameters.Cooldown, () =>
                    {
                        Main.Instance.CooldawnList.Remove(CourotineParameters);
                    }, CourotineParameters.SteamIdentifier);
                }
                else
                {
                    int a = Main.Instance.CooldawnTargetList.FindIndex(x => x.SteamIdentifier == PlayerTarget.CSteamID);
                    UnturnedChat.Say(caller, Main.Instance.Translate("Find_PlayerPerCooldown", Main.Instance.CooldawnTargetList[a].Cooldown, UnityEngine.Color.red));
                }
            }
            catch (Exception)
            {
                UnturnedChat.Say(caller, Main.Instance.Translate("Find_PlayerNotFind", UnityEngine.Color.red));
            }
        }
    }
}
    