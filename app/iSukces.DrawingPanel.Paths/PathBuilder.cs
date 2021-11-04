using System.Collections.Generic;
using System.Windows;

namespace iSukces.DrawingPanel.Paths
{
    internal class PathBuilder
    {
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
                if (res.Angle<180)
                if (CheckDot(res))
                {
                    ArcTo(res);
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
            if (result is null)
            {
                _list.Add(new InvalidPathElement(start, end, ArcValidationResult.UnableToConstructArc));
                return;
            }

            if (result.Kind == ZeroReferencePointPathCalculator.ResultKind.Line)
            {
                _list.Add(new LinePathElement(start.Point, end.Point));
                return;
            }

            ArcTo(result.Arc1);
            ArcTo(result.Arc2);
            LineTo(end.Point);
        }


        public void AddLine(Point startPoint, Point endPoint)
        {
            var linePathElement = new LinePathElement(startPoint, endPoint);
            LineTo(startPoint);
            _list.Add(linePathElement);
            CurrentPoint = endPoint;
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
            if (!(line.LengthSquared > PathBase.LengthEpsilonSquare)) return;
            _list.Add(new LinePathElement(CurrentPoint, p));
            CurrentPoint = p;
        }

        public IReadOnlyList<IPathElement> List      => _list;
        public IPathValidator              Validator { get; set; }

        private readonly List<IPathElement> _list = new List<IPathElement>();
        public Point CurrentPoint;
    }
}
