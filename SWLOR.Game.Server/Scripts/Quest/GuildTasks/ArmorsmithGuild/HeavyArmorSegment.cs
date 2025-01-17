using SWLOR.Game.Server.Enumeration;
using SWLOR.Game.Server.Quest;

namespace SWLOR.Game.Server.Scripts.Quest.GuildTasks.ArmorsmithGuild
{
    public class HeavyArmorSegment: AbstractQuest
    {
        public HeavyArmorSegment()
        {
            CreateQuest(118, "Armorsmith Guild Task: 1x Heavy Armor Segment", "arm_tsk_118")
                .IsRepeatable()

                .AddObjectiveCollectItem(1, "h_armor_segment", 1, true)

                .AddRewardGold(80)
                .AddRewardGuildPoints(GuildType.ArmorsmithGuild, 20);
        }
    }
}
