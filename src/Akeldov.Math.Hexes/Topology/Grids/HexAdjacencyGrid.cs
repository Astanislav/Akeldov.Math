using Akeldov.Math.Hexes.Geometry;
using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;
using System;
using System.Runtime.CompilerServices;

namespace Akeldov.Math.Hexes.Topology
{
    public sealed class HexAdjacencyGrid
    {
        private const int InvalidHexIndex = -1;

        private IndexedHexAdjacency[] _adjacent;
        private int[] _hexIndices;
        private bool[] _hasHex;

        public HexAdjacencyGrid(
            HexAdjacencyMap hexAdjacencyMap,
            VectorXY hexOrigin,
            float hexApothem,
            VectorXYInt resolution)
        {
            if (hexAdjacencyMap == null)
                throw new ArgumentNullException(nameof(hexAdjacencyMap));

            if (hexAdjacencyMap.Width <= 0 || hexAdjacencyMap.Height <= 0)
                throw new ArgumentException("Hex adjacency map dimensions must be positive.", nameof(hexAdjacencyMap));

            if (!hexOrigin.IsFinite)
                throw new ArgumentOutOfRangeException(nameof(hexOrigin), hexOrigin, "Hex origin components must be finite.");

            if (float.IsNaN(hexApothem) || float.IsInfinity(hexApothem) || hexApothem <= 0f)
                throw new ArgumentOutOfRangeException(nameof(hexApothem), hexApothem, "Hex apothem must be finite and positive.");

            if (resolution.X <= 0 || resolution.Y <= 0)
                throw new ArgumentOutOfRangeException(nameof(resolution), resolution, "Grid resolution components must be positive.");

            float hexRadius = hexApothem.ConvertHexApothemToRadius();
            GridBounds bounds = GetBounds(hexAdjacencyMap, hexOrigin, hexApothem, hexRadius);

            Initialize(
                hexAdjacencyMap,
                hexOrigin,
                hexApothem,
                hexRadius,
                new VectorXY(bounds.MinX, bounds.MinY),
                new VectorXY(bounds.Width, bounds.Height),
                resolution);
        }

        public HexAdjacencyGrid(
            HexAdjacencyMap hexAdjacencyMap,
            VectorXY hexOrigin,
            float hexApothem,
            VectorXY gridOrigin,
            VectorXY gridSize,
            VectorXYInt resolution)
        {
            if (hexAdjacencyMap == null)
                throw new ArgumentNullException(nameof(hexAdjacencyMap));

            if (hexAdjacencyMap.Width <= 0 || hexAdjacencyMap.Height <= 0)
                throw new ArgumentException("Hex adjacency map dimensions must be positive.", nameof(hexAdjacencyMap));

            if (!hexOrigin.IsFinite)
                throw new ArgumentOutOfRangeException(nameof(hexOrigin), hexOrigin, "Hex origin components must be finite.");

            if (float.IsNaN(hexApothem) || float.IsInfinity(hexApothem) || hexApothem <= 0f)
                throw new ArgumentOutOfRangeException(nameof(hexApothem), hexApothem, "Hex apothem must be finite and positive.");

            if (!gridOrigin.IsFinite)
                throw new ArgumentOutOfRangeException(nameof(gridOrigin), gridOrigin, "Grid origin components must be finite.");

            if (!gridSize.IsFinite || gridSize.X <= 0f || gridSize.Y <= 0f)
                throw new ArgumentOutOfRangeException(nameof(gridSize), gridSize, "Grid size components must be finite and positive.");

            if (resolution.X <= 0 || resolution.Y <= 0)
                throw new ArgumentOutOfRangeException(nameof(resolution), resolution, "Grid resolution components must be positive.");

            Initialize(
                hexAdjacencyMap,
                hexOrigin,
                hexApothem,
                hexApothem.ConvertHexApothemToRadius(),
                gridOrigin,
                gridSize,
                resolution);
        }

        public VectorXYInt HexResolution { get; private set; }

        public Layout Layout { get; private set; }

        public VectorXY HexOrigin { get; private set; }

        public float HexApothem { get; private set; }

        public float HexRadius { get; private set; }

        public VectorXY Origin { get; private set; }

        public VectorXY Size { get; private set; }

        public VectorXY CellSize { get; private set; }

        public VectorXYInt Resolution { get; private set; }

        public int ResolutionX { get; private set; }

        public int ResolutionY { get; private set; }

        public int Count => _adjacent.Length;

        public IndexedHexAdjacency[] Adjacent => _adjacent;

        public int[] HexIndices => _hexIndices;

        public bool[] HasHex => _hasHex;

        public IndexedHexAdjacency this[VectorXYInt index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _adjacent[GetHitFlatIndex(index)];
        }

        public IndexedHexAdjacency this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowIfNoHex(index);
                return _adjacent[index];
            }
        }

        public VectorXY GetCellCenter(VectorXYInt index)
        {
            ThrowIfGridIndexOutOfBounds(index);
            return GetCellCenterUnchecked(index.X, index.Y);
        }

        public bool HasHexAt(VectorXYInt index)
        {
            ThrowIfGridIndexOutOfBounds(index);
            return _hasHex[GetFlatIndex(index)];
        }

        public bool TryGetHexIndex(VectorXYInt gridIndex, out VectorXYInt hexIndex)
        {
            if (TryGetHexFlatIndex(gridIndex, out int hexFlatIndex))
            {
                hexIndex = GetHexIndex(hexFlatIndex);
                return true;
            }

            hexIndex = new VectorXYInt(-1, -1);
            return false;
        }

        public VectorXYInt GetHexIndex(VectorXYInt gridIndex)
        {
            return GetHexIndex(GetHexFlatIndex(gridIndex));
        }

        public bool TryGetHexFlatIndex(VectorXYInt gridIndex, out int hexFlatIndex)
        {
            ThrowIfGridIndexOutOfBounds(gridIndex);

            int flatIndex = GetFlatIndex(gridIndex);
            hexFlatIndex = _hexIndices[flatIndex];
            return _hasHex[flatIndex];
        }

        public int GetHexFlatIndex(VectorXYInt gridIndex)
        {
            int flatIndex = GetHitFlatIndex(gridIndex);
            return _hexIndices[flatIndex];
        }

        private void Initialize(
            HexAdjacencyMap hexAdjacencyMap,
            VectorXY hexOrigin,
            float hexApothem,
            float hexRadius,
            VectorXY gridOrigin,
            VectorXY gridSize,
            VectorXYInt resolution)
        {
            HexResolution = new VectorXYInt(hexAdjacencyMap.Width, hexAdjacencyMap.Height);
            Layout = hexAdjacencyMap.Layout;
            HexOrigin = hexOrigin;
            HexApothem = hexApothem;
            HexRadius = hexRadius;
            Origin = gridOrigin;
            Size = gridSize;
            CellSize = new VectorXY(gridSize.X / resolution.X, gridSize.Y / resolution.Y);
            Resolution = resolution;
            ResolutionX = resolution.X;
            ResolutionY = resolution.Y;
            _adjacent = new IndexedHexAdjacency[checked(resolution.X * resolution.Y)];
            _hexIndices = new int[_adjacent.Length];
            _hasHex = new bool[_adjacent.Length];

            Fill(hexAdjacencyMap);
        }

        private void Fill(HexAdjacencyMap hexAdjacencyMap)
        {
            switch (Layout)
            {
                case Layout.OddR:
                    FillRowLayout(hexAdjacencyMap, false);
                    break;
                case Layout.EvenR:
                    FillRowLayout(hexAdjacencyMap, true);
                    break;
                case Layout.OddQ:
                    FillColumnLayout(hexAdjacencyMap, false);
                    break;
                case Layout.EvenQ:
                    FillColumnLayout(hexAdjacencyMap, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Layout));
            }
        }

        private void FillRowLayout(HexAdjacencyMap hexAdjacencyMap, bool evenRowsAreShifted)
        {
            for (int y = 0; y < ResolutionY; y++)
            {
                var rowStart = y * ResolutionX;

                for (int x = 0; x < ResolutionX; x++)
                {
                    var flatIndex = rowStart + x;
                    float shiftedX = Origin.X + (x + 0.5f) * CellSize.X - HexOrigin.X;
                    float shiftedY = Origin.Y + (y + 0.5f) * CellSize.Y - HexOrigin.Y;
                    var hexIndex = GetRowLayoutHexFlatIndex(shiftedX, shiftedY, evenRowsAreShifted);

                    if (hexIndex < 0)
                    {
                        _hexIndices[flatIndex] = InvalidHexIndex;
                        _adjacent[flatIndex] = default(IndexedHexAdjacency);
                        continue;
                    }

                    _hasHex[flatIndex] = true;
                    _hexIndices[flatIndex] = hexIndex;
                    _adjacent[flatIndex] = CreateIndexedAdjacency(hexIndex, hexAdjacencyMap[hexIndex]);
                }
            }
        }

        private void FillColumnLayout(HexAdjacencyMap hexAdjacencyMap, bool evenColumnsAreShifted)
        {
            for (int y = 0; y < ResolutionY; y++)
            {
                var rowStart = y * ResolutionX;

                for (int x = 0; x < ResolutionX; x++)
                {
                    var flatIndex = rowStart + x;
                    float shiftedX = Origin.X + (x + 0.5f) * CellSize.X - HexOrigin.X;
                    float shiftedY = Origin.Y + (y + 0.5f) * CellSize.Y - HexOrigin.Y;
                    var hexIndex = GetColumnLayoutHexFlatIndex(shiftedX, shiftedY, evenColumnsAreShifted);

                    if (hexIndex < 0)
                    {
                        _hexIndices[flatIndex] = InvalidHexIndex;
                        _adjacent[flatIndex] = default(IndexedHexAdjacency);
                        continue;
                    }

                    _hasHex[flatIndex] = true;
                    _hexIndices[flatIndex] = hexIndex;
                    _adjacent[flatIndex] = CreateIndexedAdjacency(hexIndex, hexAdjacencyMap[hexIndex]);
                }
            }
        }

        private static IndexedHexAdjacency CreateIndexedAdjacency(int index, HexAdjacency adjacency)
        {
            return new IndexedHexAdjacency(
                IndexedHexAdjacencyFlags.OwnIndex | (IndexedHexAdjacencyFlags)adjacency.Flags,
                index,
                adjacency.Adjacent0Index,
                adjacency.Adjacent1Index,
                adjacency.Adjacent2Index,
                adjacency.Adjacent3Index,
                adjacency.Adjacent4Index,
                adjacency.Adjacent5Index);
        }

        private VectorXY GetCellCenterUnchecked(int x, int y)
        {
            return new VectorXY(
                Origin.X + (x + 0.5f) * CellSize.X,
                Origin.Y + (y + 0.5f) * CellSize.Y);
        }

        private int GetRowLayoutHexFlatIndex(float shiftedX, float shiftedY, bool evenRowsAreShifted)
        {
            float q = (0.5773502588f * shiftedX - 0.3333333333f * shiftedY) / HexRadius;
            float r = 0.6666666666f * shiftedY / HexRadius;

            RoundPointyTopAxial(q, r, out int qInt, out int rInt);

            int x = evenRowsAreShifted
                ? qInt + ((rInt + (rInt & 1)) / 2)
                : qInt + ((rInt - (rInt & 1)) / 2);

            return GetHexFlatIndex(x, rInt);
        }

        private int GetColumnLayoutHexFlatIndex(float shiftedX, float shiftedY, bool evenColumnsAreShifted)
        {
            float q = 0.6666666666f * shiftedX / HexRadius;
            float r = (0.5773502588f * shiftedY - 0.3333333333f * shiftedX) / HexRadius;

            RoundFlatTopAxial(q, r, out int qInt, out int rInt);

            int y = evenColumnsAreShifted
                ? rInt + ((qInt + (qInt & 1)) / 2)
                : rInt + ((qInt - (qInt & 1)) / 2);

            return GetHexFlatIndex(qInt, y);
        }

        private int GetHexFlatIndex(int x, int y)
        {
            if ((uint)x >= (uint)HexResolution.X || (uint)y >= (uint)HexResolution.Y)
                return InvalidHexIndex;

            return y * HexResolution.X + x;
        }

        private VectorXYInt GetHexIndex(int hexFlatIndex)
        {
            return new VectorXYInt(
                hexFlatIndex % HexResolution.X,
                hexFlatIndex / HexResolution.X);
        }

        private static void RoundPointyTopAxial(float q, float r, out int qInt, out int rInt)
        {
            float s = -q - r;

            qInt = (int)MathF.Round(q);
            rInt = (int)MathF.Round(r);
            int sInt = (int)MathF.Round(s);

            float qDiff = MathF.Abs(qInt - q);
            float rDiff = MathF.Abs(rInt - r);
            float sDiff = MathF.Abs(sInt - s);

            if (qDiff > rDiff && qDiff > sDiff)
                qInt = -rInt - sInt;
            else if (rDiff > sDiff)
                rInt = -qInt - sInt;
        }

        private static void RoundFlatTopAxial(float q, float r, out int qInt, out int rInt)
        {
            float s = -q - r;

            qInt = (int)MathF.Round(q);
            rInt = (int)MathF.Round(r);
            int sInt = (int)MathF.Round(s);

            float qDiff = MathF.Abs(qInt - q);
            float rDiff = MathF.Abs(rInt - r);
            float sDiff = MathF.Abs(sInt - s);

            if (rDiff > qDiff && rDiff > sDiff)
                rInt = -qInt - sInt;
            else if (qDiff > sDiff)
                qInt = -rInt - sInt;
        }

        private int GetHitFlatIndex(VectorXYInt index)
        {
            ThrowIfGridIndexOutOfBounds(index);

            int flatIndex = GetFlatIndex(index);
            ThrowIfNoHex(flatIndex);
            return flatIndex;
        }

        private void ThrowIfNoHex(int flatIndex)
        {
            if (!_hasHex[flatIndex])
                throw new InvalidOperationException("Grid cell does not hit a hex in the adjacency map.");
        }

        private void ThrowIfGridIndexOutOfBounds(VectorXYInt index)
        {
            if (index.X < 0 || index.X >= ResolutionX ||
                index.Y < 0 || index.Y >= ResolutionY)
                throw new IndexOutOfRangeException($"Grid index out of bounds: {index}");
        }

        private int GetFlatIndex(VectorXYInt index) => index.Y * ResolutionX + index.X;

        private static GridBounds GetBounds(
            HexAdjacencyMap hexAdjacencyMap,
            VectorXY hexOrigin,
            float hexApothem,
            float hexRadius)
        {
            VectorXY[] normalizedVertexes = Geometry.VectorXYExtensions.GetNormalizedHexVertexes(hexAdjacencyMap.Layout);
            GridBounds bounds = GetHexBounds(
                GetHexCenter(0, 0, hexOrigin, hexApothem, hexRadius, hexAdjacencyMap.Layout),
                hexRadius,
                normalizedVertexes);

            for (int y = 0; y < hexAdjacencyMap.Height; y++)
            {
                for (int x = 0; x < hexAdjacencyMap.Width; x++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    bounds = bounds.Include(GetHexBounds(
                        GetHexCenter(x, y, hexOrigin, hexApothem, hexRadius, hexAdjacencyMap.Layout),
                        hexRadius,
                        normalizedVertexes));
                }
            }

            return bounds;
        }

        private static VectorXY GetHexCenter(
            int x,
            int y,
            VectorXY origin,
            float apothem,
            float radius,
            Layout layout)
        {
            switch (layout)
            {
                case Layout.OddR:
                    return new VectorXY(
                        origin.X + x * 2f * apothem + ((y & 1) == 1 ? apothem : 0f),
                        origin.Y + 1.5f * radius * y);
                case Layout.EvenR:
                    return new VectorXY(
                        origin.X + x * 2f * apothem + ((y & 1) == 0 ? apothem : 0f) - apothem,
                        origin.Y + 1.5f * radius * y);
                case Layout.OddQ:
                    return new VectorXY(
                        origin.X + 1.5f * radius * x,
                        origin.Y + y * 2f * apothem + ((x & 1) == 1 ? apothem : 0f));
                case Layout.EvenQ:
                    return new VectorXY(
                        origin.X + 1.5f * radius * x,
                        origin.Y + y * 2f * apothem + ((x & 1) == 0 ? apothem : 0f) - apothem);
                default:
                    throw new ArgumentOutOfRangeException(nameof(layout));
            }
        }

        private static GridBounds GetHexBounds(
            VectorXY center,
            float radius,
            VectorXY[] normalizedVertexes)
        {
            VectorXY first = center + normalizedVertexes[0] * radius;
            float minX = first.X;
            float minY = first.Y;
            float maxX = first.X;
            float maxY = first.Y;

            for (int i = 1; i < normalizedVertexes.Length; i++)
            {
                VectorXY vertex = center + normalizedVertexes[i] * radius;
                minX = MathF.Min(minX, vertex.X);
                minY = MathF.Min(minY, vertex.Y);
                maxX = MathF.Max(maxX, vertex.X);
                maxY = MathF.Max(maxY, vertex.Y);
            }

            return new GridBounds(minX, minY, maxX, maxY);
        }

        private readonly struct GridBounds
        {
            public GridBounds(float minX, float minY, float maxX, float maxY)
            {
                MinX = minX;
                MinY = minY;
                MaxX = maxX;
                MaxY = maxY;
            }

            public float MinX { get; }

            public float MinY { get; }

            public float MaxX { get; }

            public float MaxY { get; }

            public float Width => MaxX - MinX;

            public float Height => MaxY - MinY;

            public GridBounds Include(GridBounds bounds)
            {
                return new GridBounds(
                    MathF.Min(MinX, bounds.MinX),
                    MathF.Min(MinY, bounds.MinY),
                    MathF.Max(MaxX, bounds.MaxX),
                    MathF.Max(MaxY, bounds.MaxY));
            }
        }
    }
}
