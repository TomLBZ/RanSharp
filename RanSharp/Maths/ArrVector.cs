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
        /// <summary>
        /// Creates a new <see cref="ArrVector{T}"/> with the given array.
        /// </summary>
        public ArrVector(params T[] values) { this.values = values.ToArray() ?? zeroArray; }
        /// <summary>
        /// Creates a new <see cref="ArrVector{T}"/> with the given length and initial value.
        /// </summary>
        public ArrVector(int length, T initValue = default)
        {
            values = length > 0 ? new T[length] : zeroArray;
            Span<T> span = values.AsSpan();
            for (int i = 0; i < span.Length; i++)
                span[i] = initValue;
        }
        /// <summary>
        /// Creates a new <see cref="ArrVector{T}"/> with the given <see cref="IEnumerable{T}"/>.
        /// </summary>
        public ArrVector(IEnumerable<T> values) { this.values = values.ToArray(); }
        /// <summary>
        /// Creates a new <see cref="ArrVector{T}"/> with the given <see cref="ReadOnlySpan{T}"/>.
        /// </summary>
        public ArrVector(ReadOnlySpan<T> values) { this.values = values.ToArray(); }
        /// <summary>
        /// Creates a new <see cref="ArrVector{T}"/> with the given <see cref="Span{T}"/>.
        /// </summary>
        public ArrVector(Span<T> values) { this.values = values.ToArray(); }
        /// <summary>
        /// Creates a new <see cref="ArrVector{T}"/> with the given <see cref="List{T}"/>.
        /// </summary>
        public ArrVector(List<T> values) { this.values = values.ToArray(); }
        /// <summary>
        /// Creates a new <see cref="ArrVector{T}"/> with the given <see cref="ArrVector{T}"/>.
        /// </summary>
        public ArrVector(ArrVector<T> a)
        {
            if (a.Length == 0) values = zeroArray;
            else
            {
                values = new T[a.Length];
                Array.Copy(a.values, values, a.Length);
            }
        }
        /// <summary>
        /// Creates a new <see cref="ArrVector{T}"/> with the given length and generator function.
        /// </summary>
        public ArrVector(int length, Func<int, T> generator)
        {
            values = length > 0 ? new T[length] : zeroArray;
            Span<T> span = values.AsSpan();
            for (int i = 0; i < span.Length; i++)
                span[i] = generator(i);
        }
        /// <summary>
        /// Creates a new <see cref="ArrVector{T}"/> with the given <see cref="IVect{T}"/>.
        /// </summary>
        public ArrVector(IVect<T> v)
        {
            values = v.Length > 0 ? new T[v.Length] : zeroArray;
            Span<T> span = values.AsSpan();
            for (int i = 0; i < span.Length; i++)
                span[i] = v[i];
        }
        #endregion

        #region Converters
        /// <summary>
        /// Converts the given array to a <see cref="ArrVector{T}"/> implicitly.
        /// </summary>
        public static implicit operator ArrVector<T>(T[] values) => new(values);
        /// <summary>
        /// Converts the given <see cref="List{T}"/> to a <see cref="ArrVector{T}"/> implicitly.
        /// </summary>
        public static implicit operator ArrVector<T>(List<T> values) => new(values);
        /// <summary>
        /// Converts the given <see cref="ReadOnlySpan{T}"/> to a <see cref="ArrVector{T}"/> implicitly.
        /// </summary>
        public static implicit operator ArrVector<T>(ReadOnlySpan<T> values) => new(values);
        /// <summary>
        /// Converts the given <see cref="Span{T}"/> to a <see cref="ArrVector{T}"/> implicitly.
        /// </summary>
        public static implicit operator ArrVector<T>(Span<T> values) => new(values);
        /// <summary>
        /// Converts the given <see cref="ArrVector{T}"/> to an array implicitly.
        /// </summary>
        public static implicit operator T[](ArrVector<T> values) => values.values.ToArray();
        /// <summary>
        /// Converts the given <see cref="ArrVector{T}"/> to a <see cref="List{T}"/> implicitly.
        /// </summary>
        public static implicit operator List<T>(ArrVector<T> values) => values.values.ToList();
        /// <summary>
        /// Converts the given <see cref="ArrVector{T}"/> to a <see cref="ReadOnlySpan{T}"/> implicitly. 
        /// Note that this <see cref="ReadOnlySpan{T}"/> keeps the reference to the original internal array. 
        /// Looping over it will have the same effect as looping over the original <see cref="ArrVector{T}"/>.
        /// </summary>
        public static implicit operator ReadOnlySpan<T>(ArrVector<T> values) => values.values.AsSpan(); // pointers! (readonly)
        /// <summary>
        /// Converts the given <see cref="ArrVector{T}"/> to a <see cref="Span{T}"/> implicitly.
        /// Note that this <see cref="Span{T}"/> keeps the reference to the original internal array.
        /// Modifying it will have the same effect as modifying the original <see cref="ArrVector{T}"/>.
        /// </summary>
        public static implicit operator Span<T>(ArrVector<T> values) => values.values.AsSpan(); // pointers!
        #endregion

        #region Public Override Methods
        /// <summary>
        /// Returns a string representation of the <see cref="ArrVector{T}"/>.
        /// </summary>
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
        /// <summary>
        /// Returns true if the given object is a <see cref="ArrVector{T}"/> and has the same values as this one.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (null == obj) return false;
            if (obj is not ArrVector<T>) return false;
            ArrVector<T> mv = (ArrVector<T>)obj;
            return values.Equals(mv.values);
        }
        /// <summary>
        /// Returns the hash code of the <see cref="ArrVector{T}"/>.
        /// </summary>
        public override int GetHashCode()
        {
            return values.GetHashCode();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns the Max element of the <see cref="ArrVector{T}"/>.
        /// </summary>
        public T Max() => Accumulate(ArrVector<T>.minValue, (acc, x) => x > acc ? x : acc);
        /// <summary>
        /// Returns the Min element of the <see cref="ArrVector{T}"/>.
        /// </summary>
        public T Min() => Accumulate(ArrVector<T>.maxValue, (acc, x) => x < acc ? x : acc);
        /// <summary>
        /// Returns the sum of all elements of the <see cref="ArrVector{T}"/>.
        /// </summary>
        public T Sum() => Accumulate(T.Zero, (acc, x) => acc + x);
        /// <summary>
        /// Returns the sum of squares of all elements of the <see cref="ArrVector{T}"/>.
        /// </summary>
        public T Mag2() => Accumulate(T.Zero, (acc, x) => acc + x * x);
        /// <summary>
        /// Returns the magnitude of the <see cref="ArrVector{T}"/>.
        /// </summary>
        public T Mag() => Calc<T>.Calc1(Mag2(), Math.Sqrt);
        /// <summary>
        /// Returns normalized version of the <see cref="ArrVector{T}"/>.
        /// </summary>
        public ArrVector<T> Norm() => this / Mag();
        /// <summary>
        /// Returns a new <see cref="ArrVector{T}"/> with each element equal to the absolute value of the corresponding element of this <see cref="ArrVector{T}"/>.
        /// </summary>
        public ArrVector<T> Abs() => Map(x => x < T.Zero ? -x : x);
        /// <summary>
        /// Composites the given <see cref="ArrVector{T}"/> with this one using the given function. Returns the results in a new <see cref="ArrVector{T}"/>.
        /// </summary>
        public ArrVector<T> Composite(ArrVector<T> other, Func<T, T, T> func) => NumericLoop<T>.Composite(this, other, func);
        /// <summary>
        /// Combines this <see cref="ArrVector{T}"/> with the given value using the given function. Returns the results in a new <see cref="ArrVector{T}"/>.
        /// </summary>
        public ArrVector<T> Combine(T b, Func<T, T, T> func) => NumericLoop<T>.Combine(this, b, func);
        /// <summary>
        /// Maps the given function over the elements of this <see cref="ArrVector{T}"/>. Returns the results in a new <see cref="ArrVector{T}"/>.
        /// </summary>
        public ArrVector<T> Map(Func<T, T> func) => NumericLoop<T>.Map(this, func);
        /// <summary>
        /// Maps the given function over the indices of this <see cref="ArrVector{T}"/>. Returns the results in a new <see cref="ArrVector{T}"/>.
        /// </summary>
        public ArrVector<T> ReMap(Func<int, T> func) => NumericLoop<T>.ReMap(this, func);
        /// <summary>
        /// Composites the given <see cref="ArrVector{T}"/> with this one using the given function, storing the result in this <see cref="ArrVector{T}"/>.
        /// </summary>
        public void CompositeInPlace(ArrVector<T> other, Func<T, T, T> func) => NumericLoop<T>.CompositeInPlace(this, other, func);
        /// <summary>
        /// Combines this <see cref="ArrVector{T}"/> with the given value using the given function, storing the result in this <see cref="ArrVector{T}"/>.
        /// </summary>
        public void CombineInPlace(T b, Func<T, T, T> func) => NumericLoop<T>.CombineInPlace(this, b, func);
        /// <summary>
        /// Maps the given function over the elements of this <see cref="ArrVector{T}"/>, storing the result in this <see cref="ArrVector{T}"/>.
        /// </summary>
        public void MapInPlace(Func<T, T> func) => NumericLoop<T>.MapInPlace(this, func);
        /// <summary>
        /// Maps the given function over the indices of this <see cref="ArrVector{T}"/>, storing the result in this <see cref="ArrVector{T}"/>.
        /// </summary>
        public void ReMapInPlace(Func<int, T> func) => NumericLoop<T>.ReMapInPlace(this, func);
        /// <summary>
        /// Accumulates the elements of this <see cref="ArrVector{T}"/> with the given accumulator value using the given function.
        /// </summary>
        public T Accumulate(T acc, Func<T, T, T> func) => NumericLoop<T>.Accumulate(this, acc, func);
        /// <summary>
        /// Applies the given action using each element of this <see cref="ArrVector{T}"/> as input. Does not modify the <see cref="ArrVector{T}"/>.
        /// </summary>
        public void Apply(Action<T> action) => NumericLoop<T>.Apply(this, action);
        /// <summary>
        /// Returns true if the given function returns true for all elements of this <see cref="ArrVector{T}"/>.
        /// </summary>
        public bool TrueForAll(Func<T, bool> func) => NumericLoop<T>.TrueForAll(this, func);
        /// <summary>
        /// Returns true if the given function returns true for any element of this <see cref="ArrVector{T}"/>.
        /// </summary>
        public bool TrueForAny(Func<T, bool> func) => NumericLoop<T>.TrueForAny(this, func);
        /// <summary>
        /// Checks if the given <see cref="ArrVector{T}"/> is equal to this one according to the <see cref="Epsilon"/> value of this <see cref="ArrVector{T}"/>.
        /// </summary>
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
        /// <summary>
        /// Returns a new empty <see cref="ArrVector{T}"/>.
        /// </summary>
        public static ArrVector<T> Empty() => new(0);
        /// <summary>
        /// Returns a new <see cref="ArrVector{T}"/> with the given length and all elements equal to zero.
        /// </summary>
        public static ArrVector<T> Zero(int length) => new(length, T.Zero);
        /// <summary>
        /// Returns a new <see cref="ArrVector{T}"/> with the given length and all elements equal to one.
        /// </summary>
        public static ArrVector<T> One(int length) => new(length, T.One);
        /// <summary>
        /// Returns a new unit <see cref="ArrVector{T}"/> with the given length, with one in the given axis.
        /// </summary>
        public static ArrVector<T> Unit(int length, int axis)
        {
            if (length <= axis) throw new ArgumentOutOfRangeException(nameof(axis));
            ArrVector<T> vec = Zero(length);
            vec.values[axis] = T.One;
            return vec;
        }
        /// <summary>
        /// Returns a new unit <see cref="ArrVector{T}"/> with the given length, with one in the given <see cref="Axis"/>.
        /// </summary>
        public static ArrVector<T> Unit(int length, Axis axis) => Unit(length, (int)axis);
        #endregion

        #region Operators
        // unary operators
        /// <summary>
        /// Absolute value of a
        /// </summary>
        public static ArrVector<T> operator +(ArrVector<T> a) => a.Abs();
        /// <summary>
        /// Negated value of a
        /// </summary>
        public static ArrVector<T> operator -(ArrVector<T> a) => a.Map(x => -x);
        /// <summary>
        /// Increments each element of a
        /// </summary>
        public static ArrVector<T> operator ++(ArrVector<T> a) => a.Map(x => ++x);
        /// <summary>
        /// Decrements each element of a
        /// </summary>
        public static ArrVector<T> operator --(ArrVector<T> a) => a.Map(x => --x);
        /// <summary>
        /// Normalized value of a
        /// </summary>
        public static ArrVector<T> operator ~(ArrVector<T> a) => a.Norm();
        // binary operators
        /// <summary>
        /// Adds a and b
        /// </summary>
        public static ArrVector<T> operator +(ArrVector<T> a, ArrVector<T> b) => a.Composite(b, (a, b) => a + b);
        /// <summary>
        /// Adds scalar b to each element of a
        /// </summary>
        public static ArrVector<T> operator +(ArrVector<T> a, T b) => a.Combine(b, (a, b) => a + b);
        /// <summary>
        /// Adds scalar a to each element of b
        /// </summary>
        public static ArrVector<T> operator +(T a, ArrVector<T> b) => b.Combine(a, (a, b) => a + b);
        /// <summary>
        /// Subtracts b from a
        /// </summary>
        public static ArrVector<T> operator -(ArrVector<T> a, ArrVector<T> b) => a.Composite(b, (a, b) => a - b);
        /// <summary>
        /// Subtracts scalar b from each element of a
        /// </summary>
        public static ArrVector<T> operator -(ArrVector<T> a, T b) => a.Combine(b, (a, b) => a - b);
        /// <summary>
        /// Subtracts scalar a from each element of b
        /// </summary>
        public static ArrVector<T> operator -(T a, ArrVector<T> b) => b.Combine(a, (a, b) => a - b);
        /// <summary>
        /// Multiplies scalar b to each element of a
        /// </summary>
        public static ArrVector<T> operator *(ArrVector<T> a, T b) => a.Combine(b, (a, b) => a * b);
        /// <summary>
        /// Multiplies scalar a to each element of b
        /// </summary>
        public static ArrVector<T> operator *(T a, ArrVector<T> b) => b.Combine(a, (a, b) => a * b);
        /// <summary>
        /// Divides each element of a by scalar b
        /// </summary>
        public static ArrVector<T> operator /(ArrVector<T> a, T b) => a.Combine(b, (a, b) => a / b);
        /// <summary>
        /// Divides each element of b by scalar a
        /// </summary>
        public static ArrVector<T> operator /(T a, ArrVector<T> b) => b.Combine(a, (a, b) => a / b);
        /// <summary>
        /// Raises each element of a to the power of scalar b
        /// </summary>
        public static ArrVector<T> operator ^(ArrVector<T> a, T b) => a.Combine(b, (a, b) => Calc<T>.Calc2(a, b, Math.Pow));
        /// <summary>
        /// Raises each element of b to the power of scalar a
        /// </summary>
        public static ArrVector<T> operator ^(T a, ArrVector<T> b) => b.Combine(a, (a, b) => Calc<T>.Calc2(a, b, Math.Pow));
        /// <summary>
        /// Dot product of a and b
        /// </summary>
        public static T operator *(ArrVector<T> a, ArrVector<T> b) => a.Composite(b, (a, b) => a * b).Sum();
        /// <summary>
        /// The angle between a and b
        /// </summary>
        public static T operator /(ArrVector<T> a, ArrVector<T> b) => Calc<T>.Calc1(a * b / (a.Mag() * b.Mag()), Math.Acos);
        /// <summary>
        /// Cross product of a and b
        /// </summary>
        public static ArrVector<T> operator %(ArrVector<T> a, ArrVector<T> b)
        {
            if (a.Length != 3 || b.Length != 3) throw new ArgumentException("Cross product needs 3 dimensions!");
            return new(a[1] * b[2] - a[2] * b[1], a[2] * b[0] - a[0] * b[2], a[0] * b[1] - a[1] * b[0]);
        }
        /// <summary>
        /// Returns true if a and b are equal
        /// </summary>
        public static bool operator ==(ArrVector<T> a, ArrVector<T> b) => a.Equals(b);
        /// <summary>
        /// Returns true if a and b are not equal
        /// </summary>
        public static bool operator !=(ArrVector<T> a, ArrVector<T> b) => !a.Equals(b);
        /// <summary>
        /// Returns true if the magnitude of a is greater than the magnitude of b
        /// </summary>
        public static bool operator >(ArrVector<T> a, ArrVector<T> b) => a.Mag2() > b.Mag2();
        /// <summary>
        /// Returns true if the magnitude of a is less than the magnitude of b
        /// </summary>
        public static bool operator <(ArrVector<T> a, ArrVector<T> b) => a.Mag2() < b.Mag2();
        #endregion
    }
}
