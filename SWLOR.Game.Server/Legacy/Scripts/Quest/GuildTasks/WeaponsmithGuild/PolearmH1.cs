using SWLOR.Game.Server.Legacy.Enumeration;
using SWLOR.Game.Server.Legacy.Quest;

namespace SWLOR.Game.Server.Legacy.Scripts.Quest.GuildTasks.WeaponsmithGuild
{
    public class PolearmH1: AbstractQuest
    {
        public PolearmH1()
        {
            CreateQuest(262, "Weaponsmith Guild Task: 1x Polearm H1", "wpn_tsk_262")
                .IsRepeatable()

                .AddObjectiveCollectItem(1, "halberd_1", 1, true)

                .AddRewardGold(85)
                .AddRewardGuildPoints(GuildType.WeaponsmithGuild, 19);
        }
    }
}