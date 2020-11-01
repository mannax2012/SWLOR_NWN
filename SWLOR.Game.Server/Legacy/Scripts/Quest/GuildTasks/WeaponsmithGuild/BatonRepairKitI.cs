using SWLOR.Game.Server.Legacy.Enumeration;
using SWLOR.Game.Server.Legacy.Quest;

namespace SWLOR.Game.Server.Legacy.Scripts.Quest.GuildTasks.WeaponsmithGuild
{
    public class BatonRepairKitI: AbstractQuest
    {
        public BatonRepairKitI()
        {
            CreateQuest(252, "Weaponsmith Guild Task: 1x Baton Repair Kit I", "wpn_tsk_252")
                .IsRepeatable()

                .AddObjectiveCollectItem(1, "bt_rep_1", 1, true)

                .AddRewardGold(120)
                .AddRewardGuildPoints(GuildType.WeaponsmithGuild, 28);
        }
    }
}