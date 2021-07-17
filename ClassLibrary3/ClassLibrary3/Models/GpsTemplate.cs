using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary3.Models
{
    public class GpsTemplate
    {
        public Steamworks.CSteamID SteamId { get; set; }
        public bool a { get; set; }

        public GpsTemplate(CSteamID steamId, bool a)
        {
            SteamId = steamId;
            this.a = a;
        }
    }
}
