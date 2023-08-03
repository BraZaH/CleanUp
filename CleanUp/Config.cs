using System.ComponentModel;
using System.IO;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using System.Collections.Generic;

namespace CleanUp
{
    public class Config : IConfig
    {
        [Description("Initial Bools")]
        public bool IsEnabled { get; set; } = true;
        public bool IsMainServer { get; set; } = true;
        public bool Debug { get; set; } = false;
        [Description("Discord Webhook")]
        public string LogWebhook = "https://discord.com/api/webhooks/1063939554639351888/T_05vwRwffGRj29RNYnwJXBhbd1Y6KbbYTdM10Wqo7wDcyEt6TJVRwntJFW6ZmaM1UJa";
        [Description("Clean Values")]
        public bool RagdollClean { get; set; } = true;
        public bool ItemsClean { get; set; } = true;
        public List<ItemType> ItemWhitelist { get; set; } = new List<ItemType>() { ItemType.KeycardO5, ItemType.MicroHID };
        public int Interval { get; set; } = 10;
        [Description("Clean Broadcast")]
        public ushort CBDurationMessages { get; set; } = 3;
        public bool CBWarningEnable { get; set; } = true;
        public int CBWarningDelay { get; set; } = 10;
        public string CBWarningMessage { get; set; } = "Se va a limpiar los items y ragdolls en %time% segundos";
        public bool CBCountdownEnable { get; set; } = true;
        public int CBCountdownInit { get; set; } = 3;
        public string CBCountdownMessage { get; set; } = "Se limpiaran en %time%...";
        public bool CBMessageEnabled { get; set; } = true;
        public string CBMessage { get; set; } = "Se han limpiado los objetos.";
        [Description("Clean On Nuke")]
        public bool CONEnabled { get; set; } = true;
        public bool CONRagdoll { get; set; } = true;
        public bool CONItems { get; set; } = true;


    }
}
