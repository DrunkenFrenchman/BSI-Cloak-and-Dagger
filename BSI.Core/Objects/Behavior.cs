namespace BSI.Core.Objects
{
    public abstract class Behavior
    {
        public abstract bool CanEnd(Plot plot);

        public abstract bool DoEnd(Plot plot);

        public abstract void OnDailyTick(Plot plot);
    }
}