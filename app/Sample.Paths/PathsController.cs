using System.Drawing;
using System.Drawing.Drawing2D;
using iSukces.DrawingPanel;
using iSukces.DrawingPanel.Interfaces;
using iSukces.DrawingPanel.Paths;
using Point = iSukces.Mathematics.Point;
using Vector = iSukces.Mathematics.Vector;

namespace Sample.Paths;

internal sealed class PathsController : DrawableBase, IDpMouseButtonHandler
{
    public PathsController()
    {
        _bc = new OneReferencePointPathCalculator();
        _bc.InitDemo();
    }

    private static Vector Calc(Point a, Point b)
    {
        var v = b - a;
        return SetVector100(v);
    }

    private static Vector SetVector100(Vector v)
    {
        v = v.GetNormalized();
        if (double.IsNaN(v.X))
            v = new Vector(1, 0);
        return v * 100;
    }

    public override void Draw(Graphics gr)
    {
        var p = new Presenter
        {
            Graph          = gr,
            Calculator     = _bc,
            Transformation = CanvasInfo.Transformation
        };
        gr.SmoothingMode = SmoothingMode.HighQuality;
        p.Draw();
    }

    public DrawingHandleResult HandleOnMouseDown(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            var loc = CanvasInfo.Transformation.FromCanvas(e.Location);

            PathRayWithArm M(PathRayWithArm x)
            {
                return x.With(-Calc(x.Point, loc));
            }
            // PathRay M(PathRay x) { return x.With(-Calc(x.Point, loc)); }
            switch (OptionIndex)
            {
                case 0:
                    _bc.Start = _bc.Start.With(loc);
                    break;
                case 1:
                    _bc.Start = M(_bc.Start);
                    break;
                case 2:
                    _bc.End = _bc.End.With(loc);
                    break;
                case 3:
                    _bc.End = M(_bc.End);
                    break;
                case 4:
                    _bc.SetReferencePoint(loc, 0);
                    break;
                case 5:
                    _bc.SetReferencePoint(loc, 1);
                    break;
                case 6:
                    _bc.SetReferencePoint(loc, 2);
                    break;

                case 7:
                    _bc.SetReferencePoint(loc, 100);
                    break;
                case 8:
                    _bc.SetReferencePoint(loc, 101);
                    break;
                case 9:
                    _bc.SetReferencePoint(loc, 102);
                    break;
            }

            OnChanged();
        }

        return DrawingHandleResult.Continue;
    }

    public DrawingHandleResult HandleOnMouseMove(MouseEventArgs args) { return DrawingHandleResult.Continue; }

    public DrawingHandleResult HandleOnMouseUp(MouseEventArgs e) { return DrawingHandleResult.Continue; }

    public void MaximumArc()
    {
        if (_bc is OneReferencePointPathCalculator one)
            one.MaximumArc();
        OnChanged();
    }

    public void SetPointsCount(int count)
    {
        switch (count)
        {
            case 1:
                _bc = new OneReferencePointPathCalculator();
                break;
            case 2:
                _bc = new TwoReferencePointsPathCalculator();
                break;
            case 3:
                _bc = new ThreeReferencePointsPathCalculator();
                break;
            default: throw new NotImplementedException();
        }

        _bc.InitDemo();
        OnChanged();
    }

    public int OptionIndex { get; set; }

    private ReferencePointPathCalculator _bc;
}
