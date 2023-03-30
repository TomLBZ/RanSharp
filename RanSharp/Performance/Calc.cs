using System.Numerics;

namespace RanSharp.Performance
{
    /// <summary>
    /// A class that contains static methods for performing calculations on values of type T.
    /// </summary>
    public static class Calc<T> where T : struct, INumber<T>
    {
        #region On T
        /// <summary>
        /// Applies a function on 1 double (e.g. Math functions) to 1 value of type T.
        /// </summary>
        public static T Calc1(T a, Func<double, double> f) =>
            T.CreateSaturating(f(double.CreateSaturating(a)));
        /// <summary>
        /// Applies a function on 2 doubles (e.g. Math functions) to 2 values of type T.
        /// </summary>
        public static T Calc2(T a, T b, Func<double, double, double> f) =>
            T.CreateSaturating(f(double.CreateSaturating(a), double.CreateSaturating(b)));
        /// <summary>
        /// Applies a function on 3 doubles (e.g. Math functions) to 3 values of type T.
        /// </summary>
        public static T Calc3(T a, T b, T c, Func<double, double, double, double> f) =>
            T.CreateSaturating(f(double.CreateSaturating(a), double.CreateSaturating(b), double.CreateSaturating(c)));
        /// <summary>
        /// Tests if 2 values of type T are equal within a given epsilon. Default epsilon is 1e-9.
        /// </summary>
        public static bool Near(T a, T b, double epsilon = 1e-9) =>
            Math.Abs(double.CreateSaturating(a) - double.CreateSaturating(b)) < epsilon;
        #endregion
    }
}
