using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;
using System;

namespace Akeldov.Math.Hexes.Geometry
{
    public partial class HexGeometryField
    {
        public int[,] GetLinearIndicesMatrix(VectorXYInt matrixResolution)
        {
            if (matrixResolution.X <= 0 || matrixResolution.Y <= 0)
                throw new ArgumentException(nameof(matrixResolution));

            var result = new int[matrixResolution.Y, matrixResolution.X];

            var stepX = _size.X / matrixResolution.X;
            var stepY = _size.Y / matrixResolution.Y;

            var halfStepX = stepX / 2;
            var halfStepY = stepY / 2;

            for (int py = 0; py < matrixResolution.Y; py++)
            {
                for (int px = 0; px < matrixResolution.X; px++)
                {
                    float nx = halfStepX + px * stepX;
                    float ny = halfStepY + py * stepY;

                    VectorXY worldPos = new VectorXY(nx, ny);
                    VectorXYInt hexIndex = Vectors.QRS.VectorXYExtensions.ToXYIndex(worldPos, _radius, _origin, _layout);

                    if (hexIndex.X >= 0 && hexIndex.X < _resolution.X &&
                        hexIndex.Y >= 0 && hexIndex.Y < _resolution.Y)
                    {
                        result[py, px] = GetFlatIndex(hexIndex);
                    }
                    else
                    {
                        result[py, px] = -1;
                    }
                }
            }

            return result;
        }
    }
}
