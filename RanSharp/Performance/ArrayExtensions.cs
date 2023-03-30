namespace RanSharp.Performance
{
    /// <summary>
    /// A class to provide extension methods for arrays.
    /// </summary>
    public static class ArrayExtensions
    {
        #region Composite
        /// <summary>
        /// Composite two arrays of the same length using a function.
        /// </summary>
        public static bool[] Composite(this bool[] a, bool[] b, Func<bool, bool, bool> f) => Loop<bool>.Composite(a, b, f);
        /// <summary>
        /// Composite two arrays of the same length using a function.
        /// </summary>
        public static byte[] Composite(this byte[] a, byte[] b, Func<byte, byte, byte> f) => Loop<byte>.Composite(a, b, f);
        /// <summary>
        /// Composite two arrays of the same length using a function.
        /// </summary>
        public static char[] Composite(this char[] a, char[] b, Func<char, char, char> f) => Loop<char>.Composite(a, b, f);
        /// <summary>
        /// Composite two arrays of the same length using a function.
        /// </summary>
        public static int[] Composite(this int[] a, int[] b, Func<int, int, int> f) => Loop<int>.Composite(a, b, f);
        /// <summary>
        /// Composite two arrays of the same length using a function.
        /// </summary>
        public static long[] Composite(this long[] a, long[] b, Func<long, long, long> f) => Loop<long>.Composite(a, b, f);
        /// <summary>
        /// Composite two arrays of the same length using a function.
        /// </summary>
        public static float[] Composite(this float[] a, float[] b, Func<float, float, float> f) => Loop<float>.Composite(a, b, f);
        /// <summary>
        /// Composite two arrays of the same length using a function.
        /// </summary>
        public static double[] Composite(this double[] a, double[] b, Func<double, double, double> f) => Loop<double>.Composite(a, b, f);
        /// <summary>
        /// Composite two arrays of the same length using a function.
        /// </summary>
        public static string[] Composite(this string[] a, string[] b, Func<string, string, string> f) => Loop<string>.Composite(a, b, f);
        #endregion

        #region Combine
        /// <summary>
        /// Combines each element of the array with a value using a function.
        /// </summary>
        public static bool[] Combine(this bool[] a, bool b, Func<bool, bool, bool> f) => Loop<bool>.Combine(a, b, f);
        /// <summary>
        /// Combines each element of the array with a value using a function.
        /// </summary>
        public static byte[] Combine(this byte[] a, byte b, Func<byte, byte, byte> f) => Loop<byte>.Combine(a, b, f);
        /// <summary>
        /// Combines each element of the array with a value using a function.
        /// </summary>
        public static char[] Combine(this char[] a, char b, Func<char, char, char> f) => Loop<char>.Combine(a, b, f);
        /// <summary>
        /// Combines each element of the array with a value using a function.
        /// </summary>
        public static int[] Combine(this int[] a, int b, Func<int, int, int> f) => Loop<int>.Combine(a, b, f);
        /// <summary>
        /// Combines each element of the array with a value using a function.
        /// </summary>
        public static long[] Combine(this long[] a, long b, Func<long, long, long> f) => Loop<long>.Combine(a, b, f);
        /// <summary>
        /// Combines each element of the array with a value using a function.
        /// </summary>
        public static float[] Combine(this float[] a, float b, Func<float, float, float> f) => Loop<float>.Combine(a, b, f);
        /// <summary>
        /// Combines each element of the array with a value using a function.
        /// </summary>
        /// <summary>
        /// Combines each element of the array with a value using a function.
        /// </summary>
        public static double[] Combine(this double[] a, double b, Func<double, double, double> f) => Loop<double>.Combine(a, b, f);
        /// <summary>
        /// Combines each element of the array with a value using a function.
        /// </summary>
        public static string[] Combine(this string[] a, string b, Func<string, string, string> f) => Loop<string>.Combine(a, b, f);
        #endregion

        #region Map
        /// <summary>
        /// Maps each element of the array to a value in a new array using a function.
        /// </summary>
        public static bool[] Map(this bool[] a, Func<bool, bool> f) => Loop<bool>.Map(a, f);
        /// <summary>
        /// Maps each element of the array to a value in a new array using a function.
        /// </summary>
        public static byte[] Map(this byte[] a, Func<byte, byte> f) => Loop<byte>.Map(a, f);
        /// <summary>
        /// Maps each element of the array to a value in a new array using a function.
        /// </summary>
        public static char[] Map(this char[] a, Func<char, char> f) => Loop<char>.Map(a, f);
        /// <summary>
        /// Maps each element of the array to a value in a new array using a function.
        /// </summary>
        public static int[] Map(this int[] a, Func<int, int> f) => Loop<int>.Map(a, f);
        /// <summary>
        /// Maps each element of the array to a value in a new array using a function.
        /// </summary>
        public static long[] Map(this long[] a, Func<long, long> f) => Loop<long>.Map(a, f);
        /// <summary>
        /// Maps each element of the array to a value in a new array using a function.
        /// </summary>
        public static float[] Map(this float[] a, Func<float, float> f) => Loop<float>.Map(a, f);
        /// <summary>
        /// Maps each element of the array to a value in a new array using a function.
        /// </summary>
        public static double[] Map(this double[] a, Func<double, double> f) => Loop<double>.Map(a, f);
        /// <summary>
        /// Maps each element of the array to a value in a new array using a function.
        /// </summary>
        public static string[] Map(this string[] a, Func<string, string> f) => Loop<string>.Map(a, f);
        #endregion

        #region ReMap
        /// <summary>
        /// Maps each index of the array to a value in a new array using a function.
        /// </summary>
        public static bool[] ReMap(this bool[] a, Func<int, bool> f) => Loop<bool>.ReMap(a, f);
        /// <summary>
        /// Maps each index of the array to a value in a new array using a function.
        /// </summary>
        public static byte[] ReMap(this byte[] a, Func<int, byte> f) => Loop<byte>.ReMap(a, f);
        /// <summary>
        /// Maps each index of the array to a value in a new array using a function.
        /// </summary>
        public static char[] ReMap(this char[] a, Func<int, char> f) => Loop<char>.ReMap(a, f);
        /// <summary>
        /// Maps each index of the array to a value in a new array using a function.
        /// </summary>
        public static int[] ReMap(this int[] a, Func<int, int> f) => Loop<int>.ReMap(a, f);
        /// <summary>
        /// Maps each index of the array to a value in a new array using a function.
        /// </summary>
        public static long[] ReMap(this long[] a, Func<int, long> f) => Loop<long>.ReMap(a, f);
        /// <summary>
        /// Maps each index of the array to a value in a new array using a function.
        /// </summary>
        public static float[] ReMap(this float[] a, Func<int, float> f) => Loop<float>.ReMap(a, f);
        /// <summary>
        /// Maps each index of the array to a value in a new array using a function.
        /// </summary>
        public static double[] ReMap(this double[] a, Func<int, double> f) => Loop<double>.ReMap(a, f);
        /// <summary>
        /// Maps each index of the array to a value in a new array using a function.
        /// </summary>
        public static string[] ReMap(this string[] a, Func<int, string> f) => Loop<string>.ReMap(a, f);
        #endregion

        #region Apply
        /// <summary>
        /// Applies a function using each element of the array as input. Does not modify the array.
        /// </summary>
        public static void Apply(this bool[] a, Action<bool> f) => Loop<bool>.Apply(a, f);
        /// <summary>
        /// Applies a function using each element of the array as input. Does not modify the array.
        /// </summary>
        public static void Apply(this byte[] a, Action<byte> f) => Loop<byte>.Apply(a, f);
        /// <summary>
        /// Applies a function using each element of the array as input. Does not modify the array.
        /// </summary>
        public static void Apply(this char[] a, Action<char> f) => Loop<char>.Apply(a, f);
        /// <summary>
        /// Applies a function using each element of the array as input. Does not modify the array.
        /// </summary>
        public static void Apply(this int[] a, Action<int> f) => Loop<int>.Apply(a, f);
        /// <summary>
        /// Applies a function using each element of the array as input. Does not modify the array.
        /// </summary>
        public static void Apply(this long[] a, Action<long> f) => Loop<long>.Apply(a, f);
        /// <summary>
        /// Applies a function using each element of the array as input. Does not modify the array.
        /// </summary>
        public static void Apply(this float[] a, Action<float> f) => Loop<float>.Apply(a, f);
        /// <summary>
        /// Applies a function using each element of the array as input. Does not modify the array.
        /// </summary>
        public static void Apply(this double[] a, Action<double> f) => Loop<double>.Apply(a, f);
        /// <summary>
        /// Applies a function using each element of the array as input. Does not modify the array.
        /// </summary>
        public static void Apply(this string[] a, Action<string> f) => Loop<string>.Apply(a, f);
        #endregion

        #region Accumulate
        /// <summary>
        /// Accumulates the array using a function. The function is applied to each element of the array and the result is stored in the accumulator.
        /// </summary>
        public static bool Accumulate(this bool[] a, bool b, Func<bool, bool, bool> f) => Loop<bool>.Accumulate(a, b, f);
        /// <summary>
        /// Accumulates the array using a function. The function is applied to each element of the array and the result is stored in the accumulator.
        /// </summary>
        public static byte Accumulate(this byte[] a, byte b, Func<byte, byte, byte> f) => Loop<byte>.Accumulate(a, b, f);
        /// <summary>
        /// Accumulates the array using a function. The function is applied to each element of the array and the result is stored in the accumulator.
        /// </summary>
        public static char Accumulate(this char[] a, char b, Func<char, char, char> f) => Loop<char>.Accumulate(a, b, f);
        /// <summary>
        /// Accumulates the array using a function. The function is applied to each element of the array and the result is stored in the accumulator.
        /// </summary>
        public static int Accumulate(this int[] a, int b, Func<int, int, int> f) => Loop<int>.Accumulate(a, b, f);
        /// <summary>
        /// Accumulates the array using a function. The function is applied to each element of the array and the result is stored in the accumulator.
        /// </summary>
        public static long Accumulate(this long[] a, long b, Func<long, long, long> f) => Loop<long>.Accumulate(a, b, f);
        /// <summary>
        /// Accumulates the array using a function. The function is applied to each element of the array and the result is stored in the accumulator.
        /// </summary>
        public static float Accumulate(this float[] a, float b, Func<float, float, float> f) => Loop<float>.Accumulate(a, b, f);
        /// <summary>
        /// Accumulates the array using a function. The function is applied to each element of the array and the result is stored in the accumulator.
        /// </summary>
        public static double Accumulate(this double[] a, double b, Func<double, double, double> f) => Loop<double>.Accumulate(a, b, f);
        /// <summary>
        /// Accumulates the array using a function. The function is applied to each element of the array and the result is stored in the accumulator.
        /// </summary>
        public static string Accumulate(this string[] a, string b, Func<string, string, string> f) => Loop<string>.Accumulate(a, b, f);
        #endregion

        #region TrueForAll
        /// <summary>
        /// Returns true if the function returns true for all elements of the array.
        /// </summary>
        public static bool TrueForAll(this bool[] a, Func<bool, bool> f) => Loop<bool>.TrueForAll(a, f);
        /// <summary>
        /// Returns true if the function returns true for all elements of the array.
        /// </summary>
        public static bool TrueForAll(this byte[] a, Func<byte, bool> f) => Loop<byte>.TrueForAll(a, f);
        /// <summary>
        /// Returns true if the function returns true for all elements of the array.
        /// </summary>
        public static bool TrueForAll(this char[] a, Func<char, bool> f) => Loop<char>.TrueForAll(a, f);
        /// <summary>
        /// Returns true if the function returns true for all elements of the array.
        /// </summary>
        public static bool TrueForAll(this int[] a, Func<int, bool> f) => Loop<int>.TrueForAll(a, f);
        /// <summary>
        /// Returns true if the function returns true for all elements of the array.
        /// </summary>
        public static bool TrueForAll(this long[] a, Func<long, bool> f) => Loop<long>.TrueForAll(a, f);
        /// <summary>
        /// Returns true if the function returns true for all elements of the array.
        /// </summary>
        public static bool TrueForAll(this float[] a, Func<float, bool> f) => Loop<float>.TrueForAll(a, f);
        /// <summary>
        /// Returns true if the function returns true for all elements of the array.
        /// </summary>
        public static bool TrueForAll(this double[] a, Func<double, bool> f) => Loop<double>.TrueForAll(a, f);
        /// <summary>
        /// Returns true if the function returns true for all elements of the array.
        /// </summary>
        public static bool TrueForAll(this string[] a, Func<string, bool> f) => Loop<string>.TrueForAll(a, f);
        #endregion

        #region TrueForAny
        /// <summary>
        /// Returns true if the function returns true for any element of the array.
        /// </summary>
        public static bool TrueForAny(this bool[] a, Func<bool, bool> f) => Loop<bool>.TrueForAny(a, f);
        /// <summary>
        /// Returns true if the function returns true for any element of the array.
        /// </summary>
        public static bool TrueForAny(this byte[] a, Func<byte, bool> f) => Loop<byte>.TrueForAny(a, f);
        /// <summary>
        /// Returns true if the function returns true for any element of the array.
        /// </summary>
        public static bool TrueForAny(this char[] a, Func<char, bool> f) => Loop<char>.TrueForAny(a, f);
        /// <summary>
        /// Returns true if the function returns true for any element of the array.
        /// </summary>
        public static bool TrueForAny(this int[] a, Func<int, bool> f) => Loop<int>.TrueForAny(a, f);
        /// <summary>
        /// Returns true if the function returns true for any element of the array.
        /// </summary>
        public static bool TrueForAny(this long[] a, Func<long, bool> f) => Loop<long>.TrueForAny(a, f);
        /// <summary>
        /// Returns true if the function returns true for any element of the array.
        /// </summary>
        public static bool TrueForAny(this float[] a, Func<float, bool> f) => Loop<float>.TrueForAny(a, f);
        /// <summary>
        /// Returns true if the function returns true for any element of the array.
        /// </summary>
        public static bool TrueForAny(this double[] a, Func<double, bool> f) => Loop<double>.TrueForAny(a, f);
        /// <summary>
        /// Returns true if the function returns true for any element of the array.
        /// </summary>
        public static bool TrueForAny(this string[] a, Func<string, bool> f) => Loop<string>.TrueForAny(a, f);
        #endregion

        #region CompositeInPlace
        /// <summary>
        /// Composite each element of the array with the corresponding element of the other array, modifying the original array.
        /// </summary>
        public static void CompositeInPlace(this bool[] a, bool[] b, Func<bool, bool, bool> f) => Loop<bool>.CompositeInPlace(a, b, f);
        /// <summary>
        /// Composite each element of the array with the corresponding element of the other array, modifying the original array.
        /// </summary>
        public static void CompositeInPlace(this byte[] a, byte[] b, Func<byte, byte, byte> f) => Loop<byte>.CompositeInPlace(a, b, f);
        /// <summary>
        /// Composite each element of the array with the corresponding element of the other array, modifying the original array.
        /// </summary>
        public static void CompositeInPlace(this char[] a, char[] b, Func<char, char, char> f) => Loop<char>.CompositeInPlace(a, b, f);
        /// <summary>
        /// Composite each element of the array with the corresponding element of the other array, modifying the original array.
        /// </summary>
        public static void CompositeInPlace(this int[] a, int[] b, Func<int, int, int> f) => Loop<int>.CompositeInPlace(a, b, f);
        /// <summary>
        /// Composite each element of the array with the corresponding element of the other array, modifying the original array.
        /// </summary>
        public static void CompositeInPlace(this long[] a, long[] b, Func<long, long, long> f) => Loop<long>.CompositeInPlace(a, b, f);
        /// <summary>
        /// Composite each element of the array with the corresponding element of the other array, modifying the original array.
        /// </summary>
        public static void CompositeInPlace(this float[] a, float[] b, Func<float, float, float> f) => Loop<float>.CompositeInPlace(a, b, f);
        /// <summary>
        /// Composite each element of the array with the corresponding element of the other array, modifying the original array.
        /// </summary>
        public static void CompositeInPlace(this double[] a, double[] b, Func<double, double, double> f) => Loop<double>.CompositeInPlace(a, b, f);
        /// <summary>
        /// Composite each element of the array with the corresponding element of the other array, modifying the original array.
        /// </summary>
        public static void CompositeInPlace(this string[] a, string[] b, Func<string, string, string> f) => Loop<string>.CompositeInPlace(a, b, f);
        #endregion

        #region CombineInPlace
        /// <summary>
        /// Combines each element of the array with the given value, modifying the original array.
        /// </summary>
        public static void CombineInPlace(this bool[] a, bool b, Func<bool, bool, bool> f) => Loop<bool>.CombineInPlace(a, b, f);
        /// <summary>
        /// Combines each element of the array with the given value, modifying the original array.
        /// </summary>
        public static void CombineInPlace(this byte[] a, byte b, Func<byte, byte, byte> f) => Loop<byte>.CombineInPlace(a, b, f);
        /// <summary>
        /// Combines each element of the array with the given value, modifying the original array.
        /// </summary>
        public static void CombineInPlace(this char[] a, char b, Func<char, char, char> f) => Loop<char>.CombineInPlace(a, b, f);
        /// <summary>
        /// Combines each element of the array with the given value, modifying the original array.
        /// </summary>
        public static void CombineInPlace(this int[] a, int b, Func<int, int, int> f) => Loop<int>.CombineInPlace(a, b, f);
        /// <summary>
        /// Combines each element of the array with the given value, modifying the original array.
        /// </summary>
        public static void CombineInPlace(this long[] a, long b, Func<long, long, long> f) => Loop<long>.CombineInPlace(a, b, f);
        /// <summary>
        /// Combines each element of the array with the given value, modifying the original array.
        /// </summary>
        public static void CombineInPlace(this float[] a, float b, Func<float, float, float> f) => Loop<float>.CombineInPlace(a, b, f);
        /// <summary>
        /// Combines each element of the array with the given value, modifying the original array.
        /// </summary>
        public static void CombineInPlace(this double[] a, double b, Func<double, double, double> f) => Loop<double>.CombineInPlace(a, b, f);
        /// <summary>
        /// Combines each element of the array with the given value, modifying the original array.
        /// </summary>
        public static void CombineInPlace(this string[] a, string b, Func<string, string, string> f) => Loop<string>.CombineInPlace(a, b, f);
        #endregion

        #region MapInPlace
        /// <summary>
        /// Map each element of the array to a new value, modifying the original array.
        /// </summary>
        public static void MapInPlace(this bool[] a, Func<bool, bool> f) => Loop<bool>.MapInPlace(a, f);
        /// <summary>
        /// Map each element of the array to a new value, modifying the original array.
        /// </summary>
        public static void MapInPlace(this byte[] a, Func<byte, byte> f) => Loop<byte>.MapInPlace(a, f);
        /// <summary>
        /// Map each element of the array to a new value, modifying the original array.
        /// </summary>
        public static void MapInPlace(this char[] a, Func<char, char> f) => Loop<char>.MapInPlace(a, f);
        /// <summary>
        /// Map each element of the array to a new value, modifying the original array.
        /// </summary>
        public static void MapInPlace(this int[] a, Func<int, int> f) => Loop<int>.MapInPlace(a, f);
        /// <summary>
        /// Map each element of the array to a new value, modifying the original array.
        /// </summary>
        public static void MapInPlace(this long[] a, Func<long, long> f) => Loop<long>.MapInPlace(a, f);
        /// <summary>
        /// Map each element of the array to a new value, modifying the original array.
        /// </summary>
        public static void MapInPlace(this float[] a, Func<float, float> f) => Loop<float>.MapInPlace(a, f);
        /// <summary>
        /// Map each element of the array to a new value, modifying the original array.
        /// </summary>
        public static void MapInPlace(this double[] a, Func<double, double> f) => Loop<double>.MapInPlace(a, f);
        /// <summary>
        /// Map each element of the array to a new value, modifying the original array.
        /// </summary>
        public static void MapInPlace(this string[] a, Func<string, string> f) => Loop<string>.MapInPlace(a, f);
        #endregion

        #region ReMapInPlace
        /// <summary>
        /// Map each element of the array to a new value based on its index, modifying the original array.
        /// </summary>
        public static void ReMapInPlace(this bool[] a, Func<int, bool> f) => Loop<bool>.ReMapInPlace(a, f);
        /// <summary>
        /// Map each element of the array to a new value based on its index, modifying the original array.
        /// </summary>
        public static void ReMapInPlace(this byte[] a, Func<int, byte> f) => Loop<byte>.ReMapInPlace(a, f);
        /// <summary>
        /// Map each element of the array to a new value based on its index, modifying the original array.
        /// </summary>
        public static void ReMapInPlace(this char[] a, Func<int, char> f) => Loop<char>.ReMapInPlace(a, f);
        /// <summary>
        /// Map each element of the array to a new value based on its index, modifying the original array.
        /// </summary>
        public static void ReMapInPlace(this int[] a, Func<int, int> f) => Loop<int>.ReMapInPlace(a, f);
        /// <summary>
        /// Map each element of the array to a new value based on its index, modifying the original array.
        /// </summary>
        public static void ReMapInPlace(this long[] a, Func<int, long> f) => Loop<long>.ReMapInPlace(a, f);
        /// <summary>
        /// Map each element of the array to a new value based on its index, modifying the original array.
        /// </summary>
        public static void ReMapInPlace(this float[] a, Func<int, float> f) => Loop<float>.ReMapInPlace(a, f);
        /// <summary>
        /// Map each element of the array to a new value based on its index, modifying the original array.
        /// </summary>
        public static void ReMapInPlace(this double[] a, Func<int, double> f) => Loop<double>.ReMapInPlace(a, f);
        /// <summary>
        /// Map each element of the array to a new value based on its index, modifying the original array.
        /// </summary>
        public static void ReMapInPlace(this string[] a, Func<int, string> f) => Loop<string>.ReMapInPlace(a, f);
        #endregion
    }
}
