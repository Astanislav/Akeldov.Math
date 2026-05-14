namespace Akeldov.Math.Hexes.Vectors.QRS
{
    public enum Layout
    {
        OddR = 0,
        EvenR = 1,
        OddQ = 2,
        EvenQ = 3
    }

    public static class LayoutExtensions
    {
        public static bool IsPointyTop(this Layout layout)
        {
            return layout == Layout.OddR || layout == Layout.EvenR;
        }

        public static bool IsFlatTop(this Layout layout)
        {
            return layout == Layout.OddQ || layout == Layout.EvenQ;
        }
    }
}
