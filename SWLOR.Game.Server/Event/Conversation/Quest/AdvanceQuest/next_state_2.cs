using SWLOR.Game.Server.Event.Conversation.Quest.AdvanceQuest;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
#pragma warning disable IDE1006 // Naming Styles
    public class next_state_2
#pragma warning restore IDE1006 // Naming Styles
    {
        public static int Main()
        {
            return QuestAdvance.Check(2) ? 1 : 0;
        }
    }
}
