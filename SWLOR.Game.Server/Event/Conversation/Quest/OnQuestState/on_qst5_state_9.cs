using SWLOR.Game.Server.Event.Conversation.Quest.OnQuestState;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
#pragma warning disable IDE1006 // Naming Styles
    public class on_qst5_state_9
#pragma warning restore IDE1006 // Naming Styles
    {
        public static int Main()
        {
            return QuestCheckState.Check(5, 9) ? 1 : 0;
        }
    }
}