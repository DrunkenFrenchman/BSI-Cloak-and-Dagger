using TaleWorlds.GauntletUI;

namespace BSI.CloakDagger.Screens.Widgets
{
    public class PlotItemButtonWidget : ButtonWidget
    {
        private int _plotNameXOffset;
        private int _plotNameYOffset;
        private TextWidget _plotTitleText;

        public PlotItemButtonWidget(UIContext context) : base(context)
        {
        }

        public TextWidget PlotTitleText
        {
            get => _plotTitleText;
            set
            {
                if (_plotTitleText == value)
                {
                    return;
                }

                _plotTitleText = value;
                OnPropertyChanged(value, nameof(PlotTitleText));
            }
        }

        public int PlotTitleYOffset
        {
            get => _plotNameYOffset;
            set
            {
                if (_plotNameYOffset == value)
                {
                    return;
                }

                _plotNameYOffset = value;
                OnPropertyChanged(value, nameof(PlotTitleYOffset));
            }
        }

        public int PlotTitleXOffset
        {
            get => _plotNameXOffset;
            set
            {
                if (_plotNameXOffset == value)
                {
                    return;
                }

                _plotNameXOffset = value;
                OnPropertyChanged(value, nameof(PlotTitleXOffset));
            }
        }

        protected override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);

            if (PlotTitleText == null)
            {
                return;
            }

            if (CurrentState == "Pressed")
            {
                PlotTitleText.PositionYOffset = PlotTitleYOffset;
                PlotTitleText.PositionXOffset = PlotTitleXOffset;
            }
            else
            {
                PlotTitleText.PositionYOffset = 0.0f;
                PlotTitleText.PositionXOffset = 0.0f;
            }
        }
    }
}