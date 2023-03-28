using System.Numerics;

namespace RanSharp.Performance
{
    public static class Calc<T> where T : struct, INumber<T>
    {
        #region On T
        public static T Calc1(T a, Func<double, double> f) =>
            T.CreateSaturating(f(double.CreateSaturating(a)));
        public static T Calc2(T a, T b, Func<double, double, double> f) =>
            T.CreateSaturating(f(double.CreateSaturating(a), double.CreateSaturating(b)));
        public static T Calc3(T a, T b, T c, Func<double, double, double, double> f) =>
            T.CreateSaturating(f(double.CreateSaturating(a), double.CreateSaturating(b), double.CreateSaturating(c)));
        #endregion
    }
}
