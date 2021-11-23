using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using iSukces.Mathematics;

namespace iSukces.DrawingPanel.Paths.Test
{
    internal class DpAssertsBuilder : AssertsBuilder
    {
        private static string Create(PathRay tmp) { return $"new PathRay({tmp.Point.ToCs()}, {tmp.Vector.ToCs()})"; }

        private static string FindName(ZeroReferencePointPathCalculatorResult result)
        {
            if (result is null)
                return "";
            var s = new StringBuilder();
            s.Append(result.Kind);

            void A(ArcDefinition arc)
            {
                if (arc is null) return;
                s.Append("_" + arc.GetDirectionAlternative());
                s.Append("_" + arc.Angle.Round());
            }

            A(result.Arc1);
            A(result.Arc2);

            return s.ToString();
        }

        
        private void Add(Point p, string name) { AssertExEqual(p.X.ToCs(), p.Y.ToCs(), name); }

        private void Add(PathRay p, string name)
        {
            AssertExEqual(p.Point.X.ToCs(), p.Point.Y.ToCs(), p.Vector.X.ToCs(), p.Vector.Y.ToCs(), name);
        }

        private void Add(Vector p, string name) { AssertExEqual(p.X.ToCs(), p.Y.ToCs(), name); }

        private void Add(IPathResult result, string name)
        {
            if (result is null)
            {
                AssertNull(name);
                return;
            }

            name += ".";
            if (result is ZeroReferencePointPathCalculatorResult zero)
            {
                AddEnum(zero.Kind, name + nameof(zero.Kind));
            }

            Add(result.Start, name + nameof(result.Start));
            Add(result.End, name + nameof(result.End));
            Add(result.Elements, name + nameof(result.Elements));
        }


        private void Add(LinePathElement x, string name)
        {
            var s = x.GetStartPoint();
            var e = x.GetEndPoint();
            var a = new[]
            {
                s.X.ToCs(), s.Y.ToCs(), e.X.ToCs(), e.Y.ToCs(),
                name, "6"
            };
            AssertExEqual(a);
        }


        private void Add(ArcPathMakerResult x, string name)
        {
            if (x is null)
            {
                AssertNull(name);
                return;
            }

            name += ".";
            Add(x.Segments, name + nameof(x.Segments));
        }


        private void Add(IReadOnlyList<IPathResult> x, string name) { AssertList(x, name, AssertPoint, false); }


        private void Add(IReadOnlyList<IPathElement> x, string name) { AssertList(x, name, AddIPathElement, true); }


        private void Add(ArcValidationResult value, string expression)
        {
            const string prefix = nameof(ArcValidationResult) + ".";
            AssertEqual(prefix + value, expression);
        }


        private void Add(InvalidPathElement x, string name)
        {
            name += ".";
            Add(x.Status, name + "." + nameof(x.Status));
            AddIPathElementCommon(x, name);
        }


        private void Add(ArcDefinition c, string name)
        {
            if (c is null)
                AssertNull(name);
            else
            {
                name += ".";
                AddEnum(c.Direction, name + nameof(c.Direction));
                Add(c.Angle, name + nameof(c.Angle));
                Add(c.Radius, name + nameof(c.Radius));
                Add(c.Center, name + nameof(c.Center));
                Add(c.Start, name + nameof(c.Start));
                Add(c.End, name + nameof(c.End));
                Add(c.DirectionStart, name + nameof(c.DirectionStart));
            }
        }
        
        

        private void Add(ArcPathMakerVertex x, string name, Type type)
        {
            if (x is null)
            {
                AssertNull(name);
                return;
            }

            name += ".";
            Add(x.Location, name + nameof(x.Location));
            Add(x.InVector, name + nameof(x.InVector));
            Add(x.OutVector, name + nameof(x.OutVector));
            AddEnum(x.Flags, name + nameof(x.Flags));

            AssertList(x.ReferencePoints, name + nameof(x.ReferencePoints), AssertPoint, false);
        }

        private void AddEnum(FlexiPathMakerItem2Flags n, string expression)
        {
            const string prefix   = nameof(FlexiPathMakerItem2Flags) + ".";
            var          tmp      = n.ToString().Split(',').Select(a => prefix + a.Trim()).ToArray();
            var          expexted = string.Join(" | ", tmp);
            AssertEqual(expexted, expression);
        }


        private void AddEnum(ArcDirection n, string name)
        {
            const string prefix = nameof(ArcDirection) + ".";
            AssertEqual(prefix + n, name);
        }

        private void AddEnum(ZeroReferencePointPathCalculator.ResultKind value, string expression)
        {
            const string prefix = nameof(ZeroReferencePointPathCalculator)
                                  + "."
                                  + nameof(ZeroReferencePointPathCalculator.ResultKind)
                                  + ".";
            AssertEqual(prefix + value, expression);
        }


        private void AddIPathElement(IPathElement x, string name, Type knownType)
        {
            if (x is null)
            {
                AssertNull(name);
                return;
            }

            switch (x)
            {
                case ArcDefinition arcDefinition:

                    CastAndDoAction(arcDefinition, knownType, name, e =>
                    {
                        Add(arcDefinition, e.Code);
                    });
                    break;
                case InvalidPathElement invalidPathElement:
                    CastAndDoAction(invalidPathElement, knownType, name, e =>
                    {
                        Add(invalidPathElement, e.Code);
                    });

                    break;
                case LinePathElement linePathElement:
                    CastAndDoAction(linePathElement, knownType, name, e =>
                    {
                        Add(linePathElement, e.Code);
                    });

                    break;
                default: throw new ArgumentOutOfRangeException(nameof(x));
            }
        }

        private void AddIPathElement(IPathElement x, string name)
        {
            name += ".";
            var point = x.GetStartPoint();
            var n     = Declare(point, name + nameof(x.GetStartPoint) + "()");
            Add(point, n);
            Release(n);

            point = x.GetEndPoint();
            n     = Declare(point, name + nameof(x.GetEndPoint) + "()");
            Add(point, n);
            Release(n);
        }


        private void AddIPathElementCommon(IPathElement x, string name)
        {
            name += ".";
            var point = x.GetStartPoint();
            var n     = Declare(point, TypedExpression.Make<IPathElement>(name + nameof(x.GetStartPoint) + "()"));
            Add(point, n);
            Release(n);

            point = x.GetEndPoint();
            n     = Declare(point, TypedExpression.Make<IPathElement>(name + nameof(x.GetEndPoint) + "()"));
            Add(point, n);
            Release(n);
        }


        private void AssertPoint(IPathResult x, string name, Type knownType)
        {
            if (x is null)
            {
                AssertNull(name);
                return;
            }

            name += ".";
            Add(x.Start, name + nameof(x.Start));
            Add(x.End, name + nameof(x.End));
            Add(x.Elements, name + nameof(x.Elements));
        }

        private void AssertPoint(PathRay expected, string expression, Type type)
        {
            Add(expected, expression);
        }

        public string Create(ArcPathMakerResult x, string name)
        {
            return Create(() => { Add(x, name); });
        }
        public string Create(ArcDefinition x, string name)
        {
            return Create(() => { Add(x, name); });
        }

        public string Create(IPathResult result, string name)
        {
            return Create(() =>
            {
                Add(result, name);
            });
        }


        public string Create(IReadOnlyList<ArcPathMakerVertex> result, string name)
        {
            return Create(() => { AssertList(result, name, Add, false); });
        }

        public string Create(ArcPathMakerVertex x, string name)
        {
            return Create(() => { Add(x, name, typeof(ArcPathMakerVertex)); });
        }
    }
}
