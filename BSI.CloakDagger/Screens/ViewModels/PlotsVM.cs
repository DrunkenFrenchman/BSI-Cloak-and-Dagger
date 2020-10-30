using System;
using System.Linq;
using BSI.CloakDagger.Managers;
using TaleWorlds.Library;

namespace BSI.CloakDagger.Screens.ViewModels
{
    public class PlotsVM : ViewModel
    {
        private readonly Action _closeScreenAction;
        private MBBindingList<PlotItemVM> _activePlots;
        private string _activePlotsText;
        private MBBindingList<PlotItemVM> _availablePlots;
        private string _availablePlotsText;
        private string _currentPlotTitle;
        private bool _isThereAnyPlot;
        private string _noActivePlotText;
        private string _plotTitleText;
        private PlotItemVM _selectedPlot;

        public PlotsVM(Action closeScreenAction)
        {
            _closeScreenAction = closeScreenAction;

            ActivePlots = new MBBindingList<PlotItemVM>();
            foreach (var gamePlot in GameManager.Instance.PlotManager.GamePlots)
            {
                foreach (var plot in gamePlot)
                {
                    ActivePlots.Add(new PlotItemVM(plot.Title, plot.Description, SetSelectedPlot));
                }
            }

            AvailablePlots = new MBBindingList<PlotItemVM>();
            foreach (var trigger in GameManager.Instance.Triggers.Where(trigger => trigger.CanPlayerStart()))
            {
                AvailablePlots.Add(new PlotItemVM(trigger.Title, trigger.Description, SetSelectedPlot));
            }

            SetSelectedPlot(ActivePlots.FirstOrDefault());

            IsThereAnyPlot = ActivePlots.Any();

            RefreshValues();
        }

        [DataSourceProperty]
        public string PlotTitleText
        {
            get => _plotTitleText;
            set
            {
                if (value == _plotTitleText)
                {
                    return;
                }

                _plotTitleText = value;
                OnPropertyChangedWithValue(value, nameof(PlotTitleText));
            }
        }

        [DataSourceProperty]
        public string ActivePlotsText
        {
            get => _activePlotsText;
            set
            {
                if (value == _activePlotsText)
                {
                    return;
                }

                _activePlotsText = value;
                OnPropertyChangedWithValue(value, nameof(ActivePlotsText));
            }
        }

        [DataSourceProperty]
        public string AvailablePlotsText
        {
            get => _availablePlotsText;
            set
            {
                if (value == _availablePlotsText)
                {
                    return;
                }

                _availablePlotsText = value;
                OnPropertyChangedWithValue(value, nameof(AvailablePlotsText));
            }
        }

        [DataSourceProperty]
        public PlotItemVM SelectedPlot
        {
            get => _selectedPlot;
            set
            {
                if (value == _selectedPlot)
                {
                    return;
                }

                _selectedPlot = value;
                OnPropertyChangedWithValue(value, nameof(SelectedPlot));
            }
        }

        [DataSourceProperty]
        public MBBindingList<PlotItemVM> ActivePlots
        {
            get => _activePlots;
            set
            {
                if (value == _activePlots)
                {
                    return;
                }

                _activePlots = value;
                OnPropertyChangedWithValue(value, nameof(ActivePlots));
            }
        }

        [DataSourceProperty]
        public MBBindingList<PlotItemVM> AvailablePlots
        {
            get => _availablePlots;
            set
            {
                if (value == _availablePlots)
                {
                    return;
                }

                _availablePlots = value;
                OnPropertyChangedWithValue(value, nameof(AvailablePlots));
            }
        }

        [DataSourceProperty]
        public bool IsThereAnyPlot
        {
            get => _isThereAnyPlot;
            set
            {
                if (value == _isThereAnyPlot)
                {
                    return;
                }

                _isThereAnyPlot = value;
                OnPropertyChangedWithValue(value, nameof(IsThereAnyPlot));
            }
        }

        [DataSourceProperty]
        public string NoActivePlotText
        {
            get => _noActivePlotText;
            set
            {
                if (value == _noActivePlotText)
                {
                    return;
                }

                _noActivePlotText = value;
                OnPropertyChangedWithValue(value, nameof(NoActivePlotText));
            }
        }

        [DataSourceProperty]
        public string CurrentPlotTitle
        {
            get => _currentPlotTitle;
            set
            {
                if (value == _currentPlotTitle)
                {
                    return;
                }

                _currentPlotTitle = value;
                OnPropertyChangedWithValue(value, nameof(CurrentPlotTitle));
            }
        }

        public sealed override void RefreshValues()
        {
            base.RefreshValues();

            PlotTitleText = "Plots";
            ActivePlotsText = "Active Plots";
            AvailablePlotsText = "Available Plots";
            NoActivePlotText = "There are currently no active plots.";

            ActivePlots.ApplyActionOnAllItems(p => p.RefreshValues());
            AvailablePlots.ApplyActionOnAllItems(p => p.RefreshValues());
            SelectedPlot?.RefreshValues();
        }

        private void SetSelectedPlot(PlotItemVM plot)
        {
            if (_selectedPlot == plot)
            {
                return;
            }

            if (_selectedPlot != null)
            {
                _selectedPlot.IsSelected = false;
            }

            if (plot != null)
            {
                plot.IsSelected = true;
            }

            SelectedPlot = plot;
            if (_selectedPlot != null)
            {
                CurrentPlotTitle = _selectedPlot.Title;
            }
            else
            {
                CurrentPlotTitle = string.Empty;
            }
        }

        private void ExecuteClose()
        {
            _closeScreenAction();
        }
    }
}