namespace BSI.CloakDagger.Models.PlotMod
{
    public abstract class Behavior
    {
        public Goal Goal { get; set; }
        public abstract bool CanEnd();

        public abstract void DoEnd();

        public virtual void Initialize(Goal goal)
        {
            Goal = goal;
        }
    }
}