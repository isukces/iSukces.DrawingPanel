using System;
using System.Runtime.CompilerServices;
using System.Windows;

namespace iSukces.DrawingPanel.Paths
{
    public static class MathUtils
    {
        public static Point Average(Point a, Point b)
        {
            var x = (a.X + b.X) * 0.5;
            var y = (a.Y + b.Y) * 0.5;
            return new Point(x, y);
        }

        public static Point? GetCrossPoint(Point start, Vector vStart, Point end, Vector vEnd)
        {
            var l1         = LineEquationNotNormalized.FromPointAndDeltas(start, vStart);
            var l2         = LineEquationNotNormalized.FromPointAndDeltas(end, vEnd);
            var crossPoint = l1.CrossWith(l2);
            return crossPoint;
        }

        public static Vector GetNormalized(this Vector v)
        {
            v.Normalize();
            return v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(this Vector v) { return v.X.Equals(0d) && v.Y.Equals(0d); }

        public static Vector NormalizeFast(this Vector src)
        {
            var x = src.X;
            var y = src.Y;

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
                            x       =  minusOne;
                            vLength =  Math.Sqrt(1 + y * y);
                        }
                        else
                        {
                            x       /= absY;
                            y       =  minusOne;
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
                            y       =  minusOne;
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
                            x       =  minusOne;
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
                            y /= absX;
                            x =  1;
                            var a = y * y;
                            a++;
                            vLength = Math.Sqrt(a);
                        }
                        else
                        {
                            x /= absY;
                            y =  1;
                            var a = x * x;
                            a++;
                            vLength = Math.Sqrt(a);
                        }
                    }
                }

                x /= vLength;
                y /= vLength;
            }
            return new Vector(x, y);
        }

        const double minusOne = -1d;



        public static bool IsAngleBetweenSmallEnoughtBasedOnH(Vector vector1, Vector vector2, double h)
        {

            /*vector1 = new Vector(100, 0);
            h       = 2;*/
            
            
            /*
            const double h  = 0.001;
            const double h2 = h * h;
            double       a  = 1000.0 / 2;

            var sin = (2 * a * h) / (h2 + a * a);
            
            sinus ^2 = (4*A^2*H^2)/(H^2+A^2)^2
            */

            // (2*A*H)/(H^2+A^2)
            var x1    = vector1.X;
            var x2    = vector2.X;
            var y1    = vector1.Y;
            var y2    = vector2.Y;
            
            see :
            https://raw.githubusercontent.com/isukces/iSukces.DrawingPanel/main/doc/vector_for_arc_compare.jpg
            
            var hSquare      = h * h;
            var aSquare = (x1 * x1 + y1 * y1);
            
            var m = (hSquare + aSquare * 0.25);
            m *= m;
            
            var sinSquareByH = (aSquare * hSquare) / m;
            
            
            
            var cross = x1 * y2 - x2 * y1;

            var lengthSquare2 = (x2 * x2 + y2 * y2);
            var sqr           = aSquare * lengthSquare2;

            var sinusSquare = (cross * cross) / sqr;

            //======================= 
          
            


            return sinusSquare < sinSquareByH;

        }

    }
}
