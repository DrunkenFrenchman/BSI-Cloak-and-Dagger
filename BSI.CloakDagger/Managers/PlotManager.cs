using System.Collections.Generic;
using System.Linq;
using BSI.CloakDagger.Enumerations;
using BSI.CloakDagger.Objects;

namespace BSI.CloakDagger.Managers
{
    public class PlotManager
    {
        #region Thread-Safe Singleton

        private static volatile PlotManager _instance;
        private static readonly object SyncRoot = new object();

        private PlotManager()
        {
            Plots = new List<GamePlot>();
            GamePlots = Plots.ToLookup(p => p.GameObject, p => p.Plot);
        }

        public static PlotManager Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                lock (SyncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new PlotManager();
                    }
                }

                return _instance;
            }
        }

        #endregion

        internal List<GamePlot> Plots { get; set; }

        public ILookup<GameObject, Plot> GamePlots { get; }

        public void Add(GamePlot gamePlot)
        {
            Plots.Add(gamePlot);
        }

        public void Add(GameObject gameObject, Plot plot)
        {
            Add(new GamePlot
            {
                GameObject = gameObject,
                Plot = plot
            });
        }

        public void Add(GameObjectType gameObjectType, string stringId, Plot plot)
        {
            Add(new GameObject
            {
                GameObjectType = gameObjectType,
                StringId = stringId
            }, plot);
        }

        public bool Remove(GamePlot gamePlot)
        {
            return Plots.Remove(gamePlot);
        }

        public int Remove(GameObject gameObject)
        {
            return Plots.RemoveAll(p => p.GameObject == gameObject);
        }

        public int Remove(Plot plot)
        {
            return Plots.RemoveAll(p => p.Plot == plot);
        }

        public List<Plot> GetPlots()
        {
            return GamePlots.SelectMany(p => p).ToList();
        }

        public List<Plot> GetPlots(string triggerType)
        {
            return GamePlots.SelectMany(p => p).Where(p => p.TriggerType == triggerType).ToList();
        }
    }
}