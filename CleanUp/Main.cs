using System;
using System.Net;
using System.Collections.Specialized;
using System.Collections.Generic;
using Exiled.API.Features;
using MEC;

namespace CleanUp
{
    public class Main : Plugin<Config>
    {
        public override string Name => "CleanUp";
        public override string Author => "BraZa";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(7, 0, 0);

        public static Main Singleton;
        public EventHandler EventHandler;
        public static ServerConsoleSender _serverSender = new ServerConsoleSender();

        public override void OnEnabled()
        {
            Singleton = this;
            EventHandler = new EventHandler();
            Exiled.Events.Handlers.Server.RoundStarted += EventHandler.OnRoundStart;
            Exiled.Events.Handlers.Server.EndingRound += EventHandler.OnRoundEnd;
            Exiled.Events.Handlers.Player.DroppingItem += EventHandler.DropItem;
            Exiled.Events.Handlers.Player.PickingUpItem += EventHandler.PickupItem;
            Exiled.Events.Handlers.Player.Dying += EventHandler.OnDying;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Singleton = null;
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandler.OnRoundStart;
            Exiled.Events.Handlers.Server.EndingRound -= EventHandler.OnRoundEnd;
            Exiled.Events.Handlers.Player.DroppingItem -= EventHandler.DropItem;
            Exiled.Events.Handlers.Player.PickingUpItem -= EventHandler.PickupItem;
            Exiled.Events.Handlers.Player.Dying -= EventHandler.OnDying;

            base.OnDisabled();

        }

        public static void sendDiscordWebhook(string URL, string profilepic, string username, string message)
        {
            NameValueCollection discordValues = new NameValueCollection();
            discordValues.Add("username", username);
            discordValues.Add("avatar_url", profilepic);
            discordValues.Add("content", message);
            new WebClient().UploadValues(URL, discordValues);
        }

    }

}
