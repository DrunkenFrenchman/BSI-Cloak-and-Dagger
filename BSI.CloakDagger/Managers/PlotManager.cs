using System.Collections.Generic;
using System.Linq;
using BSI.CloakDagger.Enumerations;
using BSI.CloakDagger.Models;
using BSI.CloakDagger.Models.PlotMod;

namespace BSI.CloakDagger.Managers
{
    public class PlotManager
    {
        internal List<GamePlot> Plots { get; set; }

        internal ILookup<GameObject, Plot> GamePlots { get; private set; }

        internal void Add(GamePlot gamePlot)
        {
            Plots.Add(gamePlot);
        }

        internal void Add(Plot plot)
        {
            Plots.Add(new GamePlot
            {
                GameObject = plot.Target,
                Plot = plot
            });

            GamePlots = Plots.ToLookup(p => p.GameObject, p => p.Plot);
        }

        internal void Add(GameObject gameObject, Plot plot)
        {
            Add(new GamePlot
            {
                GameObject = gameObject,
                Plot = plot
            });
        }

        internal void Add(GameObjectType gameObjectType, string stringId, Plot plot)
        {
            Add(new GameObject
            {
                GameObjectType = gameObjectType,
                StringId = stringId
            }, plot);
        }

        internal bool Remove(GamePlot gamePlot)
        {
            var result = Plots.Remove(gamePlot);
            GamePlots = Plots.ToLookup(p => p.GameObject, p => p.Plot);

            return result;
        }

        internal int Remove(GameObject gameObject)
        {
            var result = Plots.RemoveAll(p => p.GameObject == gameObject);
            GamePlots = Plots.ToLookup(p => p.GameObject, p => p.Plot);

            return result;
        }

        internal int Remove(Plot plot)
        {
            var result = Plots.RemoveAll(p => p.Plot == plot);
            GamePlots = Plots.ToLookup(p => p.GameObject, p => p.Plot);

            return result;
        }

        public IEnumerable<Plot> GetPlots()
        {
            return GamePlots.SelectMany(p => p);
        }

        public IEnumerable<Plot> GetPlots(GameObject gameObject)
        {
            return GamePlots[gameObject];
        }

        public IEnumerable<Plot> GetPlots(string triggerType)
        {
            return GamePlots.SelectMany(p => p).Where(p => p.TriggerType == triggerType);
        }

        public IEnumerable<Plot> GetPlots(GameObject gameObject, string triggerType)
        {
            return GamePlots[gameObject].Where(p => p.TriggerType == triggerType);
        }

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
    }
}