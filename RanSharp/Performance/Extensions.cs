﻿using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RanSharp.Performance
{
    public static class Extensions
    {
        #region List Extensions
        public static List<bool> Composite(this List<bool> a, List<bool> b, Func<bool, bool, bool> f) => Loop<bool>.Composite(a, b, f);
        public static List<byte> Composite(this List<byte> a, List<byte> b, Func<byte, byte, byte> f) => Loop<byte>.Composite(a, b, f);
        public static List<char> Composite(this List<char> a, List<char> b, Func<char, char, char> f) => Loop<char>.Composite(a, b, f);
        public static List<int> Composite(this List<int> a, List<int> b, Func<int, int, int> f) => Loop<int>.Composite(a, b, f);
        public static List<long> Composite(this List<long> a, List<long> b, Func<long, long, long> f) => Loop<long>.Composite(a, b, f);
        public static List<float> Composite(this List<float> a, List<float> b, Func<float, float, float> f) => Loop<float>.Composite(a, b, f);
        public static List<double> Composite(this List<double> a, List<double> b, Func<double, double, double> f) => Loop<double>.Composite(a, b, f);
        public static List<string> Composite(this List<string> a, List<string> b, Func<string, string, string> f) => Loop<string>.Composite(a, b, f);
        public static List<bool> Combine(this List<bool> a, bool b, Func<bool, bool, bool> f) => Loop<bool>.Combine(a, b, f);
        public static List<byte> Combine(this List<byte> a, byte b, Func<byte, byte, byte> f) => Loop<byte>.Combine(a, b, f);
        public static List<char> Combine(this List<char> a, char b, Func<char, char, char> f) => Loop<char>.Combine(a, b, f);
        public static List<int> Combine(this List<int> a, int b, Func<int, int, int> f) => Loop<int>.Combine(a, b, f);
        public static List<long> Combine(this List<long> a, long b, Func<long, long, long> f) => Loop<long>.Combine(a, b, f);
        public static List<float> Combine(this List<float> a, float b, Func<float, float, float> f) => Loop<float>.Combine(a, b, f);
        public static List<double> Combine(this List<double> a, double b, Func<double, double, double> f) => Loop<double>.Combine(a, b, f);
        public static List<string> Combine(this List<string> a, string b, Func<string, string, string> f) => Loop<string>.Combine(a, b, f);
        public static List<bool> Map(this List<bool> a, Func<bool, bool> f) => Loop<bool>.Map(a, f);
        public static List<byte> Map(this List<byte> a, Func<byte, byte> f) => Loop<byte>.Map(a, f);
        public static List<char> Map(this List<char> a, Func<char, char> f) => Loop<char>.Map(a, f);
        public static List<int> Map(this List<int> a, Func<int, int> f) => Loop<int>.Map(a, f);
        public static List<long> Map(this List<long> a, Func<long, long> f) => Loop<long>.Map(a, f);
        public static List<float> Map(this List<float> a, Func<float, float> f) => Loop<float>.Map(a, f);
        public static List<double> Map(this List<double> a, Func<double, double> f) => Loop<double>.Map(a, f);
        public static List<string> Map(this List<string> a, Func<string, string> f) => Loop<string>.Map(a, f);
        public static void Apply(this List<bool> a, Action<bool> f) => Loop<bool>.Apply(a, f);
        public static void Apply(this List<byte> a, Action<byte> f) => Loop<byte>.Apply(a, f);
        public static void Apply(this List<char> a, Action<char> f) => Loop<char>.Apply(a, f);
        public static void Apply(this List<int> a, Action<int> f) => Loop<int>.Apply(a, f);
        public static void Apply(this List<long> a, Action<long> f) => Loop<long>.Apply(a, f);
        public static void Apply(this List<float> a, Action<float> f) => Loop<float>.Apply(a, f);
        public static void Apply(this List<double> a, Action<double> f) => Loop<double>.Apply(a, f);
        public static void Apply(this List<string> a, Action<string> f) => Loop<string>.Apply(a, f);        
        public static bool Accumulate(this List<bool> a, bool b, Func<bool, bool, bool> f) => Loop<bool>.Accumulate(a, b, f);
        public static byte Accumulate(this List<byte> a, byte b, Func<byte, byte, byte> f) => Loop<byte>.Accumulate(a, b, f);
        public static char Accumulate(this List<char> a, char b, Func<char, char, char> f) => Loop<char>.Accumulate(a, b, f);
        public static int Accumulate(this List<int> a, int b, Func<int, int, int> f) => Loop<int>.Accumulate(a, b, f);
        public static long Accumulate(this List<long> a, long b, Func<long, long, long> f) => Loop<long>.Accumulate(a, b, f);
        public static float Accumulate(this List<float> a, float b, Func<float, float, float> f) => Loop<float>.Accumulate(a, b, f);
        public static double Accumulate(this List<double> a, double b, Func<double, double, double> f) => Loop<double>.Accumulate(a, b, f);
        public static string Accumulate(this List<string> a, string b, Func<string, string, string> f) => Loop<string>.Accumulate(a, b, f);
        public static bool TrueForAll(this List<bool> a, Func<bool, bool> f) => Loop<bool>.TrueForAll(a, f);
        public static bool TrueForAll(this List<byte> a, Func<byte, bool> f) => Loop<byte>.TrueForAll(a, f);
        public static bool TrueForAll(this List<char> a, Func<char, bool> f) => Loop<char>.TrueForAll(a, f);
        public static bool TrueForAll(this List<int> a, Func<int, bool> f) => Loop<int>.TrueForAll(a, f);
        public static bool TrueForAll(this List<long> a, Func<long, bool> f) => Loop<long>.TrueForAll(a, f);
        public static bool TrueForAll(this List<float> a, Func<float, bool> f) => Loop<float>.TrueForAll(a, f);
        public static bool TrueForAll(this List<double> a, Func<double, bool> f) => Loop<double>.TrueForAll(a, f);
        public static bool TrueForAll(this List<string> a, Func<string, bool> f) => Loop<string>.TrueForAll(a, f);
        public static bool TrueForAny(this List<bool> a, Func<bool, bool> f) => Loop<bool>.TrueForAny(a, f);
        public static bool TrueForAny(this List<byte> a, Func<byte, bool> f) => Loop<byte>.TrueForAny(a, f);
        public static bool TrueForAny(this List<char> a, Func<char, bool> f) => Loop<char>.TrueForAny(a, f);
        public static bool TrueForAny(this List<int> a, Func<int, bool> f) => Loop<int>.TrueForAny(a, f);
        public static bool TrueForAny(this List<long> a, Func<long, bool> f) => Loop<long>.TrueForAny(a, f);
        public static bool TrueForAny(this List<float> a, Func<float, bool> f) => Loop<float>.TrueForAny(a, f);
        public static bool TrueForAny(this List<double> a, Func<double, bool> f) => Loop<double>.TrueForAny(a, f);
        public static bool TrueForAny(this List<string> a, Func<string, bool> f) => Loop<string>.TrueForAny(a, f);
        public static void CompositeInPlace(this List<bool> a, List<bool> b, Func<bool, bool, bool> f) => Loop<bool>.CompositeInPlace(a, b, f);
        public static void CompositeInPlace(this List<byte> a, List<byte> b, Func<byte, byte, byte> f) => Loop<byte>.CompositeInPlace(a, b, f);
        public static void CompositeInPlace(this List<char> a, List<char> b, Func<char, char, char> f) => Loop<char>.CompositeInPlace(a, b, f);
        public static void CompositeInPlace(this List<int> a, List<int> b, Func<int, int, int> f) => Loop<int>.CompositeInPlace(a, b, f);
        public static void CompositeInPlace(this List<long> a, List<long> b, Func<long, long, long> f) => Loop<long>.CompositeInPlace(a, b, f);
        public static void CompositeInPlace(this List<float> a, List<float> b, Func<float, float, float> f) => Loop<float>.CompositeInPlace(a, b, f);
        public static void CompositeInPlace(this List<double> a, List<double> b, Func<double, double, double> f) => Loop<double>.CompositeInPlace(a, b, f);
        public static void CompositeInPlace(this List<string> a, List<string> b, Func<string, string, string> f) => Loop<string>.CompositeInPlace(a, b, f);
        public static void CombineInPlace(this List<bool> a, bool b, Func<bool, bool, bool> f) => Loop<bool>.CombineInPlace(a, b, f);
        public static void CombineInPlace(this List<byte> a, byte b, Func<byte, byte, byte> f) => Loop<byte>.CombineInPlace(a, b, f);
        public static void CombineInPlace(this List<char> a, char b, Func<char, char, char> f) => Loop<char>.CombineInPlace(a, b, f);
        public static void CombineInPlace(this List<int> a, int b, Func<int, int, int> f) => Loop<int>.CombineInPlace(a, b, f);
        public static void CombineInPlace(this List<long> a, long b, Func<long, long, long> f) => Loop<long>.CombineInPlace(a, b, f);
        public static void CombineInPlace(this List<float> a, float b, Func<float, float, float> f) => Loop<float>.CombineInPlace(a, b, f);
        public static void CombineInPlace(this List<double> a, double b, Func<double, double, double> f) => Loop<double>.CombineInPlace(a, b, f);
        public static void CombineInPlace(this List<string> a, string b, Func<string, string, string> f) => Loop<string>.CombineInPlace(a, b, f);
        public static void MapInPlace(this List<bool> a, Func<bool, bool> f) => Loop<bool>.MapInPlace(a, f);
        public static void MapInPlace(this List<byte> a, Func<byte, byte> f) => Loop<byte>.MapInPlace(a, f);
        public static void MapInPlace(this List<char> a, Func<char, char> f) => Loop<char>.MapInPlace(a, f);
        public static void MapInPlace(this List<int> a, Func<int, int> f) => Loop<int>.MapInPlace(a, f);
        public static void MapInPlace(this List<long> a, Func<long, long> f) => Loop<long>.MapInPlace(a, f);
        public static void MapInPlace(this List<float> a, Func<float, float> f) => Loop<float>.MapInPlace(a, f);
        public static void MapInPlace(this List<double> a, Func<double, double> f) => Loop<double>.MapInPlace(a, f);
        public static void MapInPlace(this List<string> a, Func<string, string> f) => Loop<string>.MapInPlace(a, f);
        #endregion

        #region Array Extensions
        public static bool[] Composite(this bool[] a, bool[] b, Func<bool, bool, bool> f) => Loop<bool>.Composite(a, b, f);
        public static byte[] Composite(this byte[] a, byte[] b, Func<byte, byte, byte> f) => Loop<byte>.Composite(a, b, f);
        public static char[] Composite(this char[] a, char[] b, Func<char, char, char> f) => Loop<char>.Composite(a, b, f);
        public static int[] Composite(this int[] a, int[] b, Func<int, int, int> f) => Loop<int>.Composite(a, b, f);
        public static long[] Composite(this long[] a, long[] b, Func<long, long, long> f) => Loop<long>.Composite(a, b, f);
        public static float[] Composite(this float[] a, float[] b, Func<float, float, float> f) => Loop<float>.Composite(a, b, f);
        public static double[] Composite(this double[] a, double[] b, Func<double, double, double> f) => Loop<double>.Composite(a, b, f);
        public static string[] Composite(this string[] a, string[] b, Func<string, string, string> f) => Loop<string>.Composite(a, b, f);
        public static bool[] Combine(this bool[] a, bool b, Func<bool, bool, bool> f) => Loop<bool>.Combine(a, b, f);
        public static byte[] Combine(this byte[] a, byte b, Func<byte, byte, byte> f) => Loop<byte>.Combine(a, b, f);
        public static char[] Combine(this char[] a, char b, Func<char, char, char> f) => Loop<char>.Combine(a, b, f);
        public static int[] Combine(this int[] a, int b, Func<int, int, int> f) => Loop<int>.Combine(a, b, f);
        public static long[] Combine(this long[] a, long b, Func<long, long, long> f) => Loop<long>.Combine(a, b, f);
        public static float[] Combine(this float[] a, float b, Func<float, float, float> f) => Loop<float>.Combine(a, b, f);
        public static double[] Combine(this double[] a, double b, Func<double, double, double> f) => Loop<double>.Combine(a, b, f);
        public static string[] Combine(this string[] a, string b, Func<string, string, string> f) => Loop<string>.Combine(a, b, f);
        public static bool[] Map(this bool[] a, Func<bool, bool> f) => Loop<bool>.Map(a, f);
        public static byte[] Map(this byte[] a, Func<byte, byte> f) => Loop<byte>.Map(a, f);
        public static char[] Map(this char[] a, Func<char, char> f) => Loop<char>.Map(a, f);
        public static int[] Map(this int[] a, Func<int, int> f) => Loop<int>.Map(a, f);
        public static long[] Map(this long[] a, Func<long, long> f) => Loop<long>.Map(a, f);
        public static float[] Map(this float[] a, Func<float, float> f) => Loop<float>.Map(a, f);
        public static double[] Map(this double[] a, Func<double, double> f) => Loop<double>.Map(a, f);
        public static string[] Map(this string[] a, Func<string, string> f) => Loop<string>.Map(a, f);
        public static void Apply(this bool[] a, Action<bool> f) => Loop<bool>.Apply(a, f);
        public static void Apply(this byte[] a, Action<byte> f) => Loop<byte>.Apply(a, f);
        public static void Apply(this char[] a, Action<char> f) => Loop<char>.Apply(a, f);
        public static void Apply(this int[] a, Action<int> f) => Loop<int>.Apply(a, f);
        public static void Apply(this long[] a, Action<long> f) => Loop<long>.Apply(a, f);
        public static void Apply(this float[] a, Action<float> f) => Loop<float>.Apply(a, f);
        public static void Apply(this double[] a, Action<double> f) => Loop<double>.Apply(a, f);
        public static void Apply(this string[] a, Action<string> f) => Loop<string>.Apply(a, f);
        public static bool Accumulate(this bool[] a, bool b, Func<bool, bool, bool> f) => Loop<bool>.Accumulate(a, b, f);
        public static byte Accumulate(this byte[] a, byte b, Func<byte, byte, byte> f) => Loop<byte>.Accumulate(a, b, f);
        public static char Accumulate(this char[] a, char b, Func<char, char, char> f) => Loop<char>.Accumulate(a, b, f);
        public static int Accumulate(this int[] a, int b, Func<int, int, int> f) => Loop<int>.Accumulate(a, b, f);
        public static long Accumulate(this long[] a, long b, Func<long, long, long> f) => Loop<long>.Accumulate(a, b, f);
        public static float Accumulate(this float[] a, float b, Func<float, float, float> f) => Loop<float>.Accumulate(a, b, f);
        public static double Accumulate(this double[] a, double b, Func<double, double, double> f) => Loop<double>.Accumulate(a, b, f);
        public static string Accumulate(this string[] a, string b, Func<string, string, string> f) => Loop<string>.Accumulate(a, b, f);
        public static bool TrueForAll(this bool[] a, Func<bool, bool> f) => Loop<bool>.TrueForAll(a, f);
        public static bool TrueForAll(this byte[] a, Func<byte, bool> f) => Loop<byte>.TrueForAll(a, f);
        public static bool TrueForAll(this char[] a, Func<char, bool> f) => Loop<char>.TrueForAll(a, f);
        public static bool TrueForAll(this int[] a, Func<int, bool> f) => Loop<int>.TrueForAll(a, f);
        public static bool TrueForAll(this long[] a, Func<long, bool> f) => Loop<long>.TrueForAll(a, f);
        public static bool TrueForAll(this float[] a, Func<float, bool> f) => Loop<float>.TrueForAll(a, f);
        public static bool TrueForAll(this double[] a, Func<double, bool> f) => Loop<double>.TrueForAll(a, f);
        public static bool TrueForAll(this string[] a, Func<string, bool> f) => Loop<string>.TrueForAll(a, f);
        public static bool TrueForAny(this bool[] a, Func<bool, bool> f) => Loop<bool>.TrueForAny(a, f);
        public static bool TrueForAny(this byte[] a, Func<byte, bool> f) => Loop<byte>.TrueForAny(a, f);
        public static bool TrueForAny(this char[] a, Func<char, bool> f) => Loop<char>.TrueForAny(a, f);
        public static bool TrueForAny(this int[] a, Func<int, bool> f) => Loop<int>.TrueForAny(a, f);
        public static bool TrueForAny(this long[] a, Func<long, bool> f) => Loop<long>.TrueForAny(a, f);
        public static bool TrueForAny(this float[] a, Func<float, bool> f) => Loop<float>.TrueForAny(a, f);
        public static bool TrueForAny(this double[] a, Func<double, bool> f) => Loop<double>.TrueForAny(a, f);
        public static bool TrueForAny(this string[] a, Func<string, bool> f) => Loop<string>.TrueForAny(a, f);
        public static void CompositeInPlace(this bool[] a, bool[] b, Func<bool, bool, bool> f) => Loop<bool>.CompositeInPlace(a, b, f);
        public static void CompositeInPlace(this byte[] a, byte[] b, Func<byte, byte, byte> f) => Loop<byte>.CompositeInPlace(a, b, f);
        public static void CompositeInPlace(this char[] a, char[] b, Func<char, char, char> f) => Loop<char>.CompositeInPlace(a, b, f);
        public static void CompositeInPlace(this int[] a, int[] b, Func<int, int, int> f) => Loop<int>.CompositeInPlace(a, b, f);
        public static void CompositeInPlace(this long[] a, long[] b, Func<long, long, long> f) => Loop<long>.CompositeInPlace(a, b, f);
        public static void CompositeInPlace(this float[] a, float[] b, Func<float, float, float> f) => Loop<float>.CompositeInPlace(a, b, f);
        public static void CompositeInPlace(this double[] a, double[] b, Func<double, double, double> f) => Loop<double>.CompositeInPlace(a, b, f);
        public static void CompositeInPlace(this string[] a, string[] b, Func<string, string, string> f) => Loop<string>.CompositeInPlace(a, b, f);
        public static void CombineInPlace(this bool[] a, bool b, Func<bool, bool, bool> f) => Loop<bool>.CombineInPlace(a, b, f);
        public static void CombineInPlace(this byte[] a, byte b, Func<byte, byte, byte> f) => Loop<byte>.CombineInPlace(a, b, f);
        public static void CombineInPlace(this char[] a, char b, Func<char, char, char> f) => Loop<char>.CombineInPlace(a, b, f);
        public static void CombineInPlace(this int[] a, int b, Func<int, int, int> f) => Loop<int>.CombineInPlace(a, b, f);
        public static void CombineInPlace(this long[] a, long b, Func<long, long, long> f) => Loop<long>.CombineInPlace(a, b, f);
        public static void CombineInPlace(this float[] a, float b, Func<float, float, float> f) => Loop<float>.CombineInPlace(a, b, f);
        public static void CombineInPlace(this double[] a, double b, Func<double, double, double> f) => Loop<double>.CombineInPlace(a, b, f);
        public static void CombineInPlace(this string[] a, string b, Func<string, string, string> f) => Loop<string>.CombineInPlace(a, b, f);
        public static void MapInPlace(this bool[] a, Func<bool, bool> f) => Loop<bool>.MapInPlace(a, f);
        public static void MapInPlace(this byte[] a, Func<byte, byte> f) => Loop<byte>.MapInPlace(a, f);
        public static void MapInPlace(this char[] a, Func<char, char> f) => Loop<char>.MapInPlace(a, f);
        public static void MapInPlace(this int[] a, Func<int, int> f) => Loop<int>.MapInPlace(a, f);
        public static void MapInPlace(this long[] a, Func<long, long> f) => Loop<long>.MapInPlace(a, f);
        public static void MapInPlace(this float[] a, Func<float, float> f) => Loop<float>.MapInPlace(a, f);
        public static void MapInPlace(this double[] a, Func<double, double> f) => Loop<double>.MapInPlace(a, f);
        public static void MapInPlace(this string[] a, Func<string, string> f) => Loop<string>.MapInPlace(a, f);
        #endregion


    }
}
