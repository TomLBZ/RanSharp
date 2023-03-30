using System.ComponentModel;
using System.Numerics;
using System.Text;
using RanSharp.Performance;

namespace RanSharp.Maths
{
    /// <summary>
    /// Array vector struct with special reloaded operators and optimized loop speeds.
    /// <br/>Absolute value (Abs(a)) =========== +a
    /// <br/>Normalized value (a.Normalized()): === ~a
    /// <br/>Dot product (a·b) =============== a * b
    /// <br/>Angle between ((a·b)/(|a|*|b|)) ======== a / b
    /// <br/>Cross product (a X b) ============ a % b
    /// <br/>Optimized Loop: ForEach, Accumulate, MapBy, CombineWith, CompositeWith
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct ArrVector<T> : IVect<T> where T : struct, INumber<T>
    {
        #region Private States
        private readonly T[] values;
        private readonly double Epsilon = 1e-9;
        #endregion

        #region Private Static Values
        private static readonly T[] zeroArray = Array.Empty<T>();
        private static readonly T minValue = T.CreateSaturating(double.MinValue);
        private static readonly T maxValue = T.CreateSaturating(double.MaxValue);
        #endregion

        #region Public Properties
        /// <summary>
        /// Length of this ArrVector.
        /// </summary>
        public int Length => values.Length;
        /// <summary>
        /// The X Component. Only available if Length >= 1.
        /// </summary>
        public T X => values[0];
        /// <summary>
        /// The Y Component. Only available if Length >= 2.
        /// </summary>
        public T Y => values[1];
        /// <summary>
        /// The Z Component. Only available if Length >= 3.
        /// </summary>
        public T Z => values[2];
        /// <summary>
        /// The W Component. Only available if Length >= 4.
        /// </summary>
        public T W => values[3];
        /// <summary>
        /// Accessing the ArrVector data by index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index] => values[index];
        #endregion

        #region Constructors
        public ArrVector(params T[] values) { this.values = values.ToArray() ?? zeroArray; }
        public ArrVector(int length, T initValue = default)
        {
            values = length > 0 ? new T[length] : zeroArray;
            Span<T> span = values.AsSpan();
            for (int i = 0; i < span.Length; i++)
                span[i] = initValue;
        }
        public ArrVector(IEnumerable<T> values) { this.values = values.ToArray(); }
        public ArrVector(ReadOnlySpan<T> values) { this.values = values.ToArray(); }
        public ArrVector(Span<T> values) { this.values = values.ToArray(); }
        public ArrVector(List<T> values) { this.values = values.ToArray(); }
        public ArrVector(ArrVector<T> a)
        {
            if (a.Length == 0) values = zeroArray;
            else
            {
                values = new T[a.Length];
                Array.Copy(a.values, values, a.Length);
            }
        }
        public ArrVector(int length, Func<int, T> generator)
        {
            values = length > 0 ? new T[length] : zeroArray;
            Span<T> span = values.AsSpan();
            for (int i = 0; i < span.Length; i++)
                span[i] = generator(i);
        }
        public ArrVector(IVect<T> v)
        {
            values = v.Length > 0 ? new T[v.Length] : zeroArray;
            Span<T> span = values.AsSpan();
            for (int i = 0; i < span.Length; i++)
                span[i] = v[i];
        }
        #endregion

        #region Converters
        public static implicit operator ArrVector<T>(T[] values) => new(values);
        public static implicit operator ArrVector<T>(List<T> values) => new(values);
        public static implicit operator ArrVector<T>(ReadOnlySpan<T> values) => new(values);
        public static implicit operator ArrVector<T>(Span<T> values) => new(values);
        public static implicit operator T[](ArrVector<T> values) => values.values.ToArray();
        public static implicit operator List<T>(ArrVector<T> values) => values.values.ToList();
        public static implicit operator ReadOnlySpan<T>(ArrVector<T> values) => values.values.AsSpan(); // pointers! (readonly)
        public static implicit operator Span<T>(ArrVector<T> values) => values.values.AsSpan(); // pointers!
        #endregion

        #region Public Override Methods
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append('(');
            ReadOnlySpan<T> span = values.AsSpan();
            for (int i = 0; i < span.Length; i++)
            {
                sb.Append(span[i]);
                if (i < span.Length - 1) sb.Append(", ");
            }
            sb.Append(')');
            return sb.ToString();
        }
        public override bool Equals(object? obj)
        {
            if (null == obj) return false;
            if (obj is not ArrVector<T>) return false;
            ArrVector<T> mv = (ArrVector<T>)obj;
            return values.Equals(mv.values);
        }
        public override int GetHashCode()
        {
            return values.GetHashCode();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T Max() => Accumulate(ArrVector<T>.minValue, (acc, x) => x > acc ? x : acc);
        public T Min() => Accumulate(ArrVector<T>.maxValue, (acc, x) => x < acc ? x : acc);
        public T Sum() => Accumulate(T.Zero, (acc, x) => acc + x);
        public T Mag2() => Accumulate(T.Zero, (acc, x) => acc + x * x);
        public T Mag() => Calc<T>.Calc1(Mag2(), Math.Sqrt);
        public ArrVector<T> Norm() => this / Mag();
        public ArrVector<T> Abs() => Map(x => x < T.Zero ? -x : x);
        public ArrVector<T> Composite(ArrVector<T> other, Func<T, T, T> func) => NumericLoop<T>.Composite(this, other, func);
        public ArrVector<T> Combine(T b, Func<T, T, T> func) => NumericLoop<T>.Combine(this, b, func);
        public ArrVector<T> Map(Func<T, T> func) => NumericLoop<T>.Map(this, func);
        public ArrVector<T> ReMap(Func<int, T> func) => NumericLoop<T>.ReMap(this, func);
        public void CompositeInPlace(ArrVector<T> other, Func<T, T, T> func) => NumericLoop<T>.CompositeInPlace(this, other, func);
        public void CombineInPlace(T b, Func<T, T, T> func) => NumericLoop<T>.CombineInPlace(this, b, func);
        public void MapInPlace(Func<T, T> func) => NumericLoop<T>.MapInPlace(this, func);
        public void ReMapInPlace(Func<int, T> func) => NumericLoop<T>.ReMapInPlace(this, func);
        public T Accumulate(T acc, Func<T, T, T> func) => NumericLoop<T>.Accumulate(this, acc, func);
        public void Apply(Action<T> action) => NumericLoop<T>.Apply(this, action);
        public bool TrueForAll(Func<T, bool> func) => NumericLoop<T>.TrueForAll(this, func);
        public bool TrueForAny(Func<T, bool> func) => NumericLoop<T>.TrueForAny(this, func);
        public bool Near(ArrVector<T> other)
        {
            ReadOnlySpan<T> spana = values.AsSpan();
            ReadOnlySpan<T> spanb = other.values.AsSpan();
            bool isNear = true;
            for (int i = 0; i < spana.Length; i++)
            {
                isNear &= Calc<T>.Near(spana[i], spanb[i], Epsilon);
                if (!isNear) break;
            }
            return isNear;
        }
        #endregion

        #region Public Static Methods
        public static ArrVector<T> Empty() => new(0);
        public static ArrVector<T> Zero(int length) => new(length, T.Zero);
        public static ArrVector<T> One(int length) => new(length, T.One);
        public static ArrVector<T> Unit(int length, int axis)
        {
            if (length <= axis) throw new ArgumentOutOfRangeException(nameof(axis));
            ArrVector<T> vec = Zero(length);
            vec.values[axis] = T.One;
            return vec;
        }
        public static ArrVector<T> Unit(int length, Axis axis) => Unit(length, (int)axis);
        #endregion

        #region Operators
        // unary operators
        /// <summary>
        /// Absolute value of a
        /// </summary>
        /// <param name="a">ArrVector a</param>
        /// <returns>ArrVector abs(a)</returns>
        public static ArrVector<T> operator +(ArrVector<T> a) => a.Abs();
        public static ArrVector<T> operator -(ArrVector<T> a) => a.Map(x => -x);
        public static ArrVector<T> operator ++(ArrVector<T> a) => a.Map(x => ++x);
        public static ArrVector<T> operator --(ArrVector<T> a) => a.Map(x => --x);
        /// <summary>
        /// Normalized value of a
        /// </summary>
        /// <param name="a">ArrVector a</param>
        /// <returns>ArrVector a.Normalized()</returns>
        public static ArrVector<T> operator ~(ArrVector<T> a) => a.Norm();
        // binary operators
        public static ArrVector<T> operator +(ArrVector<T> a, ArrVector<T> b) => a.Composite(b, (a, b) => a + b);
        public static ArrVector<T> operator +(ArrVector<T> a, T b) => a.Combine(b, (a, b) => a + b);
        public static ArrVector<T> operator +(T a, ArrVector<T> b) => b.Combine(a, (a, b) => a + b);
        public static ArrVector<T> operator -(ArrVector<T> a, ArrVector<T> b) => a.Composite(b, (a, b) => a - b);
        public static ArrVector<T> operator -(ArrVector<T> a, T b) => a.Combine(b, (a, b) => a - b);
        public static ArrVector<T> operator -(T a, ArrVector<T> b) => b.Combine(a, (a, b) => a - b);
        public static ArrVector<T> operator *(ArrVector<T> a, T b) => a.Combine(b, (a, b) => a * b);
        public static ArrVector<T> operator *(T a, ArrVector<T> b) => b.Combine(a, (a, b) => a * b);
        public static ArrVector<T> operator /(ArrVector<T> a, T b) => a.Combine(b, (a, b) => a / b);
        public static ArrVector<T> operator /(T a, ArrVector<T> b) => b.Combine(a, (a, b) => a / b);
        public static ArrVector<T> operator ^(ArrVector<T> a, T b) => a.Combine(b, (a, b) => Calc<T>.Calc2(a, b, Math.Pow));
        public static ArrVector<T> operator ^(T a, ArrVector<T> b) => b.Combine(a, (a, b) => Calc<T>.Calc2(a, b, Math.Pow));
        /// <summary>
        /// Dot product of a and b
        /// </summary>
        /// <param name="a">ArrVector a</param>
        /// <param name="b">ArrVector b</param>
        /// <returns>The dot product a·b of type T</returns>
        public static T operator *(ArrVector<T> a, ArrVector<T> b) => a.Composite(b, (a, b) => a * b).Sum();
        /// <summary>
        /// The angle between a and b
        /// </summary>
        /// <param name="a">ArrVector a</param>
        /// <param name="b">ArrVector b</param>
        /// <returns>(a·b)/(|a||b|)</returns>
        public static T operator /(ArrVector<T> a, ArrVector<T> b) => Calc<T>.Calc1(a * b / (a.Mag() * b.Mag()), Math.Acos);
        /// <summary>
        /// Cross product of a and b
        /// </summary>
        /// <param name="a">ArrVector a</param>
        /// <param name="b">ArrVector b</param>
        /// <returns>The cross product a X b of type T</returns>
        /// <exception cref="ArgumentException"></exception>
        public static ArrVector<T> operator %(ArrVector<T> a, ArrVector<T> b)
        {
            if (a.Length != 3 || b.Length != 3) throw new ArgumentException("Cross product needs 3 dimensions!");
            return new(a[1] * b[2] - a[2] * b[1], a[2] * b[0] - a[0] * b[2], a[0] * b[1] - a[1] * b[0]);
        }
        public static bool operator ==(ArrVector<T> a, ArrVector<T> b) => a.Equals(b);
        public static bool operator !=(ArrVector<T> a, ArrVector<T> b) => !a.Equals(b);
        public static bool operator >(ArrVector<T> a, ArrVector<T> b) => a.Mag2() > b.Mag2();
        public static bool operator <(ArrVector<T> a, ArrVector<T> b) => a.Mag2() < b.Mag2();
        #endregion
    }
}
