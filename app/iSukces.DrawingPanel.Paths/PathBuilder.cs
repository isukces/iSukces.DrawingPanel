#if NET5_0
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif
using System;
using System.Collections.Generic;

namespace iSukces.DrawingPanel.Paths
{
    public class PathBuilder
    {
        public PathBuilder(Point currentPoint, IPathValidator validator = null)
        {
            CurrentPoint = currentPoint;
            Validator    = validator;
        }

        public void AddConnectionAutomatic(PathRayWithArm start, PathRayWithArm end, bool reverseEnd)
        {
            var start1 = start.GetMovedRayOutput();
            var end1   = end.GetMovedRayInput();
            if (reverseEnd)
                end1 = end1.WithInvertedVector();

            bool CheckDot(ArcDefinition arc)
            {
                var dot = arc.DirectionStart * start1.Vector;
                if (dot <= 0) return false;
                dot = arc.DirectionEnd * end1.Vector;
                if (dot <= 0) return false;
                return true;
            }

            var cross = start1.Cross(end1);
            if (cross is null)
            {
                AddFlexi(start, end, reverseEnd);
                return;
            }

            var result = ArcValidationHelper.Validate(Validator, start1, end1, cross.Value, reverseEnd);
            switch (result)
            {
                case CircleCrossValidationResult.ForceLine:
                    LineTo(end.Point);
                    return;
                case CircleCrossValidationResult.Ok:
                    break;
                case CircleCrossValidationResult.Invalid:
                default:
                    AddFlexi(start, end, reverseEnd, ZeroReferencePointPathCalculatorFlags.DontUseOneArcSolution);
                    return;
            }

            var c = new OneArcFinder(cross.Value); // validated cross
            c.Setup(start1, end1);
            var res = c.CalculateArc();
            if (Validator.IsOk(res))
            {
                if (CheckDot(res))
                {
                    ArcTo(res);
                    LineTo(end.Point);
                    return;
                }
            }

            AddFlexi(start, end, reverseEnd);
        }

        /// <summary>
        ///     Adds flexible fragment containing up to 4 arcs
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="reverseEnd"></param>
        public void AddFlexi(PathRayWithArm start, PathRayWithArm end, 
            bool reverseEnd = false, ZeroReferencePointPathCalculatorFlags flags = ZeroReferencePointPathCalculatorFlags.None)
        {
            var s = start.GetMovedRayOutput();
            var e = end.GetMovedRayInput();
            if (reverseEnd)
                e = e.WithInvertedVector();
            var result = ZeroReferencePointPathCalculator.Compute(s, e, Validator, flags);
            AddPathResult(start.GetRay(), end.GetRay(), result);
        }

        public void AddFlexiFromPararell(PathRay start, PathRay end, IPathValidator validator)
        {
            var tmp = ZeroReferencePointPathCalculator.ComputeFromPararell(start, end, validator);
            AddPathResult(start, end, tmp);
        }


        public void AddLine(Point startPoint, Point endPoint)
        {
            var linePathElement = new LinePathElement(startPoint, endPoint);
            LineTo(startPoint);
            _list.Add(linePathElement);
            CurrentPoint = endPoint;
        }

        public void AddPathResult(PathRay start, PathRay end, IPathResult result)
        {
            switch (result)
            {
                case null:
                    LineTo(start.Point);
                    LineTo(end.Point);
                    return;
                case ZeroReferencePointPathCalculatorLineResult:
                    LineTo(start.Point);
                    LineTo(end.Point);
                    return;
                case ZeroReferencePointPathCalculatorResult r2:
                    ArcTo(r2.Arc1);
                    ArcTo(r2.Arc2);
                    LineTo(end.Point);
                    return;
                default: throw new NotSupportedException();
            }
        }

        public void AddZeroReference(PathRay start, PathRay end)
        {
            IPathResult z = ZeroReferencePointPathCalculator.Compute(start, end, Validator);
            AddPathResult(start, end, z);
        }

        public void ArcTo(ArcDefinition arc)
        {
            if (arc is null)
                return;
            LineTo(arc.Start);
            _list.Add(arc);
            CurrentPoint = arc.End;
        }

        public void Clear()
        {
            _list.Clear();
        }

        public void LineTo(Point p)
        {
            var line = p - CurrentPoint;
            if (line.LengthSquared < PathBase.LengthEpsilonSquare)
                return;
            _list.Add(new LinePathElement(CurrentPoint, p));
            CurrentPoint = p;
        }

        public IPathResult LineToAndCreate(Point endPoint)
        {
            LineTo(endPoint);
            if (_list.Count == 0)
                return null;
            return new PathResult(_list[0].GetStartPoint(), endPoint, List);
        }

        #region properties

        public IReadOnlyList<IPathElement> List      => _list;
        public IPathValidator              Validator { get; set; }

        #endregion

        #region Fields

        private readonly List<IPathElement> _list = new List<IPathElement>();
        public Point CurrentPoint;

        #endregion
    }
}
