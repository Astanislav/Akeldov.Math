using System.Globalization;
using System.Text;
using Akeldov.Math.Spatial2D.Sampling.Point.PoissonDisk;

namespace Akeldov.Math.Spatial2D.Tests.Sampling.PoissonDisk;

internal static class PoissonDiskSvgRenderer
{
    private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

    public static string Render(IReadOnlyList<PoissonDiskPointSample> samples, VectorXY fieldSize)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"600\" height=\"400\" viewBox=\"0 0 {Format(fieldSize.X)} {Format(fieldSize.Y)}\" overflow=\"hidden\">");
        builder.AppendLine("  <rect width=\"100%\" height=\"100%\" fill=\"#f8fafc\"/>");
        builder.AppendLine($"  <rect x=\"0\" y=\"0\" width=\"{Format(fieldSize.X)}\" height=\"{Format(fieldSize.Y)}\" fill=\"none\" stroke=\"#334155\" stroke-width=\"0.4\"/>");
        builder.AppendLine("  <g fill=\"none\" stroke=\"#cbd5e1\" stroke-width=\"0.35\">");

        for (int i = 0; i < samples.Count; i++)
        {
            var sample = samples[i];
            builder.AppendLine($"    <circle cx=\"{Format(sample.Point.X)}\" cy=\"{Format(sample.Point.Y)}\" r=\"{Format(sample.MinimalDistance)}\"/>");
        }

        builder.AppendLine("  </g>");
        builder.AppendLine("  <g fill=\"#0f766e\" stroke=\"#0f172a\" stroke-width=\"0.35\">");

        for (int i = 0; i < samples.Count; i++)
        {
            VectorXY point = samples[i].Point;
            builder.AppendLine($"    <circle cx=\"{Format(point.X)}\" cy=\"{Format(point.Y)}\" r=\"1.35\"/>");
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
