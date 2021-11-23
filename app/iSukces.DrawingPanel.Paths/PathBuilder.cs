using System;
using System.Collections.Generic;
#if NET5_0
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif

namespace iSukces.DrawingPanel.Paths
{
    public class PathBuilder
    {
        public PathBuilder(Point currentPoint, IPathValidator validator = null)
        {
            CurrentPoint = currentPoint;
            Validator    = validator;
        }

        public void AddConnectionAutomatic(PathRay start, PathRay end)
        {
            bool CheckDot(ArcDefinition arc)
            {
                var dot = arc.StartVector * start.Vector;
                if (dot <= 0) return false;
                dot = arc.EndVector * end.Vector;
                if (dot <= 0) return false;
                return true;
            }

            var cross = start.Cross(end);
            if (cross is null)
            {
                AddFlexi(start, end);
                return;
            }

            var c = new OneArcFinder
            {
                Cross = cross.Value
            };
            c.Setup(start, end);
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

            AddFlexi(start, end);
        }

        /// <summary>
        ///     Adds flexible fragment containing up to 4 arcs
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void AddFlexi(PathRay start, PathRay end)
        {
            var result = ZeroReferencePointPathCalculator.Compute(start, end, Validator);
            AddZ(start, end, result);
        }

        public void AddFlexiFromPararell(PathRay start, PathRay end, IPathValidator validator)
        {
            var tmp = ZeroReferencePointPathCalculator.ComputeFromPararell(start, end, validator);
            AddZ(start, end, tmp);
        }


        public void AddLine(Point startPoint, Point endPoint)
        {
            var linePathElement = new LinePathElement(startPoint, endPoint);
            LineTo(startPoint);
            _list.Add(linePathElement);
            CurrentPoint = endPoint;
        }

        public void AddZ(PathRay start, PathRay end, IPathResult result)
        {
            switch (result)
            {
                case null:
                    LineTo(start.Point);
                    _list.Add(new InvalidPathElement(start, end, ArcValidationResult.UnableToConstructArc));
                    CurrentPoint = end.Point;
                    return;
                case ZeroReferencePointPathCalculatorLineResult:
                    LineTo(start.Point);
                    LineTo(end.Point);
                    // _list.Add(new LinePathElement(start.Point, end.Point));
                    return;
                case ZeroReferencePointPathCalculatorResult r2:
                    ArcTo(r2.Arc1);
                    ArcTo(r2.Arc2);
                    LineTo(end.Point);
                    return;
                default: throw new NotSupportedException();
            }
        }

        public void ArcTo(ArcDefinition arc)
        {
            if (arc is null)
                return;
            LineTo(arc.Start);
            _list.Add(arc);
            CurrentPoint = arc.End;
        }

        public void Clear() { _list.Clear(); }

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

        public IReadOnlyList<IPathElement> List      => _list;
        public IPathValidator              Validator { get; set; }

        private readonly List<IPathElement> _list = new List<IPathElement>();
        public Point CurrentPoint;
    }
}
