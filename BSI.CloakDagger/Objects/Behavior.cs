namespace BSI.CloakDagger.Objects
{
    public abstract class Behavior
    {
        public Goal Goal { get; set; }

        public abstract void DailyTick();

        public abstract bool CanEnd();

        public abstract bool DoEnd();

        public virtual bool CanAbort()
        {
            return Goal.Plot.Members.Count == 0;
        }

        public virtual void Abort()
        {

        }
    }
}