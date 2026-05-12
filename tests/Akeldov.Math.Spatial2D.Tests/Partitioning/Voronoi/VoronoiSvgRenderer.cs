using System.Globalization;
using System.Text;
using Akeldov.Math.Spatial2D.Partitioning.Voronoi;

namespace Akeldov.Math.Spatial2D.Tests.Partitioning.Voronoi;

internal static class VoronoiSvgRenderer
{
    private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

    private static readonly string[] Palette =
    {
        "#38bdf8",
        "#fb7185",
        "#34d399",
        "#fbbf24",
        "#a78bfa",
        "#f472b6"
    };

    public static string Render(IReadOnlyList<VoronoiCell<VoronoiSnapshotTests.TestItem>> cells, VectorXY fieldSize)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"600\" height=\"300\" viewBox=\"0 0 {Format(fieldSize.X)} {Format(fieldSize.Y)}\" overflow=\"hidden\">");
        builder.AppendLine("  <rect width=\"100%\" height=\"100%\" fill=\"#f8fafc\"/>");
        builder.AppendLine($"  <rect x=\"0\" y=\"0\" width=\"{Format(fieldSize.X)}\" height=\"{Format(fieldSize.Y)}\" fill=\"none\" stroke=\"#334155\" stroke-width=\"0.4\"/>");

        for (int i = 0; i < cells.Count; i++)
        {
            string color = Palette[i % Palette.Length];
            builder.AppendLine($"  <g fill=\"{color}\" fill-opacity=\"0.55\" stroke=\"none\">");

            var items = cells[i].Items;
            for (int j = 0; j < items.Count; j++)
            {
                VectorXY point = items[j].Position;
                builder.AppendLine($"    <circle cx=\"{Format(point.X)}\" cy=\"{Format(point.Y)}\" r=\"1.45\"/>");
            }

            builder.AppendLine("  </g>");
        }

        builder.AppendLine("  <g fill=\"none\" stroke=\"#0f172a\" stroke-width=\"0.35\" stroke-dasharray=\"1.2 1.2\">");

        for (int i = 0; i < cells.Count; i++)
        {
            Site site = cells[i].Site;
            builder.AppendLine($"    <circle cx=\"{Format(site.Position.X)}\" cy=\"{Format(site.Position.Y)}\" r=\"{Format(site.Power * 2f)}\"/>");
        }

        builder.AppendLine("  </g>");
        builder.AppendLine("  <g fill=\"#0f172a\" stroke=\"#f8fafc\" stroke-width=\"0.5\">");

        for (int i = 0; i < cells.Count; i++)
        {
            VectorXY point = cells[i].Site.Position;
            builder.AppendLine($"    <circle cx=\"{Format(point.X)}\" cy=\"{Format(point.Y)}\" r=\"2.4\"/>");
        }

        builder.AppendLine("  </g>");
        builder.AppendLine("</svg>");
        return builder.ToString();
    }

    private static string Format(float value)
    {
        return value.ToString("0.###", Invariant);
    }
}
