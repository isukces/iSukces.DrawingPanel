#nullable disable
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace iSukces.DrawingPanel.Benchmark;

[StructLayout(LayoutKind.Auto,Size = 12) ]
public struct FastVector
{
    private readonly double _x;
    private readonly double _y;
    private readonly bool _isUnitVector;

    public FastVector(double x, double y)
    {
        _x            = x;
        _y            = y;
        _isUnitVector = false;
    }
 
    private FastVector(double x, double y, bool isUnitVector) 
    {
        _x            = x;
        _y            = y;
        _isUnitVector = isUnitVector;
    }

    public double X => _x;
    public double Y => _y;

    public bool IsUnitVector => _isUnitVector;


    public double Length
    {
        get
        {
            if (_isUnitVector)
                return 1;
            return Math.Sqrt(_x * _x + _y * _y);
        }
    }

    public double LengthSquared
    {
        get
        {
            if (_isUnitVector)
                return 1;
            return _x * _x + _y * _y;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Normalize(ref double x, ref double y)
    {
        var    xMinus = x < 0;
        var    yMinus = y < 0;
        double vLength;

        if (yMinus)
        {
            var absY = -y;

            if (xMinus)
            {
                var absX = -x;

                if (absX > absY)
                {
                    y       /= absX;
                    x       =  -1;
                    vLength =  Math.Sqrt(y * y - x);
                }
                else
                {
                    x       /= absY;
                    y       =  -1;
                    vLength =  Math.Sqrt(x * x - y);
                }
            }
            else
            {
                var absX = x;
                if (absX > absY)
                {
                    y       /= absX;
                    x       =  1;
                    vLength =  Math.Sqrt(y * y + x);
                }
                else
                {
                    x       /= absY;
                    y       =  -1;
                    vLength =  Math.Sqrt(x * x - y);
                }
            }
        }
        else
        {
            var absY = y;

            if (xMinus)
            {
                var absX = -x;

                if (absX > absY)
                {
                    y       /= absX;
                    x       =  -1;
                    vLength =  Math.Sqrt(y * y - x);
                }
                else
                {
                    x       /= absY;
                    y       =  1;
                    vLength =  Math.Sqrt(x * x + y);
                }
            }
            else
            {
                var absX = x;

                if (absX > absY)
                {
                    y       /= absX;
                    x       =  1;
                    vLength =  Math.Sqrt(y * y + x);
                }
                else
                {
                    x       /= absY;
                    y       =  1;
                    vLength =  Math.Sqrt(x * x + y);
                }
            }
        }

        x /= vLength;
        y /= vLength;
    }

    public FastVector Normalize()
    {
        if (_isUnitVector)
            return this;

        var x = _x;
        var y = _y;

        {
            var    xMinus = x < 0;
            var    yMinus = y < 0;
            double vLength;

            if (yMinus)
            {
                var absY = -y;

                if (xMinus)
                {
                    var absX = -x;

                    if (absX > absY)
                    {
                        y       /= absX;
                        x       =  -1;
                        vLength =  Math.Sqrt(1 + y * y);
                    }
                    else
                    {
                        x       /= absY;
                        y       =  -1;
                        vLength =  Math.Sqrt(x * x + 1);
                    }
                }
                else
                {
                    var absX = x;
                    if (absX > absY)
                    {
                        y       /= absX;
                        x       =  1;
                        vLength =  Math.Sqrt(1 + y * y);
                    }
                    else
                    {
                        x       /= absY;
                        y       =  -1;
                        vLength =  Math.Sqrt(x * x + 1);
                    }
                }
            }
            else
            {
                var absY = y;

                if (xMinus)
                {
                    var absX = -x;

                    if (absX > absY)
                    {
                        y       /= absX;
                        x       =  -1;
                        vLength =  Math.Sqrt(1 + y * y);
                    }
                    else
                    {
                        x       /= absY;
                        y       =  1;
                        vLength =  Math.Sqrt(x * x + 1);
                    }
                }
                else
                {
                    var absX = x;

                    if (absX > absY)
                    {
                        y       /= absX;
                        x       =  1;
                        vLength =  Math.Sqrt(1 + y * y);
                    }
                    else
                    {
                        x       /= absY;
                        y       =  1;
                        vLength =  Math.Sqrt(x * x + 1);
                    }
                }
            }

            x /= vLength;
            y /= vLength;
        }
        return new FastVector(x, y, true);
    }
}
