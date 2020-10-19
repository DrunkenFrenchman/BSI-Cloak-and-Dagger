using TaleWorlds.CampaignSystem;

namespace BSI.CloakDagger.Objects
{
    public abstract class Behavior : CampaignBehaviorBase
    {
        public Goal Goal { get; set; }

        public abstract bool CanEnd();

        public abstract bool DoEnd();
    }
}