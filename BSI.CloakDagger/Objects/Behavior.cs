namespace BSI.CloakDagger.Objects
{
    public abstract class Behavior
    {
        public abstract bool CanEnd();

        public abstract void DoEnd();

        public Goal Goal { get; set; }

        public virtual void Initialize(Goal goal)
        {
            Goal = goal;
        }
    }
}