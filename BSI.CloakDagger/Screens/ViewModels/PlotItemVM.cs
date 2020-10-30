using System;
using TaleWorlds.Library;

namespace BSI.CloakDagger.Screens.ViewModels
{
    public class PlotItemVM : ViewModel
    {
        private readonly Action<PlotItemVM> _selectionAction;
        private bool _isSelected;
        private MBBindingList<PlotItemVM> _stages;
        private string _title;
        private string _description;
        public PlotItemVM(string title, string description, Action<PlotItemVM> selectedAction)
        {
            Title = title;
            Description = description;

            _selectionAction = selectedAction;

            Stages = new MBBindingList<PlotItemVM>();

            RefreshValues();
        }

        [DataSourceProperty]
        public string Title
        {
            get => _title;
            set
            {
                if (value == _title)
                {
                    return;
                }

                _title = value;
                OnPropertyChangedWithValue(value, nameof(Title));
            }
        }

        [DataSourceProperty]
        public string Description
        {
            get => _description;
            set
            {
                if (value == _description)
                {
                    return;
                }

                _description = value;
                OnPropertyChangedWithValue(value, nameof(Description));
            }
        }

        [DataSourceProperty]
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (value == _isSelected)
                {
                    return;
                }

                _isSelected = value;
                OnPropertyChangedWithValue(value, nameof(IsSelected));
            }
        }

        [DataSourceProperty]
        public MBBindingList<PlotItemVM> Stages
        {
            get => _stages;
            set
            {
                if (value == _stages)
                {
                    return;
                }

                _stages = value;
                OnPropertyChangedWithValue(value, nameof(Stages));
            }
        }

        public sealed override void RefreshValues()
        {
            base.RefreshValues();

            Stages.ApplyActionOnAllItems(s => s.RefreshValues());
        }

        private void ExecuteSelection()
        {
            _selectionAction(this);
        }
    }
}