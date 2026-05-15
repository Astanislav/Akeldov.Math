using Akeldov.Math.Spatial2D;
using System;

namespace Akeldov.Math.Hexes.Topology
{
    public readonly struct AdjacentIndices
    {
        public readonly VectorXYInt V0, V1, V2, V3, V4, V5;

        public AdjacentIndices(
            VectorXYInt v0,
            VectorXYInt v1,
            VectorXYInt v2,
            VectorXYInt v3,
            VectorXYInt v4,
            VectorXYInt v5)
        {
            V0 = v0;
            V1 = v1;
            V2 = v2;
            V3 = v3;
            V4 = v4;
            V5 = v5;
        }

        public VectorXYInt this[int i] => i switch
        {
            0 => V0,
            1 => V1,
            2 => V2,
            3 => V3,
            4 => V4,
            5 => V5,
            _ => throw new IndexOutOfRangeException()
        };
    }
}
