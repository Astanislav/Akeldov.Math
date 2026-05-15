using Akeldov.Math.Spatial2D.Curves;
using Akeldov.Math.Hexes.Topology;
using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Akeldov.Math.Hexes.Geometry.Contours
{
    public static partial class HexMatrixExtensions
    {
        public static Segment[] ToAremContour(this bool[,] mask, float hexApothem)
        {
            return mask.ToAremContour(hexApothem, Layout.OddR);
        }

        public static Segment[] ToAremContour(this bool[,] mask, float hexApothem, Layout layout)
        {
            var extendedMask = mask.GetContour();

            var hexRadius = 1.1547f * hexApothem;

            int qsize = extendedMask.GetLength(0);
            int rsize = extendedMask.GetLength(1);

            var ringOneHexesCenters = new List<VectorXY>();

            for (int q = 0; q < qsize; q++)
            {
                for (int r = 0; r < rsize; r++)
                {
                    if (!extendedMask[q, r])
                        continue;

                    var center = Akeldov.Math.Hexes.Geometry.VectorXYExtensions.GetHexCenter(q, r, hexApothem, hexRadius, layout);
                    VectorXY[] points = Akeldov.Math.Hexes.Geometry.VectorXYExtensions.GetHexVertexes(q, r, hexApothem, hexRadius, layout);

                    var qminClause = q < 1;
                    var rminClause = r < 1;
                    var qmaxClause = q >= qsize - 1;
                    var rmaxClause = r >= rsize - 1;

                    var leftIsBorder = qminClause || !extendedMask[q - 1, r];
                    var rightIsBorder = qmaxClause || !extendedMask[q + 1, r];
                    var topLeftIsBorder = qminClause || rmaxClause || !extendedMask[q - 1, r + 1];
                    var topRightIsBorder = rmaxClause || !extendedMask[q, r + 1];
                    var bottomLeftIsBorder = rminClause || !extendedMask[q, r - 1];
                    var bottomRightIsBorder = qmaxClause || rminClause || !extendedMask[q + 1, r - 1];

                    var isBorder =
                        leftIsBorder ||
                        rightIsBorder ||
                        topLeftIsBorder ||
                        topRightIsBorder ||
                        bottomLeftIsBorder ||
                        bottomRightIsBorder;

                    if (isBorder)
                        ringOneHexesCenters.Add(center - GetAremOffset(hexApothem, hexRadius, layout));
                }
            }

            var segments = ringOneHexesCenters.GetSegments();

            var distinctSegments = segments.Distinct().ToList().Merge(hexApothem * 0.01f).OrderSegments();

            return distinctSegments.Merge(hexApothem * 0.01f).ToArray().OrderContour();
        }

        private static VectorXY GetAremOffset(float hexApothem, float hexRadius, Layout layout)
        {
            return layout == Layout.OddR || layout == Layout.EvenR
                ? new VectorXY(2f * hexApothem, hexRadius)
                : new VectorXY(hexRadius, 2f * hexApothem);
        }

        private static List<Segment> GetSegments(this List<VectorXY> ringOneHexesCenters)
        {
            var segments = new List<Segment>();

            var count = ringOneHexesCenters.Count;
            for (int i = 0; i < count; i++)
            {
                var center = ringOneHexesCenters[i];
                var closests = ringOneHexesCenters.Except(new[] { center }).OrderBy(x => x.Distance(center)).Take(2).ToArray();

                if (closests.Length == 2)
                {
                    var closetsA = closests[0];
                    var closetsB = closests[1];

                    if (center.SquaredLength < closetsA.SquaredLength)
                    {
                        var segment1 = new Segment(center, closetsA);
                        segments.Add(segment1);
                    }
                    else
                    {
                        var segment1 = new Segment(closetsA, center);
                        segments.Add(segment1);
                    }

                    if (center.SquaredLength < closetsB.SquaredLength)
                    {
                        var segment2 = new Segment(center, closetsB);
                        segments.Add(segment2);
                    }
                    else
                    {
                        var segment2 = new Segment(closetsB, center);
                        segments.Add(segment2);
                    }
                }
                else if (closests.Length == 1)
                {
                    var closetsA = closests[0];

                    if (center.SquaredLength < closetsA.SquaredLength)
                    {
                        var segment1 = new Segment(center, closetsA);
                        segments.Add(segment1);
                    }
                    else
                    {
                        var segment1 = new Segment(closetsA, center);
                        segments.Add(segment1);
                    }
                }
            }

            return segments;
        }

        private static List<Segment> OrderSegments(this List<Segment> segments)
        {
            var distinctSegments = segments.ToList();
            while (true)
            {
                var groups = distinctSegments.GroupBy(x => x.StartPoint);

                var sameStartGroups = groups.Where(x => x.Count() > 1).ToList();
                if (sameStartGroups.Any())
                {
                    var sameStartGroup = sameStartGroups[0].ToList();
                    var segmentA = sameStartGroup[0];
                    var segmentB = sameStartGroup[1];

                    var rnd = new Random();

                    if (rnd.Next(0, 100) < 50)
                    {
                        var segmentAIndex = distinctSegments.IndexOf(segmentA);
                        distinctSegments[segmentAIndex] = new Segment(segmentA.EndPoint, segmentA.StartPoint);
                    }
                    else
                    {
                        var segmentBIndex = distinctSegments.IndexOf(segmentB);
                        distinctSegments[segmentBIndex] = new Segment(segmentB.EndPoint, segmentB.StartPoint);
                    }
                }
                else
                {
                    break;
                }
            }

            for (int i = 0; i < distinctSegments.Count; i++)
            {
                var current = distinctSegments[i];
                distinctSegments[i] = new Segment(current.StartPoint, current.EndPoint, true, false);
            }

            return distinctSegments;
        }

        private static List<Segment> Merge(this List<Segment> distinctSegments, float threshold)
        {
            var mergedSegments = distinctSegments;

            var res = mergedSegments.TryMergeAnyOfSegments(threshold, out mergedSegments);
            while (res)
            {
                res = mergedSegments.TryMergeAnyOfSegments(threshold, out mergedSegments);
            }

            return mergedSegments;
        }

        private static bool TryMergeAnyOfSegments(this List<Segment> segments, float threshold, out List<Segment> mergedSegments)
        {
            bool res = false;
            mergedSegments = new List<Segment>();
            var handled = new HashSet<Segment>();

            for (int i = 0; i < segments.Count; i++)
            {
                var currentSegment = segments[i];

                var currentSegmentHasBeenMerged = false;

                if (handled.Contains(currentSegment))
                    continue;

                var currentSegmentLine = new Line(currentSegment.StartPoint, currentSegment.EndPoint);

                for (int j = 0; j < segments.Count; j++)
                {
                    if (j == i)
                        continue;

                    var anotherSegment = segments[j];

                    if (handled.Contains(anotherSegment))
                        continue;

                    if (currentSegmentLine.Distance(anotherSegment.StartPoint) < threshold && currentSegmentLine.Distance(anotherSegment.EndPoint) < threshold)
                    {
                        res = true;
                        currentSegmentHasBeenMerged = true;

                        var distAA = currentSegment.StartPoint.Distance(anotherSegment.StartPoint);
                        var distAB = currentSegment.StartPoint.Distance(anotherSegment.EndPoint);
                        var distBA = currentSegment.EndPoint.Distance(anotherSegment.StartPoint);
                        var distBB = currentSegment.EndPoint.Distance(anotherSegment.EndPoint);

                        Segment newSegment;

                        if (distAA > distAB && distAA > distBA && distAA > distBB)
                        {
                            newSegment = new Segment(currentSegment.StartPoint, anotherSegment.StartPoint);
                        }
                        else if (distAB > distAA && distAB > distBA && distAB > distBB)
                        {
                            newSegment = new Segment(currentSegment.StartPoint, anotherSegment.EndPoint);
                        }
                        else if (distBA > distAB && distBA > distAA && distBA > distBB)
                        {
                            newSegment = new Segment(currentSegment.EndPoint, anotherSegment.StartPoint);
                        }
                        else
                        {
                            newSegment = new Segment(currentSegment.EndPoint, anotherSegment.EndPoint);
                        }

                        mergedSegments.Add(newSegment);
                        handled.Add(currentSegment);
                        handled.Add(anotherSegment);

                        break;
                    }
                }

                if (!currentSegmentHasBeenMerged)
                {
                    mergedSegments.Add(currentSegment);
                    handled.Add(currentSegment);
                }
            }

            return res;
        }
    }
}
