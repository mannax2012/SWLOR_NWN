﻿using System;
using SWLOR.Game.Server.Data.Entity;
using SWLOR.Game.Server.Enumeration;
using SWLOR.Game.Server.Event.Module;
using SWLOR.Game.Server.GameObject;
using SWLOR.Game.Server.Messaging;
using SWLOR.Game.Server.ValueObject;
using static SWLOR.Game.Server.NWN._;


namespace SWLOR.Game.Server.Service
{
    public static class AuthorizationService
    {
        public static void SubscribeEvents()
        {
            MessageHub.Instance.Subscribe<OnModuleEnter>(OnModuleEnter);
        }

        private static void OnModuleEnter(OnModuleEnter @event)
        {
            NWPlayer dm = GetEnteringObject();
            if (!dm.IsDM) return;

            var cdKey = GetPCPublicCDKey(dm);
            var adminCDKeySetting = Environment.GetEnvironmentVariable("SWLOR_ADMIN_CD_KEY") ?? string.Empty;

            if (adminCDKeySetting.ToUpper() == cdKey.ToUpper())
            {
                LogDMAuthorization(dm, true);
                return;
            }

            var entity = DataService.AuthorizedDM.GetByCDKeyAndActiveOrDefault(cdKey);
            if (entity == null || !entity.IsActive)
            {
                LogDMAuthorization(dm, false);
                BootPC(dm, "You are not authorized to log in as a DM. Please contact the server administrator if this is incorrect.");
                return;
            }

            LogDMAuthorization(dm, true);
        }

        public static DMAuthorizationType GetDMAuthorizationType(NWPlayer player)
        {
            if (!player.IsPlayer && !player.IsDM) return DMAuthorizationType.None;

            string cdKey = GetPCPublicCDKey(player);
            var adminCDKeySetting = Environment.GetEnvironmentVariable("SWLOR_ADMIN_CD_KEY") ?? string.Empty;

            if (adminCDKeySetting.ToUpper() == cdKey.ToUpper())
            {
                LogDMAuthorization(player, true);
                return DMAuthorizationType.Admin;
            }

            AuthorizedDM entity = DataService.AuthorizedDM.GetByCDKeyAndActiveOrDefault(cdKey);
            if (entity == null) return DMAuthorizationType.None;

            if (entity.DMRole == 1)
                return DMAuthorizationType.DM;
            else if (entity.DMRole == 2)
                return DMAuthorizationType.Admin;

            return DMAuthorizationType.None;
        }

        private static void LogDMAuthorization(NWPlayer dm, bool isAuthorizationSuccessful)
        {
            var account = GetPCPlayerName(dm);
            var cdKey = GetPCPublicCDKey(dm);
            var now = DateTime.UtcNow;
            var eventType = isAuthorizationSuccessful ? 13 : 14;

            ModuleEvent entity = new ModuleEvent
            {
                AccountName = account,
                CDKey = cdKey,
                ModuleEventTypeID = eventType,
                PlayerID = null,
                DateOfEvent = now
            };

            // Bypass the caching logic.
            DataService.DataQueue.Enqueue(new DatabaseAction(entity, DatabaseActionType.Insert));
        }
    }
}
