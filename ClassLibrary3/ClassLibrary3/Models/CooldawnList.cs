using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary3.Models
{
    public class CooldawnList : IComparable
    {
        public float Cooldown { get; set; }
        public Steamworks.CSteamID SteamIdentifier { get; set; }

        public CooldawnList()
        {
        }

        public CooldawnList(float cooldown, CSteamID steamIdentifier)
        {
            Cooldown = cooldown;
            SteamIdentifier = steamIdentifier;
        }

        public int CompareTo(object obj)
        {
            CooldawnList item = obj as CooldawnList;
            return SteamIdentifier.CompareTo(item.SteamIdentifier);
        }
    }
}
