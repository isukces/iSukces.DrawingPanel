using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace iSukces.DrawingPanel.Benchmark;

[SimpleJob]
[MemoryDiagnoser]
public class BoolToIntConvertBenchamark
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int OptionImplementation(bool flag) { return flag ? 1 : 0; }
       
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int OptionReversedImplementation(bool flag) { return !flag ? 0 : 1; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int Test(ref bool flag)
    {
        unsafe
        {
            fixed(bool* c1 = &flag)
            {
                var cc = (byte*)c1;
                return *cc;
            }
        }
    }


    private static bool a = true;
    private static bool b = false;


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int UnsafeImplementation(bool flag) { return Test(ref flag); }


    [Benchmark]
    public int OptionReversed() { return OptionReversedImplementation(a) * 3 + OptionReversedImplementation(b) * 4; }


    [Benchmark]
    public int Option() { return OptionImplementation(a) * 3 + OptionImplementation(b) * 4; }

    [Benchmark]
    public int Unsafe() { return UnsafeImplementation(a) * 3 + UnsafeImplementation(b) * 4; }

    [Benchmark]
    public int ConvertToInt32() { return Convert.ToInt32(a) * 3 + Convert.ToInt32(b) * 4; }
}