using TaleWorlds.Library;

namespace BSI.CloakDagger.Extensions
{
    public static class ColorExtension
    {
        public static Color GetOpposingColor(this Color color)
        {
            return color.AddFactorInHSB(0.5f, 0.5f, 0.5f);
        }
    }
}