namespace RanSharp.Performance
{
    /// <summary>
    /// A class that provides extension methods for lists.
    /// </summary>
    public static class ListExtensions
    {
        #region Composite
        /// <summary>
        /// Composite two lists of the same length together using a function.
        /// </summary>
        public static List<bool> Composite(this List<bool> a, List<bool> b, Func<bool, bool, bool> f) => Loop<bool>.Composite(a, b, f);
        /// <summary>
        /// Composite two lists of the same length together using a function.
        /// </summary>
        public static List<byte> Composite(this List<byte> a, List<byte> b, Func<byte, byte, byte> f) => Loop<byte>.Composite(a, b, f);
        /// <summary>
        /// Composite two lists of the same length together using a function.
        /// </summary>
        public static List<char> Composite(this List<char> a, List<char> b, Func<char, char, char> f) => Loop<char>.Composite(a, b, f);
        /// <summary>
        /// Composite two lists of the same length together using a function.
        /// </summary>
        public static List<int> Composite(this List<int> a, List<int> b, Func<int, int, int> f) => Loop<int>.Composite(a, b, f);
        /// <summary>
        /// Composite two lists of the same length together using a function.
        /// </summary>
        public static List<long> Composite(this List<long> a, List<long> b, Func<long, long, long> f) => Loop<long>.Composite(a, b, f);
        /// <summary>
        /// Composite two lists of the same length together using a function.
        /// </summary>
        public static List<float> Composite(this List<float> a, List<float> b, Func<float, float, float> f) => Loop<float>.Composite(a, b, f);
        /// <summary>
        /// Composite two lists of the same length together using a function.
        /// </summary>
        public static List<double> Composite(this List<double> a, List<double> b, Func<double, double, double> f) => Loop<double>.Composite(a, b, f);
        /// <summary>
        /// Composite two lists of the same length together using a function.
        /// </summary>
        public static List<string> Composite(this List<string> a, List<string> b, Func<string, string, string> f) => Loop<string>.Composite(a, b, f);
        #endregion

        #region Combine
        /// <summary>
        /// Combines all elements of the list with a value using a function.
        /// </summary>
        public static List<bool> Combine(this List<bool> a, bool b, Func<bool, bool, bool> f) => Loop<bool>.Combine(a, b, f);
        /// <summary>
        /// Combines all elements of the list with a value using a function.
        /// </summary>
        public static List<byte> Combine(this List<byte> a, byte b, Func<byte, byte, byte> f) => Loop<byte>.Combine(a, b, f);
        /// <summary>
        /// Combines all elements of the list with a value using a function.
        /// </summary>
        public static List<char> Combine(this List<char> a, char b, Func<char, char, char> f) => Loop<char>.Combine(a, b, f);
        /// <summary>
        /// Combines all elements of the list with a value using a function.
        /// </summary>
        public static List<int> Combine(this List<int> a, int b, Func<int, int, int> f) => Loop<int>.Combine(a, b, f);
        /// <summary>
        /// Combines all elements of the list with a value using a function.
        /// </summary>
        public static List<long> Combine(this List<long> a, long b, Func<long, long, long> f) => Loop<long>.Combine(a, b, f);
        /// <summary>
        /// Combines all elements of the list with a value using a function.
        /// </summary>
        public static List<float> Combine(this List<float> a, float b, Func<float, float, float> f) => Loop<float>.Combine(a, b, f);
        /// <summary>
        /// Combines all elements of the list with a value using a function.
        /// </summary>
        public static List<double> Combine(this List<double> a, double b, Func<double, double, double> f) => Loop<double>.Combine(a, b, f);
        /// <summary>
        /// Combines all elements of the list with a value using a function.
        /// </summary>
        public static List<string> Combine(this List<string> a, string b, Func<string, string, string> f) => Loop<string>.Combine(a, b, f);
        #endregion

        #region Map
        /// <summary>
        /// Maps all elements of the list to a corresponding element in a new list using a function.
        /// </summary>
        public static List<bool> Map(this List<bool> a, Func<bool, bool> f) => Loop<bool>.Map(a, f);
        /// <summary>
        /// Maps all elements of the list to a corresponding element in a new list using a function.
        /// </summary>
        public static List<byte> Map(this List<byte> a, Func<byte, byte> f) => Loop<byte>.Map(a, f);
        /// <summary>
        /// Maps all elements of the list to a corresponding element in a new list using a function.
        /// </summary>
        public static List<char> Map(this List<char> a, Func<char, char> f) => Loop<char>.Map(a, f);
        /// <summary>
        /// Maps all elements of the list to a corresponding element in a new list using a function.
        /// </summary>
        public static List<int> Map(this List<int> a, Func<int, int> f) => Loop<int>.Map(a, f);
        /// <summary>
        /// Maps all elements of the list to a corresponding element in a new list using a function.
        /// </summary>
        public static List<long> Map(this List<long> a, Func<long, long> f) => Loop<long>.Map(a, f);
        /// <summary>
        /// Maps all elements of the list to a corresponding element in a new list using a function.
        /// </summary>
        public static List<float> Map(this List<float> a, Func<float, float> f) => Loop<float>.Map(a, f);
        /// <summary>
        /// Maps all elements of the list to a corresponding element in a new list using a function.
        /// </summary>
        public static List<double> Map(this List<double> a, Func<double, double> f) => Loop<double>.Map(a, f);
        /// <summary>
        /// Maps all elements of the list to a corresponding element in a new list using a function.
        /// </summary>
        public static List<string> Map(this List<string> a, Func<string, string> f) => Loop<string>.Map(a, f);
        #endregion

        #region ReMap
        /// <summary>
        /// Maps all indices of the list to a corresponding element in a new list using a function.
        /// </summary>
        public static List<bool> ReMap(this List<bool> a, Func<int, bool> f) => Loop<bool>.ReMap(a, f);
        /// <summary>
        /// Maps all indices of the list to a corresponding element in a new list using a function.
        /// </summary>
        public static List<byte> ReMap(this List<byte> a, Func<int, byte> f) => Loop<byte>.ReMap(a, f);
        /// <summary>
        /// Maps all indices of the list to a corresponding element in a new list using a function.
        /// </summary>
        public static List<char> ReMap(this List<char> a, Func<int, char> f) => Loop<char>.ReMap(a, f);
        /// <summary>
        /// Maps all indices of the list to a corresponding element in a new list using a function.
        /// </summary>
        public static List<int> ReMap(this List<int> a, Func<int, int> f) => Loop<int>.ReMap(a, f);
        /// <summary>
        /// Maps all indices of the list to a corresponding element in a new list using a function.
        /// </summary>
        public static List<long> ReMap(this List<long> a, Func<int, long> f) => Loop<long>.ReMap(a, f);
        /// <summary>
        /// Maps all indices of the list to a corresponding element in a new list using a function.
        /// </summary>
        public static List<float> ReMap(this List<float> a, Func<int, float> f) => Loop<float>.ReMap(a, f);
        /// <summary>
        /// Maps all indices of the list to a corresponding element in a new list using a function.
        /// </summary>
        public static List<double> ReMap(this List<double> a, Func<int, double> f) => Loop<double>.ReMap(a, f);
        /// <summary>
        /// Maps all indices of the list to a corresponding element in a new list using a function.
        /// </summary>
        public static List<string> ReMap(this List<string> a, Func<int, string> f) => Loop<string>.ReMap(a, f);
        #endregion

        #region Apply
        /// <summary>
        /// Applies a function using each element of the list as input. Does not modify the list.
        /// </summary>
        public static void Apply(this List<bool> a, Action<bool> f) => Loop<bool>.Apply(a, f);
        /// <summary>
        /// Applies a function using each element of the list as input. Does not modify the list.
        /// </summary>
        public static void Apply(this List<byte> a, Action<byte> f) => Loop<byte>.Apply(a, f);
        /// <summary>
        /// Applies a function using each element of the list as input. Does not modify the list.
        /// </summary>
        public static void Apply(this List<char> a, Action<char> f) => Loop<char>.Apply(a, f);
        /// <summary>
        /// Applies a function using each element of the list as input. Does not modify the list.
        /// </summary>
        public static void Apply(this List<int> a, Action<int> f) => Loop<int>.Apply(a, f);
        /// <summary>
        /// Applies a function using each element of the list as input. Does not modify the list.
        /// </summary>
        public static void Apply(this List<long> a, Action<long> f) => Loop<long>.Apply(a, f);
        /// <summary>
        /// Applies a function using each element of the list as input. Does not modify the list.
        /// </summary>
        public static void Apply(this List<float> a, Action<float> f) => Loop<float>.Apply(a, f);
        /// <summary>
        /// Applies a function using each element of the list as input. Does not modify the list.
        /// </summary>
        public static void Apply(this List<double> a, Action<double> f) => Loop<double>.Apply(a, f);
        /// <summary>
        /// Applies a function using each element of the list as input. Does not modify the list.
        /// </summary>
        public static void Apply(this List<string> a, Action<string> f) => Loop<string>.Apply(a, f);
        #endregion

        #region Accumulate
        /// <summary>
        /// Accumulates all elements of the list into one accumulator value using a function.
        /// </summary>
        public static bool Accumulate(this List<bool> a, bool b, Func<bool, bool, bool> f) => Loop<bool>.Accumulate(a, b, f);
        /// <summary>
        /// Accumulates all elements of the list into one accumulator value using a function.
        /// </summary>
        public static byte Accumulate(this List<byte> a, byte b, Func<byte, byte, byte> f) => Loop<byte>.Accumulate(a, b, f);
        /// <summary>
        /// Accumulates all elements of the list into one accumulator value using a function.
        /// </summary>
        public static char Accumulate(this List<char> a, char b, Func<char, char, char> f) => Loop<char>.Accumulate(a, b, f);
        /// <summary>
        /// Accumulates all elements of the list into one accumulator value using a function.
        /// </summary>
        public static int Accumulate(this List<int> a, int b, Func<int, int, int> f) => Loop<int>.Accumulate(a, b, f);
        /// <summary>
        /// Accumulates all elements of the list into one accumulator value using a function.
        /// </summary>
        public static long Accumulate(this List<long> a, long b, Func<long, long, long> f) => Loop<long>.Accumulate(a, b, f);
        /// <summary>
        /// Accumulates all elements of the list into one accumulator value using a function.
        /// </summary>
        public static float Accumulate(this List<float> a, float b, Func<float, float, float> f) => Loop<float>.Accumulate(a, b, f);
        /// <summary>
        /// Accumulates all elements of the list into one accumulator value using a function.
        /// </summary>
        public static double Accumulate(this List<double> a, double b, Func<double, double, double> f) => Loop<double>.Accumulate(a, b, f);
        /// <summary>
        /// Accumulates all elements of the list into one accumulator value using a function.
        /// </summary>
        public static string Accumulate(this List<string> a, string b, Func<string, string, string> f) => Loop<string>.Accumulate(a, b, f);
        #endregion

        #region TrueForAll
        /// <summary>
        /// Returns true if all elements of the list satisfy the provided function.
        /// </summary>
        public static bool TrueForAll(this List<bool> a, Func<bool, bool> f) => Loop<bool>.TrueForAll(a, f);
        /// <summary>
        /// Returns true if all elements of the list satisfy the provided function.
        /// </summary>
        public static bool TrueForAll(this List<byte> a, Func<byte, bool> f) => Loop<byte>.TrueForAll(a, f);
        /// <summary>
        /// Returns true if all elements of the list satisfy the provided function.
        /// </summary>
        public static bool TrueForAll(this List<char> a, Func<char, bool> f) => Loop<char>.TrueForAll(a, f);
        /// <summary>
        /// Returns true if all elements of the list satisfy the provided function.
        /// </summary>
        public static bool TrueForAll(this List<int> a, Func<int, bool> f) => Loop<int>.TrueForAll(a, f);
        /// <summary>
        /// Returns true if all elements of the list satisfy the provided function.
        /// </summary>
        public static bool TrueForAll(this List<long> a, Func<long, bool> f) => Loop<long>.TrueForAll(a, f);
        /// <summary>
        /// Returns true if all elements of the list satisfy the provided function.
        /// </summary>
        public static bool TrueForAll(this List<float> a, Func<float, bool> f) => Loop<float>.TrueForAll(a, f);
        /// <summary>
        /// Returns true if all elements of the list satisfy the provided function.
        /// </summary>
        public static bool TrueForAll(this List<double> a, Func<double, bool> f) => Loop<double>.TrueForAll(a, f);
        /// <summary>
        /// Returns true if all elements of the list satisfy the provided function.
        /// </summary>
        public static bool TrueForAll(this List<string> a, Func<string, bool> f) => Loop<string>.TrueForAll(a, f);
        #endregion

        #region TrueForAny
        /// <summary>
        /// Returns true if any element of the list satisfies the provided function.
        /// </summary>
        public static bool TrueForAny(this List<bool> a, Func<bool, bool> f) => Loop<bool>.TrueForAny(a, f);
        /// <summary>
        /// Returns true if any element of the list satisfies the provided function.
        /// </summary>
        public static bool TrueForAny(this List<byte> a, Func<byte, bool> f) => Loop<byte>.TrueForAny(a, f);
        /// <summary>
        /// Returns true if any element of the list satisfies the provided function.
        /// </summary>
        public static bool TrueForAny(this List<char> a, Func<char, bool> f) => Loop<char>.TrueForAny(a, f);
        /// <summary>
        /// Returns true if any element of the list satisfies the provided function.
        /// </summary>
        public static bool TrueForAny(this List<int> a, Func<int, bool> f) => Loop<int>.TrueForAny(a, f);
        /// <summary>
        /// Returns true if any element of the list satisfies the provided function.
        /// </summary>
        public static bool TrueForAny(this List<long> a, Func<long, bool> f) => Loop<long>.TrueForAny(a, f);
        /// <summary>
        /// Returns true if any element of the list satisfies the provided function.
        /// </summary>
        public static bool TrueForAny(this List<float> a, Func<float, bool> f) => Loop<float>.TrueForAny(a, f);
        /// <summary>
        /// Returns true if any element of the list satisfies the provided function.
        /// </summary>
        public static bool TrueForAny(this List<double> a, Func<double, bool> f) => Loop<double>.TrueForAny(a, f);
        /// <summary>
        /// Returns true if any element of the list satisfies the provided function.
        /// </summary>
        public static bool TrueForAny(this List<string> a, Func<string, bool> f) => Loop<string>.TrueForAny(a, f);
        #endregion

        #region CompositeInPlace
        /// <summary>
        /// Applies a function to each element of the list and the corresponding element of the other list, and stores the result in the original list.
        /// </summary>
        public static void CompositeInPlace(this List<bool> a, List<bool> b, Func<bool, bool, bool> f) => Loop<bool>.CompositeInPlace(a, b, f);
        /// <summary>
        /// Applies a function to each element of the list and the corresponding element of the other list, and stores the result in the original list.
        /// </summary>
        public static void CompositeInPlace(this List<byte> a, List<byte> b, Func<byte, byte, byte> f) => Loop<byte>.CompositeInPlace(a, b, f);
        /// <summary>
        /// Applies a function to each element of the list and the corresponding element of the other list, and stores the result in the original list.
        /// </summary>
        public static void CompositeInPlace(this List<char> a, List<char> b, Func<char, char, char> f) => Loop<char>.CompositeInPlace(a, b, f);
        /// <summary>
        /// Applies a function to each element of the list and the corresponding element of the other list, and stores the result in the original list.
        /// </summary>
        public static void CompositeInPlace(this List<int> a, List<int> b, Func<int, int, int> f) => Loop<int>.CompositeInPlace(a, b, f);
        /// <summary>
        /// Applies a function to each element of the list and the corresponding element of the other list, and stores the result in the original list.
        /// </summary>
        public static void CompositeInPlace(this List<long> a, List<long> b, Func<long, long, long> f) => Loop<long>.CompositeInPlace(a, b, f);
        /// <summary>
        /// Applies a function to each element of the list and the corresponding element of the other list, and stores the result in the original list.
        /// </summary>
        public static void CompositeInPlace(this List<float> a, List<float> b, Func<float, float, float> f) => Loop<float>.CompositeInPlace(a, b, f);
        /// <summary>
        /// Applies a function to each element of the list and the corresponding element of the other list, and stores the result in the original list.
        /// </summary>
        public static void CompositeInPlace(this List<double> a, List<double> b, Func<double, double, double> f) => Loop<double>.CompositeInPlace(a, b, f);
        /// <summary>
        /// Applies a function to each element of the list and the corresponding element of the other list, and stores the result in the original list.
        /// </summary>
        public static void CompositeInPlace(this List<string> a, List<string> b, Func<string, string, string> f) => Loop<string>.CompositeInPlace(a, b, f);
        #endregion

        #region CombineInPlace
        /// <summary>
        /// Applies a function to each element of the list and the provided value, and stores the result in the original list.
        /// </summary>
        public static void CombineInPlace(this List<bool> a, bool b, Func<bool, bool, bool> f) => Loop<bool>.CombineInPlace(a, b, f);
        /// <summary>
        /// Applies a function to each element of the list and the provided value, and stores the result in the original list.
        /// </summary>
        public static void CombineInPlace(this List<byte> a, byte b, Func<byte, byte, byte> f) => Loop<byte>.CombineInPlace(a, b, f);
        /// <summary>
        /// Applies a function to each element of the list and the provided value, and stores the result in the original list.
        /// </summary>
        public static void CombineInPlace(this List<char> a, char b, Func<char, char, char> f) => Loop<char>.CombineInPlace(a, b, f);
        /// <summary>
        /// Applies a function to each element of the list and the provided value, and stores the result in the original list.
        /// </summary>
        public static void CombineInPlace(this List<int> a, int b, Func<int, int, int> f) => Loop<int>.CombineInPlace(a, b, f);
        /// <summary>
        /// Applies a function to each element of the list and the provided value, and stores the result in the original list.
        /// </summary>
        public static void CombineInPlace(this List<long> a, long b, Func<long, long, long> f) => Loop<long>.CombineInPlace(a, b, f);
        /// <summary>
        /// Applies a function to each element of the list and the provided value, and stores the result in the original list.
        /// </summary>
        public static void CombineInPlace(this List<float> a, float b, Func<float, float, float> f) => Loop<float>.CombineInPlace(a, b, f);
        /// <summary>
        /// Applies a function to each element of the list and the provided value, and stores the result in the original list.
        /// </summary>
        public static void CombineInPlace(this List<double> a, double b, Func<double, double, double> f) => Loop<double>.CombineInPlace(a, b, f);
        /// <summary>
        /// Applies a function to each element of the list and the provided value, and stores the result in the original list.
        /// </summary>
        public static void CombineInPlace(this List<string> a, string b, Func<string, string, string> f) => Loop<string>.CombineInPlace(a, b, f);
        #endregion

        #region MapInPlace
        /// <summary>
        /// Applies a function to each element of the list and stores the result in the original list.
        /// </summary>
        public static void MapInPlace(this List<bool> a, Func<bool, bool> f) => Loop<bool>.MapInPlace(a, f);
        /// <summary>
        /// Applies a function to each element of the list and stores the result in the original list.
        /// </summary>
        public static void MapInPlace(this List<byte> a, Func<byte, byte> f) => Loop<byte>.MapInPlace(a, f);
        /// <summary>
        /// Applies a function to each element of the list and stores the result in the original list.
        /// </summary>
        public static void MapInPlace(this List<char> a, Func<char, char> f) => Loop<char>.MapInPlace(a, f);
        /// <summary>
        /// Applies a function to each element of the list and stores the result in the original list.
        /// </summary>
        public static void MapInPlace(this List<int> a, Func<int, int> f) => Loop<int>.MapInPlace(a, f);
        /// <summary>
        /// Applies a function to each element of the list and stores the result in the original list.
        /// </summary>
        public static void MapInPlace(this List<long> a, Func<long, long> f) => Loop<long>.MapInPlace(a, f);
        /// <summary>
        /// Applies a function to each element of the list and stores the result in the original list.
        /// </summary>
        public static void MapInPlace(this List<float> a, Func<float, float> f) => Loop<float>.MapInPlace(a, f);
        /// <summary>
        /// Applies a function to each element of the list and stores the result in the original list.
        /// </summary>
        public static void MapInPlace(this List<double> a, Func<double, double> f) => Loop<double>.MapInPlace(a, f);
        /// <summary>
        /// Applies a function to each element of the list and stores the result in the original list.
        /// </summary>
        public static void MapInPlace(this List<string> a, Func<string, string> f) => Loop<string>.MapInPlace(a, f);
        #endregion

        #region ReMapInPlace
        /// <summary>
        /// Applies a function to each index of the list and stores the result in the original list.
        /// </summary>
        public static void ReMapInPlace(this List<bool> a, Func<int, bool> f) => Loop<bool>.ReMapInPlace(a, f);
        /// <summary>
        /// Applies a function to each index of the list and stores the result in the original list.
        /// </summary>
        public static void ReMapInPlace(this List<byte> a, Func<int, byte> f) => Loop<byte>.ReMapInPlace(a, f);
        /// <summary>
        /// Applies a function to each index of the list and stores the result in the original list.
        /// </summary>
        public static void ReMapInPlace(this List<char> a, Func<int, char> f) => Loop<char>.ReMapInPlace(a, f);
        /// <summary>
        /// Applies a function to each index of the list and stores the result in the original list.
        /// </summary>
        public static void ReMapInPlace(this List<int> a, Func<int, int> f) => Loop<int>.ReMapInPlace(a, f);
        /// <summary>
        /// Applies a function to each index of the list and stores the result in the original list.
        /// </summary>
        public static void ReMapInPlace(this List<long> a, Func<int, long> f) => Loop<long>.ReMapInPlace(a, f);
        /// <summary>
        /// Applies a function to each index of the list and stores the result in the original list.
        /// </summary>
        public static void ReMapInPlace(this List<float> a, Func<int, float> f) => Loop<float>.ReMapInPlace(a, f);
        /// <summary>
        /// Applies a function to each index of the list and stores the result in the original list.
        /// </summary>
        public static void ReMapInPlace(this List<double> a, Func<int, double> f) => Loop<double>.ReMapInPlace(a, f);
        /// <summary>
        /// Applies a function to each index of the list and stores the result in the original list.
        /// </summary>
        public static void ReMapInPlace(this List<string> a, Func<int, string> f) => Loop<string>.ReMapInPlace(a, f);
        #endregion
    }
}
