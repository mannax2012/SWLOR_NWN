﻿using SWLOR.Game.Server.NWN;
using SWLOR.Game.Server.GameObject;

namespace SWLOR.Game.Server.Scripts.Placeable.FuelBay
{
    public class OnOpened: IScript
    {
        public void SubscribeEvents()
        {
        }

        public void UnsubscribeEvents()
        {
        }

        public void Main()
        {
            NWPlaceable chest = (_.OBJECT_SELF);
            NWPlayer player = (_.GetLastOpenedBy());

            player.SendMessage("Place the appropriate fuel inside the container. Click on the control tower when you're finished.");

            chest.IsUseable = false;
            chest.SetLocalObject("BAY_ACCESSOR", player);
        }
    }
}
