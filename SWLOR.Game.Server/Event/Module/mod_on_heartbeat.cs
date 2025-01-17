﻿using SWLOR.Game.Server.Event.Module;
using SWLOR.Game.Server.Messaging;


// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
#pragma warning disable IDE1006 // Naming Styles
    internal static class mod_on_heartbeat
#pragma warning restore IDE1006 // Naming Styles
    {
        // ReSharper disable once UnusedMember.Local
        public static void Main()
        {
            MessageHub.Instance.Publish(new OnModuleHeartbeat());
        }

    }
}
