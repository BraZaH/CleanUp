using System;
using System.Net;
using System.Collections.Specialized;
using System.Collections.Generic;
using Exiled.API.Features;
using MEC;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Warhead;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;

namespace CleanUp
{
    public class EventHandler
    {
        CoroutineHandle coroutine = new CoroutineHandle();

        static List<ushort> itemsToClear = new List<ushort>();

        public void OnRoundStart()
        {
            coroutine = Timing.RunCoroutine(NextClean());
        }

        public void OnRoundEnd(EndingRoundEventArgs ev)
        {
            Timing.KillCoroutines(coroutine);
        }

        public void OnDying(DyingEventArgs ev)
        {
            foreach (var i in ev.ItemsToDrop)
            {
                itemsToClear.Add(i.Serial);
            }
        }

        public void DropItem(DroppingItemEventArgs ev)
        {
            itemsToClear.Add(ev.Item.Serial);
        }

        public void PickupItem(PickingUpItemEventArgs ev)
        {
            if (itemsToClear.Contains(ev.Pickup.Serial))
                itemsToClear.Remove(ev.Pickup.Serial);
        }

        public void OnNuke(DetonatingEventArgs ev)
        {
            if (ev.IsAllowed)
                Log.Debug("Kaboom");
        }

        public IEnumerator<float> NextClean()
        {
            int interval = Main.Singleton.Config.Interval;
            if (Main.Singleton.Config.CBWarningEnable)
                interval -= Main.Singleton.Config.CBWarningDelay;
            if (Main.Singleton.Config.CBCountdownEnable)
                interval -= Main.Singleton.Config.CBCountdownInit;

            for(; ; )
            {
                yield return Timing.WaitForSeconds(interval);
                if (Main.Singleton.Config.CBWarningEnable)
                {
                    string message = Main.Singleton.Config.CBWarningMessage.Replace("%time%", Main.Singleton.Config.CBWarningDelay.ToString());
                    Log.Debug(message);
                    Map.Broadcast(Main.Singleton.Config.CBDurationMessages, message,Broadcast.BroadcastFlags.Normal,true);
                    yield return Timing.WaitForSeconds(Main.Singleton.Config.CBWarningDelay);
                }
                if (Main.Singleton.Config.CBCountdownEnable)
                {
                    for (int i = 0; i < Main.Singleton.Config.CBCountdownInit; i++)
                    {
                        string msg = Main.Singleton.Config.CBCountdownMessage.Replace("%time%", (Main.Singleton.Config.CBCountdownInit - i).ToString());
                        Map.Broadcast(Main.Singleton.Config.CBDurationMessages, msg, Broadcast.BroadcastFlags.Normal, true);
                        yield return Timing.WaitForSeconds(1f);
                    }
                }
                Clean();
                Map.Broadcast(Main.Singleton.Config.CBDurationMessages, Main.Singleton.Config.CBMessage, Broadcast.BroadcastFlags.Normal, true);
            }
        }

        public static string Clean()
        {
            try
            {
                int itemsCleaned = 0;
                if (Main.Singleton.Config.ItemsClean)
                {
                    foreach (var i in itemsToClear)
                    {
                        Pickup item = Pickup.Get(i);
                        if (!Main.Singleton.Config.ItemWhitelist.Contains(item.Type))
                        {
                            Log.Debug(item.Serial + " - " + item.Type);
                            item.Destroy();
                            itemsCleaned++;
                        }
                    }
                }

                Log.Debug($"Items cleaned: {itemsCleaned}");
                itemsToClear.Clear();
                if (Main.Singleton.Config.RagdollClean)
                    Server.RunCommand("/cleanup ragdolls", Main._serverSender);

                string msg = $"Limpieza de items completada => han limpiado {itemsCleaned} items";
                Main.sendDiscordWebhook(Main.Singleton.Config.LogWebhook, "","CleanUp Plugin", msg);

                return msg;
            }
            catch (Exception e)
            {
                return $"[Error] Hubo un error al limpiar | (Eventhandler:Clean)\nError: {e}";
                throw;
            }
        }

    }
}
